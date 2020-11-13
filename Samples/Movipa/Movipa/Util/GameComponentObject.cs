#region File Description
//-----------------------------------------------------------------------------
// GameComponentObject.cs
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
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Provides Update (used for update events) and Draw (used for draw events).
    /// Update checks that the Enable flag is enabled before proceeding; 
    /// similarly, Draw checks the Visible flag before proceeding.
    /// 
    /// XNA�̋@�\���g�������I�u�W�F�N�g�N���X�ł��B
    /// �X�V�p�C�x���g��Update�ƁA�`��p�̃C�x���g��Draw���p�ӂ���Ă��܂��B
    /// Update��Enable�t���O�ADraw��Visible�t���O�����āA������ԂɂȂ��Ă����
    /// ���������Ȃ��悤�ɂȂ��Ă��܂��B
    /// </summary>
    public class GameComponentObject : GameObject
    {
        #region Fields
        private Game game;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains GraphicsDevice.
        /// 
        /// GraphicsDevice���擾���܂��B
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return game.GraphicsDevice; }
        }

        /// <summary>
        /// Obtains ContentManager.
        /// 
        /// ContentManager���擾���܂��B
        /// </summary>
        public ContentManager Content
        {
            get { return game.Content; }
        }

        /// <summary>
        /// Obtains Game.
        /// 
        /// Game���擾���܂��B
        /// </summary>
        public Game Game
        {
            get { return game; }
        }
        #endregion

        #region Public Event

        public class UpdatingEventArgs : EventArgs
        {
            private GameTime gameTime;
            public GameTime GameTime
            {
                get { return gameTime; }
            }


            public UpdatingEventArgs(GameTime gameTime)
                : base()
            {
                this.gameTime = gameTime;
            }
        }
        public event EventHandler<UpdatingEventArgs> Updating;

        public class DrawingEventArgs : EventArgs
        {
            private GameTime gameTime;
            public GameTime GameTime
            {
                get { return gameTime; }
            }

            private SpriteBatch batch;
            public SpriteBatch Batch
            {
                get { return batch; }
            }

            public DrawingEventArgs(GameTime gameTime, SpriteBatch batch)
                : base()
            {
                this.gameTime = gameTime;
                this.batch = batch;
            }
        }
        public event EventHandler<DrawingEventArgs> Drawing;

        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance. 
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public GameComponentObject(Game game)
        {
            this.game = game;
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Executes UpdateEvent.
        /// 
        /// UpdateEvent�����s���܂��B
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (Updating != null && !Disposed && Enabled)
            {
                Updating(this, new UpdatingEventArgs(gameTime));
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Executes DrawEvent.
        /// 
        /// DrawEvent�����s���܂��B
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (Drawing != null && !Disposed && Visible)
            {
                Drawing(this, new DrawingEventArgs(gameTime, batch));
            }
        }
        #endregion
    }
}