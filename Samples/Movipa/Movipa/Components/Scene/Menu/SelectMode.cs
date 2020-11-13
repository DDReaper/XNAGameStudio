#region File Description
//-----------------------------------------------------------------------------
// SelectMode.cs
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
    /// Menu item for processing mode selection.
    /// It inherits MenuBase and expands menu compilation.
    /// This class implements menu mode selection, provides sequence control
    /// and moves the model used as the cursor. 
    /// 
    /// ���[�h�I�����������郁�j���[���ڂł��B
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ���̃N���X�̓��j���[�̃��[�h�I�����������A�V�[�P���X�̐����
    /// �J�[�\���Ɏg�p���Ă��郂�f���̈ړ����s���Ă��܂��B
    /// </summary>
    public class SelectMode : MenuBase
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
            /// Normal processing
            /// 
            /// �ʏ폈��
            /// </summary>
            Select,

            /// <summary>
            /// Selected
            ///
            /// �I�����o
            /// </summary>
            Selected,
        }


        /// <summary>
        /// Cursor position
        /// 
        /// �J�[�\���ʒu
        /// </summary>
        private enum CursorPosition
        {
            /// <summary>
            /// Normal mode
            /// 
            /// �m�[�}�����[�h
            /// </summary>
            Normal,

            /// <summary>
            /// Free mode
            ///
            /// �t���[���[�h
            /// </summary>
            Free,

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
        /// Cursor sphere position
        /// 
        /// �J�[�\���̋��̂̈ʒu
        /// </summary>
        private readonly Vector3[] SpherePositions;

        // Cursor position
        // 
        // �J�[�\���ʒu
        private CursorPosition cursor;

        // Processing status
        // 
        // �������
        private Phase phase;

        // Sequence
        //
        // �V�[�P���X
        private SequencePlayData seqStart;
        private SequencePlayData seqBg;
        private SequencePlayData seqNormal;
        private SequencePlayData seqNormalLoop;
        private SequencePlayData seqNormalSelect;
        private SequencePlayData seqFree;
        private SequencePlayData seqFreeLoop;
        private SequencePlayData seqFreeSelect;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SelectMode(Game game, MenuData data)
            : base(game, data)
        {
            // Sets the cursor position.
            // 
            // �J�[�\���̈ʒu��ݒ肵�܂��B
            SpherePositions = new Vector3[] {
                new Vector3(0, -4.56056f, 0),
                new Vector3(0, -24.94905f, 0)
            };
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Initializes the sphere models.
            // 
            // ���̃��f���̏����ݒ�����܂��B
            Data.Spheres[1][0].Position = SpherePositions[0];
            Data.Spheres[1][0].Rotate = new Vector3();
            Data.Spheres[1][0].Alpha = 0.0f;
            Data.Spheres[1][0].Scale = Data.CursorSphereSize;

            // Initializes the sequence.
            // 
            // �V�[�P���X�����������܂��B
            InitializeSequence();

            // Sets the initial cursor position.
            // 
            // �J�[�\���̏����ʒu��ݒ肵�܂��B
            cursor = CursorPosition.Normal;

            // Initializes the processing status.
            // 
            // ������Ԃ̏����ݒ�����܂��B
            phase = Phase.Start;

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
        /// Initializes the sequence.
        /// 
        /// �V�[�P���X�̏��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            seqStart = Data.sceneData.CreatePlaySeqData("ModeStart");
            seqBg = Data.sceneData.CreatePlaySeqData("ModeLoop");
            seqNormal = Data.sceneData.CreatePlaySeqData("ModeNormal");
            seqNormalLoop = Data.sceneData.CreatePlaySeqData("ModeNormalLoop");
            seqNormalSelect = Data.sceneData.CreatePlaySeqData("ModeNormalSelect");
            seqFree = Data.sceneData.CreatePlaySeqData("ModeFree");
            seqFreeLoop = Data.sceneData.CreatePlaySeqData("ModeFreeLoop");
            seqFreeSelect = Data.sceneData.CreatePlaySeqData("ModeFreeSelect");

            // Plays the sequence from the start.
            // 
            // �V�[�P���X���͂��߂���Đ����܂��B
            seqStart.Replay();
            seqStart.Update(new TimeSpan());
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
            // Processing is not performed if initialization is incomplete.
            // 
            // ���������������̏ꍇ�͏������s���܂���B
            if (!Initialized)
                return null;

            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V�������s���܂��B
            UpdateSequence(gameTime);

            // Updates the sphere models.
            // 
            // ���̃��f���̍X�V�������s���܂��B
            UpdateModels();

            if (phase == Phase.Start)
            {
                // Changes to selection after start animation finishes.
                // 
                // �J�n�A�j���[�V�������I��������I�������ɕύX���܂��B
                if (!seqStart.SequenceData.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                // Updates at selection.
                //
                // �I�����̍X�V�������s���܂��B
                UpdateSelect(gameTime);
            }
            else if (phase == Phase.Selected)
            {
                // Updates when selection ends.
                //
                // �I���I�����̍X�V�������s���܂��B
                return UpdateSelected();
            }

            return null;
        }


        /// <summary>
        /// Updates at selection.
        /// 
        /// �I�����̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSelect(GameTime gameTime)
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPush(dPad.Up, leftStick.Up, dPad.Down, leftStick.Down))
            {
                // Moves the cursor.
                // Use common processing since there are only two items.
                // 
                // �J�[�\�����ړ����܂��B
                // ���ڂ�2���������̂ŋ��ʂ̏������g�p���܂��B
                cursor = CursorMove();

                // Plays the SoundEffect.
                // 
                // SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                // Replays the selected cursor sequence.
                //
                // �I������Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
                ReplaySequence(gameTime);
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                // Enter key pressed; process changes.
                //
                // ����L�[�������ꂽ�̂ŁA������ύX���܂��B
                phase = Phase.Selected;

                // Plays the SoundEffect.
                // 
                // SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

                // Sets game mode and replays the selected item sequence.
                //
                // �Q�[�����[�h��ݒ肵�A�I�����ꂽ���ڂ̃V�[�P���X�����v���C���܂��B
                if (cursor == CursorPosition.Normal)
                {
                    Data.StageSetting.Mode = StageSetting.ModeList.Normal;

                    seqNormalSelect.Replay();
                    seqNormalSelect.Update(gameTime.ElapsedGameTime);
                }
                else if (cursor == CursorPosition.Free)
                {
                    Data.StageSetting.Mode = StageSetting.ModeList.Free;

                    seqFreeSelect.Replay();
                    seqFreeSelect.Update(gameTime.ElapsedGameTime);
                }
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Cancel button pressed; return to title.
                // 
                // �L�����Z���{�^���������ꂽ�̂ŁA�^�C�g���ɖ߂�܂��B
                GameComponent next = new Movipa.Components.Scene.Title(Game);
                GameData.SceneQueue.Enqueue(next);
                GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
            }
        }


        /// <summary>
        /// Updates when selection ends.
        /// 
        /// �I���I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelected()
        {
            if (cursor == CursorPosition.Normal)
            {
                // Switches to the file selection menu when normal mode is 
                // selected by the cursor and the selected animation has finished.
                // 
                // �J�[�\�����m�[�}�����[�h��I�����Ă��āA�I���A�j���[�V������
                // �I��������A�t�@�C���I���̃��j���[�ɑJ�ڂ��܂��B
                if (!seqNormalSelect.SequenceData.IsPlay)
                {
                    return CreateMenu(Game, MenuType.SelectFile, Data);
                }
            }
            else if (cursor == CursorPosition.Free)
            {
                // Switches to the style selection menu when free mode is 
                // selected by the cursor and the selected animation has finished.
                // 
                // �J�[�\�����t���[���[�h��I�����Ă��āA�I���A�j���[�V������
                // �I��������A�X�^�C���I���̃��j���[�ɑJ�ڂ��܂��B
                if (!seqFreeSelect.SequenceData.IsPlay)
                {
                    return CreateMenu(Game, MenuType.SelectStyle, Data);
                }
            }

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
            seqBg.Update(elapsedGameTime);
            seqNormal.Update(elapsedGameTime);
            seqNormalLoop.Update(elapsedGameTime);
            seqNormalSelect.Update(elapsedGameTime);
            seqFree.Update(elapsedGameTime);
            seqFreeLoop.Update(elapsedGameTime);
            seqFreeSelect.Update(elapsedGameTime);
        }


        /// <summary>
        /// Updates the model.
        /// 
        /// ���f���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateModels()
        {
            // Moves the position to the cursor location. 
            // 
            // �J�[�\���ʒu�փ|�W�V�������ړ����܂��B
            Vector3 position = Data.Spheres[1][0].Position;
            if (cursor == CursorPosition.Normal)
            {
                position += (SpherePositions[0] - position) * 0.2f;
            }
            else if (cursor == CursorPosition.Free)
            {
                position += (SpherePositions[1] - position) * 0.2f;
            }
            Data.Spheres[1][0].Position = position;

            // Modifies the transparency and scale settings. 
            // 
            // �����x�ƃX�P�[���̕ύX�����܂��B
            float alpha = Data.Spheres[1][0].Alpha;
            float fadeSpeed = Data.CursorSphereFadeSpeed;
            if (phase != Phase.Selected)
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
            // Draws the model. 
            // 
            // ���f���̕`����s���܂��B
            DrawModels(batch);

            batch.Begin();
            if (phase == Phase.Start)
            {
                seqStart.Draw(batch, null);
            }
            else if (phase == Phase.Select)
            {
                // Draws menu name and other sequences.
                // 
                // ���j���[���Ȃǂ̃V�[�P���X��`�悵�܂��B
                seqBg.Draw(batch, null);

                // Draws the item sequence.
                // 
                // ���ڂ̃V�[�P���X��`�悵�܂��B
                if (cursor == CursorPosition.Normal)
                {
                    seqNormalLoop.Draw(batch, null);
                }
                else if (cursor == CursorPosition.Free)
                {
                    seqFreeLoop.Draw(batch, null);
                }
                seqNormal.Draw(batch, null);
                seqFree.Draw(batch, null);

                // Draws the navigate button.
                // 
                // �i�r�Q�[�g�{�^����`�悵�܂��B
                DrawNavigate(gameTime, batch, false);
            }
            else if (phase == Phase.Selected)
            {
                // Draws menu name and other sequences.
                // 
                // ���j���[���Ȃǂ̃V�[�P���X��`�悵�܂��B
                seqBg.Draw(batch, null);

                // Draws the item animation.
                // 
                // ���ڂ̌���A�j���[�V������`�悵�܂��B
                if (cursor == CursorPosition.Normal)
                {
                    seqNormalSelect.Draw(batch, null);
                }
                else if (cursor == CursorPosition.Free)
                {
                    seqFreeSelect.Draw(batch, null);
                }
            }

            batch.End();
        }


        /// <summary>
        /// Draws the model.
        /// 
        /// ���f���̕`����s���܂��B
        /// </summary>
        private void DrawModels(SpriteBatch batch)
        {
            GraphicsDevice graphicsDevice = batch.GraphicsDevice;
            Matrix view;
            view = Matrix.CreateLookAt(Data.CameraPosition, Vector3.Zero, Vector3.Up);
            Data.Spheres[1][0].SetRenderState(graphicsDevice, SpriteBlendMode.Additive);
            Data.Spheres[1][0].Draw(view, GameData.Projection);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns to the next cursor position.
        /// 
        /// �J�[�\���̎��̈ʒu��Ԃ��܂��B
        /// </summary>
        private CursorPosition CursorMove()
        {
            return (CursorPosition)(((int)cursor + 1) % (int)CursorPosition.Count);
        }


        /// <summary>
        /// Replays the selected cursor sequence.
        /// 
        /// �I������Ă���J�[�\���̃V�[�P���X�����v���C���܂��B
        /// </summary>
        private void ReplaySequence(GameTime gameTime)
        {
            if (cursor == CursorPosition.Normal)
            {
                seqNormal.Replay();
                seqNormal.Update(gameTime.ElapsedGameTime);
            }
            else if (cursor == CursorPosition.Free)
            {
                seqFree.Replay();
                seqFree.Update(gameTime.ElapsedGameTime);
            }
        }
        #endregion
    }
}
