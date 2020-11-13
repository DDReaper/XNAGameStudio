#region File Description
//-----------------------------------------------------------------------------
// PanelData.cs
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

using Movipa.Util;
#endregion

namespace Movipa.Components.Scene.Puzzle
{
    /// <summary>
    /// Manages the panel status.
    /// This class inherits the Sprite class.
    /// Update and draw processing is performed in the Panel and StyleBase classes.
    /// 
    /// �p�l���̏�Ԃ��Ǘ����܂��B
    /// ���̃N���X��Sprite�N���X���p�����Ă��܂��B
    /// �X�V�ƕ`��́APanel�N���X����сAStyleBase�N���X�ōs���Ă��܂��B
    /// </summary>
    public class PanelData : Sprite
    {
        #region Public Types
        /// <summary>
        /// Panel status
        /// 
        /// �p�l���̏��
        /// </summary>
        public enum PanelStatus
        {
            /// <summary>
            /// Normal
            /// 
            /// �ʏ�
            /// </summary>
            None,

            /// <summary>
            /// Rotating left
            /// 
            /// ����]��
            /// </summary>
            RotateLeft,

            /// <summary>
            /// Rotating right
            ///
            /// �E��]��
            /// </summary>
            RotateRight,

            /// <summary>
            /// Shifting
            ///
            /// �ړ���
            /// </summary>
            Move,
        }
        #endregion

        #region Fields
        private PanelStatus status;
        private int id;
        private float flush;
        private float moveCount;
        private float toRotate;
        public Vector2 FromPosition;
        public Vector2 ToPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the panel status.
        /// 
        /// �p�l���̏�Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public PanelStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// Obtains or sets the panel ID.
        /// 
        /// �p�l����ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Obtains or sets the completed effect status.
        /// 
        /// �����G�t�F�N�g�̏�Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Flush
        {
            get { return flush; }
            set { flush = value; }
        }

        /// <summary>
        /// Obtains or sets the movement amount.
        /// 
        /// �ړ��ʂ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float MoveCount
        {
            get { return moveCount; }
            set { moveCount = value; }
        }

        /// <summary>
        /// Obtains or sets the rotation target.
        /// 
        /// ��]�ڕW���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ToRotate
        {
            get { return toRotate; }
            set { toRotate = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public PanelData(Game game)
            : base(game)
        {
            flush = 0.0f;
            moveCount = 0.0f;
            FromPosition = Vector2.Zero;
            ToPosition = Vector2.Zero;
            toRotate = 0.0f;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the panel center coordinates.
        /// 
        /// �p�l�������̍��W���擾���܂��B
        /// </summary>
        public Vector2 Center
        {
            get { return Vector2.Add(Position, Origin); }
        }
        #endregion
    }
}
