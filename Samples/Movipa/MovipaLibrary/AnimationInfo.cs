#region File Description
//-----------------------------------------------------------------------------
// AnimationInfo.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class manages animation information.
    /// If there is information used in a movie, inherit this class.
    /// Type, name, and size of an animation are managed in this class.
    ///
    /// �A�j���[�V�����������N���X�ł��B
    /// ���[�r�[�Ŏg�p����������ꍇ�͂��̃N���X���p�����܂��B
    /// �A�j���[�V�����̃^�C�v�A���O�A�����ăT�C�Y�̏�񂪂���܂��B
    /// </summary>
    public class AnimationInfo
    {
        #region Public Types
        // Animation type
        // 
        // �A�j���[�V�����̎��
        public enum AnimationInfoCategory
        {
            Layout,
            Rendering,
            SkinnedModelAnimation,
            Particle,
        }
        #endregion

        #region Fields
        private AnimationInfoCategory category;
        private string name;
        private Point size;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the type.
        ///
        /// �^�C�v���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public AnimationInfoCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Obtains or sets the name.
        ///
        /// ���O���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Obtains or sets the size.
        ///
        /// �T�C�Y���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Point Size
        {
            get { return size; }
            set { size = value; }
        }
        #endregion
    }
}
