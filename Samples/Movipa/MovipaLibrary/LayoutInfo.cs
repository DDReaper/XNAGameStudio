#region File Description
//-----------------------------------------------------------------------------
// LayoutInfo.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class manages animation information of Layout.
    /// The asset name for SceneData and the sequence name to be used
    /// are managed in this class.
    ///
    /// Layout�̃A�j���[�V�������������܂��B
    /// SceneData�ւ̃A�Z�b�g���ƁA�g�p����V�[�P���X��������܂��B
    /// </summary>
    public class LayoutInfo : AnimationInfo
    {
        #region Fields
        private string sceneDataAsset;
        private string sequence;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the asset name for the scene data.
        ///
        /// �V�[���f�[�^�̃A�Z�b�g�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string SceneDataAsset
        {
            get { return sceneDataAsset; }
            set { sceneDataAsset = value; }
        }


        /// <summary>
        /// Obtains or sets the sequence name.
        ///
        /// �V�[�P���X�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }
        #endregion
    }
}
