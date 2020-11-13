#region File Description
//-----------------------------------------------------------------------------
// SceneData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// This class manages scene data (pattern and sequence).
    /// In Layout, stage data corresponds to this scene data.
    ///
    /// �V�[���f�[�^�i�p�^�[���A�V�[�P���X�j��ێ����܂��B
    /// Layout�ł̓X�e�[�W�f�[�^�ɑ������܂��B
    /// </summary>
    public class SceneData
    {
        #region Fields
        //List of pattern groups
        //
        //�p�^�[���O���[�v�̃��X�g
        private Dictionary<String, PatternGroupData> patternGroupDictionary = 
            new Dictionary<string,PatternGroupData>();
        //List of sequence banks
        //
        //�V�[�P���X�o���N�̃��X�g
        private Dictionary<String, SequenceBankData> sequenceBankDictionary = 
            new Dictionary<string,SequenceBankData>();
        //List of sequence play data
        //
        //�V�[�P���X�Đ��f�[�^�̃��X�g
        private List<SequencePlayData> sequencePlayList = new List<SequencePlayData>();
        #endregion

        #region Propaties

        /// <summary>
        /// Obtains the dictionary list for pattern groups.
        ///
        /// �p�^�[���O���[�v�f�[�^�̎������X�g��ݒ�擾���܂��B
        /// </summary>
        public Dictionary<String, PatternGroupData> PatternGroupDictionary
        {
            get { return patternGroupDictionary; }
        }

        /// <summary>
        /// Obtains the dictionary list for sequence bank data.
        ///
        /// �V�[�P���X�o���N�f�[�^�̎������X�g��ݒ�擾���܂��B
        /// </summary>
        public Dictionary<String, SequenceBankData> SequenceBankDictionary
        {
            get { return sequenceBankDictionary; }
        }
        #endregion

        /// <summary>
        /// Creates data to play the sequence.
        /// When specifying the target sequence, uses the sequence bank name.
        /// 
        /// �V�[�P���X���Đ����邽�߂̃f�[�^���쐬���܂��B
        /// �Ώۂ́A�V�[�P���X�o���N���Ŗ��O�Ŏw�肵�܂��B
        /// </summary>
        /// <param name="name">
        /// Sequence name
        /// 
        /// �V�[�P���X�̖��O
        /// </param>
        /// <returns></returns>
        public SequencePlayData CreatePlaySeqData(String name)
        {
            return new SequencePlayData(sequenceBankDictionary[name]);
        }

        /// <summary>
        /// Adds sequences to be played.
        /// When adding them to the list, the display priority specified in 
        /// the layout tool must be considered.
        /// 
        /// �Đ�����V�[�P���X��ǉ����܂��B
        /// ���C�A�E�g�c�[���Ŏw�肳�ꂽ�\���D�揇�ʂ��l������
        /// ���X�g�ɒǉ����܂��B
        /// </summary>
        /// <param name="data">
        /// Sequence data to be played
        /// 
        /// �Đ�����V�[�P���X�̃f�[�^
        /// </param>
        public void AddPlaySeqData(SequencePlayData data)
        {
            int nInsertIndex = 0;

            for(int i = sequencePlayList.Count - 1; i >= 0; i--)
            {
                int nZPos = sequencePlayList[i].SequenceData.ZPos;

                if (nZPos <= data.SequenceData.ZPos)
                {
                    nInsertIndex = i + 1;

                    break;
                }
            }

            sequencePlayList.Insert(nInsertIndex, data);
        }

        /// <summary>
        /// Adds the sequences specified by their names to the play list.
        /// Creation of sequence play data and addition of it to the list 
        /// can be performed at the same time.
        ///
        /// ���O�Ŏw�肵���V�[�P���X���Đ����X�g�ɒǉ����܂��B
        /// �V�[�P���X�Đ��f�[�^�̍쐬�ƁA���X�g�ւ̒ǉ��𓯎��ɍs���܂��B
        /// </summary>
        /// <param name="name">
        /// Sequence name to be added
        ///
        /// �ǉ�����V�[�P���X��
        /// </param>
        public void AddPlaySeqData(String name)
        {
            AddPlaySeqData(CreatePlaySeqData(name));
        }

        /// <summary>
        /// Sets the sequence time in the play list forward.
        /// Updates the scene data.
        ///
        /// �Đ����X�g�ɂ���V�[�P���X�̎��Ԃ�i�߂܂��B
        /// �V�[���f�[�^�̍X�V�֐��ł��B
        /// </summary>
        /// <param name="elapsedGameTime">
        /// Time to be forwarded
        /// 
        /// �i�߂鎞��
        /// </param>
        public void Update(TimeSpan elapsedGameTime)
        {
            foreach (SequencePlayData data in sequencePlayList)
                data.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws the sequence in the play list.
        ///
        /// �Đ����X�g�ɂ���V�[�P���X��`�悵�܂��B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C���o�b�`
        /// </param>
        /// <param name="baseDrawData">
        /// Conversion information that affects the entire drawing target
        /// 
        /// �`��Ώۂ��ׂĂɉe������ϊ����
        /// </param>
        public void Draw(SpriteBatch sb, DrawData baseDrawData)
        {
            foreach (SequencePlayData data in sequencePlayList)
                data.Draw(sb, baseDrawData);
        }

        /// <summary>
        /// Displays the pattern group specified by its name.
        ///
        /// ���O�Ŏw�肵���p�^�[���O���[�v��\�����܂��B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C���o�b�`
        /// </param>
        /// <param name="name">
        /// Pattern group name
        /// 
        /// �p�^�[���O���[�v�̖��O
        /// </param>
        /// <param name="baseDrawData">
        /// Conversion information that affects the entire drawing target
        /// 
        /// �`��Ώۂ��ׂĂɉe������ϊ����
        /// </param>
        public void DrawPattern(SpriteBatch sb, String name, DrawData baseDrawData)
        {
            PatternGroupData group = PatternGroupDictionary[name];

            foreach (PatternObjectData pattern in group.PatternObjectList)
                pattern.Draw(sb, pattern.Data, baseDrawData);
        }

        /// <summary>
        /// Specifies a pattern group by its name and specifies a pattern object
        /// in this pattern group by its index to obtain the position information.
        ///
        /// �p�^�[���O���[�v�𖼑O�Ŏw�肵�A
        /// ���̓����ɂ���p�^�[���I�u�W�F�N�g���C���f�b�N�X�Ŏw��B
        /// �ʒu�����擾���܂��B
        /// </summary>
        /// <param name="name">
        /// Pattern group name
        /// 
        /// �p�^�[���O���[�v�̖��O
        /// </param>
        /// <param name="nObjectId">
        /// Pattern object ID
        /// 
        /// �p�^�[���I�u�W�F�N�g��ID
        /// </param>
        /// <returns></returns>
        public Point GetPatternPosition(String name, int nObjectId)
        {
            return PatternGroupDictionary[name].PatternObjectList[nObjectId].
                Data.Position;
        }
    }
}
