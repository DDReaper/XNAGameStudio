#region File Description
//-----------------------------------------------------------------------------
// DrawData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// This class manages conversion information for drawing.
    /// This class is used to indicate changes of position, rotation, scale, center, 
    /// and brightness both for pattern objects and pattern group sequences, 
    /// which are drawing units of Layout.
    /// This class also manages default conversion information for these elements.
    ///
    /// �`��p�̕ϊ�����ێ����܂��B
    /// Layout�̕`��P�ʂł���A�p�^�[���I�u�W�F�N�g�A�p�^�[���O���[�v
    /// �V�[�P���X�A���ꂼ��ɑ΂��āA�ʒu�E��]�E�X�P�[���E���S�E�P�x�̕ύX��
    /// �w������ۂɎg�p����܂��B
    /// ��L�v�f�̃f�t�H���g�̕ϊ����̕ێ������̃N���X���g�p���܂��B
    /// </summary>
    public class DrawData
    {
        #region Fields

        private Point position = new Point(); // Distance 
        private Color color = Color.White; // Color 
        private Vector2 scale = new Vector2(1.0f, 1.0f); // Enlargement scale
        private Point center = new Point(); // Center of rotation enlargement
        private float rotateZ = 0.0f; // Rotation value

        #endregion

        #region Properties
        /// <summary>
        /// Obtains and sets the display position.
        ///
        /// �\���ʒu���擾�ݒ肵�܂��B
        /// </summary>
        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Obtains and sets the display color.
        ///
        /// �\���F���擾�ݒ肵�܂��B
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Obtains and sets the enlargement scale.
        ///
        /// �g�嗦�̎擾�ݒ���s���܂��B
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        /// <summary>
        /// Obtains and sets the center position of rotation enlargement.
        ///
        /// ��]�g��̒��S�ʒu�̎擾�ݒ���s���܂��B
        /// </summary>
        public Point Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        /// <summary>
        /// Obtains and sets the rotation value.
        ///
        /// ��]�ʂ̎擾�ݒ���s���܂��B
        /// </summary>
        public float RotateZ
        {
            get
            {
                return rotateZ;
            }
            set
            {
                rotateZ = value;
            }
        }
        #endregion

        /// <summary>
        /// Converts the held data to character strings.
        ///
        /// �ێ����Ă���f�[�^�𕶎���ɕϊ����܂��B
        /// </summary>
        /// <returns>
        /// Converted character string
        /// 
        /// �ϊ����ꂽ������
        /// </returns>
        public override string ToString()
        {
            string value = base.ToString() + "\n";
            value += string.Format("Point  : {0}\n", position);
            value += string.Format("Scale  : {0}\n", scale);
            value += string.Format("Center : {0}\n", center);
            value += string.Format("Rotate : {0}\n", rotateZ);
            value += string.Format("Color  : {0}\n", color);

            return value;
        }

    }
}
