#region File Description
//-----------------------------------------------------------------------------
// RenderingInfo.cs
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
    /// This class manages animation information of a rendering movie.
    /// This information includes the total number of frames and textures, 
    /// the size of the frame drawn in one texture, and the replay rate information. 
    /// With these pieces of information, animations can be drawn in sequence textures.
    ///
    /// �����_�����O���[�r�[�̃A�j���[�V�������������܂��B
    /// �A�ԃe�N�X�`���ŕ`�悪�o����悤�ɁA���t���[�����ƁA���e�N�X�`�����A
    /// 1���̃e�N�X�`���ɕ`����Ă���t���[���̃T�C�Y�ƁA�Đ����[�g�̏�񂪂���܂��B
    /// </summary>
    public class RenderingInfo : AnimationInfo
    {
        #region Fields
        private string format;
        private uint totalTexture;
        private uint totalFrame;
        private Point imageSize;
        private uint frameRate;
        #endregion

        #region Property
        /// <summary>
        /// Obtains or sets the asset name format.
        ///
        /// �A�Z�b�g���̃t�H�[�}�b�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }


        /// <summary>
        /// Obtains or sets the total number of textures.
        ///
        /// ���e�N�X�`�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public UInt32 TotalTexture
        {
            get { return totalTexture; }
            set { totalTexture = value; }
        }


        /// <summary>
        /// Obtains or sets the total number of frames.
        ///
        /// ���t���[�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public UInt32 TotalFrame
        {
            get { return totalFrame; }
            set { totalFrame = value; }
        }


        /// <summary>
        /// Obtains or sets the image size of the frame.
        ///
        /// �t���[���̉摜�T�C�Y���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Point ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }


        /// <summary>
        /// Obtains or sets the frame rate.
        ///
        /// �t���[�����[�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public uint FrameRate
        {
            get { return frameRate; }
            set { frameRate = value; }
        }

        #endregion
    }
}
