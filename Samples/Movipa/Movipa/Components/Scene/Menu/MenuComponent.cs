#region File Description
//-----------------------------------------------------------------------------
// MenuComponent.cs
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Movipa.Util;
using Movipa.Components.Scene.Puzzle;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Scene component that draws menus.
    /// This class draws the background and renders 
    /// the animation movie.  
    /// It also switches the classes used to process 
    /// items of each menu that has inherited MenuBase.
    ///
    /// ���j���[�̕`����s���V�[���R���|�[�l���g�ł��B
    /// ���̃N���X�ł́A�w�i�̕`��ƁA�A�j���[�V�������[�r�[��
    /// �����_�����O�������s���Ă��܂��B
    /// �܂��AMenuBase���p�������e���j���[�̍��ڂ���������N���X��
    /// �؂�ւ��������s���Ă��܂��B

    /// </summary>
    public class MenuComponent : SceneComponent
    {
        #region Fields
        /// <summary>
        /// Common data structure
        ///
        /// ���ʃf�[�^�\����
        /// </summary>
        private MenuData data;

        /// <summary>
        /// Background texture 
        /// 
        /// �w�i�e�N�X�`�� 
        /// </summary>
        private Texture2D wallpaper;

        /// <summary>
        /// BackgroundMusic cue
        /// 
        /// BackgroundMusic�̃L���[
        /// </summary>
        private Cue bgm;

        /// <summary>
        /// Menu object currently being executed
        /// 
        /// ���ݎ��s���Ă��郁�j���[�I�u�W�F�N�g
        /// </summary>
        private MenuBase currentMenu;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public MenuComponent(Game game)
            : base(game)
        {
            // Creates common data used in the menu. 
            // 
            // ���j���[�Ŏg�p���鋤�ʃf�[�^���쐬���܂��B
            data = new MenuData(Game);            
        }


        /// <summary>
        /// Performs initialization processing.
        ///  
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Creates the initial menu.
            // 
            // �ŏ��̃��j���[���쐬���܂��B
            currentMenu = MenuBase.CreateMenu(Game, MenuType.SelectMode, data);

            // Registers the fade-in settings.
            // 
            // �t�F�[�h�C���̐ݒ���s���܂��B
            GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeIn);

            // Plays the BackgroundMusic and obtains the Cue.
            // 
            // BackgroundMusic���Đ����ACue���擾���܂��B 
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.SelectBackgroundMusic);

            base.Initialize();
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the background texture.
            // 
            // �w�i�e�N�X�`���̓ǂݍ���
            string asset;
            asset = "Textures/Wallpaper/Wallpaper_006";
            wallpaper = Content.Load<Texture2D>(asset);

            // Loads and sets the sphere model.
            // 
            // ���̃��f���̓ǂݍ��݂Ɛݒ�  
            InitializeModels();

            // Loads the menu scene data.
            // 
            // ���j���[�̃V�[���f�[�^��ǂݍ��� 
            data.sceneData = Content.Load<SceneData>("Layout/menu/menu_Scene");

            // Creates the render data.
            // 
            // �����_�[�^�[�Q�b�g���쐬���܂��B
            InitializeRenderTarget();

            // Initializes the first scene.
            // 
            // �ŏ��̃V�[���̏����������s����  
            currentMenu.RunInitializeThread();

            base.LoadContent();
        }


        /// <summary>
        /// Loads the model data.
        ///
        /// ���f���f�[�^�̓ǂݍ��݂��s���܂��B
        /// </summary>
        private void InitializeModels()
        {
            // Loads the model.
            // 
            // ���f���̓ǂݍ���
            Model[] models = new Model[] {
                Content.Load<Model>("Models/sphere01"),
                Content.Load<Model>("Models/sphere02"),
                Content.Load<Model>("Models/sphere11"),
                Content.Load<Model>("Models/sphere12"),
            };

            // Creates the model data.
            // 
            // ���f���f�[�^�̍쐬
            data.Spheres = new BasicModelData[][] {
                new BasicModelData[]
                {
                    new BasicModelData(models[0]), new BasicModelData(models[1])
                },
                new BasicModelData[]
                {
                    new BasicModelData(models[2]), new BasicModelData(models[3])
                },
            };

            // Model scale
            // 
            // ���f���̃X�P�[��
            float[] modelScale = {
                0.9f, 0.88f, data.CursorSphereSize, data.CursorSphereSize };

            // Loading
            // 
            // �ǂݍ��ݏ���   
            for (int i = 0; i < models.Length; i++)
            {
                BasicModelData sphere = data.Spheres[i / 2][i % 2];
                sphere.Scale = modelScale[i];
            }
        }


        /// <summary>
        /// Creates the render target.
        /// 
        /// �����_�[�^�[�Q�b�g���쐬���܂��B
        /// </summary>
        private void InitializeRenderTarget()
        {
            // Creates the split preview RenderTarget. 
            // 
            // �������v���r���[��RenderTarget���쐬
            data.DividePreview = new RenderTarget2D(
                GraphicsDevice,
                GameData.MovieWidth,
                GameData.MovieHeight,
                1,
                SurfaceFormat.Color,
                RenderTargetUsage.PreserveContents);

            // Creates the style animation RenderTarget. 
            // 
            // �X�^�C���A�j���[�V������RenderTarget���쐬
            data.StyleAnimation = new RenderTarget2D(
                GraphicsDevice,
                GameData.StyleWidth,
                GameData.StyleHeight,
                1,
                SurfaceFormat.Color,
                RenderTargetUsage.PreserveContents);
        }



        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        protected override void UnloadContent()
        {
            // Stops the BackgroundMusic.
            // 
            // BackgroundMusic�̒�~
            SoundComponent.Stop(bgm);

            base.UnloadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B  
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Updates the model.
            // 
            // ���f���̍X�V�������s���܂��B
            UpdateModels();

            if (GameData.FadeSeqComponent.FadeMode == FadeMode.FadeIn)
            {
                // Switches fade modes after the fade-in finishes.
                // 
                // �t�F�[�h�C��������������t�F�[�h�̃��[�h��؂�ւ��܂��B
                if (!GameData.FadeSeqComponent.IsPlay)
                {
                    GameData.FadeSeqComponent.FadeMode = FadeMode.None;
                }
            }
            else if (GameData.FadeSeqComponent.FadeMode == FadeMode.None)
            {
                // Performs main processing.
                // 
                // ���C���̍X�V�������s���܂��B
                UpdateMain(gameTime);
            }
            else if (GameData.FadeSeqComponent.FadeMode == FadeMode.FadeOut)
            {
                // Performs update processing at fade-out.
                // 
                // �t�F�[�h�A�E�g���̍X�V�������s���܂��B
                UpdateFadeOut();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Performs main processing.
        /// 
        /// ���C���̍X�V�������s���܂��B 
        /// </summary>
        private void UpdateMain(GameTime gameTime)
        {
            // Performs an error check.
            // 
            // �G���[�`�F�b�N���s���܂��B
            if (currentMenu == null || !currentMenu.Initialized)
                return;

            // Updates the movie.
            // 
            // ���[�r�[�̍X�V�������s���܂��B 
            UpdateMovie(gameTime);

            // Updates the menu.
            // 
            // ���j���[�̍X�V�������s���܂��B
            MenuBase menu = currentMenu.UpdateMain(gameTime);

            // Switches menus if the next menu has been specified.
            // 
            // ���̃��j���[���w�肳��Ă���ΐ؂�ւ��܂��B
            if (menu != null)
            {
                // Releases the current menu.
                // 
                // ���݂̃��j���[���J�����܂��B
                currentMenu.Dispose();

                // Sets a new menu.
                // 
                // �V�������j���[��ݒ肵�܂��B 
                currentMenu = menu;

                // Executes the initialization thread.
                // 
                // �������X���b�h�����s���܂��B
                currentMenu.RunInitializeThread();
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Performs update processing at fade-out.
        /// 
        /// �t�F�[�h�A�E�g���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateFadeOut()
        {
            // Sets the BackgroundMusic volume.
            // 
            // BackgroundMusic�̃{�����[����ݒ肵�܂��B
            float volume = 1.0f - (GameData.FadeSeqComponent.Count / 60.0f);
            SoundComponent.SetVolume(bgm, volume);

            // Performs release processing after the fade-out finishes.
            // 
            // �t�F�[�h�A�E�g���I��������J���������s���܂��B  
            if (!GameData.FadeSeqComponent.IsPlay)
            {
                Dispose();
            }
        }



        /// <summary>
        /// Updates the movie.
        /// 
        /// ���[�r�[�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMovie(GameTime gameTime)
        {
            // Switches the sequence when the animation movie 
            // loading thread is complete.
            // 
            // �A�j���[�V�������[�r�[�̓ǂݍ��݃X���b�h���������Ă�����
            // �V�[�P���X��؂�ւ��܂��B 
            if (data.movieLoader != null && data.movieLoader.Initialized)
            {
                // Releases the movie if it has already been set.
                // 
                // ���Ƀ��[�r�[���ݒ肳��Ă���������������s���܂��B
                if (data.movie != null)
                {
                    data.movie.Dispose();
                }

                data.movie = data.movieLoader.Movie;
                data.movieLoader = null;
            }

            // Updates the movie if it has been designated.
            // 
            // ���[�r�[���w�肳��Ă���΍X�V�������s���܂��B
            if (data.movie != null)
            {
                data.movie.Update(gameTime);
            }
        }

        /// <summary>
        /// Updates the model.
        /// 
        /// ���f���̍X�V�������s���܂��B 
        /// </summary>
        private void UpdateModels()
        {
            Vector3 rotate;

            // Rotates the background sphere.x
            // 
            // �w�i�̋��̂���]�����܂��B
            rotate = data.Spheres[0][0].Rotate;
            rotate.Y += MathHelper.ToRadians(0.1f);
            data.Spheres[0][0].Rotate = rotate;

            rotate = data.Spheres[0][1].Rotate;
            rotate.Y -= MathHelper.ToRadians(0.03f);
            data.Spheres[0][1].Rotate = rotate;

            // Rotates the cursor sphere.
            // 
            // �J�[�\���̋��̂���]�����܂��B
            rotate = data.Spheres[1][0].Rotate;
            rotate.Y += data.CursorSphereRotate;
            data.Spheres[1][0].Rotate = rotate;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs drawing processing.
        /// 
        /// �`�揈�����s���܂��B 
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Draws the style animation texture.
            // 
            // �X�^�C���A�j���[�V�����̃e�N�X�`����`�悵�܂��B
            DrawStyleAnimation(Batch);

            // Draws the movie animation texture.
            // 
            // ���[�r�[�A�j���[�V�����̃e�N�X�`����`�悵�܂��B             
            DrawMovie(gameTime);

            // Uses the movie animation texture 
            // to create the split preview texture.
            // 
            // ���[�r�[�A�j���[�V�����̃e�N�X�`�����g�p���A
            // �����v���r���[�̃e�N�X�`�����쐬���܂��B
          
            DrawDividePreview();

            // Draws the background.
            // 
            // �w�i��`�悵�܂��B 
            Batch.Begin();
            Batch.Draw(wallpaper, Vector2.Zero, Color.White);
            Batch.End();

            // Clears the depth buffer and draws the sphere model.
            // 
            // �[�x�o�b�t�@���N���A���A���̂̃��f����`�悵�܂��B
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSpheres(Batch);

            // Performs drawing processing if the menu is set and
            // initialization is complete.
            // 
            // ���j���[���ݒ肳��A���������I�����Ă�����`�揈�����s���܂��B        
            if (currentMenu != null && currentMenu.Initialized)
            {
                currentMenu.Draw(gameTime, Batch);
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// Draws the cursor sphere.
        /// 
        /// �J�[�\���̋��̂�`�悵�܂��B
        /// </summary>
        private void DrawSpheres(SpriteBatch batch)
        {
            GraphicsDevice graphicsDevice = batch.GraphicsDevice;
            Matrix view;
            view = Matrix.CreateLookAt(data.CameraPosition, Vector3.Zero, Vector3.Up);

            for (int i = 0; i < 2; i++)
            {
                BasicModelData basicModel = data.Spheres[0][i];
                basicModel.SetRenderState(graphicsDevice, SpriteBlendMode.Additive);
                basicModel.Draw(view, GameData.Projection);
            }
        }


        /// <summary>
        /// Draws the movie animation and sets it in the texture. 
        ///
        /// ���[�r�[�̃A�j���[�V������`�悵�A�e�N�X�`���ɐݒ肵�܂��B
        /// </summary>
        private void DrawMovie(GameTime gameTime)
        {
            // Processing is not performed if the movie has not been set.
            // 
            // ���[�r�[���ݒ肳��Ă��Ȃ��ꍇ�͏������s���܂���B
            if (data.movie == null)
                return;

            data.movie.Draw(gameTime);
            data.movieTexture = data.movie.Texture;
        }


        /// <summary>
        /// Draws the split preview and sets it in the texture.
        /// 
        /// �����v���r���[��`�悵�ăe�N�X�`���ɐݒ肵�܂��B
        /// </summary>
        private void DrawDividePreview()
        {
            // Performs an error check.
            // 
            // �G���[�`�F�b�N���s���܂��B  
            if (!data.PanelManager.IsInitialized ||
                data.DividePreview == null || 
                data.DividePreview.IsDisposed)
            {
                return;
            }


            // Changes the render target.
            // 
            // �`����ύX���܂��B
            GraphicsDevice.SetRenderTarget(0, data.DividePreview);

            // Clears the background.
            // 
            // �w�i���N���A���܂��B
            GraphicsDevice.Clear(Color.Black);

            // Draws the movie animation.
            // 
            // ���[�r�[�A�j���[�V������`�悵�܂��B
            if (data.movieTexture != null)
            {
                Batch.Begin();
                Batch.Draw(data.movieTexture, Vector2.Zero, Color.White);
                Batch.End();
            }

            // Draws the split image.
            // 
            // �����C���[�W��`�悵�܂��B
            Matrix projection = GameData.MovieScreenProjection;
            Color fillColor = new Color(0xff, 0x00, 0x00, 0x80);
            for (int x = 0; x < data.PanelManager.PanelCount.X; ++x)
            {
                for (int y = 0; y < data.PanelManager.PanelCount.Y; ++y)
                {
                    // Draws a rectangle at the panel position.
                    // 
                    // �p�l���̏ꏊ�ɋ�`��`�悵�܂��B 
                    Rectangle rect = data.PanelManager.GetPanel(x, y).RectanglePosition;
                    data.primitiveDraw.FillRect(projection, rect, fillColor);
                    data.primitiveDraw.DrawRect(projection, rect, Color.Yellow);
                }
            }

            // Returns the render target.
            // 
            // �`����߂��܂��B
            GraphicsDevice.SetRenderTarget(0, null);

            // Obtains the preview texture.
            // 
            // �v���r���[�̃e�N�X�`�����擾���܂��B
            data.divideTexture = 
                (data.DividePreview == null || data.DividePreview.IsDisposed) ? null : 
                data.DividePreview.GetTexture();
        }


        /// <summary>
        /// Draws the style animation and sets it in the texture.
        /// 
        /// �X�^�C���A�j���[�V������`�悵�ăe�N�X�`���ɐݒ肵�܂��B        
        /// </summary>
        private void DrawStyleAnimation(SpriteBatch batch)
        {
            RenderTarget2D renderTarget = data.StyleAnimation;
            SequencePlayData seqPlayData = data.SeqStyleAnimation;

            // Performs an error check.
            // 
            // �G���[�`�F�b�N�����܂��B
            if (seqPlayData == null || renderTarget == null || renderTarget.IsDisposed)
                return;

            // Changes the render target.
            // 
            // �`����ύX���܂��B             
            GraphicsDevice.SetRenderTarget(0, renderTarget);

            // Clears the background to a transparent color.
            // 
            // ���ߐF�Ŕw�i���N���A���܂��B
            GraphicsDevice.Clear(Color.TransparentBlack);

            // Draws the sequence.
            // 
            // �V�[�P���X��`�悵�܂��B 
            batch.Begin();
            seqPlayData.Draw(batch, null);
            batch.End();

            // Returns the render target.
            // 
            // �`����߂��܂��B 
            GraphicsDevice.SetRenderTarget(0, null);

            // Obtains the style animation texture.
            // 
            // �X�^�C���A�j���[�V�����̃e�N�X�`�����擾���܂��B 
            data.StyleAnimationTexture =
                (renderTarget == null || renderTarget.IsDisposed) ? null :
                renderTarget.GetTexture();
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}