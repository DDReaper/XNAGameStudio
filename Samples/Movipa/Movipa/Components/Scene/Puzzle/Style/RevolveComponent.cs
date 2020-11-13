#region File Description
//-----------------------------------------------------------------------------
// RevolveComponent.cs
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

namespace Movipa.Components.Scene.Puzzle.Style
{
    /// <summary>
    /// Implements the Revolve Mode style.
    /// This class inherits the StyleBase class and implements 
    /// panel switching. It rotates four panels expanded one 
    /// down and one to the right of the cursor position.
    /// 
    /// ���{�������[�h�̃X�^�C�����������܂��B
    /// ���̃N���X��StyleBase�N���X���p�����A�p�l���̓���ւ��������������Ă��܂��B
    /// �J�[�\���̈ʒu����A�E�Ɖ�������1�����p�l�����g������4���̃p�l����
    /// ��]���鏈�����s���Ă��܂��B
    /// </summary>
    public class RevolveComponent : StyleBase
    {
        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public RevolveComponent(Game game, 
            StageSetting setting, Point cursor, PanelManager manager)
            : base(game, setting, cursor, manager)
        {
        }


        /// <summary>
        /// Loads the content. 
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the cursor texture.
            // 
            // �J�[�\���̃e�N�X�`����ǂݍ��݂܂��B
            string asset = "Textures/Puzzle/cursor";
            cursorTexture = Content.Load<Texture2D>(asset);

            base.LoadContent();
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
            // Checks the select enabled status.
            // 
            // �I��L����Ԃ��`�F�b�N���܂��B
            if (SelectEnabled)
            {
                // Moves the cursor.
                // 
                // �J�[�\���̈ړ��������s���܂��B
                UpdateCursor();

                // Replaces the cursor.
                // 
                // �J�[�\���̓���ւ��������s���܂��B
                RevolvePanel();
            }

            // Updates all panels.
            // 
            // �S�Ẵp�l���̍X�V�������s���܂��B
            UpdatePanels(gameTime);

            base.Update(gameTime);
        }


        /// <summary>
        /// Moves the cursor.
        /// 
        /// �J�[�\���ړ��������s���܂��B
        /// </summary>
        private void UpdateCursor()
        {
            // Manages the SoundEffect by the flag only once so that the SoundEffect 
            // will not be played simultaneously.
            // 
            // SoundEffect�̓����Đ���h�����߂Ɉ�x�t���O�ŊǗ����܂��B
            bool sePlay = false;

            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPushRepeat(dPad.Up, leftStick.Up))
            {
                // Performs Up key processing.
                // 
                // ��L�[�������ꂽ�Ƃ��̏������s���܂��B
                InputUpKey();

                // Sets the SoundEffect playback flag.
                // 
                // SoundEffect�̍Đ��t���O��ݒ肵�܂��B
                sePlay = true;
            }

            if (InputState.IsPushRepeat(dPad.Down, leftStick.Down))
            {
                // Performs Down key processing.
                // 
                // ���L�[�������ꂽ�Ƃ��̏������s���܂��B
                InputDownKey();

                // Sets the SoundEffect playback flag.
                // 
                // SoundEffect�̍Đ��t���O��ݒ肵�܂��B
                sePlay = true;
            }

            if (InputState.IsPushRepeat(dPad.Left, leftStick.Left))
            {
                // Performs Left key processing.
                // 
                // ���L�[�������ꂽ�Ƃ��̏������s���܂��B
                InputLeftKey();

                // Sets the SoundEffect playback flag.
                // 
                // SoundEffect�̍Đ��t���O��ݒ肵�܂��B
                sePlay = true;
            }

            if (InputState.IsPushRepeat(dPad.Right, leftStick.Right))
            {
                // Performs Right key processing.
                // 
                // �E�L�[�������ꂽ�Ƃ��̏������s���܂��B
                InputRightKey();

                // Sets the SoundEffect playback flag.
                // 
                // SoundEffect�̍Đ��t���O��ݒ肵�܂��B
                sePlay = true;
            }


            // Plays the SoundEffect if the sound playback flag is set.
            // 
            // �T�E���h�Đ��t���O���ݒ肳��Ă���΍Đ����܂��B
            if (sePlay)
            {
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor2);
            }
        }


        /// <summary>
        /// Performs Up key processing.
        /// 
        /// ��L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputUpKey()
        {
            int newX = cursor.X;
            int newY = cursor.Y - 1;

            if (newY < 0)
            {
                newY = stageSetting.Divide.Y - 2;
            }

            cursor = new Point(newX, newY);
        }


        /// <summary>
        /// Performs Down key processing.
        /// 
        /// ���L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputDownKey()
        {
            int newX = cursor.X;
            int newY = (cursor.Y + 1) % (stageSetting.Divide.Y - 1);
            cursor = new Point(newX, newY);
        }


        /// <summary>
        /// Performs Left key processing.
        /// 
        /// ���L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputLeftKey()
        {
            int newX = cursor.X - 1;
            int newY = cursor.Y;

            if (newX < 0)
            {
                newX = stageSetting.Divide.X - 2;
            }

            cursor = new Point(newX, newY);
        }


        /// <summary>
        /// Performs Right key processing.
        /// 
        /// �E�L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputRightKey()
        {
            int newX = (cursor.X + 1) % (stageSetting.Divide.X - 1);
            int newY = cursor.Y;
            cursor = new Point(newX, newY);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws cursors at all positions. 
        /// 
        /// �S�Ă̈ʒu�̃J�[�\����`�悵�܂��B
        /// </summary>
        public override void DrawCursor(GameTime gameTime)
        {
            // Sets rectangles of four panels consisting of the panels
            // at the cursor position expanded to the right and down.
            // 
            // �J�[�\���ʒu�̃p�l���ƁA�E�����Ɖ�������1�����̂΂���
            // 4���̃p�l���̋�`��ݒ肵�܂��B
            Rectangle[] cursorRect = {
                new Rectangle(
                    (int)(cursor.X * panelManager.PanelSize.X),
                    (int)(cursor.Y * panelManager.PanelSize.Y),
                    (int)(panelManager.PanelSize.X),
                    (int)(panelManager.PanelSize.Y)
                ),
                new Rectangle(
                    (int)(cursor.X * panelManager.PanelSize.X),
                    (int)((cursor.Y * panelManager.PanelSize.Y) +
                        panelManager.PanelSize.Y),
                    (int)(panelManager.PanelSize.X),
                    (int)(panelManager.PanelSize.Y)
                ),
                new Rectangle(
                    (int)((cursor.X * panelManager.PanelSize.X) +
                        panelManager.PanelSize.X),
                    (int)(cursor.Y * panelManager.PanelSize.Y),
                    (int)(panelManager.PanelSize.X),
                    (int)(panelManager.PanelSize.Y)
                ),
                new Rectangle(
                    (int)((cursor.X * panelManager.PanelSize.X) +
                        panelManager.PanelSize.X),
                    (int)((cursor.Y * panelManager.PanelSize.Y) +
                        panelManager.PanelSize.Y),
                    (int)(panelManager.PanelSize.X),
                    (int)(panelManager.PanelSize.Y)
                ),
            };

            // Sets the four-panel cursor draw flag.
            // 
            // 4���̃p�l���̃J�[�\���`��t���O��ݒ肵�܂��B
            PanelTypes[] types = {
                PanelTypes.LeftUp | PanelTypes.LeftDown | PanelTypes.RightUp,
                PanelTypes.LeftUp | PanelTypes.LeftDown | PanelTypes.RightDown,
                PanelTypes.LeftUp | PanelTypes.RightUp | PanelTypes.RightDown,
                PanelTypes.LeftDown | PanelTypes.RightUp | PanelTypes.RightDown
            };


            Batch.Begin();
            
            // Draws the four-panel cursor.
            // 
            // 4���̃p�l���̃J�[�\����`�悵�܂��B
            for (int i = 0; i < cursorRect.Length; i++)
            {
                DrawCursor(gameTime, cursorRect[i], Color.White, types[i]);
            }

            // Draws the central cursor.
            // 
            // �����̃J�[�\����`�悵�܂��B
            DrawCenterCursor();

            Batch.End();
        }


        /// <summary>
        /// Draws the central cursor.
        /// 
        /// �����̃J�[�\����`�悵�܂��B
        /// </summary>
        public void DrawCenterCursor()
        {
            Rectangle cursorSize = new Rectangle(0, 0, 48, 48);
            
            // Calculates the coordinates of the texture source.
            // 
            // �e�N�X�`���̓]�����̍��W���v�Z���܂��B
            Rectangle centerCursorArea = new Rectangle();
            centerCursorArea.X = 192;
            centerCursorArea.Y = (cursorSize.Height * 2) * 0;
            centerCursorArea.Width = cursorSize.Width;
            centerCursorArea.Height = cursorSize.Height;

            // Calculates the source coordinates.
            // 
            // �]����̍��W���v�Z���܂��B
            Vector2 centerCursorPosition = new Vector2();
            centerCursorPosition.X = 
                (cursor.X * panelManager.PanelSize.X) + panelManager.PanelSize.X;
            centerCursorPosition.Y = 
                (cursor.Y * panelManager.PanelSize.Y) + panelManager.PanelSize.Y;
            centerCursorPosition.X -= (cursorSize.Width * 0.5f);
            centerCursorPosition.Y -= (cursorSize.Height * 0.5f);

            // Draws the cursor.
            // 
            // �J�[�\����`�悵�܂��B
            Vector2 position = centerCursorPosition + panelManager.DrawOffset;
            Batch.Draw(cursorTexture, position, centerCursorArea, Color.White);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Switches the panels.
        /// 
        /// �p�l���̓���ւ��������s���܂��B
        /// </summary>
        private void RevolvePanel()
        {
            VirtualPadState virtualPad =
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (buttons.A[VirtualKeyState.Push])
            {
                // Left rotates and switches the panel when the A button is pressed.
                // 
                // A�{�^���������ꂽ�獶��]�̃p�l������ւ��������s���܂��B
                if (RevolvePanelLeft())
                {
                    // Plays the SoundEffect if the panel switch is successful.
                    // 
                    // ����ւ��ɐ���������SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                    // Adds the move count.
                    // 
                    // �ړ��J�E���g�����Z���܂��B
                    moveCount++;
                }
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Right rotates and switches the panel when the B button is pressed.
                // 
                // B�{�^���������ꂽ��E��]�̃p�l������ւ��������s���܂��B
                if (RevolvePanelRight())
                {
                    // Plays the SoundEffect if the panel switching is successful.
                    // 
                    // ����ւ��ɐ���������SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                    // Adds the move count.
                    // 
                    // �ړ��J�E���g�����Z���܂��B
                    moveCount++;
                }
            }
        }


        /// <summary>
        /// Left rotates and switches the panel.
        /// 
        /// ����]�̃p�l������ւ��������s���܂��B
        /// </summary>
        private bool RevolvePanelLeft()
        {
            Point[] panelPoints = new Point[] {
                new Point(cursor.X + 0, cursor.Y + 0),
                new Point(cursor.X + 1, cursor.Y + 0),
                new Point(cursor.X + 1, cursor.Y + 1),
                new Point(cursor.X + 0, cursor.Y + 1),
            };

            // Specifies the panel array and performs switching. 
            // 
            // �p�l���̔z����w�肵�ē���ւ��������s���܂��B
            return RevolvePanel(panelPoints);
        }


        /// <summary>
        /// Right rotates and switches the panel.
        /// 
        /// �E��]�̃p�l������ւ��������s���܂��B
        /// </summary>
        private bool RevolvePanelRight()
        {
            Point[] panelPoints = new Point[] {
                new Point(cursor.X + 0, cursor.Y + 0),
                new Point(cursor.X + 0, cursor.Y + 1),
                new Point(cursor.X + 1, cursor.Y + 1),
                new Point(cursor.X + 1, cursor.Y + 0),
            };

            // Specifies the panel array and performs switching. 
            // 
            // �p�l���̔z����w�肵�ē���ւ��������s���܂��B
            return RevolvePanel(panelPoints);
        }


        /// <summary>
        /// Specifies the panel array and performs switching.
        /// 
        /// �p�l���̔z����w�肵�ē���ւ��������s���܂��B
        /// </summary>
        private bool RevolvePanel(Point[] panelPoints)
        {
            // Processing is not performed if the panel is in action.
            // 
            // �p�l�������쒆�̏ꍇ�͏������s���܂���B
            if (IsPanelAction)
                return false;

            // Sets the switching panel array.
            // 
            // ����ւ���p�l���̔z���ݒ肵�܂��B
            PanelData[] panels = { 
                panelManager.GetPanel(panelPoints[0]),
                panelManager.GetPanel(panelPoints[1]),
                panelManager.GetPanel(panelPoints[2]),
                panelManager.GetPanel(panelPoints[3]),
            };

            // Processing is not performed if the switching panel status is not Normal.
            // 
            // ����ւ���p�l�����ʏ��Ԃł͂Ȃ��ꍇ�A�������s���܂���B
            foreach (PanelData panelData in panels)
            {
                if (panelData.Status != PanelData.PanelStatus.None)
                {
                    return false;
                }
            }

            // Sets the panel From and To positions.
            // 
            // �p�l���̈ړ����ƁA�ړ����ݒ肵�܂��B
            Vector2[,] panelFromTo = {
                {
                    panels[0].Position,
                    panels[1].Position,
                },
                {
                    panels[1].Position,
                    panels[2].Position,
                },
                {
                    panels[2].Position,
                    panels[3].Position,
                },
                {
                    panels[3].Position,
                    panels[0].Position,
                },
            };

            // Changes and sets the draw position.
            // 
            // �`��ʒu�̕ύX�ƁA�ݒ���s���܂��B
            for (int i = 0; i < 4; i++)
            {
                panels[i].FromPosition = panelFromTo[i, 0];
                panels[i].ToPosition = panelFromTo[i, 1];
                panels[i].Status = PanelData.PanelStatus.Move;
                panels[i].Color = ActivePanelColor;
                panels[i].MoveCount = 1.0f;
            }

            // Exchanges the panel data.
            // 
            // �p�l���̃f�[�^���������܂��B
            panelManager.SetPanel(panelPoints[0], panels[3]);
            panelManager.SetPanel(panelPoints[1], panels[0]);
            panelManager.SetPanel(panelPoints[2], panels[1]);
            panelManager.SetPanel(panelPoints[3], panels[2]);

            return true;
        }


        /// <summary>
        /// Switches panels at random.
        /// 
        /// �����_���Ńp�l�������ւ��܂��B
        /// </summary>
        public override bool RandomShuffle()
        {
            // Processing is not performed if the panel is in action.
            // 
            // �p�l�������쒆�Ȃ珈�����s���܂���B
            if (IsPanelAction)
                return false;

            // Obtains the target panel.
            // 
            // �Ώۂ̃p�l�����擾���܂��B
            cursor = panelManager.GetRandomPanel(stageSetting);

            // Sets the rotation direction at random.
            // 
            // �����_���ŉ�]������ݒ肵�܂��B
            if (Random.Next(2) == 0)
            {
                // Performs left rotation panel switching.
                // 
                // ����]�̃p�l������ւ��������s���܂��B
                if (RevolvePanelLeft())
                {
                    // Plays the SoundEffect if panel switching is successful.
                    // 
                    // ����ւ�������������SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
                }
            }
            else
            {
                // Performs right rotation panel switching.
                // 
                // �E��]�̃p�l������ւ��������s���܂��B
                if (RevolvePanelRight())
                {
                    // Plays the SoundEffect if panel switching is successful.
                    // 
                    // ����ւ�������������SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
                }
            }

            return true;
        }
        #endregion
    }
}


