#region File Description
//-----------------------------------------------------------------------------
// SelectStyle.cs
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
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Menu item used for style selection.
    /// It inherits MenuBase and expands menu compilation processing.
    /// This class implements menu style selection and rotation selection, and
    /// provides sequence control and cursor model movement.
    /// 
    /// �X�^�C���I�����������郁�j���[���ڂł��B
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ���̃N���X�̓��j���[�̃X�^�C���I���ƁA��]�̗L���̑I�����������A
    /// �V�[�P���X�̐���ƃJ�[�\���Ɏg�p���Ă��郂�f���̈ړ����s���Ă��܂��B
    /// </summary>
    public class SelectStyle : MenuBase
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
            /// Start
            /// 
            /// �J�n���o
            /// </summary>
            Start,

            /// <summary>
            /// Style selection
            /// 
            /// �X�^�C���I��
            /// </summary>
            StyleSelect,

            /// <summary>
            /// Style selected
            ///
            /// �X�^�C���I�����o
            /// </summary>
            StyleSelected,

            /// <summary>
            /// Rotation item start
            ///
            /// ��]���ڊJ�n���o
            /// </summary>
            RotateStart,

            /// <summary>
            /// Rotate selection
            ///
            /// ��]�I��
            /// </summary>
            RotateSelect,

            /// <summary>
            /// Rotate selected
            ///
            /// ��]�I�����o
            /// </summary>
            RotateSelected,
        }


        /// <summary>
        /// Style item
        /// 
        /// �X�^�C���̍���
        /// </summary>
        private enum CursorStyle
        {
            /// <summary>
            /// Change mode
            /// 
            /// �`�F���W���[�h
            /// </summary>
            Change,

            /// <summary>
            /// Revolve mode
            ///
            /// ���{�������[�h
            /// </summary>
            Revolve,

            /// <summary>
            /// Slide mode
            /// 
            /// �X���C�h���[�h
            /// </summary>
            Slide,

            /// <summary>
            /// Count
            ///
            /// �J�E���g�p
            /// </summary>
            Count,
        }


        /// <summary>
        /// Rotate item
        /// 
        /// ��]�̍���
        /// </summary>
        private enum CursorRotate
        {
            /// <summary>
            /// Rotate on
            /// 
            /// ��]�L��
            /// </summary>
            On,

            /// <summary>
            /// Rotate off
            ///
            /// ��]����
            /// </summary>
            Off,

            /// <summary>
            /// Count
            /// 
            /// �J�E���g�p
            /// </summary>
            Count
        }
        #endregion

        #region Fields
        /// <summary>
        /// Style animation draw position
        /// 
        /// �X�^�C���A�j���[�V�����̕`��ʒu
        /// </summary>
        private readonly Vector2 PositionStyleAnimation;

        /// <summary>
        /// Cursor sphere position
        ///
        /// �J�[�\�����̂̈ʒu
        /// </summary>
        private readonly Vector3[][] SpherePositions;

        // Processing status
        // 
        // ������
        private Phase phase;

        // Cursor position
        //
        // �J�[�\���ʒu
        private CursorStyle cursorStyle;
        private CursorRotate cursorRotate;

        // Layout
        private SequencePlayData seqStart;
        private SequencePlayData seqLoop;
        private SequencePlayData seqChange;
        private SequencePlayData seqChangeLoop;
        private SequencePlayData seqChangeSelect;
        private SequencePlayData seqRevolve;
        private SequencePlayData seqRevolveLoop;
        private SequencePlayData seqRevolveSelect;
        private SequencePlayData seqSlide;
        private SequencePlayData seqSlideLoop;
        private SequencePlayData seqSlideSelect;
        private SequencePlayData seqRotateStart;
        private SequencePlayData seqRotateLoop;
        private SequencePlayData seqRotateOn;
        private SequencePlayData seqRotateOnLoop;
        private SequencePlayData seqRotateOff;
        private SequencePlayData seqRotateOffLoop;
        private SequencePlayData seqRotateOnSelect;
        private SequencePlayData seqRotateOffSelect;

        private SceneData sceneStyleChangeRotateOn;
        private SequencePlayData seqStyleChangeRotateOn;

        private SceneData sceneStyleChangeRotateOff;
        private SequencePlayData seqStyleChangeRotateOff;

        private SceneData sceneStyleRevolve;
        private SequencePlayData seqStyleRevolve;

        private SceneData sceneStyleSlide;
        private SequencePlayData seqStyleSlide;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SelectStyle(Game game, MenuData data)
            : base(game, data)
        {
            // Loads the position.
            // 
            // �|�W�V������ǂݍ��݂܂��B
            PatternGroupData patternGroup = 
                data.sceneData.PatternGroupDictionary["Pos_PosStyle"];
            Point point;
            point = patternGroup.PatternObjectList[0].Position;
            PositionStyleAnimation = new Vector2(point.X, point.Y);

            // Sets the cursor position.
            //
            // �J�[�\���ʒu��ݒ肵�܂��B
            SpherePositions = new Vector3[][] {
                // Select Style
                new Vector3[] {
                    new Vector3(-40.22693f, 4.972153f, 0),
                    new Vector3(-40.22693f, -13.60015f, 0),
                    new Vector3(-40.22693f, -32.24725f, 0) },
                // Rotate Select
                new Vector3[] {
                    new Vector3(-20.6443f, -11.82219f, 0),
                    new Vector3(-20.6443f, -22.5499f, 0) },
            };
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
            phase = Phase.Start;

            // Initializes the cursor position.
            // 
            // �J�[�\���̏����ʒu��ݒ肵�܂��B
            cursorStyle = CursorStyle.Change;
            cursorRotate = CursorRotate.On;

            // Initializes the sphere model.
            //
            // ���̃��f���̏����ݒ�����܂��B
            Data.Spheres[1][0].Position = SpherePositions[0][0];
            Data.Spheres[1][0].Rotate = new Vector3();
            Data.Spheres[1][0].Alpha = 0.0f;
            Data.Spheres[1][0].Scale = Data.CursorSphereSize;

            // Loads and initializes the sequence.
            //
            // �V�[�P���X�̓ǂݍ��݂Ə��������s���܂��B
            InitializeSequence();

            base.Initialize();
        }


        /// <summary>
        /// Initializes the navigate.
        /// 
        /// �i�r�Q�[�g�̏����������܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("B_Cancel"), false));
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
        }


        /// <summary>
        /// Loads and initializes the sequence.
        ///
        /// �V�[�P���X�̓ǂݍ��݂Ə��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            string asset;

            // Loads the style animation.
            // 
            // �X�^�C���A�j���[�V������ǂݍ��݂܂��B
            asset = "Layout/Style/change_rotate_on_Scene";
            sceneStyleChangeRotateOn = Content.Load<SceneData>(asset);
            seqStyleChangeRotateOn = 
                sceneStyleChangeRotateOn.CreatePlaySeqData("Anime");

            asset = "Layout/Style/change_rotate_off_Scene";
            sceneStyleChangeRotateOff = Content.Load<SceneData>(asset);
            seqStyleChangeRotateOff = 
                sceneStyleChangeRotateOff.CreatePlaySeqData("Anime");

            asset = "Layout/Style/revolve_Scene";
            sceneStyleRevolve = Content.Load<SceneData>(asset);
            seqStyleRevolve = 
                sceneStyleRevolve.CreatePlaySeqData("Anime");

            asset = "Layout/Style/slide_Scene";
            sceneStyleSlide = Content.Load<SceneData>(asset);
            seqStyleSlide =
                sceneStyleSlide.CreatePlaySeqData("Anime");


            // Loads the other sequences.
            // 
            // ���̑��̃V�[�P���X��ǂݍ��݂܂��B
            seqStart = Data.sceneData.CreatePlaySeqData("StyleStart");
            seqLoop = Data.sceneData.CreatePlaySeqData("StyleLoop");
            seqChange = Data.sceneData.CreatePlaySeqData("StyleChange");
            seqChangeLoop = Data.sceneData.CreatePlaySeqData("StyleChangeLoop");
            seqChangeSelect = Data.sceneData.CreatePlaySeqData("StyleChangeSelect");
            seqRevolve = Data.sceneData.CreatePlaySeqData("StyleRevolve");
            seqRevolveLoop = Data.sceneData.CreatePlaySeqData("StyleRevolveLoop");
            seqRevolveSelect = Data.sceneData.CreatePlaySeqData("StyleRevolveSelect");
            seqSlide = Data.sceneData.CreatePlaySeqData("StyleSlide");
            seqSlideLoop = Data.sceneData.CreatePlaySeqData("StyleSlideLoop");
            seqSlideSelect = Data.sceneData.CreatePlaySeqData("StyleSlideSelect");
            seqSlide = Data.sceneData.CreatePlaySeqData("StyleSlide");
            seqSlideLoop = Data.sceneData.CreatePlaySeqData("StyleSlideLoop");
            seqSlideSelect = Data.sceneData.CreatePlaySeqData("StyleSlideSelect");
            seqRotateStart = Data.sceneData.CreatePlaySeqData("RotateStart");
            seqRotateLoop = Data.sceneData.CreatePlaySeqData("RotateLoop");
            seqRotateOn = Data.sceneData.CreatePlaySeqData("RotateOn");
            seqRotateOnLoop = Data.sceneData.CreatePlaySeqData("RotateOnLoop");
            seqRotateOnSelect = Data.sceneData.CreatePlaySeqData("RotateOnSelect");
            seqRotateOff = Data.sceneData.CreatePlaySeqData("RotateOff");
            seqRotateOffLoop = Data.sceneData.CreatePlaySeqData("RotateOffLoop");
            seqRotateOffSelect = Data.sceneData.CreatePlaySeqData("RotateOffSelect");

            // Replays the first sequence to be displayed.
            // 
            // �ŏ��ɕ\�������V�[�P���X�����v���C���܂��B
            seqStart.Replay();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
        /// </summary>
        public override MenuBase UpdateMain(GameTime gameTime)
        {
            // Processing is not performed if initialization is not complete.
            // 
            // ���������I�����Ă��Ȃ���Ώ������s���܂���B
            if (!Initialized)
                return null;

            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V�������s���܂��B
            UpdateSequence(gameTime);

            // Updates the model.
            //
            // ���f���̍X�V�������s���܂��B
            UpdateModels();

            // Sets the style animation sequence.
            // 
            // �X�^�C���A�j���[�V�����̃V�[�P���X��ݒ肵�܂��B
            SetStyleAnimation();


            if (phase == Phase.Start)
            {
                // Sets to selection after the style start animation finishes.
                // 
                // �X�^�C���J�n�A�j���[�V�������I��������A�I�������ɐݒ肵�܂��B
                if (!seqStart.IsPlay)
                {
                    phase = Phase.StyleSelect;
                }
            }
            else if (phase == Phase.StyleSelect)
            {
                // Performs update when style is selected.
                //
                // �X�^�C���I�����̍X�V�������s���܂��B
                return UpdateStyleSelect();
            }
            else if (phase == Phase.StyleSelected)
            {
                // Performs update after style selection.
                //
                // �X�^�C���I����̍X�V�������s���܂��B
                return UpdateStyleSelected();
            }
            else if (phase == Phase.RotateStart)
            {
                // Sets to selection after the rotation select start animation finishes.
                //
                // ��]�I���̊J�n�A�j���[�V�������I��������I�������ɐݒ肵�܂��B
                if (!seqRotateStart.SequenceData.IsPlay)
                {
                    phase = Phase.RotateSelect;
                }
            }
            else if (phase == Phase.RotateSelect)
            {
                // Performs update when rotation is selected.
                //
                // ��]�I�����̍X�V�������s���܂��B
                return UpdateRotateSelect();
            }
            else if (phase == Phase.RotateSelected)
            {
                // After rotate selection
                //
                // ��]�I����
                if (!seqRotateOnSelect.IsPlay && !seqRotateOffSelect.IsPlay)
                {
                    return CreateMenu(Game, MenuType.SelectMovie, Data);
                }

            }

            return null;
        }


        /// <summary>
        /// Performs update when style is selected.
        /// 
        /// �X�^�C���I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateStyleSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPush(dPad.Up, leftStick.Up))
            {
                // Performs Up key processing when style is selected.
                // 
                // �X�^�C���I�����A��L�[�������ꂽ���̏������s���܂��B
                InputStyleUpKey();
            }
            else if (InputState.IsPush(dPad.Down, leftStick.Down))
            {
                // Performs Down key processing when style is selected.
                //
                // �X�^�C���I�����A���L�[�������ꂽ���̏������s���܂��B
                InputStyleDownKey();
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                // Performs Enter key processing when style is selected.
                //
                // �X�^�C���I�����A����L�[�������ꂽ���̏������s���܂��B
                InputStyleSelectKey();
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                return CreateMenu(Game, MenuType.SelectMode, Data);
            }

            return null;
        }


        /// <summary>
        /// Performs update after style selection.
        /// 
        /// �X�^�C���I����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateStyleSelected()
        {
            // Processing is not performed while playing the animation.
            // 
            // �A�j���[�V�����Đ����͏������s���܂���B
            if (seqChangeSelect.IsPlay ||
                seqRevolveSelect.IsPlay ||
                seqSlideSelect.IsPlay)
            {
                return null;
            }

            if (cursorStyle == CursorStyle.Change)
            {
                // If the change mode is selected by the cursor,
                // set to rotation selection.
                //
                // �J�[�\�����`�F���W���[�h��I�����Ă����ꍇ��
                // ��]�̑I�������ɐݒ肵�܂��B
                phase = Phase.RotateStart;

                // Cursor model settings
                // 
                // �J�[�\�����f���̐ݒ�
                Data.Spheres[1][0].Position = SpherePositions[1][0];
                Data.Spheres[1][0].Rotate = new Vector3();
                Data.Spheres[1][0].Alpha = 0.0f;
                Data.Spheres[1][0].Scale = Data.CursorSphereMiniSize;
            }
            else
            {
                // Switches straight to the select movie menu 
                // if the revolve mode and slide mode are selected.
                // 
                // ���{�������[�h�ƁA�X���C�h���[�h��I�����Ă����ꍇ��
                // ���̂܂܃��[�r�[�I���̃��j���[�ɑJ�ڂ��܂��B
                return CreateMenu(Game, MenuType.SelectMovie, Data);
            }

            return null;
        }


        /// <summary>
        /// Performs Up key processing when style is selected.
        /// 
        /// �X�^�C���I�����A��L�[�������ꂽ���̏������s���܂��B
        /// </summary>
        private void InputStyleUpKey()
        {
            // Moves the cursor.
            // 
            // �J�[�\�����ړ����܂��B
            cursorStyle = CursorStyleUp();

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            ReplayStyleAnimation();
            ReplayStyleSequence(cursorStyle);

            // Plays the cursor movement SoundEffect.
            // 
            // �J�[�\���ړ���SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Performs Down key processing when style is selected.
        /// 
        /// �X�^�C���I�����A���L�[�������ꂽ���̏������s���܂��B
        /// </summary>
        private void InputStyleDownKey()
        {
            // Moves the cursor.
            //
            // �J�[�\�����ړ����܂��B
            cursorStyle = CursorStyleDown();

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            ReplayStyleAnimation();
            ReplayStyleSequence(cursorStyle);

            // Plays the cursor movement SoundEffect.
            // 
            // �J�[�\���ړ���SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Performs Enter key processing when style is selected.
        /// 
        /// �X�^�C���I�����A����L�[�������ꂽ���̏������s���܂��B
        /// </summary>
        private void InputStyleSelectKey()
        {
            // Sets processing status to selection completed.
            // 
            // ������Ԃ�I�������ɐݒ肵�܂��B
            phase = Phase.StyleSelected;

            // Plays the SoundEffect.
            // 
            // �����SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

            if (cursorStyle == CursorStyle.Change)
            {
                // Sets to the change mode.
                // 
                // �`�F���W���[�h�ɐݒ肵�܂��B
                Data.StageSetting.Style = StageSetting.StyleList.Change;

                // Replays the sequence.
                //
                // �V�[�P���X�����v���C���܂��B
                seqChangeSelect.Replay();
                seqRotateStart.Replay();
            }
            else if (cursorStyle == CursorStyle.Revolve)
            {
                // Sets to the revolve mode.
                //
                // ���{�������[�h�ɐݒ肵�܂��B
                Data.StageSetting.Style = StageSetting.StyleList.Revolve;
                Data.StageSetting.Rotate = StageSetting.RotateMode.Off;

                // Replays the sequence.
                // 
                // �V�[�P���X�����v���C���܂��B
                seqRevolveSelect.Replay();
            }
            else if (cursorStyle == CursorStyle.Slide)
            {
                // Sets to the slide mode.
                // 
                // �X���C�h���[�h�ɐݒ肵�܂��B
                Data.StageSetting.Style = StageSetting.StyleList.Slide;
                Data.StageSetting.Rotate = StageSetting.RotateMode.Off;

                // Replays the sequence. 
                // 
                // �V�[�P���X�����v���C���܂��B
                seqSlideSelect.Replay();
            }

        }



        /// <summary>
        /// Performs update when rotation is selected.
        /// 
        /// ��]�I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateRotateSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPush(dPad.Up, leftStick.Up, dPad.Down, leftStick.Down))
            {
                InputRotateMoveKey();
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                InputRotateSelectKey();
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                return CreateMenu(Game, MenuType.SelectStyle, Data);
            }

            return null;
        }


        /// <summary>
        /// Performs cursor movement key processing when rotation is selected.
        /// Use common processing, since it has only two items (on and off).
        /// 
        /// ��]�I�����A�J�[�\���̈ړ��L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// ���ڂ�On��Off��2�����Ȃ����߁A���ʂ̏������g�p���܂��B
        /// </summary>
        private void InputRotateMoveKey()
        {
            // Moves the cursor.
            // 
            // �J�[�\�����ړ����܂��B
            cursorRotate = CursorRotateMove();

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            ReplayStyleAnimation();
            ReplayRotateSequence(cursorRotate);

            // Plays cursor movement SoundEffect.
            // 
            // �J�[�\���ړ���SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Performs Enter key processing when rotation is selected.
        /// 
        /// ��]�I�����A����L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputRotateSelectKey()
        {
            // Sets processing status to selection completed.
            // 
            // ������Ԃ�I�������ɐݒ肵�܂��B
            phase = Phase.RotateSelected;

            // Plays the SoundEffect.
            //
            // �����SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

            if (cursorRotate == CursorRotate.On)
            {
                // Enables the rotation settings.
                // 
                // ��]�̐ݒ��L���ɂ��܂��B
                Data.StageSetting.Rotate = StageSetting.RotateMode.On;

                // Replays the sequence.
                //
                // �V�[�P���X�����v���C���܂��B
                seqRotateOnSelect.Replay();
            }
            else if (cursorRotate == CursorRotate.Off)
            {
                // Disables the rotation settings.
                //
                // ��]�̐ݒ�𖳌��ɂ��܂��B
                Data.StageSetting.Rotate = StageSetting.RotateMode.Off;

                // Replays the sequence.
                // 
                // �V�[�P���X�����v���C���܂��B
                seqRotateOffSelect.Replay();
            }
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
            seqLoop.Update(elapsedGameTime);
            seqChange.Update(elapsedGameTime);
            seqChangeLoop.Update(elapsedGameTime);
            seqChangeSelect.Update(elapsedGameTime);
            seqRevolve.Update(elapsedGameTime);
            seqRevolveLoop.Update(elapsedGameTime);
            seqRevolveSelect.Update(elapsedGameTime);
            seqSlide.Update(elapsedGameTime);
            seqSlideLoop.Update(elapsedGameTime);
            seqSlideSelect.Update(elapsedGameTime);
            seqSlide.Update(elapsedGameTime);
            seqSlideLoop.Update(elapsedGameTime);
            seqSlideSelect.Update(elapsedGameTime);
            seqRotateStart.Update(elapsedGameTime);
            seqRotateLoop.Update(elapsedGameTime);
            seqRotateOn.Update(elapsedGameTime);
            seqRotateOnLoop.Update(elapsedGameTime);
            seqRotateOnSelect.Update(elapsedGameTime);
            seqRotateOff.Update(elapsedGameTime);
            seqRotateOffLoop.Update(elapsedGameTime);
            seqRotateOffSelect.Update(elapsedGameTime);

            seqStyleChangeRotateOn.Update(elapsedGameTime);
            seqStyleChangeRotateOff.Update(elapsedGameTime);
            seqStyleRevolve.Update(elapsedGameTime);
            seqStyleSlide.Update(elapsedGameTime);
        }


        /// <summary>
        /// Updates the model.
        ///
        /// ���f���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateModels()
        {
            // Updates the position.
            // 
            // �|�W�V�����̍X�V���s���܂��B
            Vector3 position = Data.Spheres[1][0].Position;
            if (phase == Phase.StyleSelect)
            {
                if (cursorStyle == CursorStyle.Change)
                {
                    position += (SpherePositions[0][0] - position) * 0.2f;
                }
                else if (cursorStyle == CursorStyle.Revolve)
                {
                    position += (SpherePositions[0][1] - position) * 0.2f;
                }
                else if (cursorStyle == CursorStyle.Slide)
                {
                    position += (SpherePositions[0][2] - position) * 0.2f;
                }
            }
            else if (phase == Phase.RotateSelect)
            {
                if (cursorRotate == CursorRotate.On)
                {
                    position += (SpherePositions[1][0] - position) * 0.2f;
                }
                else if (cursorRotate == CursorRotate.Off)
                {
                    position += (SpherePositions[1][1] - position) * 0.2f;
                }
            }
            Data.Spheres[1][0].Position = position;

            // Specifies the transparency and scale settings.
            // 
            // �����x�ƃX�P�[���̐ݒ�����܂��B
            float alpha = Data.Spheres[1][0].Alpha;
            float fadeSpeed = Data.CursorSphereFadeSpeed;
            if (phase != Phase.StyleSelected && phase != Phase.RotateSelected)
            {
                alpha = MathHelper.Clamp(alpha + fadeSpeed, 0.0f, 1.0f);
            }
            else
            {
                // Performs fade-out with zoom when item is selected.
                // 
                // ���ڂ����肳���ƃY�[�����Ȃ���t�F�[�h�A�E�g���܂��B
                alpha = MathHelper.Clamp(alpha - fadeSpeed, 0.0f, 1.0f);
                Data.Spheres[1][0].Scale += Data.CursorSphereZoomSpeed;
            }
            Data.Spheres[1][0].Alpha = alpha;
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
            // Draws the cursor sphere.
            // 
            // �J�[�\���̋��̂�`�悵�܂��B
            DrawSpheres(batch);

            // Switches the draw method depending on the process.
            // 
            // �������e�ɂ���ĕ`�惁�\�b�h��؂�ւ��܂��B
            batch.Begin();
            if (phase == Phase.Start)
            {
                DrawStyleStart(batch);
            }
            else if (phase == Phase.StyleSelect)
            {
                DrawStyleSelect(gameTime, batch);
            }
            else if (phase == Phase.StyleSelected)
            {
                DrawStyleSelected(batch);
            }
            else if (phase == Phase.RotateStart)
            {
                DrawRotateStart(batch);
            }
            else if (phase == Phase.RotateSelect)
            {
                DrawRotateSelect(gameTime, batch);
            }
            else if (phase == Phase.RotateSelected)
            {
                DrawRotateSelected(batch);
            }
            batch.End();

            // Draws the style animation.
            // 
            // �X�^�C���A�j���[�V������`�悵�܂��B
            DrawStyleAnimation(batch);
        }


        /// <summary>
        /// Performs render processing when the style animation starts.
        /// 
        /// �X�^�C���A�j���[�V�����J�n���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawStyleStart(SpriteBatch batch)
        {
            seqStart.Draw(batch, null);
        }


        /// <summary>
        /// Performs render processing when style is selected.
        ///
        /// �X�^�C���I�𒆂̕`�揈�����s���܂��B
        /// </summary>
        private void DrawStyleSelect(GameTime gameTime, SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);

            if (cursorStyle == CursorStyle.Change)
            {
                seqChangeLoop.Draw(batch, null);
            }
            else if (cursorStyle == CursorStyle.Revolve)
            {
                seqRevolveLoop.Draw(batch, null);
            }
            else if (cursorStyle == CursorStyle.Slide)
            {
                seqSlideLoop.Draw(batch, null);
            }

            seqChange.Draw(batch, null);
            seqRevolve.Draw(batch, null);
            seqSlide.Draw(batch, null);

            // Draws the navigate button.
            // 
            // �i�r�Q�[�g�{�^���̕`������܂��B
            DrawNavigate(gameTime, batch, false);
        }


        /// <summary>
        /// Performs render processing after the style is selected.
        /// 
        /// �X�^�C���I����̕`�揈�����s���܂��B
        /// </summary>
        private void DrawStyleSelected(SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);
            if (cursorStyle == CursorStyle.Change)
            {
                seqChangeSelect.Draw(batch, null);
                seqRotateStart.Draw(batch, null);
            }
            else if (cursorStyle == CursorStyle.Revolve)
            {
                seqRevolveSelect.Draw(batch, null);
            }
            else if (cursorStyle == CursorStyle.Slide)
            {
                seqSlideSelect.Draw(batch, null);
            }
        }


        /// <summary>
        /// Performs render processing when the rotation selected animation starts.
        /// 
        /// ��]�I���A�j���[�V�����J�n���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawRotateStart(SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);
            seqRotateStart.Draw(batch, null);
        }

        /// <summary>
        /// Performs render processing when rotation is selected.
        ///
        /// ��]�I�����̕`�揈�����s���܂��B
        /// </summary>
        private void DrawRotateSelect(GameTime gameTime, SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);
            seqRotateLoop.Draw(batch, null);
            if (cursorRotate == CursorRotate.On)
            {
                seqRotateOnLoop.Draw(batch, null);
            }
            else if (cursorRotate == CursorRotate.Off)
            {
                seqRotateOffLoop.Draw(batch, null);
            }

            seqRotateOn.Draw(batch, null);
            seqRotateOff.Draw(batch, null);

            // Draws the navigate button.
            // 
            // �i�r�Q�[�g�{�^���̕`������܂��B
            DrawNavigate(gameTime, batch, false);
        }


        /// <summary>
        /// Performs render processing after rotation is selected.
        /// 
        /// ��]�I����̕`�揈�����s���܂��B
        /// </summary>
        private void DrawRotateSelected(SpriteBatch batch)
        {
            seqLoop.Draw(batch, null);
            seqRotateLoop.Draw(batch, null);
            if (cursorRotate == CursorRotate.On)
            {
                seqRotateOnSelect.Draw(batch, null);
            }
            else if (cursorRotate == CursorRotate.Off)
            {
                seqRotateOffSelect.Draw(batch, null);
            }
        }


        /// <summary>
        /// Draws the cursor sphere.
        /// 
        /// �J�[�\���̋��̂�`�悵�܂��B
        /// </summary>
        private void DrawSpheres(SpriteBatch batch)
        {
            GraphicsDevice graphicsDevice = batch.GraphicsDevice;
            Matrix view;
            view = Matrix.CreateLookAt(Data.CameraPosition, Vector3.Zero, Vector3.Up);
            Data.Spheres[1][0].SetRenderState(graphicsDevice, SpriteBlendMode.Additive);
            Data.Spheres[1][0].Draw(view, GameData.Projection);
        }


        /// <summary>
        /// Draws the style animation.
        ///
        /// �X�^�C���A�j���[�V������`�悵�܂��B
        /// </summary>
        private void DrawStyleAnimation(SpriteBatch batch)
        {
            Texture2D texture = Data.StyleAnimationTexture;

            // The following process is not performed when there
            // is no style animation texture.
            // 
            // �X�^�C���A�j���[�V�����̃e�N�X�`���������ꍇ��
            // �ȉ��̏������s���܂���B
            if (texture == null)
                return;

            // Draws style animation texture via addition. 
            // 
            // �X�^�C���A�j���[�V�����̃e�N�X�`�������Z�ŕ`�悵�܂��B
            batch.Begin(SpriteBlendMode.Additive);
            batch.Draw(texture, PositionStyleAnimation, Color.White);
            batch.End();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Replays the sequence.
        /// 
        /// �V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplayStyleAnimation()
        {
            seqStyleChangeRotateOn.Replay();
            seqStyleChangeRotateOff.Replay();
            seqStyleRevolve.Replay();
            seqStyleSlide.Replay();
        }


        /// <summary>
        /// Sets the current style animation.
        ///
        /// ���݂̃X�^�C���A�j���[�V������ݒ肵�܂��B
        /// </summary>
        private void SetStyleAnimation()
        {
            if (cursorStyle == CursorStyle.Change)
            {
                if (cursorRotate == CursorRotate.On)
                {
                    // Change : Rotate On
                    Data.SeqStyleAnimation = seqStyleChangeRotateOn;
                }
                else
                {
                    // Change : Rotate Off
                    Data.SeqStyleAnimation = seqStyleChangeRotateOff;
                }
            }
            else if (cursorStyle == CursorStyle.Revolve)
            {
                // Revolve
                Data.SeqStyleAnimation = seqStyleRevolve;
            }
            else if (cursorStyle == CursorStyle.Slide)
            {
                // Slide
                Data.SeqStyleAnimation = seqStyleSlide;
            }
        }


        /// <summary>
        /// Moves the style selection cursor up.
        /// 
        /// �X�^�C���I���̃J�[�\������Ɉړ����܂��B
        /// </summary>
        private CursorStyle CursorStyleUp()
        {
            int count = (int)CursorStyle.Count;
            return (CursorStyle)(((int)cursorStyle + (count - 1)) % count);
        }


        /// <summary>
        /// Moves the style selection cursor down.
        /// 
        /// �X�^�C���I���̃J�[�\�������Ɉړ����܂��B
        /// </summary>
        private CursorStyle CursorStyleDown()
        {
            int count = (int)CursorStyle.Count;
            return (CursorStyle)(((int)cursorStyle + 1) % count);
        }


        /// <summary>
        /// Replays the sequence for the style selection cursor.
        /// 
        /// �X�^�C����I�����Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplayStyleSequence(CursorStyle cursorStyle)
        {
            ReplayStyleSequence((int)cursorStyle);
        }


        /// <summary>
        /// Replays the sequence for the style selection cursor.
        /// 
        /// �X�^�C����I�����Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplayStyleSequence(int id)
        {
            SequencePlayData[] seqList = { seqChange, seqRevolve, seqSlide };
            seqList[id].Replay();
        }


        /// <summary>
        /// Moves the rotation selection cursor.
        /// 
        /// ��]�I���̃J�[�\�����ړ����܂��B
        /// </summary>
        private CursorRotate CursorRotateMove()
        {
            int count = (int)CursorRotate.Count;
            return (CursorRotate)(((int)cursorRotate + 1) % count);
        }
        

        /// <summary>
        /// Replays the sequence for the rotation selection cursor.
        /// 
        /// ��]��I�����Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplayRotateSequence(CursorRotate cursorRotate)
        {
            ReplayRotateSequence((int)cursorRotate);
        }


        /// <summary>
        /// Replays the sequence for the rotation selection cursor.
        /// 
        /// ��]��I�����Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplayRotateSequence(int id)
        {
            SequencePlayData[] seqList = { seqRotateOn, seqRotateOff };
            seqList[id].Replay();
        }
        #endregion
    }
}
