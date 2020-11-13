#region File Description
//-----------------------------------------------------------------------------
// VirtualPadDPad.cs
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
    /// This class manages the cross button of a virtual pad.
    /// There are the following parameters: 
    /// Up, Down, Left, and Right.
    ///
    /// ���z�p�b�h�̏\���{�^�����Ǘ����܂��B
    /// Up�ADown�ALeft�ARight�̃p�����[�^������܂��B
    /// </summary>
    public class VirtualPadDPad
    {
        #region Fields
        private InputState up;
        private InputState down;
        private InputState left;
        private InputState right;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the input state of the Up button.
        ///
        /// Up�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Up
        {
            get { return up; }
        }


        /// <summary>
        /// Obtains the input state of the Down button.
        ///
        /// Down�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Down
        {
            get { return down; }
        }


        /// <summary>
        /// Obtains the input state of the Left button.
        ///
        /// Left�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Left
        {
            get { return left; }
        }


        /// <summary>
        /// Obtains the input state of the Right button.
        ///
        /// Right�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Right
        {
            get { return right; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public VirtualPadDPad()
        {
            up = new InputState();
            down = new InputState();
            left = new InputState();
            right = new InputState();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the state of the button.
        ///
        /// �{�^���̏�Ԃ��X�V���܂��B
        /// </summary>
        public void Update()
        {
            up.Update();
            down.Update();
            left.Update();
            right.Update();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Sets the key state for when it is pressed.
        ///
        /// �L�[�̉�����Ԃ�ݒ肵�܂��B
        /// </summary>
        public void SetPress(bool press)
        {
            up.SetPress(press);
            down.SetPress(press);
            left.SetPress(press);
            right.SetPress(press);
        }
        #endregion
    }
}