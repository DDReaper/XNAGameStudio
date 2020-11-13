#region File Description
//-----------------------------------------------------------------------------
// PuzzleAnimation.cs
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
    /// This is an abstract class of the component that performs update and 
    /// drawing processes for animations used in a puzzle. 
    /// Major implementations for drawing are written in the subclass.
    ///
    /// �p�Y���Ŏg�p����A�j���[�V�����̍X�V�ƕ`������s����R���|�[�l���g��
    /// ���ۃN���X�ł��B
    /// �`��Ɋւ����Ȏ����͌p����̃N���X�ŋL�q���܂��B
    /// </summary>
    public abstract class PuzzleAnimation : SceneComponent
    {
        #region Fields
        private AnimationInfo animationInfo;
        protected RenderTarget2D renderTarget;
        protected Texture2D renderTargetTexture;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the movie information.
        ///
        /// ���[�r�[�����擾���܂��B
        /// </summary>
        public AnimationInfo Info
        {
            get { return animationInfo; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        protected PuzzleAnimation(Game game, AnimationInfo info)
            : base(game)
        {
            animationInfo = info;
        }


        /// <summary>
        /// Reads the contents.
        ///
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Obtains the parameters.
            // 
            // �p�����[�^���擾���܂��B
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;
            MultiSampleType msType = pp.MultiSampleType;
            int msQuality = pp.MultiSampleQuality;

            // Creates a render target.
            // 
            // �����_�[�^�[�Q�b�g���쐬���܂��B
            int width = Info.Size.X;
            int height = Info.Size.Y;
            renderTarget = new RenderTarget2D(
                GraphicsDevice, width, height, 1, format, msType, msQuality, 
                RenderTargetUsage.PreserveContents);

            base.LoadContent();
        }

        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs drawing for the render target.
        ///
        /// �����_�[�^�[�Q�b�g�ւ̕`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (renderTarget != null && !renderTarget.IsDisposed)
            {
                // Sets the drawing target.
                // 
                // �`����ݒ肵�܂��B
                GraphicsDevice.SetRenderTarget(0, renderTarget);

                // Performs drawing for the render target.
                // 
                // �����_�[�^�[�Q�b�g�ւ̕`����s���܂��B
                DrawRenderTarget();

                // Returns the drawing target. 
                // 
                // �`����߂��܂��B
                GraphicsDevice.SetRenderTarget(0, null);

                // Obtains the texture.
                // 
                // �e�N�X�`�����擾���܂��B
                renderTargetTexture = renderTarget.GetTexture();
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// Performs drawing for the render target.
        /// 
        /// �����_�[�^�[�Q�b�g�ւ̕`�揈�����s���܂��B
        /// </summary>
        protected virtual void DrawRenderTarget() { }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the texture of the render target.
        /// 
        /// �����_�[�^�[�Q�b�g�̃e�N�X�`�����擾���܂��B
        /// </summary>
        public Texture2D Texture
        {
            get { return renderTargetTexture; }
        }


        /// <summary>
        /// Creates an animation component.
        /// 
        /// �A�j���[�V�����R���|�[�l���g���쐬���܂��B
        /// </summary>
        public static PuzzleAnimation CreateAnimationComponent(
            Game game, AnimationInfo info)
        {
            switch (info.Category)
            {
                case AnimationInfo.AnimationInfoCategory.Layout:
                    return new SequencePlayAnimationComponent(
                        game, (LayoutInfo)info);
                case AnimationInfo.AnimationInfoCategory.Rendering:
                    return new MovieComponent(
                        game, (RenderingInfo)info);
                case AnimationInfo.AnimationInfoCategory.SkinnedModelAnimation:
                    return new Animation.ModelAnimation.ModelAnimationComponent(
                        game, (SkinnedModelAnimationInfo)info);
                case AnimationInfo.AnimationInfoCategory.Particle:
                    return new Animation.ParticleComponent(
                        game, (ParticleInfo)info);
                default:
                    throw new ArgumentException("Invalid animation category provided.");
            }
        }

        #endregion
    }
}




