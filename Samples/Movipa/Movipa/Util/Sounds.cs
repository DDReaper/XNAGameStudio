#region File Description
//-----------------------------------------------------------------------------
// Sounds.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Class that collects sound cue name constants.
    /// Used for the SoundComponent PlayBackgroundMusic or PlaySoundEffect.
    /// 
    /// �T�E���h�̃L���[���̒萔���܂Ƃ߂�N���X�ł��B
    /// SoundComponent��PlayBackgroundMusic�܂���PlaySoundEffect�Ɏg�p���܂��B
    /// </summary>
    public static class Sounds
    {
        /// <summary>
        /// Title BackgroundMusic
        /// 
        /// �^�C�g��BackgroundMusic
        /// </summary>
        public const string TitleBackgroundMusic = "TitleBackgroundMusic";

        /// <summary>
        /// Menu BackgroundMusic
        /// 
        /// ���j���[BackgroundMusic
        /// </summary>
        public const string SelectBackgroundMusic = "SelectBackgroundMusic";

        /// <summary>
        /// Main game BackgroundMusic
        /// 
        /// ���C���Q�[��BackgroundMusic
        /// </summary>
        public const string GameBackgroundMusic = "GameBackgroundMusic";

        /// <summary>
        /// Stage completion BackgroundMusic
        /// 
        /// �X�e�[�W�N���ABackgroundMusic
        /// </summary>
        public const string GameClearBackgroundMusic = "GameClearBackgroundMusic";

        /// <summary>
        /// Game over BackgroundMusic
        /// 
        /// �Q�[���I�[�o�[BackgroundMusic
        /// </summary>
        public const string GameOverBackgroundMusic = "GameOverBackgroundMusic";

        /// <summary>
        /// OK
        /// 
        /// ����
        /// </summary>
        public const string SoundEffectOkay = "SoundEffectOkay";

        /// <summary>
        /// Cancel
        /// 
        /// �L�����Z��
        /// </summary>
        public const string SoundEffectCancel = "SoundEffectCancel";

        /// <summary>
        /// Cursor 1
        /// 
        /// �J�[�\��1
        /// </summary>
        public const string SoundEffectCursor1 = "SoundEffectCursor1";

        /// <summary>
        /// Cursor 2
        /// 
        /// �J�[�\��2
        /// </summary>
        public const string SoundEffectCursor2 = "SoundEffectCursor2";

        /// <summary>
        /// All panels completed
        /// 
        /// �S�ăp�l������
        /// </summary>
        public const string SoundEffectClear = "SoundEffectClear";

        /// <summary>
        /// Result score addition
        /// 
        /// ���U���g�̃X�R�A���Z
        /// </summary>
        public const string ResultScore = "ResultScore";
    }
}