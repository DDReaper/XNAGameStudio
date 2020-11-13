#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelInfo.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class manages information of a skin model.
    /// This class is used in SkinnedModelAnimationInfo.
    /// ContentTypeReader and ContentTypeWriter are also provided in this class
    /// so that SkinnedModelAnimationInfo can be simply written in ContentPipeline.
    ///
    /// �X�L�����f���̏��������܂��B
    /// ���̃N���X�́ASkinnedModelAnimationInfo�Ŏg�p����Ă��܂��B
    /// SkinnedModelAnimationInfo��ContentPipeline�ŃV���v���ɋL�q���邽�߁A
    /// ���̃N���X�ɂ�ContentTypeReader��ContentTypeWriter��p�ӂ��Ă��܂��B
    /// </summary>
    public class SkinnedModelInfo
    {
        #region Fields
        private string modelAsset;
        private string animationClip;
        private Vector3 position;
        #endregion

        #region Property
        /// <summary>
        /// Obtains or sets the asset name of the model.
        ///
        /// ���f���̃A�Z�b�g�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string ModelAsset
        {
            get { return modelAsset; }
            set { modelAsset = value; }
        }


        /// <summary>
        /// Obtains or sets the clip name of the animation.
        ///
        /// �A�j���[�V�����̃N���b�v�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string AnimationClip
        {
            get { return animationClip; }
            set { animationClip = value; }
        }


        /// <summary>
        /// Obtains or sets the model position.
        ///
        /// ���f���̈ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion
    }
}
