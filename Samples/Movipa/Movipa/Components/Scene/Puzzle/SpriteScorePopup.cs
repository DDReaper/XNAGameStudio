#region File Description
//-----------------------------------------------------------------------------
// SpriteScorePopup.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Movipa.Util;
#endregion

namespace Movipa.Components.Scene.Puzzle
{
    /// <summary>
    /// Sprite class that draws the score.
    /// This class inherits the Sprite class and performs update and drawing
    /// processing. At first appearance, it performs a bounce before shifting to 
    /// the designated position. Once the designated position is attained, it
    /// performs a fade-out; when this has finished, it performs release processing.
    /// 
    /// �X�R�A��`�悷��X�v���C�g�̃N���X�ł��B
    /// ���̃N���X��Sprite�N���X���p�����A�X�V�ƕ`��̏������s���܂��B
    /// �o�����A�܂��o�E���h���s���A�o�E���h���I��������w���
    /// �ʒu�ֈړ����܂��B�w��̈ʒu�֓���������t�F�[�h�A�E�g���J�n���A
    /// �t�F�[�h�A�E�g����������ƁA�J���������s���܂��B
    /// </summary>
    public class SpriteScorePopup : Sprite
    {
        #region Private Types
        /// <summary>
        /// Processing status
        /// 
        /// �������
        /// </summary>
        enum Phase
        {
            /// <summary>
            /// Bounce
            /// 
            /// �o�E���h
            /// </summary>
            Bound,

            /// <summary>
            /// Move
            /// 
            /// �ړ�
            /// </summary>
            Move,

            /// <summary>
            /// Fade-out
            /// 
            /// �t�F�[�h�A�E�g
            /// </summary>
            FadeOut,
        }
        #endregion

        #region Fields
        public int Score;
        public Vector2 DefaultPosition;
        public Vector2 TargetPosition;

        private Phase phase;
        private SpriteFont font;
        private float jumpPower;
        private float jumpBoundPower;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SpriteScorePopup(Game game)
            : base(game)
        {
            MovipaGame movipaGame = game as MovipaGame;
            if (movipaGame != null)
            {
                font = movipaGame.MediumFont;
            }
            Updating += SpriteScorePopupUpdating;
            Drawing += SpriteScorePopupDrawing;
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
            // ������Ԃ̏����ݒ���s���܂��B
            phase = Phase.Bound;

            // Sets the jump parameters.
            // 
            // �W�����v�̃p�����[�^��ݒ肵�܂��B
            jumpPower = 8.0f;
            jumpBoundPower = 8.0f;

            // Sets the draw color.
            //
            // �`��F��ݒ肵�܂��B
            Color = Color.White;

            base.Initialize();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
        /// </summary>
        void SpriteScorePopupUpdating(object sender, UpdatingEventArgs args)
        {
            if (phase == Phase.Bound)
            {
                // Performs updates from first appearance through to end of bounce.
                // 
                // �o������o�E���h�I���܂ł̍X�V�������s���܂��B
                UpdateBound();
            }
            else if (phase == Phase.Move)
            {
                // Performs updates following movement.
                // 
                // �ړ����̍X�V�������s���܂��B
                UpdateMove();
            }
            else if (phase == Phase.FadeOut)
            {
                // Performs updates following fade-out.
                // 
                // �t�F�[�h�A�E�g���̍X�V�������s���܂��B
                UpdateFadeOut();
            }
        }
        

        /// <summary>
        /// Performs updates from first appearance through to end of bounce.
        /// 
        /// �o������o�E���h�I���܂ł̍X�V�������s���܂��B
        /// </summary>
        private void UpdateBound()
        {
            // Shifts the Y coordinate.
            // 
            // Y���W���ړ������܂��B
            position.Y -= jumpPower;

            // Performs bounce processing if it is lower than the 
            // initial appearance coordinates.
            // 
            // �����o�����W�������������ꍇ�A�o�E���h���������܂��B
            if (Position.Y > DefaultPosition.Y)
            {
                // Sets the Y coordinate to the initial coordinate.
                // 
                // Y���W���������W�֐ݒ肵�܂��B
                position.Y = DefaultPosition.Y;

                // Reduces the bounce power.
                // 
                // �o�E���h�͂����炵�܂��B
                jumpBoundPower *= 0.5f;

                // Sets the amount of Y coordinate travel.
                // 
                // Y���W�̈ړ��ʂ�ݒ肵�܂��B
                jumpPower = jumpBoundPower;

                // Sets to Movement Processing when there is no more bounce power.
                // 
                // �o�E���h�͂������Ȃ����ꍇ�͈ړ������֐ݒ肵�܂��B
                if (jumpBoundPower < 0.1f)
                {
                    phase = Phase.Move;
                }
            }
            else
            {
                // Reduces the amount of Y coordinate travel.
                // 
                // Y���W�̈ړ��ʂ����炵�܂��B
                jumpPower -= 1.0f;
            }
        }


        /// <summary>
        /// Performs updates following movement.
        /// 
        /// �ړ����̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMove()
        {
            // Moves to the target position.
            // 
            // �ړI�n�܂ňړ������܂��B
            Position += (TargetPosition - Position) * 0.1f;

            // Performs fade-out upon arrival at the target position.
            // 
            // �ړI�n�ɓ��B�����ꍇ�̓t�F�[�h�A�E�g�������s���܂��B
            if (Vector2.Distance(TargetPosition, Position) < 1.0f)
            {
                phase = Phase.FadeOut;
            }
        }


        /// <summary>
        /// Performs updates following fade-out.
        /// 
        /// �t�F�[�h�A�E�g���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateFadeOut()
        {
            // Reduces the transparency color.
            // 
            // ���ߐF�����炵�܂��B
            Vector4 color = Color.ToVector4();
            color.W = MathHelper.Clamp(color.W - 0.1f, 0, 1);

            // Sets a new color.
            // 
            // �V�����F��ݒ肵�܂��B
            Color = new Color(color);

            // Performs release processing when it is totally transparent.
            // 
            // ���S�ɓ��߂��ꂽ��J���������s���܂��B
            if (color.W <= 0)
            {
                Dispose();
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs drawing processing.
        /// 
        /// �`�揈�����s���܂��B
        /// </summary>
        void SpriteScorePopupDrawing(object sender, DrawingEventArgs args)
        {
            string value = string.Format("{0:00}", Score);
            Vector2 position = Position - (font.MeasureString(value) * 0.5f);
            args.Batch.DrawString(font, value, position, Color);
        }
        #endregion
    }

}
