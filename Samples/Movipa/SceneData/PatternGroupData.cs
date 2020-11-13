#region File Description
//-----------------------------------------------------------------------------
// PatternGroupData.cs
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
    /// This class manages the data of multiple pattern objects.
    /// In Layout, pattern groups correspond to these pattern objects.
    /// Sequence objects refer to these pattern groups and display them.
    ///
    /// �����̃p�^�[���I�u�W�F�N�g�f�[�^���܂Ƃ߂ĕێ�����N���X�ł��B
    /// Layout�ł̓p�^�[���O���[�v���������܂��B
    /// �V�[�P���X�I�u�W�F�N�g�́A���̃p�^�[���O���[�v��
    /// �Q�ƁE�\�����܂��B
    /// </summary>
    public class PatternGroupData
    {
        private List<PatternObjectData> patternObjectList = 
            new List<PatternObjectData>();

        #region Properties
        /// <summary>
        /// Obtains and sets the list of pattern data.
        /// This list can be also used to display a certain pattern.
        /// For details of pattern display, refer to the PatternObjectData.Draw function.
        ///
        /// �p�^�[���f�[�^�̃��X�g��ݒ�擾���܂��B
        /// ���̃��X�g�̓��e����A�C�ӂ̃p�^�[����\�����邱�Ƃ��\�ł��B
        /// �\���̏ڍׂ́APatternObjectData.Draw�֐����Q�Ƃ��������B
        /// </summary>
        public List<PatternObjectData> PatternObjectList
        {
            get
            {
                if (null == patternObjectList)
                    patternObjectList = new List<PatternObjectData>();

                return patternObjectList;
            }
        }
        #endregion
    }

}
