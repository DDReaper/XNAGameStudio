#region File Description
//-----------------------------------------------------------------------------
// ThumbnailSprite.cs
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

using Movipa.Util;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Updates and draws the thumbnails.
    /// 
    /// �T���l�C���̍X�V�ƕ`����s���܂��B
    /// </summary>
    public class ThumbnailSprite : Sprite
    {
        #region Fields
        private Vector2 targetPosition;
        private int id;
        private int textureId;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the target position.
        /// 
        /// �ړ���̈ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }


        /// <summary>
        /// Obtains or sets the sprite ID.
        /// 
        /// �X�v���C�g��ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        /// <summary>
        /// Obtains or sets the texture ID.
        /// 
        /// �e�N�X�`����ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int TextureId
        {
            get { return textureId; }
            set { textureId = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public ThumbnailSprite(Game game)
            : base(game)
        {
            Updating += ThumbnailSpriteUpdating;
            Drawing += ThumbnailSpriteDrawing;
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        ///
        /// �X�V�������s���܂��B
        /// </summary>
        void ThumbnailSpriteUpdating(object sender, UpdatingEventArgs args)
        {
            // Slides to the designated position.
            // 
            // �w�肳�ꂽ�ʒu�܂ŃX���C�h���܂��B
            Position += (TargetPosition - Position) * 0.2f;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs render processing.
        /// 
        /// �`�揈�����s���܂��B
        /// </summary>
        void ThumbnailSpriteDrawing(object sender, DrawingEventArgs args)
        {
            // Does not draw if no texture is specified.
            // 
            // �e�N�X�`�����w�肳��Ă��Ȃ���Ε`������܂���B
            if (Texture == null)
                return;

            args.Batch.Draw(Texture, Position, Color.White);
        }
        #endregion
    }
}
