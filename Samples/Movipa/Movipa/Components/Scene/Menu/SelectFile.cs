#region File Description
//-----------------------------------------------------------------------------
// SelectFile.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SceneDataLibrary;
using Movipa.Components.Input;
using Movipa.Util;
using MovipaLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Menu item used for save file selection process. It inherits 
    /// MenuBase and expands menu compilation processing. 
    /// This class implements menu mode selection, and includes 
    /// asynchronous file searching, save file load, and 
    /// delete functions. 
    /// 
    /// �Z�[�u�t�@�C���̑I�����������郁�j���[���ڂł��B
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ���̃N���X�̓��j���[�̃��[�h�I�����������A�񓯊��̃t�@�C��������
    /// �Z�[�u�t�@�C���̓ǂݍ��݁A�폜�̋@�\������܂��B
    /// </summary>
    public class SelectFile : MenuBase
    {
        #region Private Types
        /// <summary>
        /// Processing status
        /// 
        /// �������
        /// </summary>
        private enum Phase
        {
            /// <summary>
            /// Storage selection
            /// 
            /// �X�g���[�W�I��
            /// </summary>
            StorageSelect,

            /// <summary>
            /// File search
            ///
            /// �t�@�C������
            /// </summary>
            FileSearch,

            /// <summary>
            /// Start
            ///
            /// �J�n���o
            /// </summary>
            Start,

            /// <summary>
            /// File selection
            /// 
            /// �t�@�C���I��
            /// </summary>
            Select,

            /// <summary>
            /// File selected
            /// 
            /// �t�@�C�����艉�o
            /// </summary>
            Selected,

            /// <summary>
            /// Delete start
            /// 
            /// �폜�J�n���o
            /// </summary>
            DeleteStart,

            /// <summary>
            /// Delete selection
            /// 
            /// �폜�I��
            /// </summary>
            DeleteSelect,
        }

        /// <summary>
        /// Cursor position
        /// 
        /// �J�[�\���ʒu�B
        /// </summary>
        private enum CursorPosition
        {
            /// <summary>
            /// File 1
            /// 
            /// �t�@�C��1�B
            /// </summary>
            File1,

            /// <summary>
            /// File 2
            /// 
            /// �t�@�C��2�B
            /// </summary>
            File2,

            /// <summary>
            /// File 3
            /// 
            /// �t�@�C��3�B
            /// </summary>
            File3,

            /// <summary>
            /// Count
            /// 
            /// �J�E���g�p�B
            /// </summary>
            Count
        }
        #endregion

        #region Fields
        /// <summary>
        /// Name of storage container 
        /// 
        /// �X�g���[�W�Ŏg�p����R���e�i��
        /// </summary>
        private const string ContainerName = "Movipa";

        // Cursor position
        // 
        // �J�[�\���ʒu
        private CursorPosition cursor;

        // Processing status
        // 
        // �������
        private Phase phase;

        // Game settings information 
        //
        // �Q�[���̐ݒ���
        private SaveFileLoader saveFileLoader;
        private SaveData[] saveData;

        // Delete window visibility
        // 
        // �폜�E�B���h�E�̉����
        private bool visibleDeleteWindow;

        // Sequence
        //
        // �V�[�P���X
        private SequencePlayData seqStart;
        private SequencePlayData seqPosStart;
        private SequencePlayData seqLoop;
        private SequencePlayData seqFile1;
        private SequencePlayData seqFile1Loop;
        private SequencePlayData seqFile1LoopOff;
        private SequencePlayData seqFile1FadeOut;
        private SequencePlayData seqPosFile1FadeOut;
        private SequencePlayData seqFile2;
        private SequencePlayData seqFile2Loop;
        private SequencePlayData seqFile2LoopOff;
        private SequencePlayData seqFile2FadeOut;
        private SequencePlayData seqPosFile2FadeOut;
        private SequencePlayData seqFile3;
        private SequencePlayData seqFile3Loop;
        private SequencePlayData seqFile3LoopOff;
        private SequencePlayData seqFile3FadeOut;
        private SequencePlayData seqPosFile3FadeOut;
        private SequencePlayData seqDeleteStart;
        private SequencePlayData seqDeleteLoop;
        private SequencePlayData seqPosDelete;

        // Position
        //
        // �|�W�V����
        private SequenceGroupData[] seqPosFile1;
        private SequenceGroupData[] seqPosFile2;
        private SequenceGroupData[] seqPosFile3;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SelectFile(Game game, MenuData data)
            : base(game, data)
        {
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Initializes the processing status.
            // 
            // ������Ԃ̏����ݒ�����܂��B
            phase = Phase.StorageSelect;

            // Sets the initial cursor position.
            //
            // �J�[�\���̏����ʒu��ݒ肵�܂��B
            cursor = CursorPosition.File1;

            // Sets the delete window visibility to invisible. 
            // 
            // �폜�E�B���h�E�̉���Ԃ�s���ɐݒ肵�܂��B
            visibleDeleteWindow = false;

            // Initializes the sequence.
            // 
            // �V�[�P���X�̏��������s���܂��B
            InitializeSequence();

            // Displays the storage selector.
            //
            // �X�g���[�W�Z���N�^��\�����܂��B
            InitializeStorageSelect();

            base.Initialize();
        }


        /// <summary>
        /// Initializes storage selection.
        /// 
        /// �X�g���[�W�I���̏��������s���܂��B
        /// </summary>
        private static void InitializeStorageSelect()
        {
            // Displays the storage selection screen.
            // 
            // �X�g���[�W�I���̉�ʂ�\�����܂��B
            GameData.Storage.ShowStorageDeviceSelector(ContainerName, PlayerIndex.One);
        }


        /// <summary>
        /// Initializes file searching.
        /// 
        /// �t�@�C�������̏��������s���܂��B
        /// </summary>
        private void InitializeFileSearch()
        {
            if (saveFileLoader != null)
                return;

            // Creates the file loader.
            // 
            // �t�@�C�����[�_�[���쐬���܂��B
            saveFileLoader = new SaveFileLoader(Game, 3);

            // Starts the file search.
            //
            // �t�@�C���������J�n���܂��B
            saveFileLoader.Run();
        }


        /// <summary>
        /// Initializes the navigate.
        /// 
        /// �i�r�Q�[�g�̏����������܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            InitializeNavigate_Select();
        }


        /// <summary>
        /// Initializes the sequence.
        /// 
        /// �V�[�P���X�̏��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            // Loads the sequence.
            // 
            // �V�[�P���X�̓ǂݍ��݂��s���܂��B
            seqStart = Data.sceneData.CreatePlaySeqData("FileStart");
            seqPosStart = Data.sceneData.CreatePlaySeqData("PosFileStart");
            seqLoop = Data.sceneData.CreatePlaySeqData("FileLoop");
            seqFile1 = Data.sceneData.CreatePlaySeqData("File1");
            seqFile1Loop = Data.sceneData.CreatePlaySeqData("File1Loop");
            seqFile1LoopOff = Data.sceneData.CreatePlaySeqData("File1LoopOff");
            seqFile1FadeOut = Data.sceneData.CreatePlaySeqData("File1FadeOut");
            seqPosFile1FadeOut = Data.sceneData.CreatePlaySeqData("PosFile1FadeOut");
            seqFile2 = Data.sceneData.CreatePlaySeqData("File2");
            seqFile2Loop = Data.sceneData.CreatePlaySeqData("File2Loop");
            seqFile2LoopOff = Data.sceneData.CreatePlaySeqData("File2LoopOff");
            seqFile2FadeOut = Data.sceneData.CreatePlaySeqData("File2FadeOut");
            seqPosFile2FadeOut = Data.sceneData.CreatePlaySeqData("PosFile2FadeOut");
            seqFile3 = Data.sceneData.CreatePlaySeqData("File3");
            seqFile3Loop = Data.sceneData.CreatePlaySeqData("File3Loop");
            seqFile3LoopOff = Data.sceneData.CreatePlaySeqData("File3LoopOff");
            seqFile3FadeOut = Data.sceneData.CreatePlaySeqData("File3FadeOut");
            seqPosFile3FadeOut = Data.sceneData.CreatePlaySeqData("PosFile3FadeOut");

            seqDeleteStart = Data.sceneData.CreatePlaySeqData("DelStart");
            seqDeleteLoop = Data.sceneData.CreatePlaySeqData("DelLoop");
            seqPosDelete = Data.sceneData.CreatePlaySeqData("PosDelStart");


            // Obtains the position from the sequence.
            //
            // �V�[�P���X����|�W�V�������擾���܂��B
            seqPosFile1 = new SequenceGroupData[] {
                seqPosStart.SequenceData.SequenceGroupList[0],
                seqPosStart.SequenceData.SequenceGroupList[1],
                seqPosStart.SequenceData.SequenceGroupList[2],
                seqPosStart.SequenceData.SequenceGroupList[3],
                seqPosStart.SequenceData.SequenceGroupList[4],
            };
            seqPosFile2 = new SequenceGroupData[] {
                seqPosStart.SequenceData.SequenceGroupList[5],
                seqPosStart.SequenceData.SequenceGroupList[6],
                seqPosStart.SequenceData.SequenceGroupList[7],
                seqPosStart.SequenceData.SequenceGroupList[8],
                seqPosStart.SequenceData.SequenceGroupList[9],
            };
            seqPosFile3 = new SequenceGroupData[] {
                seqPosStart.SequenceData.SequenceGroupList[10],
                seqPosStart.SequenceData.SequenceGroupList[11],
                seqPosStart.SequenceData.SequenceGroupList[12],
                seqPosStart.SequenceData.SequenceGroupList[13],
                seqPosStart.SequenceData.SequenceGroupList[14],
            };
        }


        /// <summary>
        /// Sets the navigate for file selection.
        /// 
        /// �t�@�C���I�����̃i�r�Q�[�g��ݒ肵�܂��B
        /// </summary>
        private void InitializeNavigate_Select()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("X_Delete")));
            Navigate.Add(new NavigateData(AppSettings("B_Cancel")));
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
        }


        /// <summary>
        /// Sets the navigate for file deletion.
        ///
        /// �t�@�C���폜���̃i�r�Q�[�g��ݒ肵�܂��B
        /// </summary>
        private void InitializeNavigate_Delete()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("X_Delete")));
            Navigate.Add(new NavigateData(AppSettings("B_Cancel")));
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        ///
        /// �X�V�������s���܂��B
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>
        /// Next menu
        /// 
        /// ���̃��j���[
        /// </returns>
        public override MenuBase UpdateMain(GameTime gameTime)
        {
            // Updates the sequence
            // 
            // �V�[�P���X�̍X�V����
            UpdateSequence(gameTime);

            if (phase == Phase.StorageSelect)
            {
                // Performs update at storage selection.
                // 
                // �X�g���[�W�I�����̍X�V�������s���܂��B
                return UpdateStorageSelect();
            }
            else if (phase == Phase.FileSearch)
            {
                // Performs update at file search.
                //
                // �t�@�C���������̍X�V�������s���܂��B
                return UpdateFileSearch();
            }
            else if (phase == Phase.Start)
            {
                // Sets to selection after start animation finishes.
                //
                // �J�n�A�j���[�V�������I��������I�������֐ݒ肵�܂��B
                if (!seqStart.SequenceData.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                // Updates at file selection.
                //
                // �t�@�C���I�����̍X�V�������s���܂��B
                return UpdateSelect();
            }
            else if (phase == Phase.Selected)
            {
                // Performs update when file is selected.
                //
                // �t�@�C�����I�����ꂽ���̍X�V�������s���܂��B
                return UpdateSelected();
            }
            else if (phase == Phase.DeleteStart)
            {
                // Sets to delete selection when delete
                // window start animation finishes.
                //
                // �폜�E�B���h�E�̊J�n�A�j���[�V�������I��������
                // �폜�I���̏����֐ݒ肵�܂��B
                if (!seqDeleteStart.IsPlay && !seqPosDelete.IsPlay)
                {
                    phase = Phase.DeleteSelect;
                }
            }
            else if (phase == Phase.DeleteSelect)
            {
                // Performs update at file deletion. 
                //
                // �t�@�C���폜���̍X�V�������s���܂��B
                UpdateDeleteSelect();
            }

            return null;
        }


        /// <summary>
        /// Performs update at storage selection.
        /// 
        /// �X�g���[�W�I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateStorageSelect()
        {
            // Begins file search after storage has been selected.
            // 
            // �X�g���[�W�I��������������t�@�C���������J�n���܂��B
            if (!GameData.Storage.IsVisible && GameData.Storage.IsConnected)
            {
                phase = Phase.FileSearch;
            }

            return null;
        }


        /// <summary>
        /// Performs update at file search.
        /// 
        /// �t�@�C���������̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateFileSearch()
        {
            // Starts file search thread.
            // 
            // �t�@�C�������̃X���b�h���J�n���܂��B
            if (saveFileLoader == null)
            {
                InitializeFileSearch();
            }

            // Starts file selection process after file search has finished.
            // 
            // �t�@�C���������I��������A�t�@�C���I���������J�n���܂��B
            if (saveFileLoader.GetGameSettings() != null)
            {
                saveData = saveFileLoader.GetGameSettings();
                saveFileLoader = null;

                seqStart.Replay();
                seqPosStart.Replay();

                phase = Phase.Start;
            }

            return null;
        }

        /// <summary>
        /// Performs update at file selection.
        /// 
        /// �t�@�C���I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPush(dPad.Up, leftStick.Up))
            {
                // Performs Up key processing.
                // 
                // ��L�[�����������̏������s���܂��B
                InputUpKey();
            }
            else if (InputState.IsPush(dPad.Down, leftStick.Down))
            {
                // Performs Down key processing.
                // 
                // ���L�[�����������̏������s���܂��B
                InputDownKey();
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                // Performs Enter key processing.
                // 
                // ����L�[�����������̏������s���܂��B
                InputSelectKey();
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Switches to mode selection due to cancel.
                //
                // �L�����Z�����ꂽ�̂ŁA���[�h�I���ɑJ�ڂ��܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
                return CreateMenu(Game, MenuType.SelectMode, Data);
            }
            else if (buttons.X[VirtualKeyState.Push])
            {
                // Performs Delete button processing.
                // 
                // �폜�{�^�������������̏������s���܂��B
                return InputDeleteKey();
            }

            return null;
        }


        /// <summary>
        /// Performs update when file is selected.
        /// 
        /// �t�@�C�����I�����ꂽ���̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelected()
        {
            // Processing is not performed until the selected animation has finished.
            // 
            // �I���A�j���[�V�������I������܂ŏ������s���܂���B
            if (!seqFile1FadeOut.IsPlay ||
                !seqFile2FadeOut.IsPlay ||
                !seqFile3FadeOut.IsPlay)
            {
                // Loads stage information based on number of stages recorded in file. 
                // 
                // �t�@�C���ɋL�^���ꂽ�X�e�[�W�������ɁA
                // �X�e�[�W����ǂݍ��݂܂��B
                int stage = GameData.SaveData.Stage;
                StageSetting stageSetting = GameData.StageCollection[stage];

                // Registers game window scene.
                // 
                // �Q�[����ʂ̃V�[����o�^���܂��B
                GameData.SceneQueue.Enqueue(
                    new Puzzle.PuzzleComponent(Game, stageSetting));

                // Specifies fade-out settings.
                //
                // �t�F�[�h�A�E�g�̐ݒ���s���܂��B
                GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
            }
            
            return null;
        }


        /// <summary>
        /// Performs Up key processing.
        /// 
        /// ��L�[�����������̏������s���܂��B
        /// </summary>
        private void InputUpKey()
        {
            // Moves the cursor position.
            // 
            // �J�[�\���̈ʒu���ړ����܂��B
            cursor = CursorPrev();

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            ReplaySequences(cursor);

            // Plays SoundEffect.
            // 
            // SoundEffect���Đ�
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Performs Down key processing.
        /// 
        /// ���L�[�����������̏������s���܂��B
        /// </summary>
        private void InputDownKey()
        {
            // Moves the cursor position.
            // 
            // �J�[�\���̈ʒu���ړ����܂��B
            cursor = CursorNext();

            // Replays the sequence.
            //
            // �V�[�P���X�����v���C���܂��B
            ReplaySequences(cursor);

            // Plays SoundEffect.
            // 
            // SoundEffect���Đ�
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Performs Enter key processing.
        /// 
        /// ����L�[�����������̏������s���܂��B
        /// </summary>
        private void InputSelectKey()
        {
            // Obtains settings data.
            // 
            // �ݒ�f�[�^���擾���܂��B
            SaveData gameSettings = saveData[(int)cursor];

            // Creates a new file if no data exists.
            // 
            // �f�[�^�������ꍇ�̓t�@�C����V�K�쐬���܂��B
            if (gameSettings == null)
            {
                // Sets full path filename.
                // 
                // �t�@�C�������t���p�X�Őݒ肵�܂��B
                string filename = string.Format("SaveData{0}.xml", cursor);
                string filePath = GameData.Storage.GetStoragePath(filename);

                // Creates a new file.
                // 
                // �t�@�C����V�K�쐬���܂��B
                gameSettings = CreateSaveData(filePath);
            }

            // Calculates the play count. 
            // 
            // �v���C�J�E���g�����Z���܂��B
            gameSettings.PlayCount++;

            // Sets the save data to be used.
            // 
            // �g�p����Z�[�u�f�[�^��ݒ肵�܂��B
            GameData.SaveData = gameSettings;

            // Replays the sequence.
            //
            // �V�[�P���X�����v���C���܂��B
            seqFile1FadeOut.Replay();
            seqPosFile1FadeOut.Replay();
            seqFile2FadeOut.Replay();
            seqPosFile2FadeOut.Replay();
            seqFile3FadeOut.Replay();
            seqPosFile3FadeOut.Replay();

            // Plays the chosen SoundEffect.
            //
            // �����SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

            // Sets the processing status to Selected.
            //
            // ������Ԃ�I���ς݂ɐݒ肵�܂��B
            phase = Phase.Selected;
        }

        /// <summary>
        /// Performs update processing at file deletion.
        /// 
        /// �t�@�C���폜���̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateDeleteSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (buttons.X[VirtualKeyState.Push])
            {
                // Deletes the file when X button is pressed.
                // 
                // X�{�^���������ꂽ��A�t�@�C���̍폜���������s���܂��B
                SaveData gameSettings = saveData[(int)cursor];

                // Deletes the file and (where necessary) plays the SoundEffect.  
                // 
                // �t�@�C���폜�����s���A���ʂɉ�����SoundEffect���Đ����܂��B
                DeleteSaveData(gameSettings.FileName);
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

                // Sets the processing status to file search.
                // 
                // ������Ԃ��t�@�C�������ɐݒ肵�܂��B
                phase = Phase.FileSearch;
                visibleDeleteWindow = false;
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Deletes cancelled; cancel tone sounds.
                //
                // �폜�������L�����Z�����ꂽ�̂ŁA�L�����Z�������Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);

                // Sets processing status to file search.
                // 
                // ������Ԃ��t�@�C�������ɐݒ肵�܂��B
                phase = Phase.FileSearch;
                visibleDeleteWindow = false;
            }

            return null;
        }


        /// <summary>
        /// Performs delete button processing.
        /// 
        /// �폜�{�^�����������Ƃ��̏������s���܂��B
        /// </summary>
        private MenuBase InputDeleteKey()
        {
            // Obtains settings data.
            // 
            // �ݒ�f�[�^���擾���܂��B
            SaveData gameSettings = saveData[(int)cursor];

            // Processing is not performed if there is no data.
            // 
            // �f�[�^��������Ώ������s���܂���B
            if (gameSettings == null)
            {
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
                return null;
            }

            // Replays the delete window sequence.
            // 
            // �폜�E�B���h�E�̃V�[�P���X�����v���C���܂��B
            seqDeleteStart.Replay();
            seqPosDelete.Replay();

            // Sets the delete window visibility to visible.
            // 
            // �폜�E�B���h�E������Ԃɐݒ肵�܂��B
            visibleDeleteWindow = true;

            // Sets the navigate button in delete item.
            // 
            // �i�r�Q�[�g�{�^�����폜�̍��ڂɐݒ肵�܂��B
            InitializeNavigate_Delete();

            // Sets processing status to delete selection.
            //
            // ������Ԃ��폜�I���ɐݒ肵�܂��B
            phase = Phase.DeleteStart;

            // Plays the chosen SoundEffect.
            // 
            // �����SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

            return null;
        }


        /// <summary>
        /// Updates the sequence.
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSequence(GameTime gameTime)
        {
            TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;

            seqStart.Update(elapsedGameTime);
            seqPosStart.Update(elapsedGameTime);
            seqLoop.Update(elapsedGameTime);
            seqFile1.Update(elapsedGameTime);
            seqFile1Loop.Update(elapsedGameTime);
            seqFile1LoopOff.Update(elapsedGameTime);
            seqFile1FadeOut.Update(elapsedGameTime);
            seqPosFile1FadeOut.Update(elapsedGameTime);
            seqFile2.Update(elapsedGameTime);
            seqFile2Loop.Update(elapsedGameTime);
            seqFile2LoopOff.Update(elapsedGameTime);
            seqFile2FadeOut.Update(elapsedGameTime);
            seqPosFile2FadeOut.Update(elapsedGameTime);
            seqFile3.Update(elapsedGameTime);
            seqFile3Loop.Update(elapsedGameTime);
            seqFile3LoopOff.Update(elapsedGameTime);
            seqFile3FadeOut.Update(elapsedGameTime);
            seqPosFile3FadeOut.Update(elapsedGameTime);
            seqDeleteStart.Update(elapsedGameTime);
            seqDeleteLoop.Update(elapsedGameTime);
            seqPosDelete.Update(elapsedGameTime);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs render processing.
        ///
        /// �`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin();

            if (phase == Phase.Start)
            {
                // Draws the start animation.
                // 
                // �J�n�A�j���[�V�����̕`����s���܂��B
                DrawPhaseStart(batch);
            }
            else if (phase == Phase.Selected)
            {
                // Draws the selected animation.
                // 
                // �I����̃A�j���[�V������`�悵�܂��B
                DrawPhaseSelected(batch);
            }
            else if (phase > Phase.FileSearch)
            {
                // Performs rendering except during file search 
                // and the above process branch.  Also executed 
                // during file deletion.
                //
                // �t�@�C���������ƁA��L�̏�������ȊO�ł̕`����s���܂��B
                // �t�@�C���폜�������̏����͎��s����܂��B
                DrawPhaseSelect(gameTime, batch);
            }

            // Draws the delete window.
            // 
            // �폜�E�B���h�E��`�悵�܂��B
            DrawDeleteWindow(gameTime, batch);

            batch.End();
        }


        /// <summary>
        /// Draws the start animation.
        /// 
        /// �J�n�A�j���[�V�����̕`����s���܂��B
        /// </summary>
        private void DrawPhaseStart(SpriteBatch batch)
        {
            seqStart.Draw(batch, null);
            //SelectFile_DrawString(gameTime, batch);
            DrawSequenceString(batch, seqPosFile1, 0);
            DrawSequenceString(batch, seqPosFile2, 1);
            DrawSequenceString(batch, seqPosFile3, 2);
        }


        /// <summary>
        /// Performs rendering during file selection.
        /// 
        /// �t�@�C���I�𒆂̕`����s���܂��B
        /// </summary>
        private void DrawPhaseSelect(GameTime gameTime, SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);

            seqFile1.Draw(batch, null);
            seqFile2.Draw(batch, null);
            seqFile3.Draw(batch, null);

            // Determines file selection status and performs rendering accordingly.
            // 
            // �t�@�C���I�𒆂̏�Ԃ𔻕ʂ��ĕ`�悵�܂��B
            SequencePlayData seqPlayData;
            CursorPosition file;

            file = CursorPosition.File1;
            seqPlayData = (cursor == file) ? seqFile1Loop : seqFile1LoopOff;
            seqPlayData.Draw(batch, null);

            file = CursorPosition.File2;
            seqPlayData = (cursor == file) ? seqFile2Loop : seqFile2LoopOff;
            seqPlayData.Draw(batch, null);

            file = CursorPosition.File3;
            seqPlayData = (cursor == file) ? seqFile3Loop : seqFile3LoopOff;
            seqPlayData.Draw(batch, null);

            // Draws file text strings.
            //
            // �t�@�C���̓��e�̕������`�悵�܂��B
            DrawSequenceString(batch, seqPosFile1, 0);
            DrawSequenceString(batch, seqPosFile2, 1);
            DrawSequenceString(batch, seqPosFile3, 2);

            // Draws the navigate button when the delete window is not displayed.
            // 
            // �폜�E�B���h�E����\���̏ꍇ�̓i�r�Q�[�g�{�^����`�悵�܂��B
            if (!visibleDeleteWindow)
            {
                DrawNavigate(gameTime, batch, false);
            }
        }


        /// <summary>
        /// Draws the selected animation.
        /// 
        /// �I����̃A�j���[�V������`�悵�܂��B
        /// </summary>
        private void DrawPhaseSelected(SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);

            seqFile1FadeOut.Draw(batch, null);
            seqFile2FadeOut.Draw(batch, null);
            seqFile3FadeOut.Draw(batch, null);

            // Draws the window text string.
            // 
            // �E�B���h�E�̕������`�悵�܂��B
            SequencePlayData[] sequences = new SequencePlayData[] {
                seqPosFile1FadeOut,
                seqPosFile2FadeOut,
                seqPosFile3FadeOut
            };

            for (int i = 0; i < sequences.Length; i++)
            {
                SequencePlayData sequence = sequences[i];
                
                SequenceGroupData[] sequenceGroups;
                sequenceGroups = sequence.SequenceData.SequenceGroupList.ToArray();
                DrawSequenceString(batch, sequenceGroups, i);
            }
        }


        /// <summary>
        /// Draws the delete window.
        /// 
        /// �폜�E�B���h�E��`�悵�܂��B
        /// </summary>
        private void DrawDeleteWindow(GameTime gameTime, SpriteBatch batch)
        {
            // Processing is not performed when the delete window is invisible.
            // 
            // �폜�E�B���h�E�̉���Ԃ��s���̏ꍇ�͏��������܂���B
            if (!visibleDeleteWindow)
            {
                return;
            }

            if (phase == Phase.DeleteStart)
            {
                // Draws the delete window start animation. 
                // 
                // �폜�E�B���h�E�̊J�n�A�j���[�V������`�悵�܂��B
                seqDeleteStart.Draw(batch, null);

                SequenceGroupData[] sequenceGroups;
                sequenceGroups = seqPosDelete.SequenceData.SequenceGroupList.ToArray();
                DrawSequenceString(batch, sequenceGroups, (int)cursor);
            }
            else if (phase == Phase.DeleteSelect)
            {
                seqDeleteLoop.Draw(batch, null);

                SequenceGroupData[] sequenceGroups;
                sequenceGroups = seqPosDelete.SequenceData.SequenceGroupList.ToArray();
                DrawSequenceString(batch, sequenceGroups, (int)cursor);

                // Draws the navigate button.
                // 
                // �i�r�Q�[�g�{�^���̕`����s���܂��B
                DrawNavigate(gameTime, batch, false);
            }
        }


        /// <summary>
        /// Draws the window text string.
        /// 
        /// �E�B���h�E�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch, 
            SequenceGroupData[] seqBodyDataList, int fileId)
        {
            // Processing is not performed if there is no settings data.
            // 
            // �ݒ�f�[�^�������ꍇ�͏������s���܂���B
            if (saveData == null)
            {
                return;
            }

            for (int i = 0; i < seqBodyDataList.Length; i++)
            {
                SaveData gameSettings = saveData[fileId];
                SequenceGroupData seqBodyData = seqBodyDataList[i];
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;
                if (seqPartsData == null)
                {
                    continue;
                }

                List<PatternObjectData> list = seqPartsData.PatternObjectList;
                foreach (PatternObjectData patPartsData in list)
                {
                    DrawData info = patPartsData.InterpolationDrawData;
                    int posType = i % 5;

                    if (posType == 0)
                    {
                        // Draws the file header text string.
                        // 
                        // �t�@�C���w�b�_�̕������`�悵�܂��B
                        if (!DrawFileHeader(batch, info, gameSettings, fileId))
                        {
                            // Processing omitted for new items with no data.
                            // 
                            // �f�[�^�������A�V�K�̍��ڂȂ�Ώ����𔲂��܂��B
                            return;
                        }
                    }
                    else if (posType == 1)
                    {
                        // Draws the Best Time header.
                        // 
                        // �x�X�g�^�C���̃w�b�_��`�悵�܂��B
                        DrawBestTimeHeader(batch, info);
                    }
                    else if (posType == 2)
                    {
                        // Draws the Best Score header.
                        //
                        // �x�X�g�X�R�A�̃w�b�_��`�悵�܂��B
                        DrawBestScoreHeader(batch, info);
                    }
                    else if (posType == 3)
                    {
                        // Draws the Best Time value.
                        //
                        // �x�X�g�^�C���̒l��`�悵�܂��B
                        DrawBestTimeValue(batch, info, gameSettings);
                    }
                    else if (posType == 4)
                    {
                        // Draws the Best Score value.
                        //
                        // �x�X�g�X�R�A�̒l��`�悵�܂��B
                        DrawBestScoreValue(batch, info, gameSettings);
                    }
                }
            }

        }


        /// <summary>
        /// Draws the file header text string.
        /// 
        /// �t�@�C���w�b�_�̕������`�悵�܂��B
        /// </summary>
        private bool DrawFileHeader(SpriteBatch batch, DrawData info, 
            SaveData gameSettings, int fileId)
        {
            SpriteFont font = LargeFont;
            Vector2 position = new Vector2(info.Position.X, info.Position.Y);
            string text = GetFileHeaderString(gameSettings, fileId);
            position -= font.MeasureString(text) * 0.5f;
            batch.DrawString(font, text, position, info.Color);

            return (gameSettings != null);
        }


        /// <summary>
        /// Draws the Best Time header.
        /// 
        /// �x�X�g�^�C���̃w�b�_��`�悵�܂��B
        /// </summary>
        private void DrawBestTimeHeader(SpriteBatch batch, DrawData info)
        {
            SpriteFont font = MediumFont;
            Vector2 position = new Vector2(info.Position.X, info.Position.Y);
            string text = "Best Time";
            batch.DrawString(font, text, position, info.Color);
        }


        /// <summary>
        /// Draws the Best Time value.
        ///
        /// �x�X�g�^�C���̒l��`�悵�܂��B
        /// </summary>
        private void DrawBestTimeValue(
            SpriteBatch batch, DrawData info, SaveData gameSettings)
        {
            SpriteFont font = MediumFont;
            Vector2 position = new Vector2(info.Position.X, info.Position.Y);
            string timeText = gameSettings.BestTime == TimeSpan.Zero ? "None" : 
                gameSettings.BestTime.ToString().Substring(0, 8);
            position.X -= font.MeasureString(timeText).X;
            batch.DrawString(font, timeText, position, info.Color);
        }


        /// <summary>
        /// Draws the Best Score header.
        ///
        /// �x�X�g�X�R�A�̃w�b�_��`�悵�܂��B
        /// </summary>
        private void DrawBestScoreHeader(SpriteBatch batch, DrawData info)
        {
            SpriteFont font = MediumFont;
            Vector2 position = new Vector2(info.Position.X, info.Position.Y);
            string value = "Best Score";
            batch.DrawString(font, value, position, info.Color);
        }


        /// <summary>
        /// Draws the Best Score value.
        ///
        /// �x�X�g�X�R�A�̒l��`�悵�܂��B
        /// </summary>
        private void DrawBestScoreValue(SpriteBatch batch, DrawData info, 
            SaveData gameSettings)
        {
            SpriteFont font = MediumFont;
            Vector2 position = new Vector2(info.Position.X, info.Position.Y);
            string value =
                string.Format("{0:000000}", gameSettings.BestScore);
            position.X -= font.MeasureString(value).X;
            batch.DrawString(font, value, position, info.Color);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates a new save file and returns the instance.
        /// 
        /// �Z�[�u�t�@�C����V�K�쐬���A�C���X�^���X��Ԃ��܂��B
        /// </summary>
        private static SaveData CreateSaveData(string filename)
        {
            // Sets the full path for the file.
            // 
            // �t�@�C���̃t���p�X��ݒ肵�܂��B
            string filePath = GameData.Storage.GetStoragePath(filename);

            // Creates the save file instance and set the file name.
            //
            // �Z�[�u�t�@�C���̃C���X�^���X���쐬���A�t�@�C������ݒ肵�܂��B
            SaveData setting = new SaveData();
            setting.FileName = filename;

            // Serializes and saves the save data.
            // 
            // �Z�[�u�f�[�^���V���A���C�Y���ĕۑ����܂��B
            SettingsSerializer.SaveSaveData(filePath, setting);

            return setting;
        }


        /// <summary>
        /// Deletes the file.
        /// 
        /// �t�@�C�����폜���܂��B
        /// </summary>
        private static void DeleteSaveData(string filename)
        {
            // Checks for the file; if the file is not found, 
            // processing is not performed.
            // 
            // �t�@�C���̗L�����`�F�b�N���A������Ώ������s���܂���B
            if (!File.Exists(filename))
            {
                return;
            }

            // If the file to be deleted can be found, the processing is performed.
            //
            // �폜�Ώۂ̃t�@�C����������΍폜�������s���܂��B
            File.Delete(filename);
        }


        /// <summary>
        /// Obtains the previous cursor position.
        /// 
        /// �J�[�\���̑O�̈ʒu���擾���܂��B
        /// </summary>
        private CursorPosition CursorPrev()
        {
            int count = (int)CursorPosition.Count;
            return (CursorPosition)(((int)cursor + (count - 1)) % count);
        }

        /// <summary>
        /// Obtains the next cursor position.
        /// 
        /// �J�[�\���̎��̈ʒu���擾���܂��B
        /// </summary>
        private CursorPosition CursorNext()
        {
            int count = (int)CursorPosition.Count;
            return (CursorPosition)(((int)cursor + 1) % count);
        }


        /// <summary>
        /// Replays the file item sequence.
        ///
        /// �t�@�C�����ڂ̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplaySequences(CursorPosition cursorPosition)
        {
            ReplaySequences((int)cursorPosition);
        }


        /// <summary>
        /// Replays the file item sequence.
        /// 
        /// �t�@�C�����ڂ̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplaySequences(int id)
        {
            // Sets the sequence array to be replayed.
            // 
            // ���v���C����V�[�P���X�̔z����Z�b�g
            SequencePlayData[] seqFiles = { seqFile1, seqFile2, seqFile3 };

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C����
            seqFiles[id].Replay();
        }


        /// <summary>
        /// Obtains the file header text string.
        /// 
        /// �t�@�C���w�b�_�̕�������擾���܂��B
        /// </summary>
        private static string GetFileHeaderString(SaveData gameSettings, int fileId)
        {
            string text = String.Empty;
            if (gameSettings == null)
            {
                text = "New";
            }
            else
            {
                string format = "File[{0}] Stage {1}";
                int stage = gameSettings.Stage + 1;
                text = string.Format(format, fileId, stage);
            }

            return text;
        }
        #endregion
    }
}
