#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelAnimationInfoWriter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using MovipaLibrary;

using TWrite = MovipaLibrary.SkinnedModelAnimationInfo;
using TReader = MovipaLibrary.SkinnedModelAnimationInfoReader;
#endregion

namespace MovipaPipeline
{
    /// <summary>
    /// This class writes the data passed from ContentImpoter to the xnb file.
    /// In the game, the ContentTypeReader specified in GetRuntimeReader is used 
    /// to read data.
    ///
    /// ContentImpoter����n���ꂽ�f�[�^��xnb�t�@�C���ɏ������݂܂��B
    /// �Q�[�����ł�GetRuntimeReader�Ŏw�肵��ContentTypeReader��
    /// �g�p���ēǂݍ��ݏ������s���܂��B
    /// </summary>
    [ContentTypeWriter]
    public class SkinnedModelAnimationInfoWriter : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Writes SkinnedModelAnimationInfo to the xnb file.
        /// 
        /// SkinnedModelAnimationInfo��xnb�t�@�C���֏������݂܂��B
        /// </summary>
        protected override void Write(ContentWriter output, TWrite value)
        {
            // Writes AnimationInfo.
            // 
            // AnimationInfo���������݂܂��B
            AnimationInfoWriter.WriteAnimationInfo(output, value);

            // Writes SkinnedModelAnimationInfo.
            // 
            // SkinnedModelAnimationInfo���������݂܂��B
            output.WriteObject<List<SkinnedModelInfo>>(
                value.SkinnedModelInfoCollection);
            output.WriteObject<Vector3>(value.CameraUpVector);
            output.WriteObject<Vector3>(value.CameraPosition);
            output.WriteObject<Vector3>(value.CameraLookAt);
        }


        /// <summary>
        /// Specifies the ContentTypeReader to be used.
        ///
        /// �g�p����ContentTypeReader���w�肵�܂��B
        /// </summary>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TReader).AssemblyQualifiedName;
        }
    }
}
