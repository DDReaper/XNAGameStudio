#region File Description
//-----------------------------------------------------------------------------
// MenuBase.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Abstract class for menu processing.
    /// The menu items inherit this class so that the 
    /// necessary processing is written in Update and Draw.
    /// The CreateMenu method for this class should be 
    /// used to create menu instances. 
    /// 
    /// ���j���[�̏������s�����ۃN���X�ł��B
    /// �e���j���[�̍��ڂ́A���̃N���X���p�����A�K�v�ȏ�����
    /// Update��Draw�ɋL�q����悤�ɂ��܂��B
    /// �e���j���[�̃C���X�^���X���쐬����ɂ́A���̃N���X��
    /// CreateMenu���\�b�h���g�p���ĉ������B
    /// </summary>
    public abstract class MenuBase : SceneComponent
    {
        #region Fields
        protected bool initialized = false;
        private MenuData data;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the initialization flag.
        /// 
        /// �������t���O���擾���܂��B
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
        }


        /// <summary>
        /// Obtains the menu data.
        /// 
        /// ���j���[�f�[�^���擾���܂��B
        /// </summary>
        public MenuData Data
        {
            get { return data; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        protected MenuBase(Game game, MenuData menuData)
            : base(game)
        {
            data = menuData;
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            initialized = true;
            base.Initialize();
        }


        /// <summary>
        /// Performs asynchronous initialization processing.
        /// 
        /// �񓯊��ŏ������������s���܂��B
        /// </summary>
        public void RunInitializeThread()
        {
            // Starts the initialization thread.
            // 
            // �������X���b�h���J�n���܂��B
            Thread thread = new Thread(new ThreadStart(this.Initialize));
            thread.Start();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Abstract method for update processing.
        /// 
        /// �X�V�����̒��ۃ��\�b�h�ł��B
        /// </summary>
        public virtual MenuBase UpdateMain(GameTime gameTime)
        {
            return null;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Abstract method for drawing processing.
        ///
        /// �`�揈���̒��ۃ��\�b�h�ł��B
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates menu instances.
        ///
        /// ���j���[�̃C���X�^���X���쐬���܂��B
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="menuType">MenuType</param>
        /// <param name="data">MenuData</param>
        /// <returns>Created menu instances</returns>
        ///  
        /// <returns>�쐬���ꂽ���j���[�C���X�^���X</returns>
        public static MenuBase CreateMenu(Game game, MenuType menuType, MenuData data)
        {
            switch (menuType)
            {
                case MenuType.SelectMode:
                    // Mode selection
                    // 
                    // ���[�h�I��
                    return new SelectMode(game, data);
                case MenuType.SelectFile:
                    // File selection
                    // 
                    // �t�@�C���I��
                    return new SelectFile(game, data);
                case MenuType.SelectStyle:
                    // Style selection
                    // 
                    // �X�^�C���I��
                    return new SelectStyle(game, data);
                case MenuType.SelectMovie:
                    // Movie selection
                    // 
                    // ���[�r�[�I��
                    return new SelectMovie(game, data);
                case MenuType.SelectDivide:
                    // Divisions setting
                    // 
                    // �������ݒ�
                    return new SelectDivide(game, data);
                case MenuType.Ready:
                    // Confirmation screen
                    // 
                    // �m�F���
                    return new Ready(game, data);
                default:
                    throw new ArgumentException("Invalid MenuType specified");
            }
        }
        #endregion
    }
}
