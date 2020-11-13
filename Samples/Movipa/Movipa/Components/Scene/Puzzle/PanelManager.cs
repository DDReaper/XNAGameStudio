#region File Description
//-----------------------------------------------------------------------------
// PanelManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using MovipaLibrary;
#endregion

namespace Movipa.Components.Scene.Puzzle
{
    /// <summary>
    /// Class used to create and manage panels.
    /// Also includes functions for the number of completed panels and the completion
    /// ratio as well as for obtaining panels at random.
    ///
    /// �p�l���̍쐬�ƊǗ�������N���X�ł��B
    /// ���ɂ��������������⊮�����A�����_���Ńp�l�����擾����@�\������܂��B
    /// </summary>
    public class PanelManager
    {
        #region Fields
        private Game game;
        private PanelData[,] panels;
        private Vector2 panelSize;
        private Vector4 panelArea;
        private Vector2 offset;
        private Point panelCount;
        #endregion

        #region Properties
        /// <summary> 
        /// Obtains the Game instance.
        /// 
        /// Game�C���X�^���X���擾���܂��B
        /// </summary>
        public Game Game
        {
            get { return game; }
        }


        /// <summary>
        /// Obtains the intitialized status.
        /// </summary>
        public bool IsInitialized
        {
            get { return panels != null; }
        }


        /// <summary>
        /// Obtains the panel data.
        /// 
        /// �p�l���f�[�^���擾���܂��B
        /// </summary>
        public PanelData GetPanel(int x, int y)
        {
            return panels[x, y];
        }

        /// <summary>
        /// Assigns the panel data.
        /// </summary>
        public void SetPanel(int x, int y, PanelData panelData)
        {
            panels[x, y] = panelData;
        }

        /// <summary>
        /// Obtains the panel data.
        /// 
        /// �p�l���f�[�^���擾���܂��B
        /// </summary>
        public PanelData GetPanel(Point point)
        {
            return GetPanel(point.X, point.Y);
        }

        /// <summary>
        /// Assigns the panel data.
        /// </summary>
        public void SetPanel(Point point, PanelData panelData)
        {
            SetPanel(point.X, point.Y, panelData);
        }

        /// <summary>
        /// Obtains the panel count.
        /// </summary>
        public Point PanelCount
        {
            get { return panelCount; }
        }


        /// <summary>
        /// Obtains the panel size.
        /// 
        /// �p�l���T�C�Y���擾���܂��B
        /// </summary>
        public Vector2 PanelSize
        {
            get { return panelSize; }
        }


        /// <summary>
        /// Obtains the total panel area.
        /// 
        /// �p�l���S�̂̃G���A���擾���܂��B
        /// </summary>
        public Vector4 PanelArea
        {
            get { return panelArea; }
        }


        /// <summary>
        /// Obtains or sets the drawing offset.
        /// 
        /// �`�掞�̃I�t�Z�b�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 DrawOffset
        {
            get { return offset; }
            set { offset = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public PanelManager(Game game)
        {
            this.game = game;
            panels = null;
            panelSize = Vector2.Zero;
            panelArea = Vector4.Zero;
            offset = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates the panel.
        /// 
        /// �p�l�����쐬���܂��B
        /// </summary>
        public void CreatePanel(Point movieSize, StageSetting setting)
        {
            // Obtains the panel size.
            // 
            // �p�l���T�C�Y���擾���܂��B
            panelSize = GetPanelSize(movieSize, setting);

            // Calculates the total width.
            //
            // �S�̂̕����v�Z���܂��B
            panelArea.Z = PanelSize.X * setting.Divide.X;
            panelArea.W = PanelSize.Y * setting.Divide.Y;

            // Calculates the panel start position. 
            // 
            // �p�l���̊J�n�ʒu���v�Z���܂��B
            panelArea.X = (movieSize.X - PanelArea.Z) * 0.5f;
            panelArea.Y = (movieSize.Y - PanelArea.W) * 0.5f;

            // Creates the panel.
            // 
            // �p�l�����쐬���܂��B
            panelCount = setting.Divide;
            panels = new PanelData[panelCount.X, panelCount.Y];
            for (int x = 0; x < panelCount.X; x++)
            {
                for (int y = 0; y < panelCount.Y; y++)
                {
                    panels[x, y] = GetPanelData(x, y, setting);
                }
            }
        }


        /// <summary>
        /// Obtains the panel size.
        /// 
        /// �p�l���̃T�C�Y���擾���܂��B
        /// </summary>
        private static Vector2 GetPanelSize(Point movieSize, StageSetting setting)
        {
            Vector2 panelSize = new Vector2();

            panelSize.X = movieSize.X / setting.Divide.X;
            panelSize.Y = movieSize.Y / setting.Divide.Y;

            // Aligns with minimum value if rotation is on.
            // 
            // ��]����̏ꍇ�͍ŏ��l�ɂ��킹�܂��B
            if (setting.Rotate == StageSetting.RotateMode.On)
            {
                // Splits into squares.
                // 
                // �����`�ɕ������܂��B
                float size = (panelSize.X < panelSize.Y) ? panelSize.X : panelSize.Y;
                panelSize.X = size;
                panelSize.Y = size;
            }

            return panelSize;
        }


        /// <summary>
        /// Obtains panel information.
        /// 
        /// �p�l�������擾���܂��B
        /// </summary>
        private PanelData GetPanelData(int x, int y, StageSetting setting)
        {
            PanelData panel = new PanelData(game);
            panel.Id = y + (x * setting.Divide.Y);
            panel.Size = new Vector2((int)PanelSize.X, (int)PanelSize.Y);

            Vector2 texturePosition = new Vector2();
            texturePosition.X = (int)(PanelArea.X + (x * panel.Size.X));
            texturePosition.Y = (int)(PanelArea.Y + (y * panel.Size.Y));
            panel.TexturePosition = texturePosition;

            panel.Position = texturePosition;
            panel.Origin = panel.Size * 0.5f;

            return panel;
        }


        /// <summary>
        /// Obtains the number of completed panels.
        /// 
        /// �p�l�������������擾���܂��B
        /// </summary>
        public int PanelCompleteCount(StageSetting setting)
        {
            int value = 0;
            for (int x = 0; x < setting.Divide.X; x++)
            {
                for (int y = 0; y < setting.Divide.Y; y++)
                {
                    int id = (y + (x * setting.Divide.Y));
                    PanelData panel = panels[x, y];
                    if (panel.Rotate == 0 && panel.Id == id)
                    {
                        value++;
                    }
                }
            }

            return value;
        }


        /// <summary>
        /// Obtains the number of remaining panels.
        ///
        /// �p�l���c�薇�����擾���܂��B
        /// </summary>
        public int PanelRestCount(StageSetting setting)
        {
            return ((setting.Divide.X * setting.Divide.Y) - PanelCompleteCount(setting));
        }


        /// <summary>
        /// Obtains the completion ratio.
        /// 
        /// ���������擾���܂��B
        /// </summary>
        public float PanelCompleteRatio(StageSetting setting)
        {
            Point divide = setting.Divide;
            float complete = (float)PanelCompleteCount(setting);
            float totalPanel = (float)(divide.X * divide.Y);

            return (complete / totalPanel) * 100;
        }


        /// <summary>
        /// Checks the panel completion status and obtains the list.
        /// 
        /// �p�l���̊�����Ԃ��`�F�b�N���A���X�g���擾���܂��B
        /// </summary>
        /// <returns>Newly completed panels list</returns>
        ///  
        /// <returns>�V�������������p�l���̃��X�g</returns>
        public List<PanelData> PanelCompleteCheck(StageSetting setting)
        {
            List<PanelData> list = new List<PanelData>();

            for (int x = 0; x < setting.Divide.X; x++)
            {
                for (int y = 0; y < setting.Divide.Y; y++)
                {
                    int id = (y + (x * setting.Divide.Y));
                    if (panels[x, y].Enabled == true &&
                        (MathHelper.ToDegrees(panels[x, y].Rotate) % 360) == 0 &&
                        panels[x, y].Id == id)
                    {
                        PanelData panel = panels[x, y];

                        list.Add(panel);

                        panel.Flush = -256;

                        if (setting.Style != StageSetting.StyleList.Revolve &&
                            setting.Style != StageSetting.StyleList.Slide)
                        {
                            panel.Enabled = false;
                        }
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Obtains completed panels at random.
        /// 
        /// ��������Ă���p�l���������_���Ŏ擾���܂��B
        /// </summary>
        public Point GetRandomPanel(StageSetting setting)
        {
            Random rnd = new Random();
            List<Point> list = new List<Point>();
            Point value = new Point();

            for (int x = 0; x < setting.Divide.X; x++)
            {
                for (int y = 0; y < setting.Divide.Y; y++)
                {
                    int id = (y + (x * setting.Divide.Y));
                    if (panels[x, y].Enabled == true &&
                        (MathHelper.ToDegrees(panels[x, y].Rotate) % 360) == 0 &&
                        panels[x, y].Id == id)
                    {
                        list.Add(new Point(x, y));
                    }
                }
            }


            if (list.Count == 0)
            {
                // If there are no completed panels, panels
                // are obtained at random from the entire pool.
                // 
                // �������ꂽ�p�l����������΁A�S�Ẵp�l������
                // �����_���Ŏ擾���܂��B
                value.X = rnd.Next(setting.Divide.X);
                value.Y = rnd.Next(setting.Divide.Y);
            }
            else
            {
                // If the list contains completed panels, 
                // panels are obtained at random from the list.
                //
                // �������ꂽ�p�l�������X�g�ɑ��݂��Ă���΁A
                // ���̒����烉���_���Ŏ擾���܂��B
                int id = rnd.Next(list.Count);
                value = list[id];
            }

            // Limits the range of movement in Revolve Mode.
            // 
            // ���{�������[�h�̏ꍇ�͈ړ��͈͂𐧌����܂��B
            if (setting.Style == StageSetting.StyleList.Revolve)
            {
                value.X = (int)MathHelper.Clamp(value.X, 0, setting.Divide.X - 2);
                value.Y = (int)MathHelper.Clamp(value.Y, 0, setting.Divide.Y - 2);
            }

            return value;
        }
        #endregion
    }
}
