#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelInfoReader.cs
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

using TRead = MovipaLibrary.SkinnedModelInfo;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by the content pipeline
    /// to the value of SkinnedModelInfoReader.
    /// This ContentTypeReader must be the same as the runtime 
    /// specified in ContentTypeWriter that was used to write the xnb 
    /// whose type is SkinnedModelInfoReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����SkinnedModelInfoReader�̒l�ɕ������܂��B
    /// ����ContentTypeReader��SkinnedModelInfoReader�^��xnb���������ލۂɎg�p����
    /// ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł���K�v������܂��B
    /// </summary>
    public class SkinnedModelInfoReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads SkinnedModelInfo from the xnb file.
        ///
        /// xnb�t�@�C������SkinnedModelInfo��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead info = new TRead();

            // Reads SkinnedModelInfo.
            // 
            // SkinnedModelInfo��ǂݍ��݂܂��B
            info.ModelAsset = input.ReadString();
            info.AnimationClip = input.ReadString();
            info.Position = input.ReadObject<Vector3>();

            return info;
        }
    }
}
