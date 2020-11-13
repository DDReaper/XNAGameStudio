#region File Description
//-----------------------------------------------------------------------------
// ChangeComponent.cs
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
    /// Implements the Change Mode style.
    /// This class inherits the StyleBase class and implements panel swapping.
    /// It swaps between the two panels and rotates the panels.
    /// 
    /// �`�F���W���[�h�̃X�^�C�����������܂��B
    /// ���̃N���X��StyleBase�N���X���p�����A�p�l���̓���ւ��������������Ă��܂��B
    /// 2�̃p�l�������ւ��鏈���ƁA�p�l������]�����鏈�����s���Ă��܂��B
    /// </summary>
    public class ChangeComponent : StyleBase
    {
        #region Fields
        // Panel holding status
        // 
        // �p�l���̕ێ����
        private bool isPanelHold = false;

        // Coordinates of holding panel
        // 
        // �z�[���h���Ă���p�l���̍��W
        private Point panelHold = new Point();
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance. 
        /// 
        /// �C���X�^���X�̏����������܂��B
        /// </summary>
        public ChangeComponent(Game game, 
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
            // Checks for selection enabled status.
            // 
            // �I��L����Ԃ��`�F�b�N���܂��B
            if (SelectEnabled)
            {
                // Moves the cursor.
                // 
                // �J�[�\���̈ړ��������s���܂��B
                UpdateCursor();

                // Performs panel operation.
                // 
                // �p�l���̑�����s���܂��B
                UpdatePanel();
            }

            // Updates all panels.
            // 
            // �S�Ẵp�l���̍X�V�������s���܂��B
            UpdatePanels(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Performs panel operation.
        /// 
        /// �p�l���̑�����s���܂��B
        /// </summary>
        private void UpdatePanel()
        {
            // Processing is not performed if the panel is currently in action.
            // 
            // �p�l�������쒆�Ȃ�Ώ������s���܂���B
            if (IsPanelAction)
                return;

            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (buttons.A[VirtualKeyState.Push])
            {
                // Obtains the panel.
                // 
                // �p�l�����擾���܂��B
                PanelData panel = panelManager.GetPanel(cursor);

                // Processing is not performed if the panel is disabled.
                // 
                // �p�l���������̏�ԂȂ�Ώ��������܂���B
                if (!panel.Enabled)
                    return;

                // Checks the hold status.
                // 
                // �z�[���h��Ԃ��`�F�b�N���܂��B
                if (isPanelHold == false)
                {
                    // There is no held panel, so the 
                    // selected panel is held.
                    // 
                    // �z�[���h���Ă���p�l���������̂�
                    // �I�����Ă���p�l�����z�[���h���܂��B
                    if (HoldPanel())
                    {
                        // Plays the SoundEffect if the hold is successful.
                        // 
                        // �z�[���h�ɐ���������SoundEffect���Đ����܂��B
                        GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
                    }
                }
                else
                {
                    // There is already a held panel, so the 
                    // selected panel is swapped with the panel
                    // that has already been held.
                    // 
                    // ���Ƀz�[���h���Ă���p�l��������̂ŁA
                    // �I�����Ă���p�l���ƁA�z�[���h�ς݂̃p�l����
                    // ����ւ��鏈�����s���܂��B
                    if (SwapPanel())
                    {
                        // Plays the SoundEffect if the swap is successful.
                        // 
                        // ����ւ��ɐ���������SoundEffect���Đ����܂��B
                        GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                        // Calculates the movement count. 
                        // 
                        // �ړ��J�E���g�����Z���܂��B
                        moveCount++;
                    }
                }
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Releases the held panel.
                // 
                // �z�[���h���Ă���p�l�����J�����܂��B
                ReleasePanel();
            }

            // Controls panel rotation.
            // 
            // �p�l���̉�]�̐�����s���܂��B
            RotatePanel();
        }


        /// <summary>
        /// Performs cursor movement processing.
        /// 
        /// �J�[�\���ړ��������s���܂��B
        /// </summary>
        private void UpdateCursor()
        {
            // Manages the SoundEffect by the flag only once so that
            // the SoundEffect will not be played simultaneously.
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

                // Sets the SoundEffect play flag.
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

                // Sets the SoundEffect play flag.
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

                // Sets the SoundEffect play flag.
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

                // Sets the SoundEffect play flag.
                // 
                // SoundEffect�̍Đ��t���O��ݒ肵�܂��B
                sePlay = true;
            }


            // Plays sound if the sound play flag is set.
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
            // Sets the cursor position.
            // 
            // �J�[�\���ʒu��ݒ�
            int newX = cursor.X;
            int newY = cursor.Y - 1;

            if (newY < 0)
            {
                newY = stageSetting.Divide.Y - 1;
            }

            cursor.X = newX;
            cursor.Y = newY;
        }


        /// <summary>
        /// Performs Down key processing.
        /// 
        /// ���L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputDownKey()
        {
            // Sets the cursor position.
            // 
            // �J�[�\���ʒu��ݒ�
            int newX = cursor.X;
            int newY = (cursor.Y + 1) % stageSetting.Divide.Y;
            cursor.X = newX;
            cursor.Y = newY;
        }


        /// <summary>
        /// Performs Left key processing.
        /// 
        /// ���L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputLeftKey()
        {
            // Sets the cursor position.
            // 
            // �J�[�\���ʒu��ݒ�
            int newX = cursor.X - 1;
            int newY = cursor.Y;

            if (newX < 0)
            {
                newX = stageSetting.Divide.X - 1;
            }

            cursor.X = newX;
            cursor.Y = newY;
        }


        /// <summary>
        /// Performs Right key processing.
        /// 
        /// �E�L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputRightKey()
        {
            // Sets the cursor position.
            // 
            // �J�[�\���ʒu��ݒ�
            int newX = (cursor.X + 1) % stageSetting.Divide.X;
            int newY = cursor.Y;

            cursor.X = newX;
            cursor.Y = newY;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws all position cursors.
        /// 
        /// �S�Ă̈ʒu�̃J�[�\����`�悵�܂��B
        /// </summary>
        public override void DrawCursor(GameTime gameTime)
        {
            Batch.Begin();

            // Draws the hold cursor.
            // 
            // �z�[���h�J�[�\����`�悵�܂��B
            DrawHoldCursor(gameTime);

            // Draws the normal cursor.
            // 
            // �ʏ�̃J�[�\����`�悵�܂��B
            DrawNormalCursor(gameTime);
            
            Batch.End();
        }


        /// <summary>
        /// Draws the normal cursor.
        /// 
        /// �ʏ�̃J�[�\����`�悵�܂��B
        /// </summary>
        private void DrawNormalCursor(GameTime gameTime)
        {
            // Obtains the panel at the cursor position.
            // 
            // �J�[�\���ʒu�ɂ���p�l�����擾���܂��B
            PanelData panel = panelManager.GetPanel(cursor);

            // Obtains the cursor rectangle.
            // 
            // �J�[�\���̋�`���擾���܂��B
            Rectangle rectangle = panel.RectanglePosition;

            // Draws all four corners of the cursor.
            // 
            // �l���S�ẴJ�[�\����`�悵�܂��B
            DrawCursor(gameTime, rectangle, Color.White, PanelTypes.All);
        }


        /// <summary>
        /// Draws the hold cursor.
        /// 
        /// �z�[���h�J�[�\����`�悵�܂��B
        /// </summary>
        private void DrawHoldCursor(GameTime gameTime)
        {
            // Drawing processing is not performed if there is no hold status. 
            // 
            // �z�[���h��Ԃ������ꍇ�͕`�揈�����s���܂���B
            if (!isPanelHold)
                return;

            // Obtains the held panels.
            // 
            // �z�[���h���Ă���p�l�����擾���܂��B
            PanelData panel = panelManager.GetPanel(panelHold);

            // Obtains the cursor rectangle.
            // 
            // �J�[�\���̋�`���擾���܂��B
            Rectangle rectangle = panel.RectanglePosition;

            // Draws all four corners of the cursor.
            // 
            // �l���S�ẴJ�[�\����`�悵�܂��B
            DrawCursor(gameTime, rectangle, Color.Red, PanelTypes.All);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Swaps the panels.
        /// 
        /// �p�l���̌����������s���܂��B
        /// </summary>
        private bool SwapPanel()
        {
            // Processing is not performed if it is the same panel.
            // 
            // �����p�l���Ȃ珈�����s���܂���B
            if (cursor.Equals(panelHold) == true)
            {
                return false;
            }

            // Arranges the swap panels in an array.
            // 
            // ����ւ���p�l����z��ɃZ�b�g���܂��B
            PanelData[] panels =
            {
                panelManager.GetPanel(panelHold),
                panelManager.GetPanel(cursor),
            };

            // Processing is not performed if the panel is currently in action.
            // 
            // �p�l�������쒆�Ȃ珈�����s���܂���B
            if (panels[0].Status != PanelData.PanelStatus.None ||
                panels[1].Status != PanelData.PanelStatus.None)
            {
                return false;
            }

            // Sets the panel From and To positions.
            // 
            // �p�l���̈ړ����ƁA�ړ����ݒ肵�܂��B
            Vector2[,] panelFromTo = {
                {
                    panels[0].Position,
                    panels[1].Position
                },
                {
                    panels[1].Position,
                    panels[0].Position
                }
            };

            // Changes the draw position.
            // 
            // �`��ʒu�̕ύX�����܂��B
            panels[0].FromPosition = panelFromTo[0, 0];
            panels[0].ToPosition = panelFromTo[0, 1];
            panels[1].FromPosition = panelFromTo[1, 0];
            panels[1].ToPosition = panelFromTo[1, 1];

            // Swaps the panel data.
            // 
            // �p�l���̃f�[�^���������܂��B
            panelManager.SetPanel(panelHold, panels[1]);
            panelManager.SetPanel(cursor, panels[0]);

            // Sets the panel status.
            // 
            // �p�l���̏�Ԃ�ݒ肵�܂��B
            panels[0].Status = PanelData.PanelStatus.Move;
            panels[1].Status = PanelData.PanelStatus.Move;
            panels[0].Color = ActivePanelColor;
            panels[1].Color = ActivePanelColor;
            panels[0].MoveCount = 1.0f;
            panels[1].MoveCount = 1.0f;

            // Cancels the hold status.
            // 
            // �z�[���h��Ԃ��������܂��B
            isPanelHold = false;

            return true;

        }


        /// <summary>
        /// Holds the panel.
        /// 
        /// �p�l�����z�[���h���܂��B
        /// </summary>
        private bool HoldPanel()
        {
            // Obtains the panel at the cursor position.
            // 
            // �J�[�\���ʒu�ɂ���p�l�����擾���܂��B
            PanelData panel = panelManager.GetPanel(cursor);

            // Processing is not performed if the panel is currently in action.
            //
            // �p�l�������쒆�Ȃ珈�����s���܂���B
            if (panel.Status != PanelData.PanelStatus.None)
                return false;

            // Sets to the held panel.
            //
            // �z�[���h�p�l���ɐݒ肵�܂��B
            panelHold.X = cursor.X;
            panelHold.Y = cursor.Y;
            isPanelHold = true;

            return true;
        }


        /// <summary>
        /// Releases the held panel.
        /// 
        /// �z�[���h���Ă���p�l�����J�����܂��B
        /// </summary>
        private void ReleasePanel()
        {
            // If there are held panels, the hold status
            // is cancelled and the canceled SoundEffect is played. 
            // 
            // �z�[���h���Ă���p�l��������Ȃ�A�z�[���h��Ԃ�
            // �������ăL�����Z����SoundEffect���Đ����܂��B
            if (isPanelHold == true)
            {
                isPanelHold = false;
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
            }
        }


        /// <summary>
        /// Controls panel rotation.
        /// 
        /// �p�l���̉�]�̐�����s���܂��B
        /// </summary>
        private void RotatePanel()
        {

            // Processing is not performed if rotation is disabled.
            // 
            // ��]�������̏ꍇ�͏������s���܂���B
            if (stageSetting.Rotate == StageSetting.RotateMode.Off)
                return;

            // Processing is not performed in Hold status.
            // 
            // �z�[���h��ԂȂ珈�����s���܂���B
            if (isPanelHold)
                return;


            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            
            // Obtains the panel at the cursor position.
            // 
            // �J�[�\���ʒu�ɂ���p�l�����擾���܂��B
            PanelData panel = panelManager.GetPanel(cursor);

            // Processing is not performed if the panel status is Completed.
            // 
            // �p�l���̏�Ԃ������ς݂Ȃ珈�����s���܂���B
            if (!panel.Enabled)
                return;

            if (buttons.Y[VirtualKeyState.Push])
            {
                // Rotates the panel to the left when the Y button is pressed.
                // 
                // Y�{�^���������ꂽ��p�l��������]�����܂��B
                if (RotatePanelLeft(panel))
                {
                    // Plays the SoundEffect during rotation.
                    // 
                    // ��]����SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                    // Calculates the number of moves.
                    // 
                    // �ړ��񐔂����Z���܂��B
                    moveCount++;
                }
            }
            else if (buttons.X[VirtualKeyState.Push])
            {
                    // Rotates the panel to the right when the X button is pressed.
                    // 
                // X�{�^���������ꂽ��p�l�����E��]�����܂��B
                if (RotatePanelRight(panel))
                {
                    // Plays the SoundEffect during rotation.
                    // 
                    // ��]����SoundEffect���Đ����܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                    // Calculates the number of moves.
                    // 
                    // �ړ��񐔂����Z���܂��B
                    moveCount++;
                }
            }
        }


        /// <summary>
        /// Rotates the panel to the left.
        /// 
        /// �p�l��������]�����܂��B
        /// </summary>
        private bool RotatePanelLeft(PanelData panelData)
        {
            if (panelData.Status != PanelData.PanelStatus.None)
                return false;

            // Changes the panel status.
            // 
            // �p�l���̏�Ԃ�ύX���܂��B
            panelData.Color = ActivePanelColor;
            panelData.ToRotate += 90;
            panelData.ToRotate %= 360;
            panelData.Status = PanelData.PanelStatus.RotateLeft;

            return true;
        }


        /// <summary>
        /// Rotates the panel to the right.
        /// 
        /// �p�l�����E��]�����܂��B
        /// </summary>
        private bool RotatePanelRight(PanelData panelData)
        {
            if (panelData.Status != PanelData.PanelStatus.None)
                return false;

            // Changes the panel status.
            // 
            // �p�l���̏�Ԃ�ύX���܂��B
            panelData.Color = ActivePanelColor;
            panelData.ToRotate += 270;
            panelData.ToRotate %= 360;
            panelData.Status = PanelData.PanelStatus.RotateRight;

            return true;
        }


        /// <summary>
        /// Replaces the panels at random.
        /// 
        /// �����_���Ńp�l�������ւ��܂��B
        /// </summary>
        public override bool RandomShuffle()
        {
            // Processing is not performed if the panel is currently in action.
            // 
            // �p�l�������쒆�Ȃ珈�����s���܂���B
            if (IsPanelAction)
                return false;

            // Randomly sets rotation.
            // 
            // ��]�̏����������_���Őݒ肵�܂��B
            bool rotate = false;
            if (stageSetting.Rotate == StageSetting.RotateMode.On)
            {
                // If rotation is enabled and the randomly generated value is
                // 0, shuffle rotation is enabled.
                // 
                // ��]���L���ŁA�����_���ŏo�������l��0�Ȃ�
                // ��]�̃V���b�t����L���ɂ��܂��B
                rotate = (Random.Next(0, 2) == 0);
            }

            if (rotate)
            {
                // Rotates the panel at random.
                //
                // �����_���Ńp�l������]�����܂��B
                RandomShuffleRotate();
            }
            else
            {
                // Replaces the panels at random.
                // 
                // �����_���Ńp�l�������ւ��܂��B
                RandomShuffleSwap();
            }

            return true;
        }


        /// <summary>
        /// Rotates the panel at random.
        /// 
        /// �����_���Ńp�l������]�����܂��B
        /// </summary>
        private void RandomShuffleRotate()
        {
            // Obtains the target panel.
            // 
            // �Ώۂ̃p�l�����擾���܂��B
            cursor = panelManager.GetRandomPanel(stageSetting);

            // Obtains the panel at the cursor position.
            //
            // �J�[�\���ʒu�ɂ���p�l�����擾���܂��B
            PanelData panel = panelManager.GetPanel(cursor);

            // Randomly sets the rotation direction.
            // 
            // �����_���ŉ�]������ݒ肵�܂��B
            if (Random.Next(2) == 0)
            {
                RotatePanelLeft(panel);
            }
            else
            {
                RotatePanelRight(panel);
            }

            // Plays the SoundEffect during rotation.
            // 
            // ��]��SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }


        /// <summary>
        /// Replaces the panel at random.
        /// 
        /// �����_���Ńp�l�������ւ��܂��B
        /// </summary>
        private void RandomShuffleSwap()
        {
            Point holdPanel = new Point();
            Point swapPanel = new Point();

            // Selects the panel to hold.
            // 
            // �z�[���h����p�l����I�����܂��B
            holdPanel = panelManager.GetRandomPanel(stageSetting);

            // Holds the panel.
            // 
            // �p�l�����z�[���h���܂��B
            cursor = holdPanel;
            HoldPanel();

            // Selects a panel at random until a panel 
            // other than the held panel is selected.
            // 
            // �z�[���h�����p�l���Ƃ͕ʂ̃p�l�����I�������܂�
            // �����_���ɑI�����܂��B
            do
            {
                if (panelManager.PanelCompleteCount(stageSetting) != 1)
                {
                    // Obtains the swap destination panel.
                    // 
                    // ������̃p�l�����擾���܂��B
                    swapPanel = panelManager.GetRandomPanel(stageSetting);
                }
                else
                {
                    // If there is only one panel that is not being shuffled, 
                    // this will be the only panel available for selection, 
                    // resulting in an infinite loop, so it must be avoided.
                    // 
                    // �V���b�t������Ă��Ȃ��p�l����1�����̏ꍇ��
                    // ���ꂵ���I�����ꂸ�������[�v�����̂ŉ��
                    swapPanel.X = Random.Next(0, stageSetting.Divide.X);
                    swapPanel.Y = Random.Next(0, stageSetting.Divide.Y);
                }
            } while (holdPanel.Equals(swapPanel));
            cursor = swapPanel;

            // Swaps the panels.
            // 
            // �p�l���̌����������s���܂��B
            SwapPanel();

            // Plays the panel swap SoundEffect.
            // 
            // �p�l��������SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);
        }
        #endregion
    }
}


