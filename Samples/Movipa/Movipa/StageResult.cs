#region File Description
//-----------------------------------------------------------------------------
// StageResult.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace Movipa
{
    /// <summary>
    /// Contains the stage play results information.
    /// Retains the movement count together with score and clear time information.
    /// 
    /// �X�e�[�W�̃v���C���ʂ̏��������܂��B
    /// �ړ��񐔂ƃX�R�A�A�N���A�^�C���̏���ێ����܂��B
    /// </summary>
    public class StageResult
    {
        #region Fields
        // Movement count
        // 
        // �ړ���
        private long moveCount;

        // Single completed score
        // 
        // �V���O�������X�R�A
        private long singleScore;

        // Double completed score
        // 
        // �_�u�������X�R�A
        private long doubleScore;

        // Hint score
        // 
        // �q���g�̃X�R�A
        private long hintScore;
        
        // Clear time
        // 
        // �N���A����
        private TimeSpan clearTime;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the movement count.
        /// 
        /// �ړ��񐔂��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long MoveCount
        {
            get { return moveCount; }
            set { moveCount = value; }
        }


        /// <summary>
        /// Obtains or sets the single score.
        /// 
        /// �V���O���X�R�A���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long SingleScore
        {
            get { return singleScore; }
            set { singleScore = value; }
        }


        /// <summary>
        /// Obtains or sets the double score.
        /// 
        /// �_�u���X�R�A���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long DoubleScore
        {
            get { return doubleScore; }
            set { doubleScore = value; }
        }


        /// <summary>
        /// Obtains or sets the hint score.
        /// 
        /// �q���g�X�R�A���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public long HintScore
        {
            get { return hintScore; }
            set { hintScore = value; }
        }


        /// <summary>
        /// Obtains or sets the clear time.
        /// 
        /// �N���A�^�C�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public TimeSpan ClearTime
        {
            get { return clearTime; }
            set { clearTime = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public StageResult()
        {
            moveCount = 0;
            singleScore = 0;
            doubleScore = 0;
            hintScore = 0;
            clearTime = TimeSpan.Zero;
        }
        #endregion
    }
}
