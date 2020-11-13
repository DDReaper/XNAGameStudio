#region File Description
//-----------------------------------------------------------------------------
// ParticleInfoReader.cs
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

using TRead = MovipaLibrary.ParticleInfo;
#endregion

namespace MovipaLibrary
{
    /// <summary>
    /// This class restores the xnb file converted by the content pipeline
    /// to the value of ParticleInfoReader.
    /// This ContentTypeReader must be the same as the runtime
    /// specified in ContentTypeWriter that was used to write the xnb
    /// whose type is ParticleInfoReader.
    ///
    /// ContentPipeline�ŕϊ����ꂽxnb�t�@�C����ParticleInfoReader�̒l�ɕ������܂��B
    /// ����ContentTypeReader��ParticleInfoReader�^��xnb���������ލۂɎg�p����
    /// ContentTypeWriter�Ŏw�肳�ꂽ�����^�C���Ɠ����ł���K�v������܂��B
    /// </summary>
    public class ParticleInfoReader : ContentTypeReader<TRead>
    {
        /// <summary>
        /// Reads ParticleInfo from the xnb file.
        ///
        /// xnb�t�@�C������ParticleInfo��ǂݍ��݂܂��B
        /// </summary>
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            TRead info = new TRead();

            // Reads AnimationInfo.
            // 
            // AnimationInfo��ǂݍ��݂܂��B
            AnimationInfoReader.ReadAnimationInfo(input, info);

            // Reads ParticleInfo.
            // 
            // ParticleInfo��ǂݍ��݂܂��B
            info.ParticleTexture = input.ReadString();
            info.ParticleSize = input.ReadSingle();
            info.ParticleMax = input.ReadUInt32();
            info.ParticleGenerateCount = input.ReadUInt32();
            info.ParticleJumpPower = input.ReadSingle();
            info.ParticleMoveSpeed = input.ReadSingle();
            info.ParticleBoundRate = input.ReadSingle();
            info.ParticleGravity = input.ReadSingle();
            info.CameraUpVector = input.ReadObject<Vector3>();
            info.CameraPosition = input.ReadObject<Vector3>();
            info.CameraLookAt = input.ReadObject<Vector3>();

            return info;
        }
    }
}
