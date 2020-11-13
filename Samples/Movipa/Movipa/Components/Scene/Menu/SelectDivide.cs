#region File Description
//-----------------------------------------------------------------------------
// SelectDivide.cs
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
    /// Menu item used to specify the number of 
    /// divisions. It inherits MenuBaseance and 
    /// expands menu compilation processing. When
    /// rotation is off, the number of vertical and
    /// horizontal divisions can be specified. When rotation
    /// is on, only the number of vertical divisions
    /// may be specified. The number of horizontal divisions
    /// is automatically set to the maximum possible number
    /// relative to the vertical divisions.
    ///
    /// ��������ݒ肷�郁�j���[���ڂł��B
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ��]�������̏ꍇ�͏c�Ɖ��̕�������ݒ�ł��܂����A
    /// ��]������ɐݒ肳��Ă����ꍇ�͏c�̕������̂ݐݒ�\�ŁA
    /// ���̕������͏c�̕������ɉ������ő吔�������Őݒ肵�܂��B
    /// </summary>
    public class SelectDivide : MenuBase
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
            ///�J�n���o
            /// </summary>
            Start,

            /// <summary>
            /// Normal processing
            ///
            /// �ʏ폈��
            /// </summary>
            Select,

            /// <summary>
            /// Select
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
            /// Horizontal divisions
            /// 
            /// ������
            /// </summary>
            Left,

            /// <summary>
            /// Vertical divisions
            /// 
            /// �c����
            /// </summary>
            Right,

            /// <summary>
            /// Count
            /// 
            /// �J�E���g�p
            /// </summary>
            Count,
        }
        #endregion

        #region Fields
        // Maximum number of divisions
        //
        // �������ő�l
        private readonly Point DivideMax;

        // Minimum number of divisions
        //
        // �������ŏ��l
        private readonly Point DivideMin;

        // Movie draw position
        //
        // ���[�r�[�`��ʒu
        private readonly Rectangle MoviePreviewRect;

        // Cursor sphere position
        //
        // �J�[�\�����̂̈ʒu
        private readonly Vector3[] SpherePositions;

        // Processing details
        //
        // �������e
        private Phase phase;

        // Cursor position
        //
        // �J�[�\���ʒu
        private CursorPosition cursor;

        // Sequence
        // 
        // �V�[�P���X
        private SequencePlayData seqStart;
        private SequencePlayData seqLoop;
        private SequencePlayData seqSelect;
        private SequencePlayData seqMovieWindow;
        private SequencePlayData seqLeftLoop;
        private SequencePlayData seqLeftUp;
        private SequencePlayData seqLeftDown;
        private SequencePlayData seqRightLoop;
        private SequencePlayData seqRightUp;
        private SequencePlayData seqRightDown;
        private SequencePlayData seqPosDivideWidthStart;
        private SequencePlayData seqPosDivideHeightStart;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SelectDivide(Game game, MenuData data)
            : base(game, data)
        {
            // Loads the position.
            //
            // �|�W�V������ǂݍ��݂܂��B
            PatternGroupData patternGroup = 
                data.sceneData.PatternGroupDictionary["Pos_PosMovie"];
            Point point;
            point = patternGroup.PatternObjectList[0].Position;
            MoviePreviewRect = new Rectangle(point.X, point.Y, 640, 360);

            // Sets the number of divisions to the maximum value.
            // 
            // �������̍ő�l��ݒ肵�܂��B
            DivideMax = new Point(10, 6);

            // Sets the number of divisions to the minimum value.
            // 
            // �������̍ŏ��l��ݒ肵�܂��B
            DivideMin = new Point(2, 2);

            // Sets the cursor position.
            // 
            // �J�[�\���̈ʒu��ݒ肵�܂��B
            SpherePositions = new Vector3[] {
                new Vector3(-29.43751f, -47.44127f, 0),
                new Vector3(27.08988f, -47.44127f, 0)
            };
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������̏������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Initializes the processing status.
            // 
            // ������Ԃ̏����ݒ�����܂��B
            phase = Phase.Start;

            // Initializes the sphere model.
            // 
            // ���̃��f���̏����ݒ�����܂��B
            Data.Spheres[1][0].Position += SpherePositions[0];
            Data.Spheres[1][0].Rotate = new Vector3(0, 0, MathHelper.ToRadians(90.0f));
            Data.Spheres[1][0].Alpha = 0.0f;
            Data.Spheres[1][0].Scale = Data.CursorSphereSize;

            // Initializes the cursor position.
            //
            // �J�[�\���̏����ʒu��ݒ肵�܂��B
            if (Data.StageSetting.Rotate == StageSetting.RotateMode.Off)
            {
                cursor = CursorPosition.Left;
            }
            else if (Data.StageSetting.Rotate == StageSetting.RotateMode.On)
            {
                // When rotation is on, only vertical divisions may be specified.
                // 
                // ��]�L��̏ꍇ�͏c�����̂ݐݒ�ł��܂��B
                cursor = CursorPosition.Right;
            }

            // Calculates the number of divisions.
            // 
            // �������̌v�Z���s���܂��B
            CalcDivide();

            // Initializes the sequence.
            //
            // �V�[�P���X�̏��������s���܂��B
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
        /// Initializes the sequence.
        /// 
        /// �V�[�P���X�̏��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            SceneData scene = Data.sceneData;
            seqStart = scene.CreatePlaySeqData("DivideStart");
            seqLoop = scene.CreatePlaySeqData("DivideLoop");
            seqSelect = scene.CreatePlaySeqData("DivideSelect");
            seqMovieWindow = scene.CreatePlaySeqData("MovieWindow");
            seqLeftLoop = scene.CreatePlaySeqData("DivideLeftLoop");
            seqLeftUp = scene.CreatePlaySeqData("DivideLeftUp");
            seqLeftDown = scene.CreatePlaySeqData("DivideLeftDown");
            seqRightLoop = scene.CreatePlaySeqData("DivideRightLoop");
            seqRightUp = scene.CreatePlaySeqData("DivideRightUp");
            seqRightDown = scene.CreatePlaySeqData("DivideRightDown");
            seqPosDivideWidthStart = scene.CreatePlaySeqData("PosDivideWidthStart");
            seqPosDivideHeightStart = scene.CreatePlaySeqData("PosDivideHeightStart");

            seqStart.Replay();
            seqPosDivideWidthStart.Replay();
            seqPosDivideHeightStart.Replay();
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
            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V���s���܂��B
            UpdateSequence(gameTime);

            // Updates the model.
            // 
            // ���f���̍X�V�������s���܂��B
            UpdateModels();

            if (phase == Phase.Start)
            {
                // Sets to selection process after start animation finishes.
                // 
                // �J�n�A�j���[�V�������I��������I�������֐ݒ肵�܂��B
                if (!seqStart.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                // Updates during selection.
                // 
                // �I�𒆂̍X�V�������s���܂��B
                return UpdateSelect();
            }
            else if (phase == Phase.Selected)
            {
                // Switches to the confirmation screen when the selected animation ends.
                //
                // �I���A�j���[�V�������I��������m�F��ʂ֑@�ۂ��܂��B
                if (!seqSelect.IsPlay)
                {
                    return CreateMenu(Game, MenuType.Ready, Data);
                }
            }

            return null;
        }


        /// <summary>
        /// Updates during selection.
        /// 
        /// �I�𒆂̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPushRepeat(dPad.Up, leftStick.Up))
            {
                // Down key increases the number of divisions.
                // 
                // ���L�[�������ꂽ�番�����𑝂₵�܂��B
                SetDivide(1);
            }
            else if (InputState.IsPushRepeat(dPad.Down, leftStick.Down))
            {
                // Down key decreases the number of divisions.
                //
                // ���L�[�������ꂽ�番���������炵�܂��B
                SetDivide(-1);
            }
            else if (InputState.IsPush(dPad.Left, leftStick.Left) ||
                InputState.IsPush(dPad.Right, leftStick.Right))
            {
                // Use common processing since there are only two items.
                // However, only the number of vertical divisions may be
                // specified when rotation is on, so the processing is 
                // performed only when rotation is off.
                //
                //���ڂ�2�����Ȃ��̂ŋ��ʂ̏������g�p���܂��B
                // �������A��]���L��̏ꍇ�͏c�����̂ݐݒ�\�Ȃ̂ŁA
                // ��]�����̏ꍇ�̂ݏ������s���܂��B
                if (Data.StageSetting.Rotate == StageSetting.RotateMode.Off)
                {;
                    // Moves the cursor position.
                    // 
                    // �J�[�\���ʒu���ړ����܂��B
                    cursor = CursorMove();

                    // SoundEffect playback
                    // 
                    // SoundEffect�̍Đ�
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
                }
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                // Enter button has been pressed; set processing state after selection.
                // 
                // ����{�^���������ꂽ�̂ŁA������Ԃ�I����ɐݒ肵�܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                seqSelect.Replay();
                phase = Phase.Selected;
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Cancel button has been pressed; return to movie selection.
                // 
                // �L�����Z���{�^���������ꂽ�̂Ń��[�r�[�I���֖߂�܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
                return CreateMenu(Game, MenuType.SelectMovie, Data);
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
            seqStart.Update(gameTime.ElapsedGameTime);
            seqLoop.Update(gameTime.ElapsedGameTime);
            seqSelect.Update(gameTime.ElapsedGameTime);
            seqMovieWindow.Update(gameTime.ElapsedGameTime);
            seqLeftLoop.Update(gameTime.ElapsedGameTime);
            seqLeftUp.Update(gameTime.ElapsedGameTime);
            seqLeftDown.Update(gameTime.ElapsedGameTime);
            seqRightLoop.Update(gameTime.ElapsedGameTime);
            seqRightUp.Update(gameTime.ElapsedGameTime);
            seqRightDown.Update(gameTime.ElapsedGameTime);
            seqPosDivideWidthStart.Update(gameTime.ElapsedGameTime);
            seqPosDivideHeightStart.Update(gameTime.ElapsedGameTime);
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
            if (cursor == CursorPosition.Left)
            {
                position += (SpherePositions[0] - position) * 0.2f;
            }
            else if (cursor == CursorPosition.Right)
            {
                position += (SpherePositions[1] - position) * 0.2f;
            }
            Data.Spheres[1][0].Position = position;

            // Specifies the transparency and scale settings.
            //
            // �����x�ƃX�P�[���̐ݒ�����܂��B
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

            // Draws the movie window.
            // 
            // ���[�r�[�E�B���h�E��`�悵�܂��B
            batch.Begin();
            seqMovieWindow.Draw(batch, null);
            batch.End();

            // Draws the division preview.
            // 
            // �����v���r���[��`�悵�܂��B
            DrawDivideTexture(batch);

            // Draws the sequence.
            // 
            // �V�[�P���X�̕`����s���܂��B
            batch.Begin();
            DrawSequence(gameTime, batch);
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


        /// <summary>
        /// Draws the division preview.
        ///
        /// �����v���r���[��`�悵�܂��B
        /// </summary>
        private void DrawDivideTexture(SpriteBatch batch)
        {
            // Processing is not performed if the texture is not set. 
            // 
            // �e�N�X�`�����ݒ肳��Ă��Ȃ��ꍇ�͏������s���܂���B
            if (Data.divideTexture == null)
                return;

            // Draws division preview with no alpha.
            // 
            // �A���t�@�����ŕ����v���r���[��`�悵�܂��B
            batch.Begin(SpriteBlendMode.None);
            batch.Draw(Data.divideTexture, MoviePreviewRect, Color.White);
            batch.End();
        }


        /// <summary>
        /// Draws the sequence.
        /// 
        /// �V�[�P���X�̕`����s���܂��B
        /// </summary>
        private void DrawSequence(GameTime gameTime, SpriteBatch batch)
        {
            if (phase == Phase.Start)
            {
                seqStart.Draw(batch, null);
                DrawSequenceString(batch);
            }
            else if (phase == Phase.Select)
            {
                seqLoop.Draw(batch, null);
                seqLeftLoop.Draw(batch, null);
                seqRightLoop.Draw(batch, null);
                seqLeftUp.Draw(batch, null);
                seqLeftDown.Draw(batch, null);
                seqRightUp.Draw(batch, null);
                seqRightDown.Draw(batch, null);
                DrawSequenceString(batch);

                // Draws the navigate button.
                //
                // �i�r�Q�[�g�{�^���̕`����s���܂��B
                DrawNavigate(gameTime, batch, false);
            }
            else if (phase == Phase.Selected)
            {
                seqLoop.Draw(batch, null);
                seqSelect.Draw(batch, null);
                DrawSequenceString(batch);
            }
        }


        /// <summary>
        /// Draws the sequence text string.
        /// 
        /// �V�[�P���X�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch)
        {
            SequenceBankData sequenceBank;

            // Horizontal division text string
            // 
            // �������̕�����
            sequenceBank = seqPosDivideWidthStart.SequenceData;
            DrawSequenceString(batch, sequenceBank, Data.StageSetting.Divide.X);

            // Vertical division text string
            // 
            // �c�����̕�����
            sequenceBank = seqPosDivideHeightStart.SequenceData;
            DrawSequenceString(batch, sequenceBank, Data.StageSetting.Divide.Y);
        }


        /// <summary>
        /// Draws the sequence text string.
        /// 
        /// �V�[�P���X�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch, 
            SequenceBankData sequenceBank, int value)
        {
            foreach (SequenceGroupData seqBodyData in sequenceBank.SequenceGroupList)
            {
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;
                if (seqPartsData == null)
                {
                    continue;
                }

                foreach (PatternObjectData patPartsData in
                    seqPartsData.PatternObjectList)
                {
                    DrawData putInfoData = patPartsData.InterpolationDrawData;
                    SpriteFont font = LargeFont;
                    Color color = putInfoData.Color;
                    Point point = putInfoData.Position;
                    Vector2 position = new Vector2(point.X, point.Y);
                    string text = string.Format("{0:00}", value);
                    batch.DrawString(font, text, position, color);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Calculates the number of divisions.
        /// 
        /// ���������v�Z���܂��B
        /// </summary>
        private void CalcDivide()
        {
            // Creates the panel.
            // 
            // �p�l�����쐬���܂��B
            Data.PanelManager.CreatePanel(GameData.MovieSizePoint, Data.StageSetting);

            // When rotation is off, end without changing.
            // When rotation is on, calculate the optimum number of horizontal divisions.
            // 
            // ��]���I�t�̏�Ԃ̎��͂��̂܂܏I�����܂��B
            // ��]���I���̎��͉��̕��������œK�ɂȂ�悤�Ɍv�Z���܂��B
            if (Data.StageSetting.Rotate == StageSetting.RotateMode.Off)
                return;

            // Obtains the number of divisions.
            //
            // ���������擾���܂��B
            Point divide = Data.StageSetting.Divide;

            // Recalculates using the minimum number of horizontal divisions. 
            //
            // �����������ŏ��l�ɂ��čČv�Z���܂��B
            divide.X = DivideMin.X;
            Data.StageSetting.Divide = divide;
            Data.PanelManager.CreatePanel(GameData.MovieSizePoint, Data.StageSetting);

            // Divides the movie width by the width of the rectangle. 
            //
            // ���[�r�[�̉�������`�̉����Ŋ���܂��B
            int movieWidth = GameData.MovieSizePoint.X;
            int panelWidth = (int)Data.PanelManager.PanelSize.X;
            int widthCount = (int)(movieWidth / panelWidth);

            // Updates the number of divisions to the calculation result.
            //
            // ���������𕪊����ɍĐݒ肵�܂��B
            divide.X = widthCount;
            Data.StageSetting.Divide = divide;

            // Recalculates the panel. 
            //
            // �p�l�����Čv�Z���܂��B
            Data.PanelManager.CreatePanel(GameData.MovieSizePoint, Data.StageSetting);
        }


        /// <summary>
        /// Moves the cursor position.
        /// 
        /// �J�[�\���ʒu���ړ����܂��B
        /// </summary>
        private CursorPosition CursorMove()
        {
            int length = (int)CursorPosition.Count;
            return (CursorPosition)(((int)cursor + 1) % length);
        }


        /// <summary>
        /// Sets the number of divisions.
        ///
        /// �������̐ݒ�����܂��B
        /// </summary>
        private void SetDivide(int offset)
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadTriggers triggers = virtualPad.Triggers;

            Point divide = Data.StageSetting.Divide;

            if (cursor == CursorPosition.Left)
            {
                // Calculates the number of horizontal divisions.
                //
                // ���̕��������v�Z���܂��B
                divide.X += offset;

                // Replays the sequence.
                //
                // �V�[�P���X�����v���C���܂��B
                seqLeftUp.Replay();
            }
            else if (cursor == CursorPosition.Right)
            {
                // Calculates the number of vertical divisions.
                //
                // �c�̕��������v�Z���܂��B
                divide.Y += offset;

                // Replays the sequence.
                // 
                // �V�[�P���X�����v���C���܂��B
                seqRightUp.Replay();
            }

            // Limits the number of divisions.
            // The limitation is cancelled if the L trigger or R trigger are pressed.
            // 
            // �������̐��������܂��B
            // L�g���K�[��R�g���K�[��������Ă����ꍇ�͐������������܂��B
            if (triggers.Left[VirtualKeyState.Free] || 
                triggers.Right[VirtualKeyState.Free])
            {
                int max;
                int min;

                max = DivideMax.X;
                min = DivideMin.X;
                divide.X = (divide.X > max) ? max : divide.X;
                divide.X = (divide.X < min) ? min : divide.X;

                max = DivideMax.Y;
                min = DivideMin.Y;
                divide.Y = (divide.Y > max) ? max : divide.Y;
                divide.Y = (divide.Y < min) ? min : divide.Y;
            }

            // Sets the number of divisions.
            //
            // ��������ݒ肵�܂��B
            Data.StageSetting.Divide = divide;

            // Re-creates the panel.
            // 
            // �p�l�����č쐬���܂��B
            CalcDivide();

            // Plays the SoundEffect for change in number of divisions.
            // 
            // �������ύX��SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }
        #endregion
    }
}
