#region File Description
//-----------------------------------------------------------------------------
// SaveData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#endregion

namespace Movipa
{
    /// <summary>
    /// Manages the values used by Save Data.
    /// Serialized and deserialized in XML, except for the TimeSpan type,
    /// which is serialized and deserialized via text strings by returning the value
    /// in TimeSpan.Parse.
    /// 
    /// �Z�[�u�f�[�^�Ŏg�p����l���Ǘ����܂��B
    /// XML�ŃV���A���C�Y�A�f�V���A���C�Y���s���܂����ATimeSpan�^�̂ݕ���������
    /// �V���A���C�Y�A�f�V���A���C�Y���͕������TimeSpan.Parse�Œl��߂��Ă��܂��B
    /// </summary>
    public class SaveData
    {
        #region Fields
        // Number of stages
        // 
        // �X�e�[�W��
        private int stage;

        // Play count
        // 
        // �v���C��
        private int playCount;

        // Score
        //
        //�X�R�A
        private long score;

        // Best score
        //
        // �x�X�g�X�R�A
        private long bestScore;

        // Best clear time
        //
        // �x�X�g�N���A�^�C��
        private TimeSpan bestTime;

        // Total play time
        // 
        // ���v���C����
        private TimeSpan totalPlayTime;

        // File name
        // The original file name must be used when overwriting the save. 
        //
        // �t�@�C����
        // �Z�[�u���㏑�����鎞�Ɍ��̃t�@�C���������K�v������܂��B
        private string fileName;
        #endregion

        #region Property
        /// <summary>
        /// Obtains or sets the number of stages.
        /// 
        /// �X�e�[�W�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        /// <summary>
        /// Obtains or sets the play count.
        /// 
        /// �v���C�񐔂��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int PlayCount
        {
            get { return playCount; }
            set { playCount = value; }
        }

        /// <summary>
        /// Obtains or sets the score.
        ///
        /// �X�R�A���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// Obtains or sets the best score.
        /// 
        /// �x�X�g�X�R�A���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long BestScore
        {
            get { return bestScore; }
            set { bestScore = value; }
        }

        /// <summary>
        /// Obtains or sets the best time. 
        /// 
        /// �x�X�g�^�C�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [XmlIgnoreAttribute()]
        public TimeSpan BestTime
        {
            get { return bestTime; }
            set { bestTime = value; }
        }

        /// <summary>
        /// Obtains or sets the best time as a text string. 
        /// 
        /// �x�X�g�^�C���𕶎���Ŏ擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string BestTimeString
        {
            get
            {
                return bestTime.ToString();
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
                    bestTime = result;
                }
            }
        }


        /// <summary>
        /// Obtains or sets the total play time.
        /// 
        /// ���v���C�^�C�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [XmlIgnoreAttribute()]
        public TimeSpan TotalPlayTime
        {
            get { return totalPlayTime; }
            set { totalPlayTime = value; }
        }


        /// <summary>
        /// Obtains or sets the total play time as a text string.
        /// 
        /// ���v���C�^�C���𕶎���^�Ŏ擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string TotalPlayTimeString
        {
            get
            {
                return totalPlayTime.ToString();
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
                    totalPlayTime = result;
                }
            }
        }


        /// <summary>
        /// Obtains or sets the file name.
        /// 
        /// �t�@�C�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SaveData()
        {
            stage = 0;
            playCount = 0;
            score = 0;
            bestScore = 0;
            bestTime = new TimeSpan();
            totalPlayTime = new TimeSpan();
            fileName = String.Empty;
        }
        #endregion
    }
}
