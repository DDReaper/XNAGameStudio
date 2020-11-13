#region File Description
//-----------------------------------------------------------------------------
// SequencePlayAnimationComponent.cs
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

using SceneDataLibrary;
using MovipaLibrary;
#endregion

namespace Movipa.Components.Animation
{
    /// <summary>
    /// This component is for animations used in puzzles.
    /// This class inherits PuzzleAnimation to play and draw
    /// the sequence of Layout.
    ///
    /// �p�Y���Ŏg�p����A�j���[�V�����̃R���|�[�l���g�ł��B
    /// ���̃N���X��PuzzleAnimation���p�����ALayout�̃V�[�P���X��
    /// �Đ����ĕ`�悵�܂��B
    /// </summary>
    public class SequencePlayAnimationComponent : PuzzleAnimation
    {
        #region Fields
        private SceneData sceneData = null;
        private SequencePlayData seqPlayData = null;
        private LayoutInfo layoutToolInfo;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the movie information.
        ///
        /// ���[�r�[�����擾���܂��B
        /// </summary>
        public new LayoutInfo Info
        {
            get { return layoutToolInfo; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SequencePlayAnimationComponent(Game game, LayoutInfo info)
            : base(game, info)
        {
            layoutToolInfo = info;
        }


        /// <summary>
        /// Reads the contents.
        ///
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Reads the Layout data.
            // 
            // Layout�̃f�[�^��ǂݍ��݂܂��B
            sceneData = Content.Load<SceneData>(Info.SceneDataAsset);
            seqPlayData = sceneData.CreatePlaySeqData(Info.Sequence);

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the sequence.
        ///
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            seqPlayData.Update(gameTime.ElapsedGameTime);
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
            GraphicsDevice.Clear(Color.Black);

            // Draws the sequence.
            // 
            // �V�[�P���X�̕`����s���܂��B
            Batch.Begin();
            seqPlayData.Draw(Batch, null);
            Batch.End();
        }
        #endregion
   }
}







