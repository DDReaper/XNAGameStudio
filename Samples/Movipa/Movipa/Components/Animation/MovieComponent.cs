#region File Description
//-----------------------------------------------------------------------------
// MovieComponent.cs
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

using MovipaLibrary;
#endregion

namespace Movipa.Components.Animation
{
    /// <summary>
    /// This component is for animations used in puzzles.
    /// This class inherits PuzzleAnimation to read and draw
    /// sequential textures.
    ///
    /// �p�Y���Ŏg�p����A�j���[�V�����̃R���|�[�l���g�ł��B
    /// ���̃N���X��PuzzleAnimation���p�����A�A�Ԃ̃e�N�X�`����
    /// �ǂݍ���ŕ`�悵�܂��B
    /// </summary>
    public class MovieComponent : PuzzleAnimation
    {
        #region Fields
        /// <summary>
        /// Waiting time for each frame
        ///
        /// �t���[�����̃E�F�C�g
        /// </summary>
        private readonly TimeSpan FrameWait;

        /// <summary>
        /// Movie texture
        ///
        /// ���[�r�[�̃e�N�X�`��
        /// </summary>
        private Texture2D movieTexture;

        /// <summary>
        /// Movie texture number
        ///
        /// ���[�r�[�̃e�N�X�`���ԍ�
        /// </summary>
        private UInt32 textureCount;

        /// <summary>
        /// Movie frame number
        ///
        /// ���[�r�[�̃t���[���ԍ�
        /// </summary>
        private UInt32 frameCount;

        /// <summary>
        /// Drawing size
        /// 
        /// �`��T�C�Y
        /// </summary>
        private Rectangle drawRectangle;

        /// <summary>
        /// Original image size
        ///
        /// ���摜�T�C�Y
        /// </summary>
        private Rectangle srcRectangle;

        /// <summary>
        /// Movie information
        ///
        /// ���[�r�[���
        /// </summary>
        private RenderingInfo renderingInfo;

        /// <summary>
        /// Waiting time
        ///
        /// �E�F�C�g����
        /// </summary>
        private TimeSpan waitTime;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the movie information.
        ///
        /// ���[�r�[�����擾���܂��B
        /// </summary>
        public new RenderingInfo Info
        {
            get { return renderingInfo; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public MovieComponent(Game game, RenderingInfo info)
            : base(game, info)
        {
            renderingInfo = info;

            // Sets the waiting time for one frame.
            //
            // 1�t���[���̃E�F�C�g��ݒ肵�܂��B
            long tick = TimeSpan.TicksPerSecond / renderingInfo.FrameRate;
            FrameWait = new TimeSpan(tick);
        }


        /// <summary>
        /// Performs initialization.
        ///
        /// �����������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Sets the image width.
            // 
            // �摜�̕���ݒ肵�܂��B
            srcRectangle = new Rectangle(0, 0, Info.ImageSize.X, Info.ImageSize.Y);

            // Sets the drawing size.
            //
            // �`��̃T�C�Y��ݒ肵�܂��B
            drawRectangle = new Rectangle(0, 0, Info.Size.X, Info.Size.Y);

            // Initializes the frame waiting time.
            //
            // �t���[���E�F�C�g�����������܂��B
            waitTime = TimeSpan.Zero;

            base.Initialize();
        }


        /// <summary>
        /// Reads the content.
        ///
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            string asset;

            // Reads all the sequential numbers and cache textures.
            // 
            // �ŏ��ɑS�Ă̘A�Ԃ�ǂݍ��݁A�e�N�X�`���̃L���b�V�����s���܂��B
            for (int i = 0; i < Info.TotalTexture; i++)
            {
                asset = string.Format(Info.Format, i);
                Content.Load<Texture2D>(string.Format(Info.Format, i));
            }

            // Sets the first texture.
            // 
            // �ŏ��̃e�N�X�`����ݒ肵�܂��B
            asset = string.Format(Info.Format, textureCount);
            movieTexture = Content.Load<Texture2D>(asset);

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs the update process to switch the movie frame.
        ///
        /// ���[�r�[�̃t���[����؂�ւ���X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Calculates the frame waiting time.
            // 
            // �t���[���̃E�F�C�g���v�Z���܂��B
            waitTime += gameTime.ElapsedGameTime;

            // If the frame waiting time exceeds the preset time, sets the next frame.
            // 
            // �E�F�C�g���I�����Ă����玟�̃t���[����ݒ肵�܂��B
            if (waitTime >= FrameWait)
            {
                // Initialize the waiting time.
                //
                // �E�F�C�g�����������܂��B
                waitTime = TimeSpan.Zero;

                UpdateNextFrame();
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Updates the next frame.
        ///
        /// ���̃t���[���֍X�V���܂��B
        /// </summary>
        private void UpdateNextFrame()
        {
            // Increments the frame count.
            //
            // �t���[���������̒l�֐ݒ肵�܂��B
            frameCount = (frameCount + 1) % Info.TotalFrame;

            // If all frames are completed, returns to the first texture and frame.
            //
            // �S�Ẵt���[�����I�����Ă����ꍇ��
            // �ŏ��̃e�N�X�`���ƃt���[���ɖ߂��܂��B
            if (frameCount == 0)
            {
                srcRectangle.X = 0;
                srcRectangle.Y = 0;
                textureCount = 0;
                string asset = string.Format(Info.Format, textureCount);
                movieTexture = Content.Load<Texture2D>(asset);
                return;
            }

            // Moves the X coordinate of the transfer source.
            // 
            // �]������X���W���ړ����܂��B
            int nextX = (srcRectangle.X + srcRectangle.Width);
            srcRectangle.X = nextX % movieTexture.Width;
            if (srcRectangle.X == 0)
            {
                // If the X coordinate is set to 0, moves the Y coordinate.
                // 
                // X���W��0�ɖ߂��Ă����ꍇ��Y���W���ړ����܂��B
                int nextY = (srcRectangle.Y + srcRectangle.Height);
                srcRectangle.Y = nextY % movieTexture.Height;
                    
                if (srcRectangle.Y == 0)
                {
                    // If both the X and Y coordinates are set to 0, which means 
                    // all frames in this texture are drawn, switches to the
                    // next texture. 
                    // 
                    // X���W��Y���W��0�ɖ߂�����A�e�N�X�`�����̃t���[����
                    // �S�ĕ`�悵�I������̂Ŏ��̃e�N�X�`���ɐ؂�ւ��܂��B
                    textureCount = (textureCount + 1) % Info.TotalTexture;
                    string asset = string.Format(Info.Format, textureCount);
                    movieTexture = Content.Load<Texture2D>(asset);
                }
            }
        }

        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs a drawing process for the rendering target.
        ///
        /// �����_�[�^�[�Q�b�g�ւ̕`�揈�����s���܂��B
        /// </summary>
        protected override void DrawRenderTarget()
        {
          
           // Clears the background.
           //
           // �w�i���N���A���܂��B
            GraphicsDevice.Clear(Color.Black);

            // Draws the texture.
            // 
            // �e�N�X�`����`�悵�܂��B
            Batch.Begin();
            Batch.Draw(movieTexture, drawRectangle, srcRectangle, Color.White);
            Batch.End();

        }
        #endregion
    }


}




