#region File Description
//-----------------------------------------------------------------------------
// Ready.cs
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
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// This menu is used to display the confirmation
    /// screen. It inherits MenuBaseance 
    /// and expands menu compilation processing. Menu 
    /// selection is followed by fade-out and 
    /// transition to the puzzle scene.
    /// 
    /// �m�F�̉�ʂ�\�����郁�j���[
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ���̃��j���[�Ō��肳���΁A�t�F�[�h�A�E�g���A�p�Y���̃V�[����
    /// �J�ڂ��܂��B
    /// </summary>
    public class Ready : MenuBase
    {
        #region Private Types
        /// <summary>
        ///Processing status
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
            /// Select
            ///
            /// �I�����o
            /// </summary>
            Selected,
        }
        #endregion

        #region Fields
        // Movie render area
        //
        // ���[�r�[�̕`��̈�
        private readonly Rectangle MoviePreviewRect;

        // Processing details
        //
        //�������e
        private Phase phase;

        // Sequence
        // 
        // �V�[�P���X
        private SequencePlayData seqStart;
        private SequencePlayData seqLoop;
        private SequencePlayData seqMovieWindow;
        private SequencePlayData seqPosStart;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="data">
        /// Common data structure
        ///
        /// ���ʃf�[�^�\����
        /// </param>
        public Ready(Game game, MenuData data)
            : base(game, data)
        {
            // Loads the position.
            //
            // �|�W�V������ǂݍ��݂܂��B
            Point point;
            PatternGroupData patternGroup;
            patternGroup = data.sceneData.PatternGroupDictionary["Pos_PosMovie"];
            point = patternGroup.PatternObjectList[0].Position;
            MoviePreviewRect = new Rectangle(point.X, point.Y, 640, 360);
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
            seqStart = Data.sceneData.CreatePlaySeqData("ReadyStart");
            seqLoop = Data.sceneData.CreatePlaySeqData("ReadyLoop");
            seqMovieWindow = Data.sceneData.CreatePlaySeqData("MovieWindow");
            seqPosStart = Data.sceneData.CreatePlaySeqData("PosReadyStart");

            seqStart.Replay();
            seqPosStart.Replay();
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
            // �V�[�P���X�̍X�V
            UpdateSequence(gameTime);

            if (phase == Phase.Start)
            {
                // Sets to selection process after start animation finishes.
                // 
                // �J�n�A�j���[�V�������I��������I�������ɐݒ肵�܂��B
                if (!seqStart.SequenceData.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                // Performs update processing at selection.
                //
                // �I�����̍X�V�������s���܂��B
                return UpdateSelect();
            }
            else if (phase == Phase.Selected)
            {
                // Registers puzzle scene then perform fade-out.
                // 
                // �p�Y���̃V�[����o�^���āA�t�F�[�h�A�E�g���܂��B
                GameData.SceneQueue.Enqueue(
                    new Puzzle.PuzzleComponent(Game, Data.StageSetting));
                GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
            }

            return null;
        }


        /// <summary>
        /// Performs update processing at selection.
        /// 
        /// �I�����̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (buttons.A[VirtualKeyState.Push])
            {
                // Performs the post-selection process when the enter button is pressed. 
                //
                // ����{�^���������ꂽ��I����̏����ɐݒ肵�܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                phase = Phase.Selected;
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Returns to split settings when the cancel button is pressed.
                // 
                // �L�����Z���{�^���������ꂽ�̂ŁA�������̐ݒ�ɖ߂�܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
                return CreateMenu(Game, MenuType.SelectDivide, Data);
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
            seqMovieWindow.Update(gameTime.ElapsedGameTime);
            seqPosStart.Update(gameTime.ElapsedGameTime);
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
            // Draws the movie window. 
            //
            // ���[�r�[�E�B���h�E��`�悵�܂��B
            batch.Begin();
            seqMovieWindow.Draw(batch, null);
            batch.End();

            // Draws the split preview. 
            //
            // �����v���r���[��`�悵�܂��B
            DrawDivideTexture(batch);

            batch.Begin();

            if (phase == Phase.Start)
            {
                seqStart.Draw(batch, null);
            }
            else if (phase == Phase.Select)
            {
                seqLoop.Draw(batch, null);

                // Draws the navigate.
                // 
                // �i�r�Q�[�g�̕`��
                DrawNavigate(gameTime, batch, false);
            }
            else if (phase == Phase.Selected)
            {
                seqLoop.Draw(batch, null);
            }

            DrawSequenceString(batch);

            batch.End();
        }


        /// <summary>
        /// Draws the split preview.
        /// 
        /// �����v���r���[��`�悵�܂��B
        /// </summary>
        private void DrawDivideTexture(SpriteBatch batch)
        {
            // Unable to draw if no texture.
            // 
            // �e�N�X�`���������ꍇ�͕`�揈�����s���܂���B
            if (Data.divideTexture == null)
                return;

            // Draws with no alpha.
            //
            // �A���t�@�����ŕ`������܂��B
            batch.Begin(SpriteBlendMode.None);
            batch.Draw(Data.divideTexture, MoviePreviewRect, Color.White);
            batch.End();
        }


        /// <summary>
        /// Draws text string based on sequence.
        /// 
        /// �V�[�P���X�����ɕ������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch)
        {
            SequenceBankData seqData;
            seqData = seqPosStart.SequenceData;
            foreach (SequenceGroupData seqBodyData in seqData.SequenceGroupList)
            {
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;
                if (seqPartsData == null)
                {
                    continue;
                }

                List<PatternObjectData> list = seqPartsData.PatternObjectList;
                foreach (PatternObjectData patPartsData in list)
                {
                    DrawData putInfoData = patPartsData.InterpolationDrawData;
                    SpriteFont font = MediumFont;
                    Color color = putInfoData.Color;

                    string text = GetDrawString();
                    Point point = putInfoData.Position;
                    Vector2 position = new Vector2(point.X, point.Y);
                    position -= font.MeasureString(text) * 0.5f;

                    batch.DrawString(font, text, position, color);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the text string for drawing.
        /// 
        /// �`��p�̕�������擾���܂��B
        /// </summary>
        /// <returns>
        /// Text string for drawing
        ///
        /// �`��p�̕�����
        /// </returns>
        private string GetDrawString()
        {
            Point divide = Data.StageSetting.Divide;

            string text = String.Empty;
            text += string.Format("Style : {0}\n", Data.StageSetting.Style);
            text += string.Format("Rotate : {0}\n", Data.StageSetting.Rotate);
            text += string.Format("Movie : {0}\n", Data.movie.Info.Name);
            text += string.Format("Divide : {0:00}x{1:00}", divide.X, divide.Y);

            return text;
        }
        #endregion
    }
}
