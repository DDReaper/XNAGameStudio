#region File Description
//-----------------------------------------------------------------------------
// ParticleComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Movipa.Util;
using MovipaLibrary;
#endregion

namespace Movipa.Components.Animation
{
    /// <summary>
    /// This component is for animations used in puzzles.
    /// This class inherits PuzzleAnimation to draw particles
    /// with SpriteBatch.
    ///
    /// �p�Y���Ŏg�p����A�j���[�V�����̃R���|�[�l���g�ł��B
    /// ���̃N���X��PuzzleAnimation���p�����A�p�[�e�B�N����
    /// SpriteBatch�ŕ`�悵�܂��B
    /// </summary>
    public class ParticleComponent : PuzzleAnimation
    {
        #region Fields
        private Matrix projection;
        private Matrix view;

        private Vector3 cameraUpVector;
        private Vector3 cameraPosition;
        private Vector3 cameraLookAt;
        private float cameraRotate = 0;
        private float cameraDistance = 100.0f;

        private UInt32 particleMax;
        private float particleJumpPower;
        private float particleMoveSpeed;
        private UInt32 particleGenerateCount;


        private Texture2D spriteTexture;
        private LinkedList<Particle> particleList;

        // Class to draw a floor
        // 
        // ����`�悷��N���X
        private PrimitiveFloor primitiveFloor;

        // Animation information
        // 
        // �A�j���[�V�������
        private ParticleInfo particleInfo;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the movie information.
        ///
        /// ���[�r�[�����擾���܂��B
        /// </summary>
        public new ParticleInfo Info
        {
            get { return particleInfo; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public ParticleComponent(Game game, ParticleInfo info)
            : base(game, info)
        {
            particleInfo = info;

            // Creates a class to draw a floor.
            // 
            // ����`�悷��N���X���쐬���܂��B
            primitiveFloor = new PrimitiveFloor(game.GraphicsDevice);
        }


        /// <summary>
        /// Performs initialization.
        ///
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Obtains the setting information of the camera.
            // 
            // �J�����̐ݒ���擾���܂��B
            cameraUpVector = Info.CameraUpVector;
            cameraPosition = Info.CameraPosition;
            cameraLookAt = Info.CameraLookAt;

            // Obtains the setting information of the particle.
            particleMax = Info.ParticleMax;
            particleJumpPower = Info.ParticleJumpPower;
            particleMoveSpeed = Info.ParticleMoveSpeed;
            particleGenerateCount = Info.ParticleGenerateCount;

            // Creates a particle array.
            // 
            // �p�[�e�B�N���̔z����쐬���܂��B
            particleList = new LinkedList<Particle>();

            // Creates a projection.
            // 
            // �v���W�F�N�V�������쐬���܂��B
            float aspect = (float)Info.Size.X / (float)Info.Size.Y;
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, aspect, 0.1f, 1000.0f);

            base.Initialize();
        }


        /// <summary>
        /// Loads the contents.
        ///
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the particle texture.
            // 
            // �p�[�e�B�N���̃e�N�X�`����ǂݍ��݂܂��B
            string asset = Info.ParticleTexture;
            spriteTexture = Content.Load<Texture2D>(asset);

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the particle and camera.
        ///
        /// �p�[�e�B�N���ƃJ�����̍X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Updates the camera.
            // 
            // �J�����̍X�V�������s���܂��B
            UpdateCamera();

            // Updates the particle.
            // 
            // �p�[�e�B�N���̍X�V�������s���܂��B
            UpdateParticles();

            base.Update(gameTime);
        }


        /// <summary>
        /// Updates all the particles.
        /// 
        /// �S�Ẵp�[�e�B�N���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateParticles()
        {
            // Creates a particle.
            // 
            // �p�[�e�B�N�����쐬���܂��B
            CreateParticle();

            // Moves all the particles.
            // 
            // �S�Ẵp�[�e�B�N�����ړ����܂��B
            LinkedListNode<Particle> node = particleList.First;
            LinkedListNode<Particle> removeNode;
            while (node != null)
            {
                Particle particle = node.Value;
                UpdateParticle(particle);

                // Saves the node to determine the end of the animation.
                // 
                // �A�j���[�V�����I������p�Ƀm�[�h��ێ����܂��B
                removeNode = node;

                // Moves to the next node.
                // 
                // ���̃m�[�h�ֈړ����܂��B
                node = node.Next;

                // Checks the determination node to see whether it has ended.
                // If it has ended, removes this node from the list.
                // 
                // �I������p�̃m�[�h���`�F�b�N���A�I�����Ă����ꍇ��
                // �m�[�h�����X�g����폜���܂��B
                if (!removeNode.Value.Enable)
                {
                    particleList.Remove(removeNode);
                }
            }
        }


        /// <summary>
        /// Updates the single particle. 
        ///
        /// �P�̂̃p�[�e�B�N���̍X�V�������s���܂��B
        /// </summary>
        private static void UpdateParticle(Particle particle)
        {
            // Changes the fall velocity for the distance.
            // 
            // �ړ��ʂ̗������x��ύX���܂��B
            particle.Velocity += Particle.Gravity;

            // Moves the particle.
            // 
            // �p�[�e�B�N���̈ړ������܂��B
            particle.Position += particle.Velocity;

            // If the position of the particle falls below the floor, 
            // terminates the animation.
            //
            // �p�[�e�B�N���̈ʒu������艺�ɍs���ƁA
            // �A�j���[�V�����̏I���������s���܂��B
            if (particle.Position.Y < 0)
                particle.Enable = false;
        }


        /// <summary>
        /// Updates the camera.
        /// </summary>
        private void UpdateCamera()
        {
            // Rotates the camera.
            // 
            // �J��������]�����܂��B
            cameraRotate += 0.001f;
            cameraPosition.X = (float)Math.Sin(cameraRotate) * cameraDistance;
            cameraPosition.Z = (float)Math.Cos(cameraRotate) * cameraDistance;

            // Creates a view from the camera.
            // 
            // �J��������r���[���쐬���܂��B
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUpVector);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs drawing for the render target.
        ///
        /// �����_�[�^�[�Q�b�g�ւ̕`�揈�����s���܂��B
        /// </summary>
        protected override void DrawRenderTarget()
        {
            // Clears the background.
            // 
            // �w�i���N���A���܂��B
            GraphicsDevice.Clear(Color.Black);

            // Draws the floor.
            // 
            // ����`�悵�܂��B
            primitiveFloor.SetRenderState(GraphicsDevice, SpriteBlendMode.AlphaBlend);
            primitiveFloor.Draw(projection, view);

            // Draws the particle.
            // 
            // �p�[�e�B�N����`�悵�܂��B
            DrawParticle();
        }


        /// <summary>
        /// Draws the particle.
        ///
        /// �p�[�e�B�N����`�悵�܂��B
        /// </summary>
        private void DrawParticle()
        {
            Batch.Begin(SpriteBlendMode.Additive);

            foreach (Particle particle in particleList)
            {
                Vector3 position = particle.Position;

                // Converts world coordinates to screen coordinates.
                // 
                // ���[���h���W����X�N���[�����W�ɕϊ����܂��B
                Vector2 pos1 = WorldToScreen(position);

                // Converts world coordinates to screen coordinates by 
                // turning them upside down.
                // 
                // �ʒu���㉺���]�����āA�X�N���[�����W�ɕϊ����܂��B
                position.Y = -position.Y;
                Vector2 pos2 = WorldToScreen(position);

                // Draws the particle.
                // 
                // �p�[�e�B�N����`�悵�܂��B
                Batch.Draw(spriteTexture, pos1, Color.White);
                Batch.Draw(spriteTexture, pos2, Color.Blue);
            }

            Batch.End();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates new particles.
        ///
        /// �p�[�e�B�N���̐����������s���܂��B
        /// </summary>
        private void CreateParticle()
        {
            for (int i = 0; i < particleGenerateCount; i++)
            {
                // Checks the number of current particles. 
                // If it exceeds the maximum number of allowed particles, 
                // stops creating new particles.
                //
                // �p�[�e�B�N���̍ő吔���`�F�b�N���A����ɒB���Ă�����
                // �����������s���܂���B
                if (particleList.Count >= particleMax)
                {
                    return;
                }

                // Create a new particle.
                // 
                // �p�[�e�B�N����V���ɍ쐬���܂��B
                Particle particle = new Particle();
                float direction = (float)Random.NextDouble() * 360.0f;
                float speed = ((float)Random.NextDouble() + 0.1f) * particleMoveSpeed;
                float jump = ((float)Random.NextDouble() + 0.1f) * particleJumpPower;

                particle.Enable = true;
                particle.Position = Vector3.Zero;

                Vector3 velocity = new Vector3();
                velocity.X += (float)Math.Sin(direction) * speed;
                velocity.Y += jump;
                velocity.Z += (float)Math.Cos(direction) * speed;
                particle.Velocity = velocity;

                // Adds the particle to the array.
                // 
                // �p�[�e�B�N����z��ɒǉ����܂��B
                particleList.AddLast(particle);
            }

        }


        /// <summary>
        /// Converts world coordinates to screen coordinates.
        ///
        /// ���[���h���W����X�N���[�����W�ɕϊ����܂��B
        /// </summary>
        private Vector2 WorldToScreen(Vector3 position)
        {
            Vector4 v4 = Vector4.Transform(position, Matrix.Identity);
            v4 = Vector4.Transform(v4, view);
            v4 = Vector4.Transform(v4, projection);

            Vector2 screenSize = new Vector2(Info.Size.X, Info.Size.Y);
            Vector2 screenHalf = screenSize * 0.5f;

            float x = (v4.X / v4.W + 1) * screenHalf.X;
            float y = (1 - v4.Y / v4.W) * screenHalf.Y;

            return new Vector2(x, y);
        }
        #endregion
    }
}

