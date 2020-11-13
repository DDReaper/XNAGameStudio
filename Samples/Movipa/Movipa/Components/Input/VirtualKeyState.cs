#region File Description
//-----------------------------------------------------------------------------
// VirtualKeyState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
#endregion

namespace Movipa.Components.Input
{
    /// <summary>
    /// Key state for when it is pressed
    ///
    /// �L�[�̉������
    /// </summary>
    public enum VirtualKeyState
    {
        /// <summary>
        /// Key is not pressed.
        ///
        /// ������Ă��܂��B
        /// </summary>
        Free = 0,

        /// <summary>
        /// Key was pressed.
        ///
        /// ������܂����B
        /// </summary>
        Push = 1,

        /// <summary>
        /// Key is being pressed.
        ///
        /// ������Ă��܂��B
        /// </summary>
        Press = 3,

        /// <summary>
        /// Key was released.
        ///
        /// ������܂����B
        /// </summary>
        Release = 2,
    }
}