#region File Description
//-----------------------------------------------------------------------------
// MenuData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

using Movipa.Components.Animation;
using Movipa.Components.Scene.Puzzle;
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    #region Public Types
    /// <summary>
    /// Defines the menu items.
    /// 
    /// ���j���[�̍��ڂ��`���܂��B
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// Mode selection
        /// 
        /// ���[�h�I��
        /// </summary>
        SelectMode,

        /// <summary>
        /// Save File selection
        ///
        /// �Z�[�u�t�@�C���I��
        /// </summary>
        SelectFile,

        /// <summary>
        /// Style selection
        ///
        /// �X�^�C���I��
        /// </summary>
        SelectStyle,

        /// <summary>
        /// Rotation setting
        ///
        /// ��]�̐ݒ�
        /// </summary>
        RotateSelect,

        /// <summary>
        /// Movie selection
        ///
        /// ���[�r�[�I��
        /// </summary>
        SelectMovie,

        /// <summary>
        /// Split setting
        /// 
        /// �������̐ݒ�
        /// </summary>
        SelectDivide,

        /// <summary>
        /// Confirmation screen 
        ///
        /// �m�F���
        /// </summary>
        Ready,
    }
    #endregion
}
