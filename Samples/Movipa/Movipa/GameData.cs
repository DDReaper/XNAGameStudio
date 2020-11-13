#region File Description
//-----------------------------------------------------------------------------
// GameData.cs
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
using Microsoft.Xna.Framework.Storage;
using Movipa.Components;
using Movipa.Components.Input;
using Movipa.Util;
using MovipaLibrary;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Movipa
{
    #region Public Types
    /// <summary>
    /// SpriteFont list used in game
    /// 
    /// �Q�[���Ŏg�p����SpriteFont�̃��X�g
    /// </summary>
    public enum FontList
    {
        /// <summary>
        /// Medium font size
        /// The following characters are replaced by special graphics:
        /// { Controller A button
        /// | Controller B button
        /// } Controller X button
        /// ~ Controller Y button
        /// 
        /// ���T�C�Y�̃t�H���g
        /// �ȉ��̕���������O���t�B�b�N�ɍ����ւ����Ă���
        ///  { �R���g���[����A�{�^��
        ///  | �R���g���[����B�{�^��
        ///  } �R���g���[����X�{�^��
        ///  ~ �R���g���[����Y�{�^��
        /// </summary>
        Medium,
        
        /// <summary>
        /// Large font size
        /// 
        /// �傫���T�C�Y�̃t�H���g
        /// </summary>
        Large,
    }
    #endregion

    /// <summary>
    /// Manages global variables used by the game.
    /// Contains screen size constants as well as instances for components and save data.
    /// Static member variables are initialized by a static constructor.
    /// 
    /// �Q�[���Ŏg�p����L��ϐ����Ǘ����܂��B
    /// ��ʃT�C�Y�̒萔��A�R���|�[�l���g�A�Z�[�u�f�[�^�Ȃǂ̃C���X�^���X�������Ă��܂��B
    /// �ÓI�����o�ϐ��̏������́A�ÓI�R���X�g���N�^�ŏ������s���悤�ɂ��Ă��܂��B
    /// </summary>
    public static class GameData
    {
        #region Fields
        #region SizeInfo
        // Screen size
        // 
        // �X�N���[���T�C�Y
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;
        public static readonly Rectangle ScreenSizeRect = 
            new Rectangle(0, 0, ScreenWidth, ScreenHeight);
        public static readonly Point ScreenSizePoint = 
            new Point(ScreenWidth, ScreenHeight);
        public static readonly Vector2 ScreenSizeVector2 = 
            new Vector2(ScreenWidth, ScreenHeight);

        // Movie size
        // 
        // ���[�r�[�T�C�Y
        public const int MovieWidth = 1024;
        public const int MovieHeight = 576;
        public static readonly Rectangle MovieSizeRect = 
            new Rectangle(0, 0, MovieWidth, MovieHeight);
        public static readonly Point MovieSizePoint = 
            new Point(MovieWidth, MovieHeight);
        public static readonly Vector2 MovieSizeVector2 = 
            new Vector2(MovieWidth, MovieHeight);

        // Style animation size
        // 
        // �X�^�C���A�j���[�V�����T�C�Y
        public const int StyleWidth = 480;
        public const int StyleHeight = 270;
        public static readonly Rectangle StyleSizeRect = 
            new Rectangle(0, 0, StyleWidth, StyleHeight);
        public static readonly Point StyleSizePoint = 
            new Point(StyleWidth, StyleHeight);
        public static readonly Vector2 StyleSizeVector2 =
            new Vector2(StyleWidth, StyleHeight); 

        // Projection
        // 
        // �v���W�F�N�V����
        public static readonly Matrix Projection = 
            CreateProjection(ScreenSizeVector2, 10000);
        public static readonly Matrix ScreenProjection = 
            CreateScreenProjection(ScreenSizeVector2);
        public static readonly Matrix MovieScreenProjection =
            CreateScreenProjection(MovieSizeVector2);

        #endregion

        private static Dictionary<string, string> appSettings;
        private static List<StageSetting> stageCollection;
        private static SaveData saveData = null;
        private static List<string> movieList;

        // Components
        private static StorageComponent storageComponent;
        private static InputComponent input = null;
        private static FadeSeqComponent fadeSeqComponent;
        private static SoundComponent soundComponent;
        private static Queue<GameComponent> sceneQueue = new Queue<GameComponent>();
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the storage component.
        /// 
        /// �X�g���[�W�R���|�[�l���g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static StorageComponent Storage
        {
            get { return storageComponent; }
            set { storageComponent = value; }
        }


        /// <summary>
        /// Obtains or sets the associative array where the text string is stored.
        /// 
        /// �����񂪊i�[�����A�z�z����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static Dictionary<string, string> AppSettings
        {
            get { return appSettings; }
        }


        /// <summary>
        /// Obtains or sets the Normal Mode storage information.
        /// 
        /// �m�[�}�����[�h�̃X�e�[�W�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static List<StageSetting> StageCollection
        {
            get { return stageCollection; }
        }


        /// <summary>
        /// Obtains or sets the Save Data used in Normal Mode.
        /// 
        /// �m�[�}�����[�h�Ŏg�p�����Z�[�u�f�[�^���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static SaveData SaveData
        {
            get { return saveData; }
            set { saveData = value; }
        }


        /// <summary>
        /// Obtains or sets the input component.
        /// 
        /// ���̓R���|�[�l���g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static InputComponent Input
        {
            get { return input; }
            set { input = value; }
        }


        /// <summary>
        /// Obtains or sets the entry scene queue.
        /// 
        /// �G���g���[����V�[���L���[���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static Queue<GameComponent> SceneQueue
        {
            get { return sceneQueue; }
        }


        /// <summary>
        /// Obtains or sets the AnimationInfo asset name 
        /// list for movies used in the game.
        ///
        /// �Q�[�����Ŏg�p���郀�[�r�[��AnimationInfo�̃A�Z�b�g�����X�g��
        /// �擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static List<string> MovieList
        {
            get { return movieList; }
        }


        /// <summary>
        /// Obtains or sets the component for fade processing.
        /// 
        /// �t�F�[�h�����p�̃R���|�[�l���g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static FadeSeqComponent FadeSeqComponent
        {
            get { return fadeSeqComponent; }
            set { fadeSeqComponent = value; }
        }


        /// <summary>
        /// Obtains or sets the component for fade processing.
        /// 
        /// �t�F�[�h�����p�̃R���|�[�l���g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public static SoundComponent Sound
        {
            get { return soundComponent; }
            set { soundComponent = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Load content for the static types.
        /// </summary>
        /// <param name="content">The content manager used to load.</param>
        public static void LoadContent(ContentManager content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            // Loads App.config.
            // 
            // App.config��ǂݍ��݂܂��B
            appSettings = content.Load<Dictionary<string, string>>("App.config");

            // Loads the stage settings.
            // 
            // �X�e�[�W�ݒ��ǂݍ��݂܂��B
            stageCollection = content.Load<List<StageSetting>>("StageData");

            // Loads the movie list.
            // 
            // ���[�r�[���X�g��ǂݍ��݂܂��B
            movieList = content.Load<List<string>>("MovieList");
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates projection transformation matrix.
        /// 
        /// �ˉe�ϊ��s����쐬���܂��B
        /// </summary>
        private static Matrix CreateProjection(Vector2 size, float far)
        {
            return Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                size.X / size.Y,
                0.1f,
                far);
        }


        /// <summary>
        /// Creates orthogonal projection matrix.
        /// 
        /// �����ˉe�s����쐬���܂��B
        /// </summary>
        private static Matrix CreateScreenProjection(Vector2 size)
        {
            return Matrix.CreateOrthographicOffCenter(
                0.0f,
                size.X,
                size.Y,
                0.0f,
                0.0f,
                1.0f);
        }
        #endregion
    }
}


