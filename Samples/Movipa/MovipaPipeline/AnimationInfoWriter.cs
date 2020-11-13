#region File Description
//-----------------------------------------------------------------------------
// AnimationInfoWriter.cs
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

using TWrite = MovipaLibrary.AnimationInfo;
using TReader = MovipaLibrary.AnimationInfoReader;
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
    public class AnimationInfoWriter : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Writes AnimationInfo to the xnb file.
        ///
        /// AnimationInfo��xnb�t�@�C���֏������݂܂��B
        /// </summary>
        protected override void Write(ContentWriter output, TWrite value)
        {
            // Writes AnimationInfo.
            // 
            // AnimationInfo���������݂܂��B
            WriteAnimationInfo(output, value);
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


        /// <summary>
        /// Writes AnimationInfo.
        ///
        /// AnimationInfo���������݂܂��B
        /// </summary>
        public static void WriteAnimationInfo(ContentWriter output, TWrite value)
        {
            output.WriteObject<AnimationInfo.AnimationInfoCategory>(value.Category);
            output.Write(value.Name);
            output.WriteObject<Point>(value.Size);
        }
    }
}
