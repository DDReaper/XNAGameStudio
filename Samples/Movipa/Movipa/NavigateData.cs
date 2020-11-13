#region File Description
//-----------------------------------------------------------------------------
// NavigateData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
#endregion

namespace Movipa
{
    /// <summary>
    /// Manages the Navigate button and text string status,
    /// including text string to display and blink status.
    /// Navigate drawing is performed by the SceneComponent class.
    /// 
    /// �i�r�Q�[�g�{�^���ƕ�����̏�Ԃ��Ǘ����܂��B
    /// �\�����镶����ƁA�_�ł̏�Ԃ������Ă��܂��B
    /// �i�r�Q�[�g�̕`���SceneComponent�N���X�ōs���Ă��܂��B
    /// </summary>
    public class NavigateData
    {
        #region Fields
        /// <summary>
        /// Text string displayed in Navigate
        /// 
        /// �i�r�Q�[�g�ɕ\�����镶����
        /// </summary>
        private string message = String.Empty;

        /// <summary>
        /// Blink mode
        /// 
        /// �_�ł��郂�[�h
        /// </summary>
        private bool blink = false;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public NavigateData()
        {
        }


        /// <summary>
        /// Specifies the text string to display and initializes the instance.
        ///
        /// �\�����镶������w�肵�ăC���X�^���X�����������܂��B
        /// </summary>
        /// <param name="text">Text string to display</param>
        ///  
        /// <param name="text">�\�����镶����</param>
        public NavigateData(string text)
        {
            Message = text;
        }


        /// <summary>
        /// Specifies the text string to display and the blink mode, 
        /// then initializes the instance.
        /// <param name="text">Text string to display</param>
        /// <param name="blink">Blink status</param>
        ///  
        /// </summary>
        /// �\�����镶����Ɠ_�Ń��[�h���w�肵�ăC���X�^���X�����������܂��B
        /// <param name="text">�\�����镶����</param>
        /// <param name="blink">�_�ŏ��</param>
        public NavigateData(string text, bool blink)
        {
            Message = text;
            Blink = blink;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the text string to display in the Navigate.
        /// 
        /// �i�r�Q�[�g�ɕ\�����镶������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        /// <summary>
        /// Obtains or sets the blink mode.
        /// 
        /// �_�Ń��[�h���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool Blink
        {
            get { return blink; }
            set { blink = value; }
        }
        #endregion
    }
}
