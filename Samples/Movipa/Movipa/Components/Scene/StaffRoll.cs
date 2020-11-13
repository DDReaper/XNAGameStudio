#region File Description
//-----------------------------------------------------------------------------
// StaffRoll.cs
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
    /// Scene component that displays Staff Roll.
    /// Changes the playback speed for sequences created in Layout.
    /// Pressing the A button during sequence playback switches to 5x playback speed.
    /// 
    /// �X�^�b�t���[����\������V�[���R���|�[�l���g�ł��B
    /// Layout�ō쐬���ꂽ�V�[�P���X�̍Đ����x��ύX���鏈�����s���Ă��܂��B
    /// �Đ�����A�{�^���������ƁA5�{�̑��x�ŃV�[�P���X���Đ����܂��B
    /// </summary>
    public class StaffRoll : SceneComponent
    {
        #region Fields
        // Staff Roll playback speed
        // 
        // �X�^�b�t���[���̍Đ��{��
        private const int SkipSpeed = 5;

        // Layout4 scene data
        // 
        // Layout4�V�[���f�[�^
        private SceneData sceneData;

        // Layout4 sequence array
        // 
        // Layout4�V�[�P���X�z��
        private SequencePlayData[] seqStaffRoll;

        // Current sequence number
        // 
        // ���݂̃V�[�P���X�ԍ�
        int seqIndex;

        // BackgroundMusic Cue
        // 
        // BackgroundMusic�̃L���[
        Cue bgm;

        // BackgroundMusic volume
        // 
        // BackgroundMusic�̃{�����[��
        float bgmVolume;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public StaffRoll(Game game)
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
            // Initializes the sequence number.
            // 
            // �V�[�P���X�̔ԍ������������܂��B
            seqIndex = 0;

            // Sets the initial volume value.
            // 
            // �{�����[���̏����l��ݒ肵�܂��B
            bgmVolume = 1.0f;

            // Sets the fade-in.
            // 
            // �t�F�[�h�C���̐ݒ���s���܂��B
            GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeIn);

            base.Initialize();
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the Layout scene data.
            // 
            // Layout�̃V�[���f�[�^��ǂݍ��݂܂��B
            string asset = "Layout/StaffRoll/staffroll_Scene";
            sceneData = Content.Load<SceneData>(asset);

            // Loads the Layout sequence data into the array.
            // 
            // 
            // Layout�̃V�[�P���X�f�[�^��z��ɓǂݍ��݂܂��B
            seqStaffRoll = new SequencePlayData[] {
                sceneData.CreatePlaySeqData("Planner01"),
                sceneData.CreatePlaySeqData("Programmer02"),
                sceneData.CreatePlaySeqData("MusicComposer03"),
                sceneData.CreatePlaySeqData("GraphicDesigner04"),
                sceneData.CreatePlaySeqData("DevelopedBy05"),
            };

            // Plays the BackgroundMusic and obtains the Cue.
            // 
            // BackgroundMusic�̍Đ����s���ACue���擾���܂��B
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.TitleBackgroundMusic);

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
            if (seqIndex < seqStaffRoll.Length)
            {
                // Updates the sequence.
                // 
                // �V�[�P���X�̍X�V�������s���܂��B
                UpdateSequence(gameTime);
            }
            else
            {
                // Performs fade-out since 
                // all sequences are completed.
                // 
                // �V�[�P���X���S�ďI�����Ă���̂ŁA
                // �t�F�[�h�A�E�g�������s���܂��B
                UpdateFadeOut();
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Updates the sequence.
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSequence(GameTime gameTime)
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Sets the update count.
            // If the A button has been pressed, performs update processing for 
            // the number of times specified in SkipSpeed.
            // 
            // �X�V�񐔂�ݒ肵�܂��B
            // A�{�^����������Ă����SkipSpeed�Ŏw�肳��Ă���񐔂���
            // �X�V�������s���܂��B
            int updateCount = (buttons.A[VirtualKeyState.Press]) ? SkipSpeed : 1;

            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V���s���܂��B
            for (int i = 0; i < updateCount; i++)
            {
                seqStaffRoll[seqIndex].Update(gameTime.ElapsedGameTime);
            }

            // If sequence playback has finished, changes to the next sequence.
            // 
            // �V�[�P���X���Đ��I�����Ă���Ύ��̃V�[�P���X�֕ύX���܂��B
            if (!seqStaffRoll[seqIndex].IsPlay || 
                buttons.B[VirtualKeyState.Push] || buttons.Back[VirtualKeyState.Push])
            {
                seqIndex++;
            }
        }


        /// <summary>
        /// Performs fade-out.
        /// 
        /// �t�F�[�h�A�E�g�������s���܂��B
        /// </summary>
        private void UpdateFadeOut()
        {
            // Sets the BackgroundMusic volume.
            // 
            // BackgroundMusic�̃{�����[����ݒ肵�܂��B
            bgmVolume -= 0.01f;
            SoundComponent.SetVolume(bgm, bgmVolume);

            // If the BackgroundMusic volume reaches 0, ends the scene
            // and registers the game over scene.
            // 
            // BackgroundMusic�{�����[����0�ɂȂ�΃V�[�����I�����A
            // �Q�[���I�[�o�[�̃V�[����o�^���܂��B
            if (bgmVolume < 0)
            {
                GameData.SceneQueue.Enqueue(new GameOver(Game));
                Dispose();
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

            // Performs drawing processing if the sequences are not all finished.
            // 
            // �܂��V�[�P���X���S�ďI�����Ă��Ȃ���Ε`�揈�����s���܂��B
            if (seqIndex < seqStaffRoll.Length)
            {
                Batch.Begin();
                seqStaffRoll[seqIndex].Draw(Batch, null);
                Batch.End();
            }

            base.Draw(gameTime);
        }
        #endregion

    }
}


