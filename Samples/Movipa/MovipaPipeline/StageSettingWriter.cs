#region File Description
//-----------------------------------------------------------------------------
// StageSettingWriter.cs
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

using TWrite = MovipaLibrary.StageSetting;
using TReader = MovipaLibrary.StageSettingReader;
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
    public class StageSettingWriter : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Writes StageSetting to the xnb file.
        ///
        /// StageSetting��xnb�t�@�C���֏������݂܂��B
        /// </summary>
        protected override void Write(ContentWriter output, TWrite value)
        {
            // Writes StageSetting.
            // 
            // StageSetting���������݂܂��B
            output.WriteObject<StageSetting.ModeList>(value.Mode);
            output.WriteObject<StageSetting.StyleList>(value.Style);
            output.WriteObject<StageSetting.RotateMode>(value.Rotate);
            output.Write(value.Movie);
            output.WriteObject<Point>(value.Divide);
            output.Write(value.TimeLimitString);
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
