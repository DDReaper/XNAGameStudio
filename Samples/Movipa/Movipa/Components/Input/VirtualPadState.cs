#region File Description
//-----------------------------------------------------------------------------
// VirtualPadState.cs
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
    /// This class manages the input state of a virtual pad.
    /// There are the following parameters: 
    /// button, cross button, stick, and trigger.
    ///
    /// ���z�p�b�h�̓��͏�Ԃ��Ǘ����܂��B
    /// �{�^���A�\���{�^���A�X�e�B�b�N�A�g���K�[�̃p�����[�^������܂��B
    /// </summary>
    public class VirtualPadState
    {
        #region Fields
        private VirtualPadButtons buttons;
        private VirtualPadDPad dPad;
        private VirtualPadThumbSticks thumbSticks;
        private VirtualPadTriggers triggers;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the input state of the button.
        ///
        /// �{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadButtons Buttons
        {
            get { return buttons; }
        }


        /// <summary>
        /// Obtains the input state of the cross button.
        ///
        /// �\���{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadDPad DPad
        {
            get { return dPad; }
        }


        /// <summary>
        /// Obtains the input state of the stick.
        ///
        /// �X�e�B�b�N�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadThumbSticks ThumbSticks
        {
            get { return thumbSticks; }
        }


        /// <summary>
        /// Obtains the input state of the trigger.
        ///
        /// �g���K�[�̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public VirtualPadTriggers Triggers
        {
            get { return triggers; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public VirtualPadState()
        {
            buttons = new VirtualPadButtons();
            dPad = new VirtualPadDPad();
            thumbSticks = new VirtualPadThumbSticks();
            triggers = new VirtualPadTriggers();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the input state of the virtual pad.
        ///
        /// ���z�p�b�h�̓��͏�Ԃ��X�V���܂��B
        /// </summary>
        public void Update()
        {
            buttons.Update();
            dPad.Update();
            thumbSticks.Update();
            triggers.Update();
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
            buttons.SetPress(press);
            dPad.SetPress(press);
            thumbSticks.SetPress(press);
            triggers.SetPress(press);
        }
        #endregion
    }
}