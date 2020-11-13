#region File Description
//-----------------------------------------------------------------------------
// SequencePlayData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// This class manages the sequence play status.
    /// This status is managed for each sequence bank in Layout.
    /// This class contains display frame data assigned to each 
    /// sequence group in a sequence bank.
    /// This data value will be the same in all sequence groups 
    /// unless special operations are performed.
    ///
    /// �V�[�P���X�̍Đ��󋵂��Ǘ����܂��B
    /// �Ǘ�����̂�Layout��ł̃V�[�P���X�o���N�P�ʂł��B
    /// �ێ�����f�[�^�́A�V�[�P���X�o���N���̊e�V�[�P���X�O���[�v�Ɋ��蓖�Ă���
    /// �\���t���[���ł��B���ʑ��삵�Ȃ�����A���ׂẴV�[�P���X�O���[�v��
    /// ���̃f�[�^�͓����l�ɂȂ�܂��B
    /// </summary>
    public class SequencePlayData
    {
        #region Fields

        //Sequence bank to be displayed 
        //�\������V�[�P���X�o���N
        private SequenceBankData sequenceData;

        //Reverse play flag 
        //�t�Đ��t���O
        private bool reverse;

        //Current frame of each sequence group contained 
        // in the sequence bank to be displayed
        //
        //�\������V�[�P���X�o���N�Ɋ܂܂��e�V�[�P���X�O���[�v�̌��݂̃t���[��
        private float[] playFrames;

        #endregion

        #region Properties

        /// <summary>
        /// Obtains the sequence bank to be displayed.
        ///
        /// �\������V�[�P���X�o���N���擾���܂��B
        /// </summary>
        public SequenceBankData SequenceData
        {
            get
            {
                return sequenceData;
            }
        }

        /// <summary>
        /// If the sequence data is being played, returns true.
        ///
        /// �Đ����̏ꍇ��true
        /// </summary>
        public bool IsPlay
        {
            get { return sequenceData.IsPlay; }
        }

        #endregion

        /// <summary>
        /// Sets the sequence time forward.
        ///
        /// �V�[�P���X�̎��Ԃ�i�߂܂��B
        /// </summary>
        /// <param name="elapsedGameTime">
        /// Time to be forwarded
        /// 
        /// �i�߂鎞��
        /// </param>
        public void Update(TimeSpan elapsedGameTime)
        {
            sequenceData.Update(playFrames, elapsedGameTime, reverse);
        }

        /// <summary>
        /// Draws the sequence data.
        ///
        /// �`�悵�܂��B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C�g�o�b�`
        /// </param>
        /// <param name="data">
        /// Conversion information for drawing
        /// 
        /// �`��ϊ����
        /// </param>
        public void Draw(SpriteBatch sb, DrawData data)
        {
            sequenceData.Draw(sb, data);
        }

        /// <summary>
        /// Constructor
        /// Sets the sequence bank to be played and resets the time and play direction.
        ///
        /// �R���X�g���N�^
        /// �Đ�����V�[�P���X�o���N��ݒ肵�A�����ƍĐ����������Z�b�g���܂��B
        /// </summary>
        /// <param name="bank">
        /// Sets the sequence bank to be drawn
        /// 
        /// �`�悷��V�[�P���X�o���N��ݒ肵�܂�
        /// </param>
        public SequencePlayData(SequenceBankData bank)
        {
            sequenceData = bank;
            reverse = false;
            playFrames = new float[bank.SequenceGroupList.Count];
        }

        /// <summary>
        /// Resets the play time and plays the sequence data from the beginning.
        ///
        /// �Đ����������Z�b�g���čŏ�����Đ����܂��B
        /// </summary>
        public void Replay()
        {
            foreach (SequenceGroupData group in sequenceData.SequenceGroupList)
            {
                group.Replay();
            }

            Update(new TimeSpan());
        }
    }
}
