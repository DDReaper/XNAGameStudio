#region File Description
//-----------------------------------------------------------------------------
// SlideComponent.cs
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
    /// Implements the Slide Mode style.
    /// This class inherits the StyleBase class and implements panel switching.
    /// It switches panels extended from the cursor position in all four directions
    /// as far as the movie end panel by sliding them.
    /// 
    /// �X���C�h���[�h�̃X�^�C�����������܂��B
    /// ���̃N���X��StyleBase�N���X���p�����A�p�l���̓���ւ��������������Ă��܂��B
    /// �J�[�\���̈ʒu����A���[�r�[�̒[�̃p�l���܂ŏ㉺���E�ɐL�΂����p�l����
    /// �X���C�h�����ē���ւ��鏈�����s���Ă��܂��B
    /// </summary>
    public class SlideComponent : StyleBase
    {
        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SlideComponent(Game game,
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

                // Slides the panels in the vertical direction.
                // 
                // �p�l�����c�����ɃX���C�h���鏈�����s���܂��B
                UpdateSlidePanel();
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
            // Sets the  cursor position.
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


        /// <summary>
        /// Performs update processing to judge panel slide.
        /// 
        /// �p�l���̃X���C�h�𔻒肷��X�V�������s���܂��B
        /// </summary>
        private void UpdateSlidePanel()
        {
            bool action = false;

            VirtualPadState virtualPad =
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (buttons.Y[VirtualKeyState.Push])
            {
                // Slides the panel up when the Y button is pressed.
                // 
                // Y�{�^����������Ă�����A������Ƀp�l�����X���C�h���܂��B
                action = SlidePanelUp();
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                // Slides the panel down when the A button is pressed.
                // 
                // A�{�^����������Ă�����A�������Ƀp�l�����X���C�h���܂��B
                action = SlidePanelDown();
            }
            else if (buttons.X[VirtualKeyState.Push])
            {
                // Slides the panel to the left when the X button is pressed.
                // 
                // X�{�^����������Ă�����A�������Ƀp�l�����X���C�h���܂��B
                action = SlidePanelLeft();
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                // Slides the panel to the right when the B button is pressed.
                // 
                // B�{�^����������Ă�����A�E�����Ƀp�l�����X���C�h���܂��B
                action = SlidePanelRight();
            }

            // Performs processing in response to panel action.
            // 
            // �p�l�������삵���珈�����s���܂��B
            if (action)
            {
                // Plays the SoundEffect when sliding.
                // 
                // �X���C�h����SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                // Adds up the move count.
                // 
                // �ړ��񐔂����Z���܂��B
                moveCount++;
            }
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
            Batch.Begin();

            // Draws the center cursor.
            // 
            // �����̃J�[�\����`�悵�܂��B
            DrawCenterCursor(gameTime);

            // Draws the vertical cursor.
            // 
            // �������̃J�[�\����`�悵�܂��B
            DrawVerticalCursor(gameTime);

            // Draws the horizontal cursor.
            // 
            // �c�����̃J�[�\����`�悵�܂��B
            DrawHorizontalCursor(gameTime);
            
            Batch.End();
        }


        /// <summary>
        /// Draws the center cursor.
        /// 
        /// �����̃J�[�\����`�悵�܂��B
        /// </summary>
        private void DrawCenterCursor(GameTime gameTime)
        {
            Rectangle cursorRect = new Rectangle();
            cursorRect.X = (int)(cursor.X * panelManager.PanelSize.X);
            cursorRect.Y = (int)(cursor.Y * panelManager.PanelSize.Y);
            cursorRect.Width = (int)(panelManager.PanelSize.X);
            cursorRect.Height = (int)(panelManager.PanelSize.Y);
            DrawCursor(gameTime, cursorRect, Color.Red, PanelTypes.All);
        }


        /// <summary>
        /// Draws the vertical cursor.
        /// 
        /// �������̃J�[�\����`�悵�܂��B
        /// </summary>
        private void DrawVerticalCursor(GameTime gameTime)
        {
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                // Processing is not performed if the draw location is 
                // the same as the cursor position.
                // 
                // �`��悪�J�[�\���ʒu�Ɠ����ꍇ�͏������s���܂���B
                if (x == cursor.X)
                    continue;

                Rectangle cursorRect = new Rectangle();
                cursorRect.X = (int)(x * panelManager.PanelSize.X);
                cursorRect.Y = (int)(cursor.Y * panelManager.PanelSize.Y);
                cursorRect.Width = (int)(panelManager.PanelSize.X);
                cursorRect.Height = (int)(panelManager.PanelSize.Y);

                DrawCursor(gameTime, cursorRect, Color.White, PanelTypes.All);
            }
        }


        /// <summary>
        /// Draws the horizontal cursor.
        /// 
        /// �c�����̃J�[�\����`�悵�܂��B
        /// </summary>
        private void DrawHorizontalCursor(GameTime gameTime)
        {
            for (int y = 0; y < stageSetting.Divide.Y; y++)
            {
                // Proccessing is not performed if the draw location is 
                // the same as the cursor position.
                // 
                // �`��悪�J�[�\���ʒu�Ɠ����ꍇ�͏������s���܂���B
                if (y == cursor.Y)
                    continue;

                Rectangle cursorRect = new Rectangle();
                cursorRect.X = (int)(cursor.X * panelManager.PanelSize.X);
                cursorRect.Y = (int)(y * panelManager.PanelSize.Y);
                cursorRect.Width = (int)(panelManager.PanelSize.X);
                cursorRect.Height = (int)(panelManager.PanelSize.Y);

                DrawCursor(gameTime, cursorRect, Color.White, PanelTypes.All);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Slides the panels up.
        /// 
        /// �p�l����������ɃX���C�h�����܂��B
        /// </summary>
        private bool SlidePanelUp()
        {
            List<Point> panelPointList = new List<Point>();
            for (int y = stageSetting.Divide.Y - 1; y >= 0; y--)
            {
                panelPointList.Add(new Point(cursor.X, y));
            }

            if (panelPointList.Count == 0)
            {
                return false;
            }

            // Specifies the panel array to slide and switches the panels.
            // 
            // �X���C�h����p�l���̔z����w�肵�ē���ւ��܂��B
            return SlidePanel(panelPointList);
        }


        /// <summary>
        /// Slides the panels down.
        /// 
        /// �p�l�����������ɃX���C�h�����܂��B
        /// </summary>
        private bool SlidePanelDown()
        {
            List<Point> panelPointList = new List<Point>();
            for (int y = 0; y < stageSetting.Divide.Y; y++)
            {
                panelPointList.Add(new Point(cursor.X, y));
            }

            if (panelPointList.Count == 0)
            {
                return false;
            }

            // Specifies the panel array to slide and switches the panels.
            // 
            // �X���C�h����p�l���̔z����w�肵�ē���ւ��܂��B
            return SlidePanel(panelPointList);
        }


        /// <summary>
        /// Slides the panels to the left.
        /// 
        /// �p�l�����������ɃX���C�h�����܂��B
        /// </summary>
        private bool SlidePanelLeft()
        {
            List<Point> panelPointList = new List<Point>();
            for (int x = stageSetting.Divide.X - 1; x >= 0; x--)
            {
                panelPointList.Add(new Point(x, cursor.Y));
            }

            if (panelPointList.Count == 0)
            {
                return false;
            }

            // Specifies the panel array to slide and switches the panels.
            // 
            // �X���C�h����p�l���̔z����w�肵�ē���ւ��܂��B
            return SlidePanel(panelPointList);
        }


        /// <summary>
        /// Slides the panels to the right.
        /// 
        /// �p�l�����E�����ɃX���C�h�����܂��B
        /// </summary>
        private bool SlidePanelRight()
        {
            List<Point> panelPointList = new List<Point>();
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                panelPointList.Add(new Point(x, cursor.Y));
            }

            if (panelPointList.Count == 0)
            {
                return false;
            }

            // Specifies the panel array to slide and switches the panels.
            // 
            // �X���C�h����p�l���̔z����w�肵�ē���ւ��܂��B
            return SlidePanel(panelPointList);
        }


        /// <summary>
        /// Specifies the panel array to slide and switches the panels.
        /// 
        /// �X���C�h����p�l���̔z����w�肵�ē���ւ��܂��B
        /// </summary>
        private bool SlidePanel(List<Point> panelPointList)
        {
            // Processing is not performed if the panel status is not Normal.
            // 
            // �p�l�����ʏ��Ԃł͂Ȃ��ꍇ�͏������s���܂���B
            if (IsPanelAction)
                return false;

            // Processing is not performed if there is no switching panel array.
            // 
            // ����ւ���p�l���̔z�񂪖����ꍇ�͏������s���܂���B
            if (panelPointList == null || panelPointList.Count == 0)
                return false;

            // Obtains the switching panel list.
            // 
            // ����ւ���p�l���̃��X�g���擾���܂��B
            List<PanelData> panels = new List<PanelData>();
            foreach (Point point in panelPointList)
            {
                panels.Add(panelManager.GetPanel(point));
            }

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
            List<Vector2[]> panelFromTo = new List<Vector2[]>();
            for (int i = 0; i < panels.Count; i++)
            {
                int currentId = i;
                int nextId = (i + 1) % panels.Count;

                Vector2[] item = {
                    panels[currentId].Position,
                    panels[nextId].Position,
                };
                panelFromTo.Add(item);
            }

            // Changes and sets the draw position.
            // 
            // �`��ʒu�̕ύX�ƁA�ݒ���s���܂��B
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].FromPosition = panelFromTo[i][0];
                panels[i].ToPosition = panelFromTo[i][1];
                panels[i].Status = PanelData.PanelStatus.Move;
                panels[i].Color = ActivePanelColor;
                panels[i].MoveCount = 1.0f;
            }

            // Exchanges the panel data.
            // 
            // �p�l���̃f�[�^���������܂��B
            for (int i = 0; i < panels.Count; i++)
            {
                int panelId = (i + (panels.Count - 1)) % panels.Count;
                panelManager.SetPanel(panelPointList[i], panels[panelId]);
            }

            return true;
        }


        /// <summary>
        /// Switches panels at random.
        /// 
        /// �����_���Ńp�l�������ւ��܂��B
        /// </summary>
        public override bool RandomShuffle()
        {
            // Processing is not performed while panels are in action.
            // 
            // �p�l�������쒆�Ȃ珈�����s���܂���B
            if (IsPanelAction)
                return false;

            // Obtains the target panels.
            // 
            // �Ώۂ̃p�l�����擾���܂��B
            cursor = panelManager.GetRandomPanel(stageSetting);

            // Selects the panel sliding direction at random.
            // 
            // �p�l���̃X���C�h�����������_���Ō��肵�܂��B
            int rndValue = Random.Next(0, 4);
            if (rndValue == 0)
            {
                // Slides the panel up.
                // 
                // ������ɃX���C�h���܂��B
                SlidePanelUp();
            }
            else if (rndValue == 1)
            {
                // Slides the panel down.
                // 
                // �������ɃX���C�h���܂��B
                SlidePanelDown();
            }
            else if (rndValue == 2)
            {
                // Slides the panel to the left.
                // 
                // �������ɃX���C�h���܂��B
                SlidePanelLeft();
            }
            else if (rndValue == 3)
            {
                // Slides the panel to the right.
                // 
                // �E�����ɃX���C�h���܂��B
                SlidePanelRight();
            }

            // Plays the sliding SoundEffect.
            // 
            // �X���C�h��SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

            return true;
        }
        #endregion
    }
}


