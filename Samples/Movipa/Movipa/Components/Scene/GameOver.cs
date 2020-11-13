#region File Description
//-----------------------------------------------------------------------------
// GameOver.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Movipa.Components.Input;
using Movipa.Util;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene
{
    /// <summary>
    /// This scene component displays game over.
    /// It provides fade control and basic replay
    /// functionality for Layout sequences.
    ///
    /// �Q�[���I�[�o�[��\������V�[���R���|�[�l���g�ł��B
    /// �t�F�[�h�̐���ƁALayout�ō쐬���ꂽ�V�[�P���X��
    /// �V���v���ȍĐ��������s���Ă��܂��B

    /// </summary>
    public class GameOver : SceneComponent
    {
        #region Fields
        // Components
        private FadeSeqComponent fade;

        // Layout
        private SceneData sceneData;
        private SequencePlayData seqStart;

        // BackgroundMusic
        private Cue bgm;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public GameOver(Game game)
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
            // Obtains the fade component instance. 
            //
            // �t�F�[�h�R���|�[�l���g�̃C���X�^���X���擾���܂��B
            fade = GameData.FadeSeqComponent;

            // Specifies the fade-in settings.
            //
            // �t�F�[�h�C���̐ݒ���s���܂��B
            fade.Start(FadeType.Normal, FadeMode.FadeIn);

            base.Initialize();
        }


        /// <summary>
        /// Initializes the navigate.
        /// 
        /// �i�r�Q�[�g�̏����������܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
            base.InitializeNavigate();
        }


        /// <summary>
        /// Loads the content.
        ///
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Obtains the Layout sequence.
            // 
            // Layout�̃V�[�P���X���擾���܂��B
            string asset = "Layout/GameOver/gameover_Scene";
            sceneData = Content.Load<SceneData>(asset);
            seqStart = sceneData.CreatePlaySeqData("GameOver");

            // Plays the BackgroundMusic and obtains Cue.
            // 
            // BackgroundMusic���Đ����ACue���擾���܂��B
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.GameOverBackgroundMusic);

            base.LoadContent();
        }


        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        protected override void UnloadContent()
        {
            // Stops the BackgroundMusic.
            // 
            // BackgroundMusic���~���܂��B
            SoundComponent.Stop(bgm);

            base.UnloadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
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
                UpdateMain();
            }
            else if (fade.FadeMode == FadeMode.FadeOut)
            {
                // Updates fade-out. 
                // 
                // �t�F�[�h�A�E�g�̍X�V�������s���܂��B
                UpdateFadeOut();
            }

            // Updates sequences except fade-in.
            //
            // �t�F�[�h�C���ȊO�ŃV�[�P���X�̍X�V�����܂��B
            if (fade.FadeMode != FadeMode.FadeIn)
            {
                seqStart.Update(gameTime.ElapsedGameTime);
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Performs main update processing.
        ///
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMain()
        {
            // Obtains Pad information. 
            //
            // �p�b�h�̏����擾���܂��B
            VirtualPadState virtualPad =
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Performs the fade-out after the sequence terminates 
            // or if the A button is pressed.
            //
            // �V�[�P���X���Đ��I���A�܂���A�{�^���������ꂽ��t�F�[�h�A�E�g���܂��B
            if (!seqStart.IsPlay || buttons.A[VirtualKeyState.Push] || 
                buttons.B[VirtualKeyState.Push] || buttons.Back[VirtualKeyState.Push])
            {
                fade.Start(FadeType.Normal, FadeMode.FadeOut);
            }
        }


        /// <summary>
        /// Performs update processing at fade-out.
        /// 
        /// �t�F�[�h�A�E�g���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateFadeOut()
        {
            // Fades out the BackgroundMusic volume.
            // 
            // BackgroundMusic�̃{�����[�����t�F�[�h�A�E�g���܂��B
            float volume = 1.0f - (fade.Count / 60.0f);
            SoundComponent.SetVolume(bgm, volume);

            // Switches scenes after the fade finishes.
            //
            // �t�F�[�h���I������ƃV�[���̐؂�ւ����s���܂��B
            if (!fade.IsPlay)
            {
                Dispose();

                // Performs entry for title screen scenes.
                // 
                // �^�C�g����ʂ̃V�[�����G���g���[���܂��B
                GameData.SceneQueue.Enqueue(new Title(Game));
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the sequence.
        ///
        /// �V�[�P���X�̕`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Clears the background.
            // 
            // �w�i���N���A���܂��B
            GraphicsDevice.Clear(Color.Black);

            // Draws the sequence and navigate button. 
            // 
            // �V�[�P���X�ƃi�r�Q�[�g�{�^����`�悵�܂��B
            Batch.Begin();
            seqStart.Draw(Batch, null);
            DrawNavigate(gameTime, false);
            Batch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}


