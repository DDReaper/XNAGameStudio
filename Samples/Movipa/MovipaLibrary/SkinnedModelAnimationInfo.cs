#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelAnimationInfo.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class manages animation information of a skin model.
    /// Camera position and direction as well as model data are managed
    /// in this class.  SkinnedModelInfo is defined in a list format 
    /// so that multiple models can be managed.
    ///
    /// �X�L�����f���̃A�j���[�V�������������܂��B
    /// ���f���f�[�^�̑��ɁA�J�����̈ʒu������̏��������܂��B
    /// �����̃��f�����Ǘ��ł���悤��SkinnedModelInfo�̓��X�g�`���Œ�`����Ă��܂��B
    /// </summary>
    public class SkinnedModelAnimationInfo : AnimationInfo
    {
        #region Fields
        private List<SkinnedModelInfo> skinnedModelInfoCollection = 
            new List<SkinnedModelInfo>();
        private Vector3 cameraUpVector;
        private Vector3 cameraPosition;
        private Vector3 cameraLookAt;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the list of the skin model information.
        ///
        /// �X�L�����f�����̃��X�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public List<SkinnedModelInfo> SkinnedModelInfoCollection
        {
            get { return skinnedModelInfoCollection; }
        }


        /// <summary>
        /// Obtains or sets the camera coordinate system.
        ///
        /// �J�����̍��W�n���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraUpVector
        {
            get { return cameraUpVector; }
            set { cameraUpVector = value; }
        }


        /// <summary>
        /// Obtains or sets the camera position.
        ///
        /// �J�����̈ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }


        /// <summary>
        /// Obtains or sets the camera viewpoint.
        ///
        /// �J�����̎��_���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraLookAt
        {
            get { return cameraLookAt; }
            set { cameraLookAt = value; }
        }
        #endregion
    }
}
