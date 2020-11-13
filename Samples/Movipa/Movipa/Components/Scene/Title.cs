#region File Description
//-----------------------------------------------------------------------------
// Title.cs
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

using Movipa.Components.Animation;
using Movipa.Components.Input;
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene
{
    /// <summary>
    /// Scene component that displays the title.
    /// Provides fade control and item selection.
    /// Items obtain the position from Layout data and draw the text string.
    /// The texture that draws the movie is specified 
    /// in the BasicEffect Texture properties to draw the film model texture. 
    /// 
    /// 
    /// �^�C�g����\������V�[���R���|�[�l���g�ł��B
    /// �t�F�[�h�̐���ƁA���ڂ̑I���������s���Ă��܂��B
    /// ���ڂ�Layout�̃f�[�^����ʒu���擾���A�������`�悵�Ă��܂��B
    /// �t�B�������f���̃e�N�X�`���ɂ�BasicEffect��Texture�v���p�e�B��
    /// ���[�r�[��`�悵���e�N�X�`�����w�肵�ĕ`�悵�Ă��܂��B
    /// </summary>
    public class Title : SceneComponent
    {
        #region Private Types
        /// <summary>
        /// Item type specified with cursor 
        /// 
        /// �J�[�\�����w�肵�Ă��鍀�ڂ̎��
        /// </summary>
        private enum CursorType
        {
            /// <summary>
            /// Game start
            /// 
            /// �Q�[���J�n
            /// </summary>
            Start,

            /// <summary>
            /// Game end
            /// 
            /// �Q�[���I��
            /// </summary>
            Quit,

            /// For count
            /// 
            // �J�E���g�p
            Count,
        }
        #endregion

        #region Fields
        // Position and size of animation mounted on film
        // 
        // �t�B�����ɓ\��t����A�j���[�V�����̈ʒu�ƃT�C�Y
        private readonly Rectangle animationTexturePosition;

        // Components
        private FadeSeqComponent fade;

        // Film model
        // 
        // �t�B�����̃��f��
        private BasicModelData film;
        private Vector3 filmCameraPosition;
        private Vector3 filmCameraLookAt;
        private RenderTarget2D filmRenderTarget;
        private RenderTarget2D filmModelRenderTarget;

        // Title skinned animation model
        // 
        // �^�C�g���̃X�L���A�j���[�V�������f��
        private SkinnedModelData titleModel;
        private Vector3 titleModelCameraPosition;
        private Vector3 titleModelCameraLookAt;
        private Vector3 titleModelLightPosition;
        private Plane titleModelLightPlane;
        private RenderTarget2D titleModelRenderTarget;
        private RenderTarget2D shadowRenderTarget;
        private Color shadowColor;

        // Animation
        // 
        // �A�j���[�V����
        private PuzzleAnimation animation;

        // Cursor position
        // 
        // �J�[�\���ʒu
        private CursorType cursor;

        // Coordinates defined in Layout
        // 
        // Layout�Œ�`���ꂽ���W
        private Dictionary<string, Vector2> positions;

        // Start text string
        // 
        // �J�n�̕�����
        private string stringStart;

        // End text string
        // 
        // �I���̕�����
        private string stringQuit;

        // Menu draw font
        // 
        // ���j���[�̕`��t�H���g
        private SpriteFont menuFont;

        // Developer text string
        // 
        // �J����Ђ̕�����
        private string stringDeveloper;

        // Developer draw font
        // 
        // �J����Ђ̕`��t�H���g
        private SpriteFont developerFont;

        // Background texture
        // 
        // �w�i�̃e�N�X�`��
        private Texture2D wallpaperTexture;

        // Film texture
        // 
        // �t�B�����̃e�N�X�`��
        private Texture2D filmTexture;
        
        // BackgroundMusic Cue
        // 
        // BackgroundMusic�̃L���[
        private Cue bgm;

        // Layout
        private SceneData sceneData = null;
        private SequencePlayData seqSubTitle = null;
        private SequencePlayData seqStart = null;
        private SequencePlayData seqQuit = null;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public Title(Game game)
            : base(game)
        {
            // Sets the size of the animation to be drawn in the film texture.
            // 
            // �t�B�����̃e�N�X�`���ɕ`�悷��A�j���[�V�����̃T�C�Y��ݒ肵�܂��B
            animationTexturePosition = new Rectangle(142, 40, 640, 360);
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Sets the initial cursor position.
            // 
            // �J�[�\���̏����ʒu��ݒ肵�܂��B
            cursor = CursorType.Start;

            // Sets the film model camera.
            // 
            // �t�B�����̃��f���̃J������ݒ肵�܂��B
            filmCameraPosition = new Vector3(0.0f, 0.0f, 150.0f);
            filmCameraLookAt = Vector3.Zero;

            // Sets the title logo camera.
            // 
            // �^�C�g�����S�̃J������ݒ肵�܂��B
            titleModelCameraPosition = new Vector3(0, 50, -300f);
            titleModelCameraLookAt = new Vector3(0, -20, 0);

            // Sets the position of the light that creates the title logo shadow.
            // 
            // �^�C�g�����S�̉e����郉�C�g�̈ʒu��ݒ肵�܂��B
            titleModelLightPosition = new Vector3(50, 40, -30);
            titleModelLightPlane = new Plane(Vector3.Up, 0);

            // Obtains the fade component instance.
            // 
            // �t�F�[�h�R���|�[�l���g�̃C���X�^���X���擾���܂��B
            fade = GameData.FadeSeqComponent;

            // Sets the fade-in.
            // 
            // �t�F�[�h�C���̐ݒ���s���܂��B
            fade.Start(FadeType.Gonzales, FadeMode.FadeIn);

            // Plays the BackgroundMusic and obtains the Cue.
            // 
            // BackgroundMusic���Đ����ACue���擾���܂��B
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.TitleBackgroundMusic);

            base.Initialize();
        }


        /// <summary>
        /// Initializes the Navigate.
        /// 
        /// �i�r�Q�[�g�̏����������܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
            Navigate.Add(new NavigateData(AppSettings("B_Cancel"), true));
            base.InitializeNavigate();
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Obtains the font.
            // 
            // �t�H���g���擾���܂��B
            menuFont = LargeFont;
            developerFont = MediumFont;

            // Obtains the menu text string.
            // 
            // ���j���[�̕�������擾���܂��B
            stringStart = GameData.AppSettings["TitleStart"];
            stringQuit = GameData.AppSettings["TitleQuit"];
            stringDeveloper = GameData.AppSettings["DeveloperName"];

            // Loads the texture.
            // 
            // �e�N�X�`����ǂݍ��݂܂��B
            string asset;
            asset = "Textures/Wallpaper/Wallpaper_002";
            wallpaperTexture = Content.Load<Texture2D>(asset);

            asset = "Textures/Title/Film";
            filmTexture = Content.Load<Texture2D>(asset);

            // Loads and initializes the model data.
            // 
            // ���f���f�[�^�̓ǂݍ��݂Ə��������s���܂��B
            InitializeModels();

            // Loads and initializes the sequence.
            // 
            // �V�[�P���X�̓ǂݍ��݂Ə��������s���܂��B
            InitializeSequence();

            // Loads the movie animation.
            // 
            // ���[�r�[�A�j���[�V�����̓ǂݍ��݂��s���܂��B
            InitializeAnimation();

            // Obtains the positions from the sequence.
            // 
             // �V�[�P���X����z�u���擾���܂��B
            InitializePositions(sceneData);

            // Creates the render target.
            // 
            // �����_�[�^�[�Q�b�g�̍쐬���s���܂��B
            InitializeRenderTarget();

            base.LoadContent();
        }


        /// <summary>
        /// Loads and initializes the model data.
        /// 
        /// ���f���f�[�^�̓ǂݍ��݂Ə��������s���܂��B
        /// </summary>
        private void InitializeModels()
        {
            // Loads the film model.
            // 
            // �t�B�����̃��f����ǂݍ��݂܂��B
            film = new BasicModelData(Content.Load<Model>("Models/film"));
            film.Rotate = new Vector3(0.0f, -0.1765043f, -0.4786051f);
            film.FogEnabled = true;
            film.FogColor = Vector3.Zero;
            film.FogStart = 10;
            film.FogEnd = 500;

            // Loads the title model.
            // 
            // �^�C�g���̃��f����ǂݍ��݂܂��B
            titleModel = new SkinnedModelData(Content.Load<Model>(GetTitleAsset()), 
                "Take 001");
            titleModel.Rotate = new Vector3(0, MathHelper.ToRadians(180), 0);
            shadowColor = new Color(0, 0, 0, 192);
        }


        /// <summary>
        /// Loads and initializes the sequence.
        /// 
        /// �V�[�P���X�̓ǂݍ��݂Ə��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            // Loads the Layout data.
            // 
            // Layout�̃f�[�^��ǂݍ��݂܂��B
            sceneData = Content.Load<SceneData>("Layout/title/Title_Scene");

            // Creates the sequence.
            // 
            // �V�[�P���X���쐬���܂��B
            seqStart = sceneData.CreatePlaySeqData("Start");
            seqQuit = sceneData.CreatePlaySeqData("Quit");
            seqSubTitle = sceneData.CreatePlaySeqData("SubTitle");
        }


        /// <summary>
        /// Loads the movie animation.
        /// 
        /// ���[�r�[�A�j���[�V�����̓ǂݍ��݂��s���܂��B
        /// </summary>
        private void InitializeAnimation()
        {
            // Loads animations at random.
            // 
            // �����_���ŃA�j���[�V������ǂݍ��݂܂��B
            Random rnd = new Random();
            int id = rnd.Next(GameData.MovieList.Count);
            string asset = GameData.MovieList[id];
            AnimationInfo animationInfo = Content.Load<AnimationInfo>(asset);
            animation = PuzzleAnimation.CreateAnimationComponent(Game, animationInfo);
            AddComponent(animation);
        }


        /// <summary>
        /// Obtains the positions from the sequence.
        /// 
        /// �V�[�P���X����z�u���擾���܂��B
        /// </summary>
        private void InitializePositions(SceneData sceneData)
        {
            PatternGroupData patternGroup;
            Point point;
            Vector2 position;

            positions = new Dictionary<string, Vector2>();

            // Obtains the position of the Start text string.
            // 
            // Start�̕�����̈ʒu���擾���܂��B
            patternGroup = sceneData.PatternGroupDictionary["Title_Start_Normal"];
            point = patternGroup.PatternObjectList[0].Position;
            position = new Vector2(point.X, point.Y);
            positions.Add(stringStart, position);

            // Obtains the position of the Quit text string.
            // 
            // Quit�̕�����̈ʒu���擾���܂��B
            patternGroup = sceneData.PatternGroupDictionary["Title_Quit_Normal"];
            point = patternGroup.PatternObjectList[0].Position;
            position = new Vector2(point.X, point.Y);
            positions.Add(stringQuit, position);

            // Obtains the position of the developer.
            // 
            // �J���Ж��̈ʒu���擾���܂��B
            patternGroup = sceneData.PatternGroupDictionary["Title_Developer"];
            point = patternGroup.PatternObjectList[0].Position;
            position = new Vector2(point.X, point.Y);

            // Aligns with the centered coordinates.
            // 
            // �Z���^�����O���ꂽ���W�ɍ��킹�܂��B
            position -= developerFont.MeasureString(stringDeveloper) * 0.5f;

            positions.Add(stringDeveloper, position);
        }



        /// <summary>
        /// Creates the render target.
        /// 
        /// �����_�[�^�[�Q�b�g���쐬���܂��B
        /// </summary>
        private void InitializeRenderTarget()
        {
            // Obtains the parameters.
            // 
            // �p�����[�^���擾���܂��B
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;
            MultiSampleType msType = pp.MultiSampleType;
            int msQuality = pp.MultiSampleQuality;

            // Obtains the film size.
            // 
            // �t�B�����̃T�C�Y���擾���܂��B
            Point filmSize = new Point(filmTexture.Width, filmTexture.Height);

            // Obtains the screen size.
            // 
            // �X�N���[���̃T�C�Y���擾���܂��B
            int width = GameData.ScreenWidth;
            int height = GameData.ScreenHeight;

            // Creates the render target.
            // 
            // �����_�[�^�[�Q�b�g���쐬���܂��B
            filmRenderTarget = new RenderTarget2D(GraphicsDevice,
                filmSize.X, filmSize.Y, 1, format, msType, msQuality, 
                RenderTargetUsage.PreserveContents);

            filmModelRenderTarget = new RenderTarget2D(GraphicsDevice,
                width, height, 1, format, msType, msQuality,
                RenderTargetUsage.PreserveContents);

            titleModelRenderTarget = new RenderTarget2D(GraphicsDevice,
                width, height, 1, format, msType, msQuality,
                RenderTargetUsage.PreserveContents);

            shadowRenderTarget = new RenderTarget2D(GraphicsDevice,
                width, height, 1, format, msType, msQuality,
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
            // BackgroundMusic���~���܂��B
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
            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V�������s���܂��B
            UpdateSequence(gameTime);

            // Updates the model.
            // 
            // ���f���̍X�V�������s���܂��B
            UpdateModels(gameTime);


            if (fade.FadeMode == FadeMode.FadeIn)
            {
                // Switches fade modes after the fade-in has finished.
                // 
                // �t�F�[�h�C��������������t�F�[�h�̃��[�h��؂�ւ��܂��B
                if (!fade.IsPlay)
                {
                    fade.FadeMode = FadeMode.None;
                }
            }
            else if (fade.FadeMode == FadeMode.None)
            {
                // Performs main update processing.
                // 
                // ���C���̍X�V�������s���܂��B
                UpdateMain();
            }
            else if (fade.FadeMode == FadeMode.FadeOut)
            {
                // Performs update processing at fade-out.
                // 
                // �t�F�[�h�A�E�g���̍X�V�������s���܂��B
                UpdateFadeOut();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the sequence.
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSequence(GameTime gameTime)
        {
            seqStart.Update(gameTime.ElapsedGameTime);
            seqQuit.Update(gameTime.ElapsedGameTime);
            seqSubTitle.Update(gameTime.ElapsedGameTime);
        }


        /// <summary>
        /// Updates the model.
        /// 
        /// ���f���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateModels(GameTime gameTime)
        {
            // Rotates the film.
            // 
            // �t�B��������]�����܂��B
            Vector3 rotate = film.Rotate;
            rotate.X += MathHelper.ToRadians(0.1f);
            film.Rotate = rotate;

            // Runs the title logo animation.
            // 
            // �^�C�g�����S�̃A�j���[�V�����������܂��B
            titleModel.AnimationPlayer.Update(
                gameTime.ElapsedGameTime, true, Matrix.Identity);
        }


        /// <summary>
        /// Performs main update processing.
        /// 
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMain()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;
            VirtualPadDPad dPad = virtualPad.DPad;

            if (InputState.IsPush(buttons.A))
            {
                // Confirms with the A button or the Start button,
                // then executes fade-out.
                // 
                // A�{�^���܂��̓X�^�[�g�{�^���Ō�������A
                // �t�F�[�h�A�E�g�̏������s���܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                fade.Start(FadeType.RotateBox, FadeMode.FadeOut);
            }
            else if (InputState.IsPush(buttons.B, buttons.Back))
            {
                cursor = CursorType.Quit;
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                fade.Start(FadeType.RotateBox, FadeMode.FadeOut);               
            }
            else if (InputState.IsPush(dPad.Up, leftStick.Up, dPad.Down, leftStick.Down))
            {
                // Moves the cursor.
                // Common processing is used since there are only two items:
                // Start and Quit.
                // 
                // �J�[�\���̈ړ��������s���܂��B
                // ���ڂ�Start��Quit��2�����Ȃ��̂ŁA���ʂ̏������g�p���܂��B
                cursor = CursorMove();
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

                // Replays the sequence.
                // 
                // �V�[�P���X�����v���C�����܂��B
                seqStart.Replay();
                seqQuit.Replay();
            }

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
            float volume = 1.0f - (fade.Count / 60.0f);
            SoundComponent.SetVolume(bgm, volume);

            if (!fade.IsPlay)
            {
                // Performs release processing after the fade-out has finished.
                // 
                // �t�F�[�h�A�E�g������������J���������s���܂��B
                Dispose();

                // Registers the next scene if the cursor has selected Start.
                // 
                // �J�[�\����Start��I�����Ă����玟�̃V�[����o�^���܂��B
                if (cursor == CursorType.Start)
                {
                    Game.Components.Add(new Menu.MenuComponent(Game));
                }
            }
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
            // Draws the film model to the render target.
            // 
            // �t�B�����̃��f���������_�[�^�[�Q�b�g�ɕ`�悵�܂��B
            if (!DrawFilmModel()) return;

            // Draws the title model to the render target.
            // 
            // �^�C�g���̃��f���������_�[�^�[�Q�b�g�ɕ`�悵�܂��B
            if (!DrawTitleModel()) return;

            // Draws the title shadow to the render target.
            // 
            // �^�C�g���̉e�������_�[�^�[�Q�b�g�ɕ`�悵�܂��B
            if (!DrawTitleModelShadow()) return;

            Batch.Begin();

            // Draws the background.
            // 
            // �w�i��`�悵�܂��B
            Batch.Draw(wallpaperTexture, Vector2.Zero, Color.Silver);

            if ((filmModelRenderTarget != null) && !filmModelRenderTarget.IsDisposed)
            {
                // Draws the film.
                // 
                // �t�B������`�悵�܂��B
                Batch.Draw(filmModelRenderTarget.GetTexture(), Vector2.Zero,
                    Color.Silver);
            }

            if ((shadowRenderTarget != null) && !shadowRenderTarget.IsDisposed)
            {
                // Draws the title shadow.
                // 
                // �^�C�g���̉e��`�悵�܂��B
                Batch.Draw(shadowRenderTarget.GetTexture(), Vector2.Zero, shadowColor);
            }

            if ((titleModelRenderTarget != null) && !titleModelRenderTarget.IsDisposed)
            {
                // Draws the title.
                // 
                // �^�C�g����`�悵�܂��B
                Batch.Draw(titleModelRenderTarget.GetTexture(), Vector2.Zero, 
                    Color.White);
            }

            // Draws the title sequence.
            // 
            // �^�C�g���V�[�P���X�̕`�悵�܂��B
            DrawSequence(gameTime, Batch);

            Batch.End();

            base.Draw(gameTime);
        }


        /// <summary>
        /// Draws the sequence.
        /// 
        /// �V�[�P���X��`�悵�܂��B
        /// </summary>
        private void DrawSequence(GameTime gameTime, SpriteBatch batch)
        {
            // Draws the sub-title.
            // 
            // �T�u�^�C�g����`�悵�܂��B
            seqSubTitle.Draw(batch, null);

            // Draws the developer name.
            // 
            // �J���Ж���`�悵�܂��B
            Vector2 position = positions[stringDeveloper];
            batch.DrawString(developerFont, stringDeveloper, position, Color.White);

            // Draws the sequence text string.
            // 
            // �V�[�P���X�̕������`�悵�܂��B
            DrawSequenceString(batch);

            // Draws the Navigate button.
            // 
            // �i�r�Q�[�g�{�^����`�悵�܂��B
            DrawNavigate(gameTime, false);
        }


        /// <summary>
        /// Draws the sequence text string.
        /// 
        /// �V�[�P���X�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch)
        {
            Vector2 position;
            bool selected;

            position = positions[stringStart];
            selected = (cursor == CursorType.Start);
            DrawSequenceString(batch, seqStart, stringStart, position, selected);

            position = positions[stringQuit];
            selected = (cursor == CursorType.Quit);
            DrawSequenceString(batch, seqQuit, stringQuit, position, selected);
        }


        /// <summary>
        /// Draws the sequence text string.
        /// 
        /// �V�[�P���X�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString(SpriteBatch batch, SequencePlayData sequence, 
            string text, Vector2 position, bool selected)
        {
            Color color;

            if (selected)
            {
                // Obtains the selected status color.
                // 
                // �I����Ԃ̐F���擾���܂��B
                color = sequence.SequenceData.GetDrawPatternObjectDrawData(0, 0).Color;
            }
            else
            {
                // Obtains the non-selected status color.
                // 
                // ��I����Ԃ̐F���擾���܂��B
                SequenceGroupData sequenceGroup;
                sequenceGroup = sequence.SequenceData.SequenceGroupList[0];
                color = sequenceGroup.SequenceObjectList[0].PatternObjectList[0].Color;
            }

            // Draws the text string. 
            // 
            // �������`�悵�܂��B
            batch.DrawString(menuFont, text, position, color);
        }


        /// <summary>
        /// Draws the film model.
        /// 
        /// �t�B�����̃��f����`�悵�܂��B
        /// </summary>
        private bool DrawFilmModel()
        {
            // Draws the film texture.
            // 
            // �t�B�����̃e�N�X�`����`�悵�܂��B
            if (!DrawFilmTexture())
            {
                return false;
            }

            if ((filmRenderTarget == null) || filmRenderTarget.IsDisposed)
            {
                return false;
            }
            // Sets the texture in the film model.
            // 
            // �t�B�����̃��f���Ƀe�N�X�`����ݒ肵�܂��B
            film.Texture = filmRenderTarget.GetTexture();

            // Changes the render target.
            // 
            // �`����ύX���܂��B
            GraphicsDevice.SetRenderTarget(0, filmModelRenderTarget);

            // Clears the background to transparent.
            // 
            // �w�i�𓧉ߐF�ŃN���A���܂��B
            GraphicsDevice.Clear(Color.TransparentBlack);

            // Enables the depth buffer.
            // 
            // �[�x�o�b�t�@��L���ɂ��܂��B
            GraphicsDevice.RenderState.DepthBufferEnable = true;

            // Draws the film model.
            // 
            // �t�B�����̃��f����`�悵�܂��B
            Matrix view = Matrix.CreateLookAt(
                filmCameraPosition, filmCameraLookAt, Vector3.Up);
            film.SetRenderState(GraphicsDevice, SpriteBlendMode.AlphaBlend);
            GraphicsDevice.RenderState.CullMode = CullMode.None;
            film.Draw(view, GameData.Projection);

            // Returns the render target.
            // 
            // �`����߂��܂��B
            GraphicsDevice.SetRenderTarget(0, null);

            return true;
        }


        /// <summary>
        /// Draws the film texture.
        /// 
        /// �t�B�����̃e�N�X�`����`�悵�܂��B
        /// </summary>
        private bool DrawFilmTexture()
        {
            Texture2D animationTexture = animation.Texture;

            // Performs an error check.
            // 
            // �G���[�`�F�b�N���s���܂��B
            if (animationTexture == null ||
                animationTexture.IsDisposed ||
                filmRenderTarget == null ||
                filmRenderTarget.IsDisposed)
            {
                return false;
            }

            // Changes the render target.
            // 
            // �`����ύX���܂��B
            GraphicsDevice.SetRenderTarget(0, filmRenderTarget);

            // Clears the background to transparent.
            // 
            // �w�i�𓧉ߐF�ŃN���A���܂��B
            GraphicsDevice.Clear(Color.TransparentBlack);

            // Draws the film frame.
            //
            // �t�B�����̊O�g��`�悵�܂��B
            Batch.Begin();
            Batch.Draw(filmTexture, Vector2.Zero, Color.White);
            Batch.End();

            // Draws the animation.
            //
            // �A�j���[�V������`�悵�܂��B
            Batch.Begin(SpriteBlendMode.None);
            Batch.Draw(animationTexture, animationTexturePosition, Color.White);
            Batch.End();

            // Returns the render target.
            // 
            // �`����߂��܂��B
            GraphicsDevice.SetRenderTarget(0, null);

            return true;
        }


        /// <summary>
        /// Draws the title model.
        /// 
        /// �^�C�g���̃��f����`�悵�܂��B
        /// </summary>
        private bool DrawTitleModel()
        {
            if ((titleModelRenderTarget == null) || titleModelRenderTarget.IsDisposed)
            {
                return false;
            }
            // Changes the render target.
            // 
            // �`����ύX���܂��B
            GraphicsDevice.SetRenderTarget(0, titleModelRenderTarget);

            // Clears the background to transparent.
            // 
            // �w�i�𓧉ߐF�ŃN���A���܂��B
            GraphicsDevice.Clear(Color.TransparentBlack);

            // Enables the depth buffer.
            // 
            // �[�x�o�b�t�@��L���ɂ��܂��B
            GraphicsDevice.RenderState.DepthBufferEnable = true;

            // Draws the title model.
            // 
            // �^�C�g���̃��f����`�悵�܂��B
            Matrix view = Matrix.CreateLookAt(
                titleModelCameraPosition, titleModelCameraLookAt, Vector3.Up);
            titleModel.SetRenderState(GraphicsDevice, SpriteBlendMode.AlphaBlend);
            titleModel.Draw(Matrix.Identity, view, GameData.Projection,
                true, Vector3.One, Vector3.Zero);

            // Returns the render target.
            // 
            // �`����߂��܂��B
            GraphicsDevice.SetRenderTarget(0, null);

            return true;
        }


        /// <summary>
        /// Draws the title model shadow.
        /// 
        /// �^�C�g���̃��f���̉e��`�悵�܂��B
        /// </summary>
        private bool DrawTitleModelShadow()
        {
            if ((shadowRenderTarget == null) || shadowRenderTarget.IsDisposed)
            {
                return false;
            }

            // Changes the render target.
            // 
            // �`����ύX���܂��B
            GraphicsDevice.SetRenderTarget(0, shadowRenderTarget);

            // Clears the background to transparent.
            // 
            // �w�i�𓧉ߐF�ŃN���A���܂��B
            GraphicsDevice.Clear(Color.TransparentBlack);

            // Enables the depth buffer.
            // 
            // �[�x�o�b�t�@��L���ɂ��܂��B
            GraphicsDevice.RenderState.DepthBufferEnable = true;

            // Creates the shadow matrix.
            // 
            // �e�̃}�g���b�N�X���쐬���܂��B
            Matrix shadowMatrix = Matrix.CreateShadow(
                titleModelLightPosition, titleModelLightPlane);

            // Draws the shadow model.
            // 
            // �e�̃��f����`�悵�܂��B
            Matrix view = Matrix.CreateLookAt(
                titleModelCameraPosition, titleModelCameraLookAt, Vector3.Up);
            Vector3 rotate = titleModel.Rotate;
            titleModel.Rotate = Vector3.Zero;
            titleModel.SetRenderState(GraphicsDevice, SpriteBlendMode.AlphaBlend);
            titleModel.Draw(shadowMatrix, view, GameData.Projection, false);
            titleModel.Rotate = rotate;

            // Returns the render target.
            // 
            // �`����߂��܂��B
            GraphicsDevice.SetRenderTarget(0, null);

            return true;
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Obtains the title asset name at random.
        /// 
        /// �^�C�g���̃A�Z�b�g���������_���Ŏ擾���܂��B
        /// </summary>
        private string GetTitleAsset()
        {
            int i = Random.Next(6) + 1;
            string asset = string.Format("Models/Title/movipa_title_{0:00}", i);
            return asset;
        }


        /// <summary>
        /// Returns the next cursor position.
        /// 
        /// �J�[�\���̎��̈ʒu��Ԃ��܂��B
        /// </summary>
        private CursorType CursorMove()
        {
            return (CursorType)(((int)cursor + 1) % (int)CursorType.Count);
        }
        #endregion
    }
}


