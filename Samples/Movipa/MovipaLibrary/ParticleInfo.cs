#region File Description
//-----------------------------------------------------------------------------
// ParticleInfo.cs
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
    /// This class manages animation information of particles.
    /// There are the following parameters:
    /// asset name of the texture used in particles, the number of particles 
    /// to be generated, and particle movement information.
    ///
    /// �p�[�e�B�N���̃A�j���[�V�������������܂��B
    /// �p�[�e�B�N���Ŏg�p����e�N�X�`���̃A�Z�b�g���ƁA��������
    /// �ړ��Ɋւ���p�����[�^������܂��B
    /// </summary>
    public class ParticleInfo : AnimationInfo
    {
        #region Fields
        private string particleTexture;
        private float particleSize;
        private UInt32 particleMax;
        private UInt32 particleGenerateCount;
        private float particleJumpPower;
        private float particleMoveSpeed;
        private float particleBoundRate;
        private float particleGravity;
        private Vector3 cameraUpVector;
        private Vector3 cameraPosition;
        private Vector3 cameraLookAt;
        #endregion

        #region Property
        /// <summary>
        /// Obtains or sets the asset name of the particle texture.
        ///
        /// �p�[�e�B�N���e�N�X�`���̃A�Z�b�g�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string ParticleTexture
        {
            get { return particleTexture; }
            set { particleTexture = value; }
        }

        
        /// <summary>
        /// Obtains or sets the particle size.
        ///
        /// �p�[�e�B�N���̃T�C�Y���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ParticleSize
        {
            get { return particleSize; }
            set { particleSize = value; }
        }


        /// <summary>
        /// Obtains or sets the maximum number of total particles to be generated. 
        ///
        /// �p�[�e�B�N���̏�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public UInt32 ParticleMax
        {
            get { return particleMax; }
            set { particleMax = value; }
        }


        /// <summary>
        /// Obtains or sets the number of particles to be generated at one time.
        ///
        /// �p�[�e�B�N���̈�x�ɐ������鐔���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public UInt32 ParticleGenerateCount
        {
            get { return particleGenerateCount; }
            set { particleGenerateCount = value; }
        }


        /// <summary>
        /// Obtains or sets the jump power of the particle.
        ///
        /// �p�[�e�B�N�������˂�͂��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ParticleJumpPower
        {
            get { return particleJumpPower; }
            set { particleJumpPower = value; }
        }


        /// <summary>
        /// Obtains or sets the movement speed of the particle.
        ///
        /// �p�[�e�B�N���̈ړ����x���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ParticleMoveSpeed
        {
            get { return particleMoveSpeed; }
            set { particleMoveSpeed = value; }
        }


        /// <summary>
        /// Obtains or sets the bound rate of the particle.
        ///
        /// �p�[�e�B�N���̒��˕Ԃ�̗͂��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ParticleBoundRate
        {
            get { return particleBoundRate; }
            set { particleBoundRate = value; }
        }


        /// <summary>
        /// Obtains or sets the gravity of the particle.
        ///
        /// �p�[�e�B�N���̗����鋭�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float ParticleGravity
        {
            get { return particleGravity; }
            set { particleGravity = value; }
        }


        /// <summary>
        /// Obtains or sets the coordinate system of the camera.
        ///
        /// �J�����̍��W�n���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraUpVector
        {
            get { return cameraUpVector; }
            set { cameraUpVector = value; }
        }


        /// <summary>
        /// Obtains or sets the camera position.
        ///
        /// �J�����̈ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }


        /// <summary>
        /// Obtains or sets the camera viewpoint.
        ///
        /// �J�����̎��_���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 CameraLookAt
        {
            get { return cameraLookAt; }
            set { cameraLookAt = value; }
        }
        #endregion
    }
}
