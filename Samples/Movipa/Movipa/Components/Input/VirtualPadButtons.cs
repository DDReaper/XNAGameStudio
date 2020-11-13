#region File Description
//-----------------------------------------------------------------------------
// VirtualPadButtons.cs
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
    /// This class manages the buttons of a virtual pad.
    /// There are the following buttons:
    /// A, B, X, Y, LeftShoulder, RightShoulder, LeftStick, RightStick, Back, and Start.
    ///
    /// ���z�p�b�h�̃{�^�����Ǘ����܂��B
    /// �{�^���ɂ�A�AB�AX�AY�ALeftShoulder�ARightShoulder�ALeftStick�ARightStick�A
    /// ������Back��Start������܂��B
    /// </summary>
    public class VirtualPadButtons
    {
        #region Fields
        private InputState a;
        private InputState b;
        private InputState x;
        private InputState y;
        private InputState leftShoulder;
        private InputState rightShoulder;
        private InputState leftStick;
        private InputState rightStick;
        private InputState back;
        private InputState start;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the input state of the A button.
        ///
        /// A�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState A
        {
            get { return a; }
        }


        /// <summary>
        /// Obtains the input state of the B button.
        ///
        /// B�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState B
        {
            get { return b; }
        }


        /// <summary>
        /// Obtains the input state of the X button.
        ///
        /// X�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState X
        {
            get { return x; }
        }


        /// <summary>
        /// Obtains the input state of the Y button.
        /// 
        /// Y�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Y
        {
            get { return y; }
        }


        /// <summary>
        /// Obtains the input state of the LeftShoulder button.
        ///
        /// LeftShoulder�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState LeftShoulder
        {
            get { return leftShoulder; }
        }


        /// <summary>
        /// Obtains the input state of the RightShoulder button.
        ///
        /// RightShoulder�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState RightShoulder
        {
            get { return rightShoulder; }
        }


        /// <summary>
        /// Obtains the input state of the LeftStick button.
        ///
        /// LeftStick�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState LeftStick
        {
            get { return leftStick; }
        }


        /// <summary>
        /// Obtains the input state of the RightStick button.
        ///
        /// RightStick�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState RightStick
        {
            get { return rightStick; }
        }


        /// <summary>
        /// Obtains the input state of the Back button.
        ///
        /// Back�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Back
        {
            get { return back; }
        }


        /// <summary>
        /// Obtains the input state of the Start button.
        ///
        /// Start�{�^���̓��͏�Ԃ��擾���܂��B
        /// </summary>
        public InputState Start
        {
            get { return start; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public VirtualPadButtons()
        {
            a = new InputState();
            b = new InputState();
            x = new InputState();
            y = new InputState();
            leftShoulder = new InputState();
            rightShoulder = new InputState();
            leftStick = new InputState();
            rightStick = new InputState();
            back = new InputState();
            start = new InputState();
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
            a.Update();
            b.Update();
            x.Update();
            y.Update();
            leftShoulder.Update();
            rightShoulder.Update();
            leftStick.Update();
            rightStick.Update();
            back.Update();
            start.Update();
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
            a.SetPress(press);
            b.SetPress(press);
            x.SetPress(press);
            y.SetPress(press);
            leftShoulder.SetPress(press);
            rightShoulder.SetPress(press);
            leftStick.SetPress(press);
            rightStick.SetPress(press);
            back.SetPress(press);
            start.SetPress(press);
        }
        #endregion
    }
}