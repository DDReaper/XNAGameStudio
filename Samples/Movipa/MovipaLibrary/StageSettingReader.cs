#region File Description
//-----------------------------------------------------------------------------
// StageSettingReader.cs
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

using TRead = MovipaLibrary.StageSetting;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by the content pipeline
    /// to the value of StageSettingReader.
    /// This ContentTypeReader must be the same as the runtime
    /// specified in ContentTypeWriter that was used to write the xnb
    /// whose type is StageSettingReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����StageSettingReader�̒l�ɕ������܂��B
    /// ����ContentTypeReader��StageSettingReader�^��xnb���������ލۂɎg�p����
    /// ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł���K�v������܂��B
    /// </summary>
    public class StageSettingReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads StageSetting from the xnb file.
        ///
        /// xnb�t�@�C������StageSetting��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead setting = new TRead();

            // Reads StageSetting.
            // 
            // StageSetting��ǂݍ��݂܂��B
            setting.Mode = input.ReadObject<StageSetting.ModeList>();
            setting.Style = input.ReadObject<StageSetting.StyleList>();
            setting.Rotate = input.ReadObject<StageSetting.RotateMode>();
            setting.Movie = input.ReadString();
            setting.Divide = input.ReadObject<Point>();
            setting.TimeLimitString = input.ReadString();

            return setting;
        }
    }
}
