#region File Description
//-----------------------------------------------------------------------------
// MovipaGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Movipa.Components;
using Movipa.Components.Input;
using Movipa.Util;
using MovipaLibrary;
#endregion

namespace Movipa
{
    /// <summary>
    /// This class adds the necessary initial components and stores the number of them.
    /// Scene components are added with each game scene. When all components
    /// are finished, if the component count is the same as the initial number,
    /// the game ends. 
    /// 
    /// �Q�[���̏������s�����C���̃N���X�ł��B
    /// ���̃N���X�ł́A�ŏ��ɕK�v�ȃR���|�[�l���g��ǉ����A���̐���ێ����Ă����܂��B
    /// �Q�[���̊e�V�[���̓V�[���R���|�[�l���g���ǉ�����A�������s���܂����A
    /// ���̃V�[���R���|�[�l���g���S�ďI�����A�R���|�[�l���g�̐��������l�Ɠ�������
    /// �Ȃ�����Q�[�����I�����܂��B
    /// </summary>
    public class MovipaGame : Game
    {
        #region Fields
        GraphicsDeviceManager graphics;

        // Default component count 
        // 
        // �W���Ŏg�p�����R���|�[�l���g��
        private int defaultComponentCount = -1;

        private SpriteBatch batch;
        public SpriteBatch Batch
        {
            get { return batch; }
        }

        private SpriteFont mediumFont;
        public SpriteFont MediumFont
        {
            get { return mediumFont; }
        }

        private SpriteFont largeFont;
        public SpriteFont LargeFont
        {
            get { return largeFont; }
        }

        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance. 
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public MovipaGame()
        {
            this.IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this);

            // Sets the resolution.
            // 
            // �𑜓x�̐ݒ�����܂��B
            graphics.PreferredBackBufferWidth = GameData.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameData.ScreenHeight;
            
            // set the minimum required shader model
            // -- the skinned shader requires shader model 2.0
            graphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;
            graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            graphics.PreparingDeviceSettings += 
                new EventHandler<PreparingDeviceSettingsEventArgs>(
                graphics_PreparingDeviceSettings);

            // Sets the default directory for ContentManager.
            // 
            // ContentManager�̃��[�g�f�B���N�g���̐ݒ�����܂��B
            Content.RootDirectory = "Content";
        }

        void graphics_PreparingDeviceSettings(object sender, 
            PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = 
                RenderTargetUsage.PreserveContents;
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// ���������s���܂��B
        /// </summary>
        protected override void Initialize()
        {
            // Registers the initial component settings.
            // 
            // �R���|�[�l���g�̏����ݒ���s���܂��B
            InitializeComponent();

            // Registers the start scene settings.
            // 
            // �J�n�V�[���̐ݒ���s���܂��B
            GameData.SceneQueue.Enqueue(new Movipa.Components.Scene.Logo(this));
            GameData.SceneQueue.Enqueue(new Movipa.Components.Scene.Title(this));

            base.Initialize();
        }


        /// <summary>
        /// Initializes the component.
        /// 
        /// �R���|�[�l���g�̏��������s���܂��B
        /// </summary>
        private void InitializeComponent()
        {
            // Adds the gamer service component.
            // 
            // �Q�[�}�[�T�[�r�X�R���|�[�l���g��ǉ����܂��B
            Components.Add(new GamerServicesComponent(this));

            // Adds the input component.
            // 
            // ���̓R���|�[�l���g��ǉ����܂��B
            GameData.Input = new InputComponent(this);
            Components.Add(GameData.Input);

            // Adds the sound component.
            // 
            // �T�E���h�R���|�[�l���g��ǉ����܂��B
            GameData.Sound = new SoundComponent(this);
            Components.Add(GameData.Sound);

            // Adds the storage selection component.
            // 
            // �X�g���[�W�I���R���|�[�l���g��ǉ����܂��B
            GameData.Storage = new StorageComponent(this);
            Components.Add(GameData.Storage);

            // Adds the fade component.
            // 
            // �t�F�[�h�R���|�[�l���g��ǉ����܂��B
            GameData.FadeSeqComponent = new FadeSeqComponent(this);
            GameData.FadeSeqComponent.DrawOrder = 100;
            Components.Add(GameData.FadeSeqComponent);

#if DEBUG
            // Adds the safe area display component (debug only).
            // 
            // �Z�[�t�G���A�\���R���|�[�l���g��ǉ����܂��B�i�f�o�b�O�̂݁j
            SafeAreaComponent safeAreaComponent = new SafeAreaComponent(this);
            //safeAreaComponent.DrawOrder = 1000;
            Components.Add(safeAreaComponent);
#endif

            // Obtains the component count.
            // 
            // �R���|�[�l���g�����擾���܂��B
            defaultComponentCount = Components.Count;
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g�̓ǂݍ��݂��s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            mediumFont = Content.Load<SpriteFont>("Textures/Font/MediumGameFont");
            largeFont = Content.Load<SpriteFont>("Textures/Font/LargeGameFont");

            GameData.LoadContent(Content);

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Terminates if all scenes are completed. 
            // 
            // �Q�[���p�b�h��Back���������A�L�[�{�[�h��ESC�L�[���������A
            // �S�ẴV�[�����I��������I�����܂��B
            if ((GameData.SceneQueue.Count == 0 &&
                Components.Count == defaultComponentCount))
            {
                this.Exit();
            }


            // Performs transition if the scene is completed and 
            // the next scene remains in the queue.
            // 
            // �V�[�����I�����Ă��ăL���[�Ɏ��̃V�[�����c���Ă���ΑJ�ڂ��܂��B
            if (Components.Count == defaultComponentCount &&
                GameData.SceneQueue.Count > 0)
            {
                // Releases memory for scene switching.
                // 
                // �V�[���؂�ւ����Ƀ������̊J�����s���܂��B
                //System.GC.Collect();

                // Retrieves the scene from the queue and adds it to the component.
                //
                // �L���[����V�[�������o���A�R���|�[�l���g�ɒǉ����܂��B
                GameComponent nextScene = GameData.SceneQueue.Dequeue();
                Components.Add(nextScene);
            }

            base.Update(gameTime);
        }
        #endregion


        #region Static Entry Point


        /// <summary>
        /// Generates and executes the MovipaGame class.
        /// 
        /// ���̃A�v���P�[�V�����̃v���O�����J�n�ʒu�ł��B
        /// Main�N���X�𐶐����A���s���܂��B
        /// </summary>
        static void Main()
        {
            using (MovipaGame game = new MovipaGame())
            {
                game.Run();
            }
        }


        #endregion
    }
}
