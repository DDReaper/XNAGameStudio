#region File Description
//-----------------------------------------------------------------------------
// InputState.cs
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
    /// This class manages input state.
    /// Specifically, it manages the following states:
    /// When there is no key input, when key input is started,  
    /// while key input is being performed, and when key input is ended.
    /// It also determines key repeat while key input is being performed.
    ///
    /// ���͏�Ԃ��Ǘ����܂��B
    /// ���͂���Ă��Ȃ���ԁA���͂��ꂽ�u�ԁA���͂��������Ă����ԁA
    /// �����ē��͂��I������u�Ԃ̏�Ԃ��Ǘ����܂��B
    /// �܂��A���͂��������Ă����Ԃ̃��s�[�g����Ȃǂ��s���܂��B
    /// </summary>
    public class InputState
    {
        #region Fields
        private int startInterval = 60;
        private int repeatInterval = 10;
        private VirtualKeyState state = VirtualKeyState.Free;
        private int pressCount = 0;
        private bool isPress = false;
        #endregion

        #region Properties
        /// <summary>
        /// Specify the key state.
        /// If it is equal to the virtual key state, return True.
        ///
        /// �L�[��Ԃ��w�肵�ē����ł����True��Ԃ��܂��B
        /// </summary>
        /// <param name="virtualKeyState"></param>
        /// <returns></returns>
        public bool this[VirtualKeyState virtualKeyState]
        {
            get { return (virtualKeyState == State); }
        }


        /// <summary>
        /// Obtains or sets the number of frames to start key repeat.
        ///
        /// �L�[���s�[�g���J�n����܂ł̃t���[�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int StartInterval
        {
            get { return startInterval; }
            set { startInterval = value; }
        }


        /// <summary>
        /// Obtains or sets the frame interval for key repeat.
        ///
        /// �L�[���s�[�g�̃t���[���Ԋu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int RepeatInterval
        {
            get { return repeatInterval; }
            set { repeatInterval = value; }
        }


        /// <summary>
        /// Obtains the key state for when it is pressed. 
        /// The value is masked by "3". Determine the key state by the following bits:
        /// 
        /// bits
        ///  00  : key is not pressed.
        ///  01  : key was pressed.
        ///  11  : key is being pressed.
        ///  10  : key was released.
        /// 
        /// �L�[�̉�����Ԃ��擾���܂��B
        /// �l��3�Ń}�X�N����A�r�b�g�ɂ���ĉ�����Ԃ𔻒肵�܂��B
        /// 
        /// bits
        ///  00  : ������Ă��܂��B
        ///  01  : ������܂����B
        ///  11  : ������Ă��܂��B
        ///  10  : ������܂����B
        /// </summary>
        public VirtualKeyState State
        {
            get { return (VirtualKeyState)((int)state & 0x03); }
        }


        /// <summary>
        /// Obtains the repeat state of the key.
        ///
        /// �L�[�̃��s�[�g��Ԃ��擾���܂��B
        /// </summary>
        public bool Repeat
        {
            get
            {
                // Does not determine key repeat when the key is not pressed.
                // 
                // �L�[��������Ă��鎞�̓��s�[�g��������܂���B
                if (State == VirtualKeyState.Free || State == VirtualKeyState.Release)
                    return false;

                if (State == VirtualKeyState.Push)
                    return true;

                if (pressCount < StartInterval)
                    return false;

                return (((pressCount - StartInterval) % RepeatInterval) == 0);
            }
        }


        /// <summary>
        /// Obtains or sets the key state for when it is pressed.
        ///
        /// �L�[�̉�����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool IsPress
        {
            get { return isPress; }
            set { isPress = value; }
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the key state.
        ///
        /// �L�[��Ԃ̍X�V�������s���܂��B
        /// </summary>
        public void Update()
        {
            // Updates the frame in which the key is pressed.
            // When obtaining the key state, uses the properties and masks it by "3".
            //
            // �L�[�̉����t���[�����X�V���܂��B
            // �擾����ꍇ�̓v���p�e�B�g�p���A3�Ń}�X�N���s���܂��B
            state = (VirtualKeyState)((int)State << 1);
            if (IsPress)
                state = (VirtualKeyState)((int)State | 1);

            // Increments the interval value.
            // 
            // �C���^�[�o���̏������s���܂��B
            pressCount = (IsPress) ? pressCount + 1 : 0;
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
            isPress = press;
        }


        /// <summary>
        /// Checks if any of the keys is in the "Push" state.
        ///
        /// �����̃L�[�̂����ꂩ���APush��ԂɂȂ��Ă��邩�`�F�b�N���܂��B
        /// </summary>
        public static bool IsPush(params InputState[] keys)
        {
            foreach (InputState inputState in keys)
            {
                if (inputState[VirtualKeyState.Push])
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Checks if any of the keys is in the "Push" or "Repeat" state.
        ///
        /// �����̃L�[�̂����ꂩ���APush�܂���Repeat��ԂɂȂ��Ă��邩�`�F�b�N���܂��B
        /// </summary>
        public static bool IsPushRepeat(params InputState[] keys)
        {
            foreach (InputState inputState in keys)
            {
                if (inputState[VirtualKeyState.Push] || inputState.Repeat)
                    return true;
            }

            return false;
        }
        #endregion
    }
}