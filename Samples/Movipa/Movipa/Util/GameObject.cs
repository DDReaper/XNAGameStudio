#region File Description
//-----------------------------------------------------------------------------
// GameObject.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Includes initialized, released, visible and enabled status flags.
    /// 
    /// �@�\���g�������I�u�W�F�N�g�N���X�ł��B
    /// �������ƊJ���A����Ԃ⓮���Ԃ̃t���O�������܂��B
    /// </summary>
    public class GameObject : IDisposable
    {
        #region Fields
        private bool initialized = false;
        private bool disposed = false;
        private bool visible = true;
        private bool enabled = true;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the initialization status.
        /// 
        /// ��������Ԃ��擾���܂��B
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
        }

        /// <summary>
        /// Obtains the release status. 
        /// 
        /// �J����Ԃ��擾���܂��B
        /// </summary>
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Obtains or sets the visibility status.
        ///
        /// ����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// Obtains or sets the enabled status.
        /// 
        /// �����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public virtual void Initialize()
        {
            initialized = true;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed) 
            {
                disposed = true;
            }
        }

        #endregion
    }
}