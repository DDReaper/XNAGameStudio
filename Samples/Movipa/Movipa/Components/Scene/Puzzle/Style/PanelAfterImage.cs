#region File Description
//-----------------------------------------------------------------------------
// PanelAfterImage.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Movipa.Util;
#endregion

namespace Movipa.Components.Scene.Puzzle.Style
{
    /// <summary>
    /// Panel after-image class.
    /// This class inherits the Sprite class and performs update and draw processing.
    /// Sprites are managed by the StyleBase class.
    /// 
    /// �p�l���̎c���N���X�ł��B
    /// ���̃N���X��Sprite�N���X���p�����A�X�V�ƕ`��̏������s���܂��B
    /// �X�v���C�g�̊Ǘ���StyleBase�N���X�ōs���Ă��܂��B
    /// </summary>
    public class PanelAfterImage : Sprite
    {
        #region Initialization
        /// <summary>
        /// Initializes the instance. 
        /// 
        /// �C���X�^���X�̏��������s���܂��B
        /// </summary>
        public PanelAfterImage(Game game)
            : base(game)
        {
            Updating += PanelAfterImageUpdating;
            Drawing += PanelAfterImageDrawing;
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
        /// </summary>
        void PanelAfterImageUpdating(object sender, UpdatingEventArgs args)
        {
            // Reduces the transparency color value.
            // 
            // ���ߐF�̒l�������܂��B
            Vector4 colorVector4 = Color.ToVector4();
            colorVector4.W = MathHelper.Clamp(colorVector4.W - 0.1f, 0.0f, 1.0f);
            Color = new Color(colorVector4);

            // Performs release processing when the transparency color value reaches 0.
            //
            // ���ߐF�̒l��0�ɂȂ�����J���������s���܂��B
            if (Color.A == 0)
                Dispose();
        }
        #endregion

        #region Drawing Methods

        /// <summary>
        /// Performs a primitive draw.
        /// This method needs to be invoked in advance since the 
        /// SpriteBatch Begin and End are not performed.
        ///
        /// ��{�I�ȕ`����s���܂��B
        /// ���̃��\�b�h�ł�SpriteBatch��Begin/End���s���Ȃ��̂�
        /// ���O�ɌĂяo���ĉ������B
        /// </summary>
        void PanelAfterImageDrawing(object sender, DrawingEventArgs args)
        {
            base.Draw(args.Batch);
        }

        #endregion
    }
}
