#region File Description
//-----------------------------------------------------------------------------
// SoundComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Movipa.Components
{
    /// <summary>
    /// Component that plays sound. 
    /// If nothing is specified in the Constructor, it loads the file 
    /// created with the default name. 
    /// The PlaySoundEffect method should be used for SoundEffect playback, 
    /// and the PlayBackgroundMusic method for BackgroundMusic playback.
    /// Cue is specified as the return value for the PlayBackgroundMusic method.
    /// "Volume" and "Pitch" must be provided in the Cue variables on the 
    /// XACT side for the SetVolume and SetPitch methods.
    ///
    /// �T�E���h��炷�R���|�[�l���g�ł��B
    /// �R���X�g���N�^�œ��ɉ����w�肵�Ȃ���΁A�f�t�H���g�ō쐬����閼�O��
    /// �t�@�C����ǂݍ��ނ悤�ɂȂ��Ă��܂��B
    /// SoundEffect���Đ�����Ƃ���PlaySoundEffect���\�b�h���g�p���ABackgroundMusic�
    /// ��Đ����鎞��PlayBackgroundMusic���\�b�h��
    /// �g�p���Ă��������B
    /// PlayBackgroundMusic���\�b�h�͖߂�l��cue���w�肳��Ă��܂��B
    /// SetVolume�����SetPitch���\�b�h�Ɋւ��ẮAXACT����Cue��Variable��
    /// "Volume"��"Pitch"���p�ӂ���Ă���K�v������܂��B
    /// </summary>
    public class SoundComponent : GameComponent
    {
        #region Fields
        private const string DefaultAudioEnginePath = "Content/Audio/Movipa.xgs";
        private const string DefaultWaveBankPath = "Content/Audio/Movipa.xwb";
        private const string DefaultSoundBankPath = "Content/Audio/Movipa.xsb";
        private const string VolumeVariableName = "Volume";
        private const string PitchVariableName = "Pitch";

        private string audioEnginePath;
        private string waveBankPath;
        private string soundBankPath;

        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the audio engine path.
        /// 
        /// �I�[�f�B�I�G���W���̃p�X���擾���܂��B
        /// </summary>
        public string AudioEnginePath
        {
            get { return audioEnginePath; }
        }


        /// <summary>
        /// Obtains the wave bank path.
        /// 
        /// �E�F�[�u�o���N�̃p�X���擾���܂��B
        /// </summary>
        public string WaveBankPath
        {
            get { return waveBankPath; }
        }


        /// <summary>
        /// Obtains the sound bank path.
        ///
        /// �T�E���h�o���N�̃p�X���擾���܂��B
        /// </summary>
        public string SoundBankPath
        {
            get { return soundBankPath; }
        }


        /// <summary>
        /// Obtains the audio engine.
        ///
        /// �I�[�f�B�I�G���W�����擾���܂��B
        /// </summary>
        public AudioEngine AudioEngine
        {
            get { return audioEngine; }
        }


        /// <summary>
        /// Obtains the wave bank.
        ///
        /// �E�F�[�u�o���N���擾���܂��B
        /// </summary>
        public WaveBank WaveBank
        {
            get { return waveBank; }
        }


        /// <summary>
        /// Obtains the sound bank.
        ///
        /// �T�E���h�o���N���擾���܂��B
        /// </summary>
        public SoundBank SoundBank
        {
            get { return soundBank; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// The default audio file is used.
        ///
        /// �C���X�^���X�����������܂��B
        /// �I�[�f�B�I�t�@�C���̓f�t�H���g�̂��̂��g�p���܂��B
        /// </summary>
        public SoundComponent(Game game)
            : this(game, 
            DefaultAudioEnginePath, DefaultWaveBankPath, DefaultSoundBankPath)
        {
        }


        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SoundComponent(Game game, 
            string audioEnginePath, string waveBankPath, string soundBankPath)
            : base(game)
        {
            this.audioEnginePath = audioEnginePath;
            this.waveBankPath = waveBankPath;
            this.soundBankPath = soundBankPath;
        }


        /// <summary>
        /// Initializes the sound.
        ///
        /// �T�E���h�̏��������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            audioEngine = new AudioEngine(AudioEnginePath);
            waveBank = new WaveBank(audioEngine, WaveBankPath);
            soundBank = new SoundBank(audioEngine, SoundBankPath);

            audioEngine.Update();

            base.Initialize();
        }


        /// <summary>
        /// Releases all sound resources.
        ///
        /// �T�E���h���\�[�X��S�ĊJ�����܂��B
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (soundBank != null && !soundBank.IsDisposed)
                soundBank.Dispose();

            if (waveBank != null && !waveBank.IsDisposed)
                waveBank.Dispose();

            if (audioEngine != null && !audioEngine.IsDisposed)
                audioEngine.Dispose();

            base.Dispose(disposing);
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the sound resource.
        /// 
        /// �T�E���h���\�[�X�̍X�V�������s���܂��B
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (audioEngine != null && !audioEngine.IsDisposed)
            {
                audioEngine.Update();
            }

            base.Update(gameTime);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the Cue.
        ///
        /// Cue���擾���܂��B
        /// </summary>
        /// <param name="sound">Sound name</param>
        ///  
        /// <param name="sound">�T�E���h�̖��O</param>
        /// <returns>�T�E���h��Cue</returns>
        public Cue GetCue(string cueName)
        {
            return soundBank.GetCue(cueName);
        }


        /// <summary>
        /// Plays the sound.
        /// 
        /// �T�E���h�̍Đ������܂��B
        /// </summary>
        /// <param name="sound">Sound for playback</param>
        ///  
        /// <param name="sound">�Đ�����T�E���h</param>
        ///
        /// <returns>Played Cue</returns>
        ///  
        /// <returns>�Đ����ꂽCue</returns>
        public Cue PlayBackgroundMusic(string cueName)
        {
            Cue cue = GetCue(cueName);

            if (cue != null && !cue.IsDisposed)
                cue.Play();

            return cue;
        }


        /// <summary>
        /// Plays the sound.
        /// 
        /// �T�E���h�̍Đ������܂��B
        /// </summary>
        /// <param name="sound">Sound for playback</param>
        ///  
        /// <param name="sound">�Đ�����T�E���h</param>
        public void PlaySoundEffect(string cueName)
        {
            if (soundBank != null && !soundBank.IsDisposed)
                soundBank.PlayCue(cueName);
        }


        /// <summary>
        /// Sets the Cue variable.
        /// 
        /// Cue�̕ϐ���ݒ肵�܂��B
        /// </summary>
        /// <param name="cue">Cue to be changed</param>
        ///  
        /// <param name="cue">�ύX����L���[</param>
        ///
        /// <param name="value">Value to be changed</param>
        ///  
        /// <param name="value">�ύX����l</param>
        public static void SetVariable(Cue cue, string name, float value)
        {
            if (cue == null || cue.IsDisposed)
                return;

            cue.SetVariable(name, value);
        }


        /// <summary>
        /// Changes the Cue volume.
        /// Specified as float type between 0 and 1.
        /// 
        /// �L���[�̃{�����[����ύX���܂��B
        /// 0�`1�܂ł�float�^�Ŏw�肵�܂��B
        /// </summary>
        /// <param name="cue">Cue to be changed</param>
        ///  
        /// <param name="cue">�ύX����L���[</param>
        ///
        /// <param name="value">Value to be changed</param>
        ///  
        /// <param name="value">�ύX����l</param>
        public static void SetVolume(Cue cue, float value)
        {
            float volume = 100.0f * MathHelper.Clamp(value, 0.0f, 1.0f);
            SetVariable(cue, VolumeVariableName, volume);
        }


        /// <summary>
        /// Alters the Cue pitch.
        /// Alters the pitch by semitone in the range -12 to +12.
        /// 
        /// �L���[�̃s�b�`��ύX���܂��B
        /// -12�`+12�܂ŁA�������ω����܂��B
        /// </summary>
        /// <param name="cue">Cue to be changed</param>
        ///  
        /// <param name="cue">�ύX����L���[</param>
       ///
        /// <param name="value">Value to be changed</param>
        ///  
        /// <param name="value">�ύX����l</param>
        public static void SetPitch(Cue cue, float value)
        {
            float pitch = 12.0f * MathHelper.Clamp(value, -1.0f, 1.0f);
            SetVariable(cue, PitchVariableName, pitch);
        }


        /// <summary>
        /// Stops the Cue.
        /// 
        /// Cue���~���܂��B
        /// </summary>
        /// <param name="cue">Cue to be stopped</param>
        ///  
        /// <param name="cue">��~����L���[</param>
        public static void Stop(Cue cue)
        {
            if (cue != null && !cue.IsDisposed && cue.IsPlaying)
                cue.Stop(AudioStopOptions.Immediate);
        }
        #endregion
    }
}
