#region File Description
//-----------------------------------------------------------------------------
// PrimitiveRenderState.cs
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
#endregion


namespace Movipa.Util
{
    /// <summary>
    /// Class that sets the render state.
    /// Although the render state can be declared and used separately, 
    /// it is basically designed to be used through inheritance of this class. 
    /// 
    /// �����_�[�X�e�[�g��ݒ肷��N���X�ł��B
    /// �P�̂Ő錾���Ă��g�p�ł��܂����A��{�I�ɂ͂��̃N���X���p������
    /// �g�p���邱�Ƃ�ړI�Ƃ��܂��B
    /// </summary>
    public class PrimitiveRenderState
    {
        #region Helper Methods
        /// <summary>
        /// Sets the render state.
        /// 
        /// �����_�[�X�e�[�g��ݒ肵�܂��B
        /// </summary>
        public virtual void SetRenderState(GraphicsDevice graphics, SpriteBlendMode mode)
        {
            RenderState state = graphics.RenderState;
            if (mode == SpriteBlendMode.AlphaBlend)
            {
                // Enables AlphaBlend.
                // 
                // �A���t�@�u�����h�L��ɐݒ肵�܂��B
                state.AlphaBlendEnable = true;
                state.AlphaBlendOperation = BlendFunction.Add;
                state.SourceBlend = Blend.SourceAlpha;
                state.DestinationBlend = Blend.InverseSourceAlpha;
                state.SeparateAlphaBlendEnabled = false;

                state.AlphaTestEnable = true;
                state.AlphaFunction = CompareFunction.Greater;
                state.ReferenceAlpha = 0;
            }
            else if (mode == SpriteBlendMode.Additive)
            {
                // Sets to Addition. 
                // 
                // ���Z�ɐݒ肵�܂��B
                state.AlphaBlendEnable = true;
                state.AlphaBlendOperation = BlendFunction.Add;
                state.SourceBlend = Blend.SourceAlpha;
                state.DestinationBlend = Blend.SourceAlpha | Blend.InverseSourceAlpha;
                state.SeparateAlphaBlendEnabled = false;

                state.AlphaTestEnable = true;
                state.AlphaFunction = CompareFunction.Greater;
                state.ReferenceAlpha = 0;
            }
            else if (mode == SpriteBlendMode.None)
            {
                // Disables AlphaBlend.
                // 
                // �A���t�@�u�����h�����ɐݒ肵�܂��B
                state.AlphaBlendEnable = false;
                state.AlphaTestEnable = false;
            }
        }
        #endregion
    }
}