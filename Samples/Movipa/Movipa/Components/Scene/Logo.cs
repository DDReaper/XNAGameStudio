#region File Description
//-----------------------------------------------------------------------------
// Logo.cs
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

using Movipa.Components.Input;
#endregion

namespace Movipa.Components.Scene
{
    /// <summary>
    /// This scene component draws the logo. 
    /// It provides fade control and text string rendering. 
    /// 
    /// ���S�̕`����s���V�[���R���|�[�l���g�ł��B
    /// �t�F�[�h�̐���ƁA������̕`�揈�����s���Ă��܂��B
    /// </summary>
    public class Logo : SceneComponent
    {
        #region Fields
        private readonly TimeSpan WaitTime = new TimeSpan(0, 0, 2);

        // Components
        private FadeSeqComponent fade;

        private string developerName;
        private SpriteFont developerFont;
        private Vector2 drawPosition;
        private TimeSpan viewTime;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public Logo(Game game)
            : base(game)
        {
        }


        /// <summary>
        /// Performs initialization processing.
        ///
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Obtains the developer�fs name.
            // 
            // �J�������擾���܂��B
            developerName = GameData.AppSettings["DeveloperName"];

            // Specifies the font settings.
            // 
            // �t�H���g��ݒ肵�܂��B
            developerFont = MediumFont;

            // Sets the draw position.
            // 
            // �`��ʒu��ݒ肵�܂��B
            drawPosition = GameData.ScreenSizeVector2 * 0.5f;
            drawPosition -= developerFont.MeasureString(developerName) * 0.5f;

            // Initializes the display time.
            // 
            // �\�����Ԃ̏��������܂��B
            viewTime = TimeSpan.Zero;

            // Obtains the fade component instance. 
            // 
            // �t�F�[�h�R���|�[�l���g�̃C���X�^���X���擾���܂��B
            fade = GameData.FadeSeqComponent;

            // Specifies the fade-in settings.
            //
            // �t�F�[�h�C���̏�����ݒ肵�܂��B
            fade.Start(FadeType.Normal, FadeMode.FadeIn);

            base.Initialize();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs scene update processing.
        ///
        /// �V�[���̍X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (fade.FadeMode == FadeMode.FadeIn)
            {
                // Switches the fade mode after the fade-in finishes.
                //
                // �t�F�[�h�C��������������t�F�[�h�̃��[�h��؂�ւ��܂��B
                if (!fade.IsPlay)
                {
                    fade.FadeMode = FadeMode.None;
                }
            }
            else if (fade.FadeMode == FadeMode.None)
            {
                // Updates Main.
                //
                // ���C���̍X�V�������s���܂��B
                UpdateMain(gameTime);
            }
            else if (fade.FadeMode == FadeMode.FadeOut)
            {
                // Fade-out
                // 
                // �t�F�[�h�A�E�g
                if (!fade.IsPlay)
                {
                    // Terminates after the fade-out finishes.
                    // 
                    // �t�F�[�h�A�E�g���I��������I�����܂��B
                    Dispose();
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Performs main update processing.
        /// 
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMain(GameTime gameTime)
        {
            // Obtains Pad information.
            // 
            // �p�b�h�̏����擾���܂��B
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];

            // Calculates the display time.
            // 
            // �\�����Ԃ����Z���܂��B
            viewTime += gameTime.ElapsedGameTime;

            // Starts the fade-out when the display time is 
            // exceeded or the A button is pressed.
            // 
            // �\���̎��Ԃ��߂������A�܂���A�{�^���������ꂽ��
            // �t�F�[�h�A�E�g���J�n���܂��B
            if (viewTime > WaitTime || virtualPad.Buttons.A[VirtualKeyState.Push])
            {
                fade.Start(FadeType.Gonzales, FadeMode.FadeOut);
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the scene.
        ///
        /// �V�[���̕`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Clears the background to white.
            // 
            // ���Ŕw�i���N���A���܂��B
            GraphicsDevice.Clear(Color.White);

            // Draws the text.
            // 
            // ������`�悵�܂��B
            Batch.Begin();
            Batch.DrawString(developerFont, developerName, drawPosition, Color.White);
            Batch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}


