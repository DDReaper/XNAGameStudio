#region File Description
//-----------------------------------------------------------------------------
// SafeAreaComponent.cs
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
#endregion

namespace Movipa.Components
{
    /// <summary>
    /// Component that draws the Safe Area.
    /// Push the right stick on the Game Pad to draw.
    /// The scope is set in Scale Properties, and the draw
    /// color in Color Properties. The default scope value is 0.9f (90%).
    /// 
    /// �Z�[�t�G���A��`�悷��R���|�[�l���g�ł��B
    /// �Q�[���p�b�h�̉E�X�e�B�b�N���������񂾏�Ԃŕ`�悵�܂��B
    /// �L���͈͂̐ݒ��Scale�v���p�e�B�Őݒ肵�A�`��F��Color�v���p�e�B��
    /// �ݒ肵�܂��B�L���͈͂̏����l��0.9f(90%)�ɐݒ肳��Ă��܂��B
    /// </summary>
    public class SafeAreaComponent : DrawableGameComponent
    {
        #region Fields
        /// <summary>
        /// Primitive Draw class
        /// 
        /// ��{�`��N���X
        /// </summary>
        private PrimitiveDraw2D primitiveDraw;

        /// <summary>
        /// Scale
        ///
        /// �X�P�[��
        /// </summary>
        private float scale;

        /// <summary>
        /// Draw color
        ///
        /// �`��F
        /// </summary>
        private Vector4 color;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the scale.
        ///
        /// �X�P�[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = MathHelper.Clamp(value, 0, 1); }
        }


        /// <summary>
        /// Obtains or sets the draw color.
        ///
        /// �`��F���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector4 Color
        {
            get { return color; }
            set { color = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SafeAreaComponent(Game game)
            : base(game)
        {
            primitiveDraw = new PrimitiveDraw2D(game.GraphicsDevice);
            Scale = 0.9f;
            color = new Vector4(1, 0, 1, 0.25f);
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Switches the component visibility status from the pad input status.
        /// 
        /// �p�b�h�̓��͏�Ԃ���R���|�[�l���g�̉���Ԃ�؂�ւ��܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Make visible when player 1 pushes the right stick.
            // 
            // �v���C���[1�̉E�X�e�B�b�N����������Ă��������Ԃɂ��܂��B
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            Visible = (gamePad.Buttons.RightStick == ButtonState.Pressed);

            base.Update(gameTime);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the Safe Area.
        /// 
        /// �Z�[�t�G���A��`�悵�܂��B
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Sets the draw mode.
            // 
            // �`�惂�[�h��ݒ�
            primitiveDraw.SetRenderState(GraphicsDevice, SpriteBlendMode.AlphaBlend);

            // Fills the rectangle list.
            // 
            // ��`���X�g��h��Ԃ�
            foreach (Rectangle region in GetRegions())
            {
                primitiveDraw.FillRect(null, region, new Color(color));
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the Safe Area rectangle.
        /// 
        /// �Z�[�t�G���A�̋�`���擾���܂��B
        /// </summary>
        private Rectangle[] GetRegions()
        {
            Vector2 safeSize = GameData.ScreenSizeVector2 * Scale;
            Vector2 outSize = GameData.ScreenSizeVector2 - safeSize;
            Vector2 halfOutSize = outSize * 0.5f;

            Rectangle[] regions = new Rectangle[4];

            // Up 
            // 
            // ��
            regions[0].X = 0;
            regions[0].Y = 0;
            regions[0].Width = GameData.ScreenWidth;
            regions[0].Height = (int)halfOutSize.Y;

            // Down
            // 
            // ��
            regions[1].X = 0;
            regions[1].Y = GameData.ScreenHeight - (int)halfOutSize.Y;
            regions[1].Width = GameData.ScreenWidth;
            regions[1].Height = (int)halfOutSize.Y;

            // Left
            // 
            // ��
            regions[2].X = 0;
            regions[2].Y = (int)halfOutSize.Y;
            regions[2].Width = (int)halfOutSize.X;
            regions[2].Height = GameData.ScreenHeight - (int)outSize.Y;

            // Right
            // 
            // �E
            regions[3].X = GameData.ScreenWidth - (int)halfOutSize.X;
            regions[3].Y = (int)halfOutSize.Y;
            regions[3].Width = (int)halfOutSize.X;
            regions[3].Height = GameData.ScreenHeight - (int)outSize.Y;

            return regions;
        }
        #endregion
    }
}


