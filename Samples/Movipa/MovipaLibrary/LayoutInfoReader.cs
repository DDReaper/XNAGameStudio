#region File Description
//-----------------------------------------------------------------------------
// LayoutInfoReader.cs
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

using TRead = MovipaLibrary.LayoutInfo;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by the content pipeline
    /// to the value of LayoutInfoReader.
    /// This ContentTypeReader must be the same as the runtime 
    /// specified in ContentTypeWriter that was used to write the xnb 
    /// whose type is LayoutInfoReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����LayoutInfoReader�̒l�ɕ������܂��B
    /// ����ContentTypeReader��LayoutInfoReader�^��xnb���������ލۂɎg�p����
    /// ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł���K�v������܂��B
    /// </summary>
    public class LayoutInfoReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads LayoutInfo from the xnb file.
        ///
        /// xnb�t�@�C������LayoutInfo��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead info = new TRead();

            // Reads AnimationInfo.
            // 
            // AnimationInfo��ǂݍ��݂܂��B
            AnimationInfoReader.ReadAnimationInfo(input, info);

            // Reads LayoutInfo.
            // 
            // LayoutInfo��ǂݍ��݂܂��B
            info.SceneDataAsset = input.ReadString();
            info.Sequence = input.ReadString();

            return info;
        }
    }
}
