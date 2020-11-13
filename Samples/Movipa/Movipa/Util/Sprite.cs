#region File Description
//-----------------------------------------------------------------------------
// Sprite.cs
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
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Manages Sprite information.
    /// Contains the main parameters used by SpriteBatch, as well as 
    /// a primitive draw method available for Draw events. 
    /// 
    /// �X�v���C�g�̏����Ǘ����܂��B
    /// SpriteBatch�Ŏg�p������ȃp�����[�^�������A
    /// Draw�C�x���g�Ŏg�p�ł����{�I�ȕ`�惁�\�b�h��L���܂��B
    /// </summary>
    public class Sprite : Movipa.Util.GameComponentObject
    {
        #region Fields
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 texturePosition;
        protected Vector2 size;
        protected Color color;
        protected float scale;
        protected Vector2 origin;
        protected float priority;
        protected float rotate;
        protected float direction;
        protected float speed;
        protected Sprite parent;
        protected List<Sprite> child;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the texture.
        /// 
        /// �e�N�X�`�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


        /// <summary>
        /// Obtains or sets the position.
        /// 
        /// �|�W�V�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// Obtains or sets the texture source coordinates.
        /// 
        /// �e�N�X�`���̓]�������W���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 TexturePosition
        {
            get { return texturePosition; }
            set { texturePosition = value; }
        }


        /// <summary>
        /// Obtains or sets the size.
        /// 
        /// �T�C�Y���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }


        /// <summary>
        /// Obtains or sets the draw color.
        /// 
        /// �`��F���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }


        /// <summary>
        /// Obtains or sets the scale.
        /// 
        /// �X�P�[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }


        /// <summary>
        /// Obtains or sets the center coordinates.
        /// 
        /// ���S���W���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }


        /// <summary>
        /// Obtains or sets the priority.
        /// 
        /// �D��x���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Priority
        {
            get { return priority; }
            set { priority = value; }
        }


        /// <summary>
        /// Obtains or sets the rotation angle.
        /// 
        /// ��]�p�x���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }


        /// <summary>
        /// Obtains or sets the movement direction.
        /// 
        /// �ړ��������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Direction
        {
            get { return direction; }
            set { direction = value; }
        }


        /// <summary>
        /// Obtains or sets the movement speed. 
        /// 
        /// �ړ����x���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        /// <summary>
        /// Obtains or sets the parent sprite.
        /// 
        /// �e�̃X�v���C�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Sprite Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        /// <summary>
        /// Obtains the child sprite.
        /// 
        /// �q�̃X�v���C�g���X�g���擾���܂��B
        /// </summary>
        public List<Sprite> Child
        {
            get { return child; }
        }


        /// <summary>
        /// Obtains the position.
        /// 
        /// �|�W�V�������擾���܂��B
        /// </summary>
        public Rectangle RectanglePosition
        {
            get
            {
                Rectangle value = new Rectangle();

                value.X = (int)Position.X;
                value.Y = (int)Position.Y;
                value.Width = (int)Size.X;
                value.Height = (int)Size.Y;

                return value;
            }
        }


        /// <summary>
        /// Obtains the texture source.
        /// 
        /// �e�N�X�`���̓]�������擾���܂��B
        /// </summary>
        public Rectangle SourceRectangle
        {
            get
            {
                Rectangle value = new Rectangle();

                value.X = (int)TexturePosition.X;
                value.Y = (int)TexturePosition.Y;
                value.Width = (int)Size.X;
                value.Height = (int)Size.Y;

                return value;
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public Sprite(Game game)
            : base(game)
        {
            // Initializes the member variables.
            // 
            // �����o�ϐ������������܂��B
            texture = null;
            position = Vector2.Zero;
            texturePosition = Vector2.Zero;
            size = Vector2.Zero;
            color = Color.White;
            scale = 1.0f;
            origin = Vector2.Zero;
            priority = 0.0f;
            rotate = 0.0f;
            direction = 0.0f;
            speed = 1.0f;
            parent = null;
            child = new List<Sprite>();
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs a primitive draw.
        /// This method needs to be invoked in advance since the 
        /// SpriteBatch Begin and End are not performed.
        ///
        /// ��{�I�ȕ`����s���܂��B
        /// ���̃��\�b�h�ł�SpriteBatch��Begin/End���s���Ȃ��̂�
        /// ���O�ɌĂяo���ĉ������B
        /// </summary>
        protected void Draw(SpriteBatch batch)
        {
            // Processing is not performed if there is no texture.
            // 
            // �e�N�X�`���������ꍇ�͏������s��Ȃ��悤�ɂ��܂��B
            if (Texture == null)
                return;

            // Performs drawing processing.
            // 
            // �`����s���܂��B
            batch.Draw(
                Texture,
                Position,
                SourceRectangle,
                Color,
                Rotate,
                Origin,
                Scale,
                SpriteEffects.None,
                Priority);
        }
        #endregion
   }
}