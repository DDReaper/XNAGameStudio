#region File Description
//-----------------------------------------------------------------------------
// StageSetting.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class manages setting information of stages.
    /// ContentTypeReader and ContentTypeWriter are provided in this class
    /// so that stage construction information can be specified by ContentPipeline.
    ///
    /// �X�e�[�W�̐ݒ�����Ǘ����܂��B
    /// ���̃N���X�́A�X�e�[�W�\����ContentPipeline��ʂ��Đݒ�o����悤��
    /// ContentTypeReader��ContentTypeWriter��p�ӂ��Ă��܂��B
    /// </summary>
    public class StageSetting
    {
        #region Public Types
        /// <summary>
        /// Game mode
        ///
        /// �Q�[�����[�h
        /// </summary>
        public enum ModeList
        {
            Normal,
            Free,
        }

        /// <summary>
        /// Panel switch mode
        ///
        /// �p�l���̓���ւ��郂�[�h
        /// </summary>
        public enum StyleList
        {
            Change,
            Revolve,
            Slide,
        }

        /// <summary>
        /// Rotation
        ///
        /// ��]
        /// </summary>
        public enum RotateMode
        {
            On,
            Off,
        }
        #endregion

        #region Fields
        private ModeList mode;
        private StyleList style;
        private RotateMode rotate;
        private string movie;
        private Point divide;
        private TimeSpan timeLimit;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the game mode.
        ///
        /// �Q�[�����[�h���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public ModeList Mode
        {
            get { return mode; }
            set { mode = value; }
        }


        /// <summary>
        /// Obtains or sets the game style.
        ///
        /// �Q�[���X�^�C�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public StyleList Style
        {
            get { return style; }
            set { style = value; }
        }


        /// <summary>
        /// Obtains or sets the rotation information (rotation is enabled or not).
        ///
        /// ��]�̗L�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public RotateMode Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }


        /// <summary>
        /// Obtains or sets the asset name for the movie information.
        ///
        /// ���[�r�[���ւ̃A�Z�b�g�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Movie
        {
            get { return movie; }
            set { movie = value; }
        }


        /// <summary>
        /// Obtains or sets the number of divisions.
        ///
        /// ���������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Point Divide
        {
            get { return divide; }
            set { divide = value; }
        }


        /// <summary>
        /// Obtains or sets the time limit of the stage.
        ///
        /// �X�e�[�W�̐������Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [ContentSerializerIgnore]
        public TimeSpan TimeLimit
        {
            get { return timeLimit; }
        }

        /// <summary>
        /// Obtains or sets the time limit of the stage as a character string.
        ///
        /// �X�e�[�W�̐������Ԃ𕶎���^�Ŏ擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string TimeLimitString
        {
            get
            {
                return timeLimit.ToString();
            }
            set
            {
                TimeSpan result = new TimeSpan();
                try
                {
                    result = TimeSpan.Parse(value);
                }
                finally
                {
                    timeLimit = result;
                }
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public StageSetting()
        {
            mode = ModeList.Normal;
            style = StyleList.Change;
            rotate = RotateMode.Off;
            movie = String.Empty;
            divide = new Point(3, 3);
            timeLimit = new TimeSpan();
        }
        #endregion
    }

}
