#region File Description
//-----------------------------------------------------------------------------
// VirtualPadTriggers.cs
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
    /// This class gets the input state of the triggers of a virtual pad.
    /// The obtained values are converted to digital values, not to analog values.
    ///
    /// ���z�p�b�h�̃g���K�[�̓��͏�Ԃ��擾���܂��B
    /// �擾�����l�̓A�i���O�ł͂Ȃ��A�f�W�^���ɕϊ�����܂��B
    /// </summary>
    public class VirtualPadTriggers
    {
        #region Fields
        private InputState left = new InputState();
        private InputState right = new InputState();
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the input state of the left trigger.
        ///
        /// ���g���K�[�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Left
        {
            get { return left; }
        }


        /// <summary>
        /// Obtains the input value of the right trigger.
        ///
        /// �E�g���K�[�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Right
        {
            get { return right; }
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Obtains the input state of the trigger.
        ///
        /// �g���K�[�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public void Update()
        {
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
            left.SetPress(press);
            right.SetPress(press);
        }
        #endregion
    }
}