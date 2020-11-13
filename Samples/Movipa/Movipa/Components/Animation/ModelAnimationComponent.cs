#region File Description
//-----------------------------------------------------------------------------
// ModelAnimationComponent.cs
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

using Movipa.Util;
using MovipaLibrary;
using SkinnedModel;
#endregion

namespace Movipa.Components.Animation.ModelAnimation
{
    /// <summary>
    /// This component is for animations used in puzzles.
    /// This class inherits PuzzleAnimation to animate and draw
    /// skin models.
    ///
    /// �p�Y���Ŏg�p����A�j���[�V�����̃R���|�[�l���g�ł��B
    /// ���̃N���X��PuzzleAnimation���p�����A�X�L�����f����
    /// �A�j���[�V���������ĕ`�悵�܂��B
    /// </summary>
    public class ModelAnimationComponent : PuzzleAnimation
    {
        #region Fields
        private readonly Color ClearColor;
        private Matrix projection;
        private Matrix view;
        private Vector3 cameraUpVector;
        private Vector3 cameraPosition;
        private Vector3 cameraLookAt;
        private List<SkinnedModelData> modelList;
        private SkinnedModelAnimationInfo skinnedModelAnimationInfo;
        #endregion

        #region Property
        /// <summary>
        /// Obtains the movie information.
        ///
        /// ���[�r�[�����擾���܂��B
        /// </summary>
        public new SkinnedModelAnimationInfo Info
        {
            get { return skinnedModelAnimationInfo; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public ModelAnimationComponent(Game game, SkinnedModelAnimationInfo info)
            : base(game, info)
        {
            skinnedModelAnimationInfo = info;

            // Sets the color to clear the background.
            // 
            // �w�i���N���A����F��ݒ肵�܂��B
            ClearColor = Color.CornflowerBlue;
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
            cameraPosition = Info.CameraPosition;
            cameraLookAt = Info.CameraLookAt;
            cameraUpVector = Info.CameraUpVector;

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
            // Loads the skin model data.
            // 
            // ���f���f�[�^��ǂݍ��݂܂��B
            modelList = new List<SkinnedModelData>();
            List<SkinnedModelInfo> list = Info.SkinnedModelInfoCollection;
            foreach (SkinnedModelInfo skinnedModelInfo in list)
            {
                LoadModel(skinnedModelInfo);
            }

            base.LoadContent();
        }


        /// <summary>
        /// Loads the skin model data.
        ///
        /// ���f���f�[�^��ǂݍ��݂܂��B
        /// </summary>
        private void LoadModel(SkinnedModelInfo skinnedModelInfo)
        {
            // Loads the skin model data.
            // 
            // ���f���f�[�^��ǂݍ��݂܂��B
            Model model = Content.Load<Model>(skinnedModelInfo.ModelAsset);
            SkinnedModelData modelData = new SkinnedModelData(model, "Take 001");

            // Sets the animation data.
            // 
            // �A�j���[�V�����f�[�^��ݒ肵�܂��B
            SkinningData skinningData = modelData.Model.Tag as SkinningData;
            modelData.AnimationPlayer = new AnimationPlayer(skinningData);
            modelData.AnimationClip =
                skinningData.AnimationClips[skinnedModelInfo.AnimationClip];
            modelData.AnimationPlayer.StartClip(modelData.AnimationClip);

            // Obtains the position of the skin model.
            // 
            // �|�W�V�������擾���܂��B
            modelData.Position = skinnedModelInfo.Position;

            // Adds the skin model data to the list.
            // 
            // ���X�g�ɒǉ����܂��B
            modelList.Add(modelData);
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the skin model and camera.
        ///
        /// ���f���ƃJ�����̍X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Updates the animation for all the skin models.
            // 
            // �S�Ẵ��f���̃A�j���[�V�������X�V���܂��B
            foreach (SkinnedModelData model in modelList)
            {
                model.AnimationPlayer.Update(
                    gameTime.ElapsedGameTime, true, Matrix.Identity);
            }

            // Updates the camera.
            // 
            // �J�����̍X�V�����܂��B
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUpVector);

            base.Update(gameTime);
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
            GraphicsDevice.Clear(ClearColor);

            // Enable the depth buffer.
            GraphicsDevice.RenderState.DepthBufferEnable = true;

            // Draws all the skin models.
            // 
            // �S�Ẵ��f����`�悵�܂��B
            foreach (SkinnedModelData model in modelList)
            {
                model.Draw(view, projection);
            }
        }
        #endregion
    }
}

