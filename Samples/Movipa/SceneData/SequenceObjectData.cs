#region File Description
//-----------------------------------------------------------------------------
// SequenceObjectData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// Sequence object data is stored in a sequence group and refers
    /// to a pattern group.  A sequence group displays pattern groups 
    /// referred to by sequence objects in sequence.
    /// In this case, if interpolation is needed, necessary calculations 
    /// are performed in advance.  Otherwise, pictures will be only 
    /// sequentially switched like a simple animation.
    /// In Layout, sequence objects correspond to this sequence object data.
    ///
    /// �V�[�P���X�O���[�v���ɕێ�����A�p�^�[���O���[�v���Q�Ƃ��܂��B
    /// �V�[�P���X�O���[�v�͏����V�[�P���X�I�u�W�F�N�g���Q�Ƃ��Ă���
    /// �p�^�[���O���[�v��\�����Ă����܂��B
    /// ���̍ہA��Ԃ��K�v�ł���΂��炩���ߌv�Z���s���܂��B
    /// �⊮���Ȃ��ꍇ�́A�p�^�p�^�A�j���̂悤��
    /// �G�������؂�ւ�邾���ł��B
    /// Layout�ł̓V�[�P���X�I�u�W�F�N�g���������܂��B
    /// </summary>
    public class SequenceObjectData
    {
        #region Fields

        ///Frame to be displayed 
        ///�\������t���[��
        private int frame = 0;

        //Pattern group name to be displayed 
        //�\������p�^�[���O���[�v�̖��O
        private String patternGroupName = null;

        //Pattern group to be displayed 
        //�\������p�^�[���O���[�v
        private PatternGroupData patternGroup = null;

        #endregion

        #region Properties
        /// <summary>
        /// Obtains and sets the frame to be displayed.
        ///
        /// �\������t���[����ݒ�擾���܂��B
        /// </summary>
        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }

        /// <summary>
        /// Obtains and sets the name of the pattern group to be displayed.
        ///
        /// �\������p�^�[���O���[�v����ݒ�擾���܂��B
        /// </summary>
        public String PatternGroupName
        {
            get
            {
                return patternGroupName;
            }
            set
            {
                patternGroupName = value;
            }
        }

        /// <summary>
        /// Obtains the pattern object list in the pattern group to be displayed.
        ///
        /// �\������p�^�[���O���[�v���̃p�^�[���I�u�W�F�N�g���X�g���擾���܂��B
        /// </summary>
        [ContentSerializerIgnore()]
        public List<PatternObjectData> PatternObjectList
        {
            get
            {
                return patternGroup.PatternObjectList;
            }
        }
        #endregion


        /// <summary>
        /// Performs initialization.
        /// Obtains the pattern group by using the specified name.
        ///
        /// ���������܂��B
        /// �ݒ肳��Ă��閼�O����A�p�^�[���O���[�v���擾���܂��B
        /// </summary>
        /// <param name="list">
        /// Collection of pattern groups
        /// 
        /// �p�^�[���O���[�v�̃R���N�V����
        /// </param>
        public void Init(Dictionary<String, PatternGroupData> list)
        {
            if (list.ContainsKey(PatternGroupName))
                patternGroup = list[PatternGroupName];
        }
    }
}
