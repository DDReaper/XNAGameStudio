#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelAnimationInfoReader.cs
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

using TRead = MovipaLibrary.SkinnedModelAnimationInfo;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by the content pipeline
    /// to the value of SkinnedModelAnimationInfoReader.
    /// This ContentTypeReader must be the same as the runtime 
    /// specified in ContentTypeWriter that was used to write the xnb
    /// whose type is SkinnedModelAnimationInfoReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����SkinnedModelAnimationInfoReader�
    /// ̒l�ɕ������܂��B ����ContentTypeReader��SkinnedModelAnimationInfoReader�
    /// �xnb���������ލۂɎg�p���� ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł
    /// ��K�v������܂��B
    /// </summary>
    public class SkinnedModelAnimationInfoReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads SkinnedModelAnimationInfo from the xnb file.
        ///
        /// xnb�t�@�C������SkinnedModelAnimationInfo��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead info = new TRead();

            // Reads AnimationInfo.
            // 
            // AnimationInfo��ǂݍ��݂܂��B
            AnimationInfoReader.ReadAnimationInfo(input, info);

            // Reads SkinnedModelAnimationInfo.
            // 
            // SkinnedModelAnimationInfo��ǂݍ��݂܂��B
            info.SkinnedModelInfoCollection.AddRange(
                input.ReadObject<List<SkinnedModelInfo>>());
            info.CameraUpVector = input.ReadObject<Vector3>();
            info.CameraPosition = input.ReadObject<Vector3>();
            info.CameraLookAt = input.ReadObject<Vector3>();

            return info;
        }
    }
}
