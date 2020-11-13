#region File Description
//-----------------------------------------------------------------------------
// AnimationInfoReader.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = MovipaLibrary.AnimationInfo;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by 
    /// the content pipeline to the value of AnimationInfoReader.
    /// This ContentTypeReader must be the same as the runtime 
    /// specified in ContentTypeWriter that was used to write the xnb 
    /// whose type is AnimationInfoReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����AnimationInfoReader�̒l�ɕ������܂��B
    /// ����ContentTypeReader��AnimationInfoReader�^��xnb���������ލۂɎg�p����
    /// ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł���K�v������܂��B
    /// </summary>
    public class AnimationInfoReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads AnimationInfo from the xnb file.
        ///
        /// xnb�t�@�C������AnimationInfo��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead info = new TRead();

            // Reads AnimationInfo.
            // 
            // AnimationInfo��ǂݍ��݂܂��B
            ReadAnimationInfo(input, info);

            return info;
        }


        /// <summary>
        /// Reads AnimationInfo.
        ///
        /// AnimationInfo��ǂݍ��݂܂��B
        /// </summary>
        public static void ReadAnimationInfo(ContentReader input, TRead info)
        {
            info.Category = input.ReadObject<AnimationInfo.AnimationInfoCategory>();
            info.Name = input.ReadString();
            info.Size = input.ReadObject<Point>();
        }
    }
}
