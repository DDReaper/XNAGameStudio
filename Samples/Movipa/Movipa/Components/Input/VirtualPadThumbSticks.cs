#region File Description
//-----------------------------------------------------------------------------
// VirtualPadThumbSticks.cs
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
    /// This class manages the state of the sticks of a virtual pad.
    /// There are the following two parameters: left stick and right stick.
    /// The obtained values are converted to digital values, not to analog values.
    ///
    /// ���z�p�b�h�̃X�e�B�b�N�̏�Ԃ��Ǘ����܂��B
    /// ���X�e�B�b�N�ƉE�X�e�B�b�N�̃p�����[�^������܂��B
    /// �擾�����l�̓A�i���O�ł͂Ȃ��A�f�W�^���ɕϊ�����܂��B
    /// </summary>
    public class VirtualPadThumbSticks
    {
        #region Fields
        private VirtualPadDPad left = new VirtualPadDPad();
        private VirtualPadDPad right = new VirtualPadDPad();
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the input state of the left stick.
        ///
        /// ���X�e�B�b�N�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadDPad Left
        {
            get { return left; }
        }


        /// <summary>
        /// Obtains the input state of the right stick.
        ///
        /// �E�X�e�B�b�N�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadDPad Right
        {
            get { return right; }
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the input state of the stick.
        ///
        /// �X�e�B�b�N�̓��͏�Ԃ��X�V���܂��B
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