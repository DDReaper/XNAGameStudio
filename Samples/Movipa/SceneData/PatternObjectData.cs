#region File Description
//-----------------------------------------------------------------------------
// PatternObjectData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// Pattern data
    /// This data class is for a rectangle that has conversion parameters and textures.
    /// In Layout, pattern objects correspond to this pattern data.
    /// The data managed in this class contains:
    /// textures to be used, cutting range, display position (within a pattern group), 
    /// scale, angle, center position, color, flip information, etc.
    ///
    /// �p�^�[���f�[�^
    /// �e�N�X�`���𔺂��A�ϊ��p�̃p�����[�^����������`�̃f�[�^�N���X�ł��B
    /// Layout�ł̓p�^�[���I�u�W�F�N�g���������܂��B
    /// �ێ�����f�[�^�Ƃ��āA�g�p����e�N�X�`���A�؂���͈́A
    /// �i�p�^�[���O���[�v���ł́j�\���ʒu�A�X�P�[���A�p�x�A���S�ʒu�A�F�A���]���
    /// �Ȃǂ�����܂��B
    /// </summary>
    public class PatternObjectData
    {
        #region Fields
        private String textureName = String.Empty;//Texture name //�e�N�X�`����
        private Texture2D texture = null;//Texture substance //�e�N�X�`���̎���
        private Rectangle patternRect = new Rectangle();//Pattern rectangle
        private bool flipH = false;//Horizontal flip flag //�������]�t���O
        private bool flipV = false;//Vertical flip flag //�������]�t���O
        private DrawData drawData = new DrawData();//Conversion information //�ϊ����
        //Temporary conversion information to play a sequence
        //
        //�V�[�P���X�Đ��p�̈ꎞ�I�ȕϊ����
        private DrawData interpolationDrawData = new DrawData();
        #endregion

        #region Properties
        /// <summary>
        /// Obtains and sets the texture name.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �e�N�X�`�����̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        public String TextureName
        {
            get
            {
                return textureName;
            }
            set
            {
                textureName = value;
            }
        }

        /// <summary>
        /// Obtains and sets the pattern rectangle.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �p�^�[����`�̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return patternRect;
            }
            set
            {
                patternRect = value;
            }
        }

        /// <summary>
        /// Obtains and sets the horizontal flip flag for patterns.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �p�^�[���̐������]�t���O�̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        public bool FlipH
        {
            get
            {
                return flipH;
            }
            set
            {
                flipH = value;
            }
        }

        /// <summary>
        /// Obtains and sets the vertical flip flag for patterns.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �p�^�[���̐������]�t���O�̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        public bool FlipV
        {
            get
            {
                return flipV;
            }
            set
            {
                flipV = value;
            }
        }

        /// <summary>
        /// Obtains and sets the conversion parameters for drawing.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �`��p�ϊ��p�����[�^�̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        public DrawData Data
        {
            get
            {
                return drawData;
            }
            set
            {
                drawData = value;
            }
        }

        /// <summary>
        /// Obtains the drawing position.
        ///
        /// �`��ʒu���擾���܂��B
        /// </summary>
        public Point Position
        {
            get
            {
                return drawData.Position;
            }
        }

        /// <summary>
        /// Obtains the drawing color.
        ///
        /// �`��F���擾���܂��B
        /// </summary>
        public Color Color
        {
            get
            {
                return drawData.Color;
            }
        }

        /// <summary>
        /// Obtains the drawing conversion scale.
        ///
        /// �`��ϊ��X�P�[�����擾���܂��B
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return drawData.Scale;
            }
        }

        /// <summary>
        /// Obtains the center position for drawing conversion.
        ///
        /// �`��ϊ����S���擾���܂��B
        /// </summary>
        public Point Center
        {
            get
            {
                return drawData.Center;
            }
        }

        /// <summary>
        /// Obtains the rotation value for drawing conversion.
        ///
        /// �`��ϊ���]�ʂ��擾���܂��B
        /// </summary>
        public float RotateZ
        {
            get
            {
                return drawData.RotateZ;
            }
        }

        /// <summary>
        /// Obtains and sets the texture.
        /// The setting is called from SceneDataReader on initialization of scenes.
        ///
        /// �e�N�X�`���̐ݒ�擾���s���܂��B
        /// �ݒ�̓V�[���̏���������SceneDataReader����Ăяo����܂��B
        /// </summary>
        private Texture2D Texture
        {
            get{
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        /// <summary>
        /// The temporary drawing conversion information 
        /// specified during sequence play.  Based on this information, 
        /// display items synchronized with the sequence can be positioned.
        /// </summary>
        [ContentSerializerIgnore]
        public DrawData InterpolationDrawData
        {
            get { return interpolationDrawData; }
            set { interpolationDrawData = value; }
        }

        #endregion

        /// <summary>
        /// Performs initialization.
        /// Loads the XNA graphic textures through ContentManager.
        ///
        /// ���������s���܂��B
        /// ContentManager��ʂ��āA
        /// XNA�O���t�B�b�N�̃e�N�X�`����ǂݍ��݂܂��B
        /// </summary>
        /// <param name="content">
        /// ContentManager
        /// 
        /// �R���e���g�}�l�[�W���[
        /// </param>
        public void Init(ContentManager content)
        {
            if (!String.IsNullOrEmpty(TextureName))
            {
                Texture = content.Load<Texture2D>(TextureName);
            }
        }

        /// <summary>
        /// Performs drawing.
        /// baseDrawData contains information for entire sequence conversion, 
        /// and sequenceDrawData contains conversion information interpolated for 
        /// sequence display (including conversion information for pattern objects
        /// themselves). 
        /// 
        /// �`����s���܂��B
        /// baseDrawData�́A�V�[�P���X�S�̂̕ϊ����A
        /// sequenceDrawData�́A�V�[�P���X�\���̂��߂ɓ�����⊮���ꂽ
        /// �ϊ���񂪓����Ă��܂�(����̓p�^�[���I�u�W�F�N�g���̂̕ϊ�����
        /// �܂�ł��܂�)�B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C�g�o�b�`
        /// </param>
        /// <param name="sequenceDrawData">
        /// Conversion information for sequence
        /// 
        /// �V�[�P���X�p�ϊ����
        /// </param>
        /// <param name="baseDrawData">
        /// Basic conversion information for drawing
        /// 
        /// �`��p��{�ϊ����
        /// </param>
        public void Draw(SpriteBatch sb, DrawData sequenceDrawData, 
                                            DrawData baseDrawData)
        {
            // If no texture is specified, returns.
            // 
            // �e�N�X�`�����w�肳��Ă��Ȃ���Δ�����
            if ((Texture == null) || Texture.IsDisposed)
            {
                return;
            }

            Vector2 position = new Vector2();

            //Creates a matrix and colors for drawing 
            //from the interpolated conversion information.
            //This matrix is temporarily used to determine the display position.
            //
            //�⊮���ꂽ�ϊ���񂩂�
            //�`��̂��߂̃}�g���N�X�ƐF���쐬���܂�
            //�}�g���N�X�́A�\���ʒu�����߂邽�߂̈ꎞ�I�Ȃ��̂ł��B
            float rotateZ = sequenceDrawData.RotateZ;
            Vector2 vectorScale = sequenceDrawData.Scale;
            Color color = sequenceDrawData.Color;

            Matrix matrix = Matrix.CreateTranslation(
                sequenceDrawData.Position.X + sequenceDrawData.Center.X,
                sequenceDrawData.Position.Y + sequenceDrawData.Center.Y,
                0.0f
                );

            //If the basic conversion information is valid, 
            //creates the matrix and colors.
            //
            //��{�ϊ���񂪗L���Ȃ�A
            //�}�g���N�X�ƐF���쐬���܂��B
            if (null != baseDrawData)
            {
                rotateZ += baseDrawData.RotateZ;
                vectorScale *= baseDrawData.Scale;
                color = new Color(  (byte)(color.R * baseDrawData.Color.R / 0xFF), 
                                    (byte)(color.G * baseDrawData.Color.G / 0xFF), 
                                    (byte)(color.B * baseDrawData.Color.B / 0xFF),
                                    (byte)(color.A * baseDrawData.Color.A / 0xFF));

                position = new Vector2(baseDrawData.Position.X, 
                                        baseDrawData.Position.Y);

                matrix *= 
                    Matrix.CreateScale(new Vector3(baseDrawData.Scale.X, 
                                        baseDrawData.Scale.Y, 1.0f)) *
                                        Matrix.CreateRotationZ(baseDrawData.RotateZ);
            }

            //Determines the final display position.
            //
            //�ŏI�̕\���ʒu�����߂܂��B
            position += new Vector2(matrix.Translation.X, matrix.Translation.Y);

            SpriteEffects effects = SpriteEffects.None;

            //If the sprite needs to be flipped, apply the flip information to it.
            //
            //���]������ꍇ�A�K�p���܂��B
            if (flipH)
                effects |= SpriteEffects.FlipHorizontally;
            if (flipV)
                effects |= SpriteEffects.FlipVertically;

            //Drawing
            //
            //�`��
            sb.Draw(Texture, position, Rect, color, 
                    MathHelper.ToRadians(rotateZ),
                    new Vector2(sequenceDrawData.Center.X, sequenceDrawData.Center.Y),
                    vectorScale,
                    effects,
                    1.0f);
        }
    }
}
