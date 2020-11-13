#region File Description
//-----------------------------------------------------------------------------
// NormalResult.cs
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
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Result
{
    /// <summary>
    /// Scene component displaying the Normal Mode results.
    /// It inherits ResultBase and loads the content to be used,
    /// then performs main update processing.
    /// Additional scores are obtained from completion results for 
    /// the specified stage and the count increments by one points. It also 
    /// implements a fast forward (100 x speed) function which is accessed 
    /// by pressing the A button. 
    /// Save data is recorded when the scene ends. 
    /// 
    /// �m�[�}�����[�h�̃��U���g��\������V�[���R���|�[�l���g�ł��B
    /// ResultBase���p�����A�g�p����R���e���g�̓ǂݍ��݂ƁA
    /// ���C���̍X�V�����̓��e���킯�Ă��܂��B
    /// �w�肳�ꂽ�X�e�[�W�̃N���A���ʂ���ǉ��X�R�A���擾���A
    /// 1�_���J�E���g���Ă����������s���Ă��܂����AA�{�^����������
    /// 100�{�̑��x�ő����肷�鏈�����������Ă��܂��B
    /// ���̃V�[���̏I�����ɃZ�[�u�f�[�^���L�^���Ă��܂��B
    /// </summary>
    public class NormalResult : ResultBase
    {
        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public NormalResult(Game game, StageResult stageResult)
            : base(game, stageResult)
        {
        }


        /// <summary>
        /// Initializes the Navigate button.
        ///
        /// �i�r�Q�[�g�{�^���̏������������s���܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("A_Next"), true));

            base.InitializeNavigate();
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            string asset = "Layout/Result/result_Scene";
            sceneData = Content.Load<SceneData>(asset);
            seqStart = sceneData.CreatePlaySeqData("ResultNormalStart");
            seqPosition = sceneData.CreatePlaySeqData("PosNormalStart");

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs main update processing.
        ///
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        protected override void UpdateMain(GameTime gameTime)
        {
            if (phase == Phase.Start)
            {
                if (!seqStart.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else
            {
                UpdateScore();
            }

            base.UpdateMain(gameTime);
        }


        /// <summary>
        /// Calculates the score.
        /// 
        /// �X�R�A�̉��Z�������s���܂��B
        /// </summary>
        private void UpdateScore()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Sets the number of score calculations.
            // The count is normally 1, but if the A button is 
            // pressed it becomes 100.
            // 
            // �X�R�A�̉��Z�񐔂�ݒ肵�܂��B
            // �ʏ��1��̃J�E���g�ł����AA�{�^����������Ă����
            // 100��J�E���g����悤�ɂ��܂��B
            int loopCount = (buttons.A[VirtualKeyState.Press]) ? 100 : 1;

            // Calculates the score.
            // 
            // �X�R�A�̉��Z�������s���܂��B
            if (CalcScore(loopCount) == true)
            {
                // Plays the SoundEffect while the score calculation continues.
                // 
                // �X�R�A�̉��Z�������p�����Ă���ꍇ��SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.ResultScore);
            }
            else
            {
                // Score calculation has finished; Navigate button
                // is displayed.
                // 
                // �X�R�A�̉��Z�������I�������̂ŁA�i�r�Q�[�g�{�^����
                // �\������悤�ɂ��܂��B
                drawNavigate = true;

                // Performs fade-out if the A button is pressed.
                // 
                // A�{�^���������ꂽ��t�F�[�h�A�E�g���s���܂��B
                if (buttons.A[VirtualKeyState.Push])
                {
                    // Sets the next scene.
                    // 
                    // ���̃V�[����ݒ肵�܂��B
                    SetNextScene();

                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                    GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Calculates the score.
        /// 
        /// �X�R�A�̌v�Z�����܂��B
        /// </summary>
        private bool CalcScore(int loopCount)
        {
            bool addFlag = false;

            // Performs the designated number of loop repetitions.
            // 
            // �w�肳�ꂽ�񐔕����[�v���܂��B
            for (int i = 0; i < loopCount; i++)
            {
                if (CalcScore())
                {
                    addFlag = true;
                }
                else
                {
                    // Processing is suspended if the score is not updated.
                    // 
                    // �X�R�A�̍X�V���s���Ȃ��ꍇ�͏����𒆒f���܂��B
                    break;
                }
            }

            return addFlag;
        }


        /// <summary>
        /// Calculates the score.
        /// 
        /// �X�R�A�̌v�Z�����܂��B
        /// </summary>
        private bool CalcScore()
        {
            bool addFlag = false;

            if (result.SingleScore > 0)
            {
                addFlag = true;

                result.SingleScore--;
                GameData.SaveData.Score++;
            }
            else if (result.DoubleScore > 0)
            {
                addFlag = true;

                result.DoubleScore--;
                GameData.SaveData.Score++;
            }
            else if (result.HintScore > 0)
            {
                addFlag = true;

                result.HintScore--;
                GameData.SaveData.Score++;
            }

            return addFlag;
        }


        /// <summary>
        /// Checks if all stages have been completed, and readies the next scene.
        /// 
        /// �S�ẴX�e�[�W���N���A�������ǂ������f���A���̃V�[����p�ӂ��܂��B
        /// </summary>
        private void SetNextScene()
        {
            // Obtains the stage settings.
            // 
            // �X�e�[�W�̐ݒ���擾���܂��B
            int stage = GameData.SaveData.Stage;

            // Sets the high score.
            // 
            // �n�C�X�R�A��ݒ肵�܂��B
            if (GameData.SaveData.BestScore < GameData.SaveData.Score)
            {
                GameData.SaveData.BestScore = GameData.SaveData.Score;
            }
            
            // Checks if the next stage exists.
            // 
            // ���̃X�e�[�W�����݂��邩�`�F�b�N���܂��B
            if ((stage + 1) >= GameData.StageCollection.Count)
            {
                // Next stage does not exist; all stages have been completed.
                // 
                // ���̃X�e�[�W�����݂��Ȃ��̂ŁA�S�ẴX�e�[�W���N���A���܂����B

                // Sets the best time.
                // 
                // �x�X�g�^�C����ݒ肵�܂��B
                if (GameData.SaveData.BestTime == TimeSpan.Zero ||
                    GameData.SaveData.BestTime > GameData.SaveData.TotalPlayTime)
                {
                    GameData.SaveData.BestTime = GameData.SaveData.TotalPlayTime;
                }
                
                // Initializes the stages.
                // 
                // �X�e�[�W�����������܂��B
                GameData.SaveData.Stage = 0;

                // Initializes the score.
                // 
                // �X�R�A�����������܂��B
                GameData.SaveData.Score = 0;

                // Registers the Staff Roll scenes.
                // 
                // �X�^�b�t���[���̃V�[����o�^���܂��B
                GameData.SceneQueue.Enqueue(new StaffRoll(Game));
            }
            else
            {
                // If the next stage exists, it is set.
                // 
                // �X�e�[�W�����݂���Ȃ玟�̃X�e�[�W��ݒ肵�܂��B
                stage++;
                GameData.SaveData.Stage = stage;

                // Obtains setting information for the next stage.
                // 
                // ���̃X�e�[�W�̐ݒ�����擾���܂��B
                StageSetting stageSetting = GameData.StageCollection[stage];

                // Registers the main game scenes.
                // 
                // ���C���Q�[���̃V�[����o�^���܂��B
                GameData.SceneQueue.Enqueue(
                    new Puzzle.PuzzleComponent(Game, stageSetting));
            }

            // Saves the Save Data.
            // 
            // �Z�[�u�f�[�^��ۑ����܂��B
            string filename = GameData.SaveData.FileName;
            string filePath = GameData.Storage.GetStoragePath(filename);
            SettingsSerializer.SaveSaveData(filePath, GameData.SaveData);
        }


        /// <summary>
        /// Returns the text string to display in the sequence.
        /// 
        /// �V�[�P���X�ɕ\�����镶�����Ԃ��܂��B
        /// </summary>
        protected override string GetSequenceString(int id)
        {
            switch (id)
            {
                case 0:
                    return string.Format("{0:000000}", result.SingleScore);
                case 1:
                    return string.Format("{0:000000}", result.DoubleScore);
                case 2:
                    return string.Format("{0:000000}", result.HintScore);
                case 3:
                    return string.Format("{0:000000}", GameData.SaveData.Score);
            }

            return String.Empty;
        }
        #endregion
    }
}


