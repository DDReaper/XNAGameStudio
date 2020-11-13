#region File Description
//-----------------------------------------------------------------------------
// SequenceBankData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// This class manages multiple sequence groups.
    /// In Layout, sequence banks correspond to these sequence groups.
    /// The display priority can be specified by the sequence bank property of Layout.
    ///
    /// �����̃V�[�P���X�O���[�v��ێ�����N���X�ł��B
    /// Layout�ł́A�V�[�P���X�o���N���������܂��B
    /// �\���v���C�I���e�B�[�́ALayout�̃V�[�P���X�o���N�v���p�e�B�[��
    /// �ݒ�ł��܂��B
    /// </summary>
    public class SequenceBankData
    {
        #region Fields

        private int zPosition;//Display priority //�\���v���C�I���e�B�[

        //Sequence group data 
        //�V�[�P���X�O���[�v�f�[�^
        private List<SequenceGroupData> sequenceGroupList = 
            new List<SequenceGroupData>();

        #endregion

        #region Properties
        /// <summary>
        /// Obtains and sets the display order.
        ///
        /// �\�����ʂ�ݒ�擾���܂��B
        /// </summary>
        public int ZPos
        {
            get
            {
                return zPosition;
            }
            set
            {
                zPosition = value;
            }
        }


        /// <summary>
        /// Obtains the list of sequence groups.
        ///
        /// �V�[�P���X�O���[�v�̂̃��X�g��ݒ�擾���܂��B
        /// </summary>
        public List<SequenceGroupData> SequenceGroupList
        {
            get { return sequenceGroupList; }
        }

        /// <summary>
        /// Obtains whether the held sequences are being played or not.
        /// If they are not being played, returns false.
        ///
        /// �ێ����Ă���V�[�P���X�Q���Đ������ǂ������擾���܂��B
        /// ��~���Ă���ꍇ��false�ł��B
        /// </summary>
        public bool IsPlay
        {
            get
            {
                bool result = false;
                foreach (SequenceGroupData group in SequenceGroupList)
                {
                    if (!group.IsStop)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }
        #endregion

        /// <summary>
        /// Sets the held sequence time forward.
        ///
        /// �ێ����Ă���V�[�P���X�̎��Ԃ�i�߂܂��B
        /// </summary>
        /// <param name="playFrames">
        /// Frame of each current sequence
        /// 
        /// ���݂̊e�V�[�P���X�̃t���[��
        /// </param>
        /// <param name="elapsedGameTime">
        /// Time to be forwarded
        /// 
        /// �i�߂鎞��
        /// </param>
        /// <param name="bReverse">
        /// Specifies true in case of reverse play
        ///
        /// �t�Đ��̏ꍇtrue
        /// </param>
        public void Update(float[] playFrames, TimeSpan elapsedGameTime, bool bReverse)
        {
            int nIndex = 0;

            foreach(SequenceGroupData group in SequenceGroupList)
            {
                playFrames[nIndex] = group.Update(   playFrames[nIndex],
                                                        elapsedGameTime, 
                                                        bReverse);

                nIndex++;
            }
        }

        /// <summary>
        /// Draws the held sequence group.
        /// Conversion settings can be applied to the entire sequence by specifying 
        /// values to baseDrawData.
        /// 
        /// �ێ����Ă���V�[�P���X�O���[�v��`�悵�܂��B
        /// baseDrawData�ɒl��ݒ肷�邱�ƂŁA�V�[�P���X�S�̂ɕϊ���K�p���邱�Ƃ�
        /// �o���܂��B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C�g�o�b�`
        /// </param>
        /// <param name="baseDrawData">
        /// Conversion information that affects the entire drawing target
        ///
        /// �`��ΏۑS�̉e������ϊ��p���
        /// </param>
        public void Draw(SpriteBatch sb, DrawData baseDrawData)
        {
            foreach (SequenceGroupData group in SequenceGroupList)
            {
                group.Draw(sb, baseDrawData);
            }
        }

        /// <summary>
        /// Obtains the conversion information for the patterns belonging 
        /// to the held sequence.  Any display operation related to animations 
        /// can be performed by using this data.
        ///
        /// �ێ����Ă���V�[�P���X�ɏ�������p�^�[���̕ϊ������擾���܂��B
        /// ���̃f�[�^��p���ăA�j���[�V�����ɒǐ������C�ӂ̕\�����s�����Ƃ��ł��܂��B
        /// </summary>
        /// <param name="sequenceGroupId">
        /// Sequence group ID
        /// 
        /// �V�[�P���X�O���[�v��ID
        /// </param>
        /// <param name="sequenceObjectId">
        /// Sequence object ID
        /// 
        /// �V�[�P���X�I�u�W�F�N�g��ID
        /// </param>
        /// <param name="patternObjectId">
        /// Pattern object ID
        /// 
        /// �p�^�[���I�u�W�F�N�g��ID
        /// </param>
        /// <returns></returns>
        public DrawData GetDrawPatternObjectDrawData( int sequenceGroupId,
                                                int sequenceObjectId,
                                                int patternObjectId)
        {
            return SequenceGroupList[sequenceGroupId].
                    SequenceObjectList[sequenceObjectId].
                    PatternObjectList[patternObjectId].InterpolationDrawData;
        }

        /// <summary>
        /// Obtains the conversion information for the patterns belonging 
        /// to the sequence object that is being drawn in the held sequence.
        /// Any display operation related to animations can be performed
        /// by using this data.
        ///
        /// �ێ����Ă���V�[�P���X�̌��ݕ`�撆�̃V�[�P���X�I�u�W�F�N�g�ɏ�������
        /// �p�^�[���̕ϊ������擾���܂��B
        /// ���̃f�[�^��p���ăA�j���[�V�����ɒǐ������C�ӂ̕\�����s�����Ƃ��ł��܂��B
        /// </summary>
        /// <param name="sequenceGroupId">
        /// Sequence group ID
        /// 
        /// �V�[�P���X�O���[�v��ID
        /// </param>
        /// <param name="patternObjectId">
        /// Pattern object ID
        /// 
        /// �p�^�[���I�u�W�F�N�g��ID
        /// </param>
        /// <returns></returns>
        public DrawData GetDrawPatternObjectDrawData(int sequenceGroupId,
                                                    int patternObjectId)
        {
            return SequenceGroupList[sequenceGroupId].CurrentObjectList.
                PatternObjectList[patternObjectId].InterpolationDrawData;
        }



    }
}
