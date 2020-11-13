#region File Description
//-----------------------------------------------------------------------------
// PuzzleComponent.cs
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

using Movipa.Components.Animation;
using Movipa.Components.Input;
using Movipa.Components.Scene.Puzzle.Style;
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Puzzle
{
    /// <summary>
    /// Scene Component for processing the puzzle.
    /// Invoked in both Normal Mode and Free Mode, it
    /// changes the processing within individual methods.
    /// ChangeComponent, RevolveComponent and SlideComponent inherited
    /// from StyleBase are used to interchange panels.
    /// This component is created using the CreateStyleComponent method.
    /// 
    /// �p�Y���̏���������V�[���R���|�[�l���g�ł��B
    /// �m�[�}�����[�h�ƁA�t���[���[�h���ʂŌĂяo����A
    /// �e���\�b�h���ŏ������e��ύX���Ă��܂��B
    /// �p�l���̓���ւ������ɂ́AStyleBase���p������ChangeComponent�A
    /// RevolveComponent�ASlideComponent���g�p���܂��B
    /// ���̃R���|�[�l���g�̍쐬�ɂ�CreateStyleComponent���\�b�h���g�p���Ă��܂��B
    /// </summary>
    public class PuzzleComponent : SceneComponent
    {
        #region Public Types
        /// <summary>
        /// Game status
        /// 
        /// �Q�[���̏��
        /// </summary>
        public enum Phase
        {
            /// <summary>
            /// Display name and number of stages 
            /// 
            /// �X�e�[�W���Ɩ��O�\��
            /// </summary>
            StageTitle,

            /// <summary>
            /// Display completed diagram
            /// 
            /// �����}�\��
            /// </summary>
            FirstPreview,

            /// <summary>
            /// Shuffle initial panels
            ///
            /// �����p�l���̃V���b�t��
            /// </summary>
            Shuffle,

            /// <summary>
            /// Start countdown
            ///
            /// �J�n�J�E���g�_�E��
            /// </summary>
            CountDown,

            /// <summary>
            /// Panel select
            ///
            /// �p�l���Z���N�g
            /// </summary>
            PanelSelect,

            /// <summary>
            /// Panel action
            ///
            /// �p�l���ړ����o
            /// </summary>
            PanelAction,

            /// <summary>
            /// Completed judgment 
            ///
            /// ��������
            /// </summary>
            Judge,

            /// <summary>
            /// Completed
            ///
            /// �������o
            /// </summary>
            Complete,

            /// <summary>
            /// Time over
            ///
            /// �^�C���I�[�o�[���o
            /// </summary>
            Timeover,
        };


        /// <summary>
        /// Paused cursor select status 
        /// 
        /// �|�[�Y���̃J�[�\���I�����
        /// </summary>
        public enum PauseCursor
        {
            /// <summary>
            /// Return to game
            ///
            /// �Q�[���ɖ߂�
            /// </summary>
            ReturnToGame,

            /// <summary>
            /// Return to title
            ///
            /// �^�C�g���ɖ߂�
            /// </summary>
            GoToTitle,

            /// <summary>
            /// Count
            /// 
            /// �J�E���g�p
            /// </summary>
            Count
        };
        #endregion

        #region Private Types
        /// <summary>
        /// Position list
        ///
        /// �|�W�V�������X�g
        /// </summary>
        enum PositionList
        {
            /// <summary>
            /// Preview
            ///
            /// �v���r���[
            /// </summary>
            Preview,

            /// <summary>
            /// Movie
            ///
            /// ���[�r�[
            /// </summary>
            Movie,

            /// <summary>
            /// Score
            ///
            /// �X�R�A
            /// </summary>
            Score,

            /// <summary>
            /// Time
            ///
            /// �^�C��
            /// </summary>
            Time,

            /// <summary>
            /// Number remaining
            ///
            /// �c�萔
            /// </summary>
            Rest,

            /// <summary>
            /// Help icon
            ///
            /// �w���v�A�C�R��
            /// </summary>
            HelpIcon,
        }
        #endregion

        #region Fields
        // Seconds for first preview shown
        // 
        // �ŏ��ɕ\������v���r���[�̕b��
        private readonly TimeSpan FirstPreviewTime = new TimeSpan(0, 0, 3);

        // Seconds to stop animation in help item
        // 
        // �w���v�A�C�e���ŃA�j���[�V�������~�߂�b��
        private readonly TimeSpan HelpItemTime = new TimeSpan(0, 0, 3);

        /// <summary>
        /// Preview zoom speed
        /// 
        /// �v���r���[�g��k�����x
        /// </summary>
        private const float ThumbZoomSpeed = 0.05f;

        /// <summary>
        /// Score when aligned in single
        ///
        /// �V���O���ő��������̃X�R�A
        /// </summary>
        private const int ScoreSingle = 10;

        /// <summary>
        /// Score when aligned in double
        ///
        /// �_�u���ő��������̃X�R�A
        /// </summary>
        private const int ScoreDouble = 50;

        /// <summary>
        /// Bonus score for help item
        ///
        /// �w���v�A�C�e���̃{�[�i�X�X�R�A
        /// </summary>
        private const int ScoreHelpItem = 100;

        /// <summary>
        /// Help item count
        ///
        /// �w���v�A�C�e���̏�����
        /// </summary>
        private const int HelpItemCount = 3;

        // Components
        private StyleBase style;
        private FadeSeqComponent fade;

        // Processing status
        // 
        // �������
        private Phase phase;

        // Display time for first preview shown
        // 
        // �ŏ��ɕ\������v���r���[�̕\������
        private TimeSpan firstPreviewTime;

        // Play time
        //
        // �v���C����
        private TimeSpan playTime;

        // Movie rectangle vertices list
        // 
        // ���[�r�[��`�̒��_���X�g
        private Vector2[] movieFramePosition;

        // Rectangle list of movie excluding panels 
        // 
        // �p�l�������������[�r�[�̋�`���X�g
        private Rectangle[] movieFrameSrc;

        // Preview zoom status
        // 
        // �v���r���[�̃Y�[�����
        private bool thumbZoom;

        // Preview zoom ratio
        // 
        // �v���r���[�̊g�嗦
        private float thumbZoomRate;

        // Movie display rectangle
        // 
        // ���[�r�[�̕\����`
        private Rectangle movieRect;

        // Preview display rectangle
        // 
        // �v���r���[�̕\����`
        private Rectangle movieThumbRect;

        // Popup score sprite list
        // 
        // �|�b�v�A�b�v�X�R�A�̃X�v���C�g���X�g
        private List<SpriteScorePopup> scorePopupList;

        // Current score
        // 
        // ���݂̃X�R�A
        private int score;

        // Displayed score
        // 
        // �\������X�R�A
        // Defined separately for addition animation.
        // 
        // ���Z�A�j���[�V�����ׂ̈ɕʂɂ��Ă���B
        private int scoreView;

        // Remaining shuffle count
        // 
        // �V���b�t���̎c���
        private int shuffleCount;

        // Pause status
        // 
        // �|�[�Y���
        private bool isPause;

        // Paused cursor position
        // 
        // �|�[�Y�̃J�[�\���ʒu
        private PauseCursor pauseCursor;

        // Cross-fade color
        // 
        // �N���X�t�F�[�h�p�J���[
        private Vector4 completeColor;

        // Cross-fade texture
        // 
        // �N���X�t�F�[�h�p�e�N�X�`��
        private ResolveTexture2D resolveTexture = null;

        // Stage information 
        // 
        // �X�e�[�W���
        private StageSetting setting;

        // Play result
        // 
        // �v���C����
        private StageResult result;

        // Panel manager class
        // 
        // �p�l���Ǘ��N���X
        private PanelManager panelManager;

        // Background texture
        // 
        // �w�i�e�N�X�`��
        private Texture2D wallpaperTexture;

        // Cursor position
        // 
        // �J�[�\���ʒu
        private Point cursor = new Point();

        // BackgroundMusic
        private Cue bgm;

        // Movie animation
        // 
        // ���[�r�[�A�j���[�V����
        private PuzzleAnimation movie;

        // Movie animation texture
        // 
        // ���[�r�[�A�j���[�V�����̃e�N�X�`��
        private Texture2D movieTexture;

        // Help item count
        // 
        // �w���v�A�C�e���̏�����
        private int helpItemCount;

        // Help item stop time
        // 
        // �w���v�A�C�e���̒�~����
        private TimeSpan helpItemTime;

        // Layout
        private SceneData sceneData;
        private SequencePlayData seqMainFrame;
        private SequencePlayData seqCountDown;
        private SequencePlayData seqTimeUp;
        private SequencePlayData seqHelpIcon;
        private SequencePlayData seqRefStart;
        private SequencePlayData seqRefLoop;
        private SequencePlayData seqRefNaviStart;
        private SequencePlayData seqRefNaviLoop;
        private SequencePlayData seqRefReturnToGameOn;
        private SequencePlayData seqRefGoToTitleOn;
        private SequencePlayData seqStage;
        private SequencePlayData seqComplete;

        // Position list
        // 
        // �|�W�V�������X�g
        private Dictionary<PositionList, Vector2> positions;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the processing status.
        /// 
        /// ������Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Phase GamePhase
        {
            get { return phase; }
            set { phase = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public PuzzleComponent(Game game, StageSetting stageSetting)
            : base(game)
        {
            setting = stageSetting;
        }


        /// <summary>
        /// Performs initialization processing.
        ///
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Initializes the processing status. 
            // In Normal Mode, sets to stage display.
            // 
            // ������Ԃ̏����ݒ���s���܂��B
            // �m�[�}�����[�h�̏ꍇ�̓X�e�[�W��\�����鏈���ɐݒ肵�܂��B
            GamePhase = (setting.Mode == StageSetting.ModeList.Normal) ?
                Phase.StageTitle : Phase.FirstPreview;

            // Initializes the member variables.
            // 
            // �����o�ϐ��̏��������s���܂��B
            score = 0;
            scoreView = 0;
            result = new StageResult();
            isPause = false;
            playTime = new TimeSpan();
            firstPreviewTime = FirstPreviewTime;
            helpItemCount = HelpItemCount;
            helpItemTime = new TimeSpan();
            thumbZoomRate = 1.0f;
            thumbZoom = true;
            completeColor = Vector4.One;

            // Sets the shuffle count.
            // The shuffle count is twice the total number of panels.
            // 
            // �V���b�t������񐔂�ݒ肵�܂��B
            // �V���b�t���񐔂͑��p�l������2�{�������l�ł��B
            shuffleCount = (setting.Divide.X * setting.Divide.Y) * 2;

            // Creates the panel management class.
            // 
            // �p�l���Ǘ��N���X���쐬���܂��B
            panelManager = new PanelManager(Game);

            // Creates the panel.
            // 
            // �p�l�����쐬���܂��B
            panelManager.CreatePanel(GameData.MovieSizePoint, setting);

            // Adds the component for interchanging panels.
            // 
            // �p�l�������ւ���R���|�[�l���g��ǉ����܂��B
            style = CreateStyleComponent();
            AddComponent(style);

            // Sets to No Drawing on the component side.
            // 
            // �R���|�[�l���g���ŕ`�悵�Ȃ��ݒ�����܂��B
            style.Visible = false;

            // Creates the popup score sprite array.
            // 
            // �|�b�v�A�b�v����X�R�A�̃X�v���C�g�z����쐬���܂��B
            scorePopupList = new List<SpriteScorePopup>();

            // Obtains the fade component instance. 
            // 
            // �t�F�[�h�R���|�[�l���g�̃C���X�^���X���擾���܂��B
            fade = GameData.FadeSeqComponent;

            // Sets the fade-in processing.
            // 
            // �t�F�[�h�C���̏�����ݒ肵�܂��B
            fade.Start(FadeType.Normal, FadeMode.FadeIn);

            base.Initialize();
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g��ǂݍ��݂܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the background texture.
            // 
            // �w�i�̃e�N�X�`����ǂݍ��݂܂��B
            string asset = "Textures/Wallpaper/Wallpaper_006";
            wallpaperTexture = Content.Load<Texture2D>(asset);

            // Initializes the sequence.
            // 
            // �V�[�P���X�̏��������s���܂��B
            InitializeSequence();

            // Initializes the movie.
            // 
            // ���[�r�[�̏��������s���܂��B
            InitializeMovie();

            // Plays the BackgroundMusic and obtains the Cue.
            // 
            // BackgroundMusic���Đ����ACue���擾���܂��B
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.GameBackgroundMusic);
            
            base.LoadContent();
        }


        /// <summary>
        /// Initializes the sequence.
        /// 
        /// �V�[�P���X�̏��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            // Loads the SceneData.
            // 
            // SceneData��ǂݍ��݂܂��B
            string asset = "Layout/game/game_Scene";
            sceneData = Content.Load<SceneData>(asset);

            // Creates the sequence.
            // 
            // �V�[�P���X���쐬���܂��B
            string name = (setting.Mode == StageSetting.ModeList.Normal) ?
                "GameNormal" : "GameFree";
            seqMainFrame = sceneData.CreatePlaySeqData(name);

            seqCountDown = sceneData.CreatePlaySeqData("StartCount");
            seqTimeUp = sceneData.CreatePlaySeqData("TimeUp");
            seqHelpIcon = sceneData.CreatePlaySeqData("HelpIcon");
            seqRefStart = sceneData.CreatePlaySeqData("RefStart");
            seqRefLoop = sceneData.CreatePlaySeqData("RefLoop");
            seqRefReturnToGameOn = sceneData.CreatePlaySeqData("RefReturnToGameOn");
            seqRefGoToTitleOn = sceneData.CreatePlaySeqData("RefGoToTitleOn");
            seqStage = sceneData.CreatePlaySeqData("Stage");
            seqComplete = sceneData.CreatePlaySeqData("Complete");

            // Creates the sequence in accordance with the style.
            // 
            // �X�^�C���ɉ������V�[�P���X���쐬���܂��B
            if (setting.Style == StageSetting.StyleList.Change &&
                setting.Rotate == StageSetting.RotateMode.On)
            {
                seqRefNaviStart = sceneData.CreatePlaySeqData("RefChangeRotateStart");
                seqRefNaviLoop = sceneData.CreatePlaySeqData("RefChangeRotateLoop");
            }
            else if (setting.Style == StageSetting.StyleList.Change &&
                setting.Rotate == StageSetting.RotateMode.Off)
            {
                seqRefNaviStart = sceneData.CreatePlaySeqData("RefChangeStart");
                seqRefNaviLoop = sceneData.CreatePlaySeqData("RefChangeLoop");
            }
            else if (setting.Style == StageSetting.StyleList.Revolve)
            {
                seqRefNaviStart = sceneData.CreatePlaySeqData("RefRevolveStart");
                seqRefNaviLoop = sceneData.CreatePlaySeqData("RefRevolveLoop");
            }
            else if (setting.Style == StageSetting.StyleList.Slide)
            {
                seqRefNaviStart = sceneData.CreatePlaySeqData("RefSlideStart");
                seqRefNaviLoop = sceneData.CreatePlaySeqData("RefSlideLoop");
            }

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            seqCountDown.Replay();
            seqTimeUp.Replay();
            seqComplete.Replay();

            // Loads the position.
            // 
            // �|�W�V������ǂݍ��݂܂��B
            positions = new Dictionary<PositionList, Vector2>();
            PatternGroupData patternGroup;
            PatternObjectData patternObject;
            Point point;

            // Obtains the pattern group.
            // 
            // �p�^�[���O���[�v���擾���܂��B
            patternGroup = sceneData.PatternGroupDictionary["Main_Pos"];

            // Obtains the preview position.
            // 
            // �v���r���[�̃|�W�V�������擾���܂��B
            patternObject = patternGroup.PatternObjectList[0];
            point = patternObject.Position;
            positions.Add(PositionList.Preview, new Vector2(point.X, point.Y));
            movieThumbRect = new Rectangle(
                point.X,
                point.Y,
                (int)(patternObject.Rect.Width * patternObject.Scale.X),
                (int)(patternObject.Rect.Height * patternObject.Scale.Y));


            // Obtains the movie position.
            // 
            // ���[�r�[�̃|�W�V�������擾���܂��B
            patternObject = patternGroup.PatternObjectList[1];
            point = patternObject.Position;
            positions.Add(PositionList.Movie, new Vector2(point.X, point.Y));
            movieRect = new Rectangle(
                point.X,
                point.Y,
                (int)(patternObject.Rect.Width * patternObject.Scale.X),
                (int)(patternObject.Rect.Height * patternObject.Scale.Y));

            // Obtains the score position.
            // 
            // �X�R�A�̃|�W�V�������擾���܂��B
            point = patternGroup.PatternObjectList[2].Position;
            positions.Add(PositionList.Score, new Vector2(point.X, point.Y));

            // Obtains the time position.
            // 
            // �^�C���̃|�W�V�������擾���܂��B
            point = patternGroup.PatternObjectList[3].Position;
            positions.Add(PositionList.Time, new Vector2(point.X, point.Y));

            // Obtains the remaining panel count position.
            // 
            // �c��p�l�����̃|�W�V�������擾���܂��B
            point = patternGroup.PatternObjectList[4].Position;
            positions.Add(PositionList.Rest, new Vector2(point.X, point.Y));

            // Obtains the help icon position.
            // 
            // �w���v�A�C�R���̃|�W�V�������擾���܂��B
            point = patternGroup.PatternObjectList[5].Position;
            positions.Add(PositionList.HelpIcon, new Vector2(point.X, point.Y));
        }


        /// <summary>
        /// Initializes the movie.
        /// 
        /// ���[�r�[�̏��������s���܂��B
        /// </summary>
        private void InitializeMovie()
        {
            // Loads movie information.
            // 
            // ���[�r�[�̏���ǂݍ��݂܂��B
            AnimationInfo info = Content.Load<AnimationInfo>(setting.Movie);

            // Adds movie components.
            // 
            // ���[�r�[�̃R���|�[�l���g��ǉ����܂��B
            movie = CreateMovie(info);
            AddComponent(movie);

            // Turns automatic movie updating off.
            //
            // ���[�r�[�������ōX�V���Ȃ��悤�ɐݒ肵�܂��B
            movie.Enabled = false;

            // Obtains the movie outer rectangle.
            // 
            // ���[�r�[�O�̋�`���擾���܂��B
            movieFramePosition = GetMovieFramePosition();
            movieFrameSrc = GetMovieFrameSource();

            // Sets the panel draw position offset.
            // 
            // �p�l���`��ʒu�̃I�t�Z�b�g��ݒ肵�܂��B
            panelManager.DrawOffset = positions[PositionList.Movie];
        }


        /// <summary>
        /// Creates the screen back buffer.
        /// 
        /// ��ʂ̃o�b�N�o�b�t�@���쐬���܂��B
        /// </summary>
        private void InitializeBackBuffer()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            resolveTexture = new ResolveTexture2D(
                GraphicsDevice,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                1,
                pp.BackBufferFormat);
        }


        /// <summary>
        /// Releases all content.
        ///
        /// �S�ẴR���e���g���J�����܂��B
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

            // Updates the preview.
            //
            // �v���r���[�̍X�V�������s���܂��B
            UpdatePreview();

            // Updates the movie.
            // 
            // ���[�r�[�̍X�V�������s���܂��B
            UpdateMovie(gameTime);

            // Updates the score.
            // 
            // �X�R�A�̍X�V�������s���܂��B
            UpdateScore(gameTime);

            if (fade.FadeMode == FadeMode.FadeIn)
            {
                // Sets to Main Processing after the fade-in finishes.
                // 
                // �t�F�[�h�C�����I��������A���C���̏����ɐݒ肵�܂��B
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
                UpdateMain(gameTime);
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
            // Updates the background sequence.
            // 
            // �w�i�̃V�[�P���X���X�V���܂��B
            seqMainFrame.Update(gameTime.ElapsedGameTime);

            // Updates the help icon sequence.
            // 
            // �w���v�A�C�R���̃V�[�P���X���X�V���܂��B
            seqHelpIcon.Update(gameTime.ElapsedGameTime);

            // Updates the pause screen sequence. 
            // 
            // �|�[�Y��ʂ̃V�[�P���X���X�V���܂��B
            seqRefStart.Update(gameTime.ElapsedGameTime);
            seqRefLoop.Update(gameTime.ElapsedGameTime);
            seqRefNaviStart.Update(gameTime.ElapsedGameTime);
            seqRefNaviLoop.Update(gameTime.ElapsedGameTime);
            seqRefReturnToGameOn.Update(gameTime.ElapsedGameTime);
            seqRefGoToTitleOn.Update(gameTime.ElapsedGameTime);
        }


        /// <summary>
        /// Updates the preview.
        /// 
        /// �v���r���[�̍X�V�������s���܂��B
        /// </summary>
        private void UpdatePreview()
        {
            float zoomRate = (thumbZoom) ? ThumbZoomSpeed : -ThumbZoomSpeed;
            thumbZoomRate = MathHelper.Clamp(thumbZoomRate + zoomRate, 0.0f, 1.0f);
        }


        /// <summary>
        /// Updates the movie. 
        /// 
        /// ���[�r�[�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateMovie(GameTime gameTime)
        {
            GameTime time;

            // Reduces the hint item elapsed time.
            // 
            // �q���g�A�C�e���̎g�p���Ԃ����炵�܂��B
            helpItemTime -= gameTime.ElapsedGameTime;

            // If the hint time is greater than 0, the movie update time is set to zero.
            // If the hint time is negative, the normal update time is set.
            // 
            // �q���g�^�C����0��葽����΁A���[�r�[�̍X�V���Ԃ��[���ɐݒ肵�܂��B
            // �q���g�^�C�����}�C�i�X�Ȃ�΁A�ʏ�̍X�V���Ԃ�ݒ肵�܂��B
            time = (helpItemTime <= TimeSpan.Zero) ? gameTime : new GameTime();

            // Updates the movie.
            // 
            // ���[�r�[���X�V���܂��B
            movie.Update(time);
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

            // Performs release processing when the fade-out has finished.
            // 
            // �t�F�[�h�A�E�g���I��������J���������s���܂��B
            if (!fade.IsPlay)
            {
                Dispose();
            }
        }




        /// <summary>
        /// Performs main update processing.
        /// 
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public void UpdateMain(GameTime gameTime)
        {
            // Determines the pause.
            // 
            // �|�[�Y�̔�������܂��B
            if (isPause)
            {
                // Performs update processing during pause.
                // 
                // �|�[�Y���̍X�V�������s���܂��B
                UpdatePause();
                return;
            }
            else
            {
                // Checks the pause button status, and sets to Pause 
                // if it has been pressed.
                // 
                // �|�[�Y�{�^���������ꂽ���`�F�b�N���A
                // ������Ă���΃|�[�Y�̐ݒ���s���܂��B
                if (CheckPause())
                {
                    return;
                }
            }

            if (GamePhase == Phase.StageTitle)
            {
                // Performs update processing when the number of stages 
                // and the name are displayed. 
                // 
                // �X�e�[�W���𖼑O��\�����Ă���Ƃ��̍X�V�������s���܂��B
                UpdateStageTitle(gameTime);
            }
            else if (GamePhase == Phase.FirstPreview)
            {
                // Performs update processing to display the completed picture preview.
                // 
                // �����}�̃v���r���[��\������X�V�������s���܂��B
                UpdateFirstPreview(gameTime);
            }
            else if (GamePhase == Phase.Shuffle)
            {
                // Performs update processing at panel shuffle.
                //
                // �p�l���V���b�t�����̍X�V�������s���܂��B
                UpdateShuffle();
            }
            else if (GamePhase == Phase.CountDown)
            {
                // Performs update processing during countdown.
                // 
                // �J�E���g�_�E�����̍X�V�������s���܂��B
                UpdateCountDown(gameTime);
            }
            else if (GamePhase == Phase.PanelSelect)
            {
                // Performs update processing at panel selection.
                // 
                // �p�l���I�����̍X�V�������s���܂��B
                UpdatePanelSelect(gameTime);
            }
            else if (GamePhase == Phase.PanelAction)
            {
                // Performs updates processing during panel action.
                // 
                // �p�l�����쒆�̍X�V�������s���܂��B
                UpdatePanelAction();
            }
            else if (GamePhase == Phase.Judge)
            {
                // Performs update processing to judge panels.
                // 
                // �p�l���̔��������X�V�������s���܂��B
                UpdateJudge();
            }
            else if (GamePhase == Phase.Complete)
            {
                // Performs update processing at completion.
                // 
                // �������̍X�V�������s���܂��B
                UpdateComplete(gameTime);
            }
            else if (GamePhase == Phase.Complete)
            {
                // Performs update processing at completion.
                // 
                // �������̍X�V�������s���܂��B
                UpdateComplete(gameTime);
            }
            else if (GamePhase == Phase.Timeover)
            {
                // Performs update processing at time over.
                // 
                // �^�C���I�[�o�[���̍X�V�������s���܂��B
                UpdateTimeOver(gameTime);
            }
        }


        /// <summary>
        /// Performs update processing when the number of stages 
        /// and the name are displayed.
        /// 
        /// �X�e�[�W���𖼑O��\�����Ă���Ƃ��̍X�V�������s���܂��B
        /// </summary>
        public void UpdateStageTitle(GameTime gameTime)
        {
            seqStage.Update(gameTime.ElapsedGameTime);

            // Sets to Initial Preview Processing after sequence playback finishes.

            // 
            // �V�[�P���X�̍Đ����I��������A�ŏ��̃v���r���[�����ɐݒ肵�܂��B
            if (!seqStage.IsPlay)
            {
                GamePhase = Phase.FirstPreview;
            }
        }


        /// <summary>
        /// Performs update processing to display completed picture preview.
        /// 
        /// �����}�̃v���r���[��\������X�V�������s���܂��B
        /// </summary>
        public void UpdateFirstPreview(GameTime gameTime)
        {
            // Reduces the display time.
            // 
            // �\�����Ԃ����炵�܂��B
            firstPreviewTime -= gameTime.ElapsedGameTime;

            if (firstPreviewTime < TimeSpan.Zero)
            {
                // Sets the thumbnail to reduced size.
                // 
                // �T���l�C�����k���ɐݒ肵�܂��B
                if (thumbZoom)
                {
                    thumbZoom = false;
                }

                // Sets processing status to Shuffle after 
                // thumbnail reduction has finished.
                // 
                // �T���l�C���̏k�������������珈����Ԃ��V���b�t���ɐݒ肵�܂��B
                if (thumbZoomRate == 0.0f)
                {
                    GamePhase = Phase.Shuffle;
                }
            }
        }


        /// <summary>
        /// Performs update processing at panel shuffle.
        /// 
        /// �p�l���V���b�t�����̍X�V�������s���܂��B
        /// </summary>
        public void UpdateShuffle()
        {
            // Obtains the number of completed panels.
            // 
            // �����������擾���܂��B
            int complete = panelManager.PanelCompleteCount(setting);

            // Checks to see if all shuffles are completed.
            // 
            // �S�ẴV���b�t���������������`�F�b�N���܂��B
            if (shuffleCount == 0 && complete == 0)
            {
                // Performs the panel judgment and sets the processing status
                // to Countdown.
                // 
                // �p�l���̔�������A������Ԃ��J�E���g�_�E���ɐݒ肵�܂��B
                panelManager.PanelCompleteCheck(setting);

                GamePhase = Phase.CountDown;
            }

            // Shuffles the panels.
            // 
            // �p�l�����V���b�t�����܂��B
            if (style.RandomShuffle())
            {
                // Reduces the shuffle count.
                // 
                // �V���b�t���񐔂����炵�܂��B
                if (shuffleCount > 0)
                {
                    shuffleCount--;
                }
            }
        }


        /// <summary>
        /// Performs update processing during countdown.
        /// 
        /// �J�E���g�_�E�����̍X�V�������s���܂��B
        /// </summary>
        public void UpdateCountDown(GameTime gameTime)
        {
            seqCountDown.Update(gameTime.ElapsedGameTime);

            // Sets the processing status to Panel Selection
            // after the sequence finishes.
            // 
            // �V�[�P���X���I�������珈����Ԃ��p�l���̑I���ɐݒ肵�܂��B
            if (!seqCountDown.IsPlay)
            {
                // Enables panel selection.
                // 
                // �p�l���̑I�����\�ɂ��܂��B
                style.SelectEnabled = true;

                GamePhase = Phase.PanelSelect;
            }
        }


        /// <summary>
        /// Performs update processing at panel selection.
        /// 
        /// �p�l���I�����̍X�V�������s���܂��B
        /// </summary>
        public void UpdatePanelSelect(GameTime gameTime)
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Updates the play time. 
            // 
            // �v���C�^�C���̍X�V�������s���܂��B
            UpdatePlayTime(gameTime);

            // Performs time limit judgment.
            // 
            // �^�C�����~�b�g�̔�����s���܂��B
            if (IsTimeLimit())
            {
                // Sets to the same value as the time limit 
                // to prevent a negative time readout.
                // 
                // �^�C���\�L���}�C�i�X�ɍs���Ȃ��悤�ɁA
                // �������ԂƓ����l��ݒ肵�܂��B
                playTime = setting.TimeLimit;

                // Sets the processing status to Time Over.
                //
                // ������Ԃ��^�C���I�[�o�[�ɐݒ肵�܂��B
                GamePhase = Phase.Timeover;
            }

            // Changes the processing status if the panel is currently in action.
            // 
            // �p�l�������쒆�Ȃ�΁A������Ԃ�ύX���܂��B
            if (style.IsPanelAction)
            {
                GamePhase = Phase.PanelAction;
            }

            // Uses the help item.
            //
            // �w���v�A�C�e�����g�p���܂��B
            if (buttons.LeftShoulder[VirtualKeyState.Push])
            {
                UseHelpItem();
            }

            // Sets the thumbnail size status.
            // 
            // �T���l�C���̃Y�[����Ԃ�ݒ肵�܂��B
            thumbZoom = buttons.RightShoulder[VirtualKeyState.Press];
        }


        /// <summary>
        /// Updates the play time.
        /// 
        /// �v���C�^�C���̍X�V�������s���܂��B
        /// </summary>
        private void UpdatePlayTime(GameTime gameTime)
        {
            playTime += gameTime.ElapsedGameTime;
            result.ClearTime = playTime;
            if (setting.Mode == StageSetting.ModeList.Normal)
            {
                GameData.SaveData.TotalPlayTime += gameTime.ElapsedGameTime;
            }
        }


        /// <summary>
        /// Performs update processing during panel actions.
        /// 
        /// �p�l�����쒆�̍X�V�������s���܂��B
        /// </summary>
        public void UpdatePanelAction()
        {
            // Sets the processing status to Judge after the panel actions finish.
            // 
            // �p�l���̓��삪����������A������Ԃ𔻒�ɐݒ肵�܂��B
            if (!style.IsPanelAction)
            {
                GamePhase = Phase.Judge;
            }
        }


        /// <summary>
        /// Updates panel judgment.
        /// 
        /// �p�l���̔��������X�V�������s���܂��B
        /// </summary>
        public void UpdateJudge()
        {
            // Obtains the newly completed panel list.
            // 
            // �V���Ɋ������ꂽ�p�l���̃��X�g���擾���܂��B
            List<PanelData> list = panelManager.PanelCompleteCheck(setting);

            // Plays the SoundEffect if all panels are prepared.
            // 
            // �p�l���������Ă����SoundEffect���Đ����܂��B
            if (list.Count > 0)
            {
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
            }

            // Checks the number of prepared panels.
            // 
            // �����������`�F�b�N���܂��B
            if (list.Count == 1)
            {
                PanelCompleteSingle(list);
            }
            else if (list.Count == 2)
            {
                PanelCompleteDouble(list);
            }

            // Sets the processing status to Completed if all panels are complete.
            // 
            // �S�Ċ���������A������Ԃ������ɐݒ肵�܂��B
            if (panelManager.PanelCompleteRatio(setting) >= 100)
            {
                GamePhase = Phase.Complete;

                // Disables panel selection.
                // 
                // �p�l����I��s�ɐݒ肵�܂��B
                style.SelectEnabled = false;
            }
            else
            {
                // If still not completed, sets the processing status to Panel Select.
                // 
                // �܂��������Ă��Ȃ��ꍇ�́A������Ԃ��p�l���I���ɐݒ肵�܂��B
                GamePhase = Phase.PanelSelect;
            }
        }


        /// <summary>
        /// Performs update processing at completion.
        /// 
        /// �������̍X�V�������s���܂��B
        /// </summary>
        public void UpdateComplete(GameTime gameTime)
        {
            // Processing is not performed during score updates.
            // 
            // �X�R�A���X�V���Ȃ珈�������܂���B
            if (scorePopupList.Count > 0 || score != scoreView)
                return;

            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Updates the complete sequence.
            // 
            // �R���v���[�g�̃V�[�P���X���X�V���܂��B
            seqComplete.Update(gameTime.ElapsedGameTime);

            // Sets the cross-fade buffer.
            // 
            // �N���X�t�F�[�h�p�̃o�b�t�@��ݒ肵�܂��B
            if (resolveTexture == null)
            {
                // Creates the back buffer.
                // 
                // �o�b�N�o�b�t�@�̍쐬�����܂��B
                InitializeBackBuffer();

                // Transfers the current screen to the back buffer.
                // 
                // ���݂̉�ʂ��o�b�N�o�b�t�@�ɓ]�����܂��B
                GraphicsDevice.ResolveBackBuffer(resolveTexture);

                // Plays the clear SoundEffect.
                // 
                // �N���A��SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectClear);

                // Registers the Navigate button settings.
                // 
                // �i�r�Q�[�g�{�^���̐ݒ�����܂��B
                Navigate.Clear();
                Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
            }

            // Reduces the cross-fade transparency color.
            // 
            // �N���X�t�F�[�h�p�̓��ߐF�������Ă����܂��B
            completeColor.W = MathHelper.Clamp(completeColor.W - 0.01f, 0.0f, 1.0f);

            // Omits processing where transparency color still remains.
            // 
            // ���ߐF���܂��c���Ă���ꍇ�͏����𔲂��܂��B
            if (completeColor.W > 0)
                return;

            // Once the performance is completely finished, the A button switches 
            // to the results display screen.
            // 
            // ���o���S�ďI�������A�{�^���Ō��ʕ\����ʂɑJ�ڂ��܂��B
            if (buttons.A[VirtualKeyState.Push])
            {
                // Plays the SoundEffect of the Enter button.
                // 
                // ����{�^����SoundEffect���Đ����܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

                // Adds the bonus based on the number of help items remaining.
                //
                // �c��̃w���v�A�C�e���̐��ɉ����ă{�[�i�X�����Z���܂��B
                result.HintScore = ScoreHelpItem * helpItemCount;

                // Sets the move count.
                // 
                // �ړ��񐔂̐ݒ�����܂��B
                result.MoveCount = style.MoveCount;

                // Adds the scene for the next transition.
                // Divides the result display classes for 
                // Normal Mode and Free Mode. 
                // 
                // ���ɑJ�ڂ���V�[���̒ǉ������܂��B
                // �m�[�}�����[�h�ƁA�t���[���[�h�Ō��ʕ\����ʂ�
                // �N���X���킯�Ă��܂��B
                SceneComponent scene;
                if (setting.Mode == StageSetting.ModeList.Normal)
                {
                    scene = new Result.NormalResult(Game, result);
                }
                else
                {
                    scene = new Result.FreeResult(Game, result);
                }
                GameData.SceneQueue.Enqueue(scene);

                // Sets the fade-out process.
                // 
                // �t�F�[�h�A�E�g�̏�����ݒ肵�܂��B
                GameData.FadeSeqComponent.Start(FadeType.RotateBox, FadeMode.FadeOut);
            }
        }


        /// <summary>
        /// Performs time over update processing.
        /// 
        /// �^�C���I�[�o�[���̍X�V�������s���܂��B
        /// </summary>
        public void UpdateTimeOver(GameTime gameTime)
        {
            seqTimeUp.Update(gameTime.ElapsedGameTime);

            // When the time over animation finishes, registers the 
            // game over scene and sets the fade-out process.
            // 
            // �^�C���I�[�o�[�̃A�j���[�V�������I��������Q�[���I�[�o�[��
            // �V�[����o�^���A�t�F�[�h�A�E�g�̏�����ݒ肵�܂��B
            if (!seqTimeUp.IsPlay)
            {
                GameData.SceneQueue.Enqueue(new GameOver(Game));
                GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
            }
        }


        /// <summary>
        /// Performs update processing during pause.
        /// 
        /// �|�[�Y���̍X�V�������s���܂��B
        /// </summary>
        public void UpdatePause()
        { 
            // The following process is not performed while the sequence is playing.
            // 
            // �V�[�P���X���Đ����Ȃ�ȉ��̏������s���܂���B
            if (seqRefNaviStart.IsPlay)
                return;

            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (InputState.IsPush(buttons.B, buttons.Start))
            {
                // Plays the canceled SoundEffect and releases the pause status
                // when either the B button or the start button is pressed.
                //
                // B�{�^�����A�X�^�[�g�{�^���������ꂽ��L�����Z����
                // SoundEffect���Đ����A�|�[�Y��Ԃ��������܂��B
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);

                isPause = false;

                // Enables the panel selection.
                // 
                // �p�l���I���\��Ԃɂ���
                style.SelectEnabled = true;
            }
            else if (buttons.A[VirtualKeyState.Push])
            {
                if (pauseCursor == PauseCursor.GoToTitle)
                {
                    // When confirmed with the A button, Return to Title
                    // is selected, so it plays the determined SoundEffect and 
                    // registers the title scene, then sets the fade-out process.
                    // 
                    // A�{�^���Ō��莞�A�^�C�g���ɖ߂��I�����Ă���̂�
                    // �����SoundEffect���Đ����A�^�C�g���̃V�[����o�^���Ă���
                    // �t�F�[�h�A�E�g�̏�����ݒ肵�܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

                    // Adds the scene for the next transition
                    // 
                    // ���ɑJ�ڂ���V�[���̒ǉ�
                    GameData.SceneQueue.Enqueue(new Title(Game));
                    fade.Start(FadeType.RotateBox, FadeMode.FadeOut);
                }
                else if (pauseCursor == PauseCursor.ReturnToGame)
                {
                    // When confirmed with the A button, the Return to Game
                    // item is selected, so the behavior is the same as Cancel.
                    // 
                    // A�{�^���Ō��莞�A�Q�[���ɖ߂鍀�ڂ�I�����Ă���̂�
                    // �L�����Z���Ɠ������������܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);

                    isPause = false;

                    // Enables the panel selection.
                    //
                    // �p�l���I���\��Ԃɂ���
                    style.SelectEnabled = true;
                }
            }
            else if (InputState.IsPush(dPad.Up, leftStick.Up, dPad.Down, 
                leftStick.Down))
            {
                // Moves the cursor vertically. 
                // There are only two items, so common processing is used.
                //
                // �㉺�ŃJ�[�\�����ړ����܂��B
                // ���ڂ�2�����Ȃ��̂ŁA���ʂ̏������g�p���܂��B

                int count = (int)PauseCursor.Count;
                int cursor = (int)pauseCursor;

                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor2);
                pauseCursor = (PauseCursor)((cursor + 1) % count);
            }
        }


        /// <summary>
        /// Updates the score.
        /// 
        /// �X�R�A�̍X�V�������s���܂��B
        /// </summary>
        public void UpdateScore(GameTime gameTime)
        {
            // Updates the sprite.
            // 
            // �X�v���C�g�̍X�V���s���܂��B
            for (int i = 0; i < scorePopupList.Count; i++)
            {
                SpriteScorePopup sprite = scorePopupList[i];
                sprite.Update(gameTime);

                // Adds up the score and deletes it from the array
                // after the sprite action finishes.
                // 
                // �X�v���C�g�̓��삪�I�������X�R�A�����Z���A
                // �z�񂩂�폜���܂��B
                if (sprite.Disposed)
                {
                    score += sprite.Score;
                    scorePopupList.Remove(sprite);
                }
            }

            // Adds up the display score so that it tracks the actual score.
            // 
            // �\���p�̃X�R�A�́A���ۂ̃X�R�A��ǂ��悤�ɉ��Z���܂��B
            if (score > scoreView)
            {
                scoreView++;
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs drawing processing.
        /// 
        /// �`�揈�����s���܂��B
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draws the game.
            // 
            // ���[�r�[�`��
            movieTexture = movie.Texture;
            style.MovieTexture = movieTexture;

            // Draws the background.
            // 
            // �w�i��`�悷��
            Batch.Begin();
            {
                // Draws the BG.
                // 
                // BG��`��
                Batch.Draw(wallpaperTexture, Vector2.Zero, Color.White);

                // Draws the main frame.
                // 
                // ���C���̃t���[����`��
                seqMainFrame.Draw(Batch, null);

                // Draws the help icon.
                // 
                // �w���v�A�C�R���̕`��
                DrawHelpIcon(Batch);

                // Draws the text string.
                // 
                // ������̕`��
                SpriteFont font = LargeFont;
                DrawTextScore(Batch, font);
                DrawTextTime(Batch, font);
                DrawTextRest(Batch, font);
            }
            Batch.End();

            // Draws the movie outer frame.
            // 
            // ���[�r�[�̊O�g��`��
            DrawOutMovie();

            // Draws the movie panels.
            // 
            // ���[�r�[�̃p�l���`��
            style.DrawPanels(gameTime);

            // Draws the cursor.
            //
            // �J�[�\���`��
            style.DrawCursor(gameTime);

            // Draws the popup score.
            //
            // �|�b�v�A�b�v�X�R�A��`��
            Batch.Begin();
            {
                SpriteBatch batch = Batch;
                foreach (SpriteScorePopup sprite in scorePopupList)
                {
                    sprite.Draw(gameTime, batch);
                }
            }
            Batch.End();

            // Draws the preview.
            //
            // �v���r���[��`��
            DrawThumbnail();

            // Performs drawing processing as per status.
            //
            // ��Ԃɉ������`��
            DrawMain(gameTime);

            base.Draw(gameTime);
        }


        /// <summary>
        /// Performs main drawing processing.
        /// 
        /// ���C���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawMain(GameTime gameTime)
        {
            if (GamePhase == Phase.StageTitle)
            {
                // Draws the number of stages and the title.
                // 
                // �X�e�[�W���ƃ^�C�g���̕`�揈�����s���܂��B
                DrawStageTitle();
            }
            else if (GamePhase == Phase.CountDown)
            {
                // Performs countdown drawing.
                // 
                // �J�E���g�_�E���̕`�揈�����s���܂��B
                DrawCountDown();
            }
            else if (GamePhase == Phase.Complete)
            {
                // Performs drawing processing at completion.
                // 
                // �������̕`�揈�����s���܂��B
                DrawComplete(gameTime);
            }
            else if (GamePhase == Phase.Timeover)
            {
                // Performs over time drawing.
                // 
                // �^�C���I�[�o�[�̕`�揈�����s���܂��B
                DrawTimeOver();
            }

            // Performs drawing processing during pause.
            // 
            // �|�[�Y���̕`����s���܂��B
            if (isPause)
            {
                DrawPause(gameTime);
            }

        }


        /// <summary>
        /// Draws the number of stages and the title.
        /// 
        /// �X�e�[�W���ƃ^�C�g���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawStageTitle()
        {
            Batch.Begin();

            // Draws the sequence.
            // 
            // �V�[�P���X�̕`������܂��B
            seqStage.Draw(Batch, null);


            SequenceGroupData seqBodyData;
            string text;

            // Draws the text string for the number of stages.
            // 
            // �X�e�[�W���̕������`�悵�܂��B
            text = string.Format("{0:00}", GameData.SaveData.Stage + 1);
            seqBodyData = seqStage.SequenceData.SequenceGroupList[2];
            DrawStageTitle(seqBodyData, text);

            // Draws the text string for the stage name.
            // 
            // �X�e�[�W���̕������`�悵�܂��B
            text = movie.Info.Name;
            seqBodyData = seqStage.SequenceData.SequenceGroupList[3];
            DrawStageTitle(seqBodyData, text);

            Batch.End();
        }


        /// <summary>
        /// Draws the number of stages and the title.
        /// 
        /// �X�e�[�W���ƃ^�C�g���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawStageTitle(SequenceGroupData sequenceGroup, string text)
        {
            SequenceObjectData sequenceObject = sequenceGroup.CurrentObjectList;

            // Processing is not performed if data cannot be obtained.
            // 
            // �f�[�^���擾�ł��Ȃ������ꍇ�͏������s���܂���B
            if (sequenceObject == null)
                return;

            List<PatternObjectData> list = sequenceObject.PatternObjectList;
            foreach (PatternObjectData patternObject in list)
            {
                SpriteFont font = LargeFont;
                DrawData putInfoData = patternObject.InterpolationDrawData;
                Color color = putInfoData.Color;
                Point point = putInfoData.Position;
                Vector2 position = new Vector2(point.X, point.Y);

                // Centers the Y coordinate.
                // 
                // Y���W���Z���^�����O���܂��B
                position.Y -= (font.MeasureString(text).Y * 0.5f);

                // Draws the text string.
                // 
                // ������̕`������܂��B
                Batch.DrawString(font, text, position, color);
            }
        }


        /// <summary>
        /// Performs countdown drawing.
        /// 
        /// �J�E���g�_�E���̕`�揈�����s���܂��B
        /// </summary>
        private void DrawCountDown()
        {
            Batch.Begin();
            seqCountDown.Draw(Batch, null);
            Batch.End();
        }


        /// <summary>
        /// Performs drawing processing at completion.
        /// 
        /// �������̕`�揈�����s���܂��B
        /// </summary>
        private void DrawComplete(GameTime gameTime)
        {
            // Processing is not performed while score is displayed. 
            // 
            // �X�R�A���\�����Ȃ�Ώ��������Ȃ�
            if (scorePopupList.Count > 0 || score != scoreView)
                return;

            // Draws the movie in full screen.
            // 
            // ���[�r�[���t���X�N���[���ŕ`�悵�܂��B
            if (resolveTexture != null && movieTexture != null)
            {
                Batch.Begin(SpriteBlendMode.None);
                Batch.Draw(movieTexture, GameData.ScreenSizeRect, null, Color.White);
                Batch.End();
            }


            Batch.Begin();
            
            // Draws the cross-fade back buffer.
            // 
            // �N���X�t�F�[�h�p�̃o�b�N�o�b�t�@��`�悵�܂��B
            if (resolveTexture != null)
            {
                Batch.Draw(resolveTexture, Vector2.Zero, new Color(completeColor));

                // Draws the Navigate button.
                // 
                // �i�r�Q�[�g�{�^����`�悵�܂��B
                DrawNavigate(gameTime, false);
            }

            // Performs drawing processing while the completed animation is being played.
            // 
            // �R���v���[�g�A�j���[�V�������Đ����Ȃ�`�揈�����s���܂��B
            if (seqComplete.SequenceData.IsPlay)
                seqComplete.Draw(Batch, null);

            Batch.End();
        }


        /// <summary>
        /// Performs time over drawing processing.
        /// 
        /// �^�C���I�[�o�[�̕`�揈�����s���܂��B
        /// </summary>
        public void DrawTimeOver()
        {
            Batch.Begin();
            {
                seqTimeUp.Draw(Batch, null);
            }
            Batch.End();
        }


        /// <summary>
        /// Draws the Pause screen.
        /// 
        /// �|�[�Y��ʂ̕`�揈�����s���܂��B
        /// </summary>
        public void DrawPause(GameTime gameTime)
        {
            Batch.Begin();

            if (seqRefNaviStart.IsPlay)
            {
                seqRefStart.Draw(Batch, null);
                seqRefNaviStart.Draw(Batch, null);
            }
            else
            {
                seqRefLoop.Draw(Batch, null);
                seqRefNaviLoop.Draw(Batch, null);

                // Sets the selection cursor sequence.
                // 
                // �I���J�[�\���̃V�[�P���X��ݒ肵�܂��B
                SequencePlayData seqPlayData;
                seqPlayData = (pauseCursor == PauseCursor.ReturnToGame) ?
                    seqRefReturnToGameOn : seqRefGoToTitleOn;

                // Draws the sequence.
                // 
                // �V�[�P���X��`�悵�܂��B
                seqPlayData.Draw(Batch, null);

                // Draws the Navigate button.
                // 
                // �i�r�Q�[�g�{�^���̕`����s���܂��B
                DrawNavigate(gameTime, false);
            }

            Batch.End();
        }


        /// <summary>
        /// Draws the Preview screen.
        /// 
        /// �v���r���[��ʂ̕`�揈�����s���܂��B
        /// </summary>
        public void DrawThumbnail()
        {
            // Processing is not performed if there is no movie texture.
            // 
            // ���[�r�[�̃e�N�X�`���������ꍇ�͏������s���܂���B
            if (movieTexture == null)
                return;

            // Sets the movie rectangle.
            // 
            // ���[�r�[�̋�`��ݒ肵�܂��B
            Vector4 movie = new Vector4();
            movie.X = movieRect.X;
            movie.Y = movieRect.Y;
            movie.Z = movieRect.Width;
            movie.W = movieRect.Height;

            // Sets the thumbnail rectangle.
            // 
            // �T���l�C���̋�`��ݒ肵�܂��B
            Vector4 thumb = new Vector4();
            thumb.X = movieThumbRect.X;
            thumb.Y = movieThumbRect.Y;
            thumb.Z = movieThumbRect.Width;
            thumb.W = movieThumbRect.Height;

            // Calculates the size.
            // 
            // �T�C�Y���v�Z���܂��B
            Vector4 size = Vector4.Lerp(thumb, movie, thumbZoomRate);
            Rectangle rect = new Rectangle();
            rect.X = (int)size.X;
            rect.Y = (int)size.Y;
            rect.Width = (int)size.Z;
            rect.Height = (int)size.W;

            // Draws the thumbnail.
            // 
            // �T���l�C����`�悵�܂��B
            Batch.Begin(SpriteBlendMode.None);
            Batch.Draw(movieTexture, rect, Color.White);
            Batch.End();
        }


        /// <summary>
        /// Draws the help icon.
        /// 
        /// �w���v�A�C�R���̕`�揈�����s���܂��B
        /// </summary>
        public void DrawHelpIcon(SpriteBatch batch)
        {
            // hints are only used in Normal mode
            if (setting.Mode != StageSetting.ModeList.Normal)
            {
                return;
            }

            SequenceBankData sequenceBank = seqHelpIcon.SequenceData;
            SequenceGroupData sequenceGroup = sequenceBank.SequenceGroupList[0];
            SequenceObjectData sequenceObject = sequenceGroup.SequenceObjectList[0];
            PatternObjectData patternObject = sequenceObject.PatternObjectList[0];
            Rectangle rect = patternObject.Rect;

            DrawData info = new DrawData();

            // Shifts the drawing position by the number of remaining help icons.
            // 
            // �w���v�A�C�R���̎c��̐������A�ʒu�����炵�ĕ`�悵�܂��B
            for (int i = 0; i < helpItemCount; i++)
            {
                Vector2 position = positions[PositionList.HelpIcon];
                position.X += (rect.Width * i);
                info.Position = new Point((int)position.X, (int)position.Y);
                seqHelpIcon.Draw(batch, info);
            }
        }


        /// <summary>
        /// Draws the score.
        /// 
        /// �X�R�A�̕`�揈�����s���܂��B
        /// </summary>
        public void DrawTextScore(SpriteBatch batch, SpriteFont font)
        {
            // The score is drawn only in Normal Mode.
            // 
            // Normal���[�h�ȊO�̓X�R�A�̕`����s���܂���B
            if (setting.Mode != StageSetting.ModeList.Normal)
                return;


            string text = string.Format("{0:00000}", scoreView);
            Vector2 position = positions[PositionList.Score];
            batch.DrawString(font, text, position, Color.White);
        }


        /// <summary>
        /// Draws the remaining time and elapsed time.
        /// 
        /// �c�莞�ԁE�o�ߎ��Ԃ̕`�揈�����s���܂��B
        /// </summary>
        public void DrawTextTime(SpriteBatch batch, SpriteFont font)
        {
            string text;
            if (setting.Mode == StageSetting.ModeList.Normal)
            {
                string time = (setting.TimeLimit - playTime).ToString(); 
                text = time.Substring(0, 8);
            }
            else
            {
                text = playTime.ToString().Substring(0, 8);
            }
            Vector2 position = positions[PositionList.Time];
            batch.DrawString(font, text, position, Color.White);
        }


        /// <summary>
        /// Draws the number of remaining panels.
        /// 
        /// �p�l���̎c�薇���̕`�揈�����s���܂��B
        /// </summary>
        public void DrawTextRest(SpriteBatch batch, SpriteFont font)
        {
            int rest = panelManager.PanelRestCount(setting);
            string value = string.Format("{0:00}", rest);
            Vector2 position = positions[PositionList.Rest];
            batch.DrawString(font, value, position, Color.White);
        }


        /// <summary>
        /// Draws the area with movie panels excluded.
        /// 
        /// ���[�r�[�̃p�l�����������G���A�̕`�揈�����s���܂��B
        /// </summary>
        private void DrawOutMovie()
        {
            // Drawing processing is not performed if there is no movie texture.
            // 
            // ���[�r�[�̃e�N�X�`����������Ε`�揈�����s���܂���B
            if (movieTexture == null)
                return;

            // Obtains the rectangle list.
            // 
            // ��`�̃��X�g���擾���܂��B
            Vector2[] positions = movieFramePosition;
            Rectangle[] rectangles = movieFrameSrc;

            Batch.Begin(SpriteBlendMode.None);
            for (int i = 0; i < rectangles.Length; i++)
            {
                Batch.Draw(movieTexture, positions[i], rectangles[i], Color.Silver);
            }
            Batch.End();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Movie sizes that do not fit in the panels are returned as rectangles.
        /// 
        /// �p�l���Ɏ��܂�Ȃ��������[�r�[�̃T�C�Y����`�ŕԂ��܂��B
        /// </summary>
        private Vector2[] GetMovieFramePosition()
        {
            Vector2 offset = positions[PositionList.Movie];
            Vector2 top;
            Vector2 bottom;
            Vector2 left;
            Vector2 right;

            float movieHeight = movieRect.Height;
            float panelHeight = panelManager.PanelArea.W;

            float panelX = panelManager.PanelArea.X;
            float panelWidth = panelManager.PanelArea.Z;

            top = offset;

            bottom = offset;
            bottom.Y += (movieHeight - (movieHeight - panelHeight) * 0.5f);
            
            left = offset;

            right = offset;
            right.X += panelX + panelWidth;

            return new Vector2[] { top, bottom, left, right };
        }


        /// <summary>
        /// Obtains rectangles for non-panel sections.
        /// 
        /// �p�l���ȊO�̕����̋�`���擾���܂��B
        /// </summary>
        private Rectangle[] GetMovieFrameSource()
        {
            Rectangle top;
            Rectangle bottom;
            Rectangle left;
            Rectangle right;

            float panelX = panelManager.PanelArea.X;
            float panelWidth = panelManager.PanelArea.Z;
            float panelHeight = panelManager.PanelArea.W;

            top = new Rectangle();
            top.Width = movieRect.Width;
            top.Height = (int)((movieRect.Height - panelHeight) * 0.5f);

            bottom = new Rectangle();
            bottom.Y = movieRect.Height - (int)((movieRect.Height - panelHeight) * 0.5f);
            bottom.Width = movieRect.Width;
            bottom.Height = (int)((movieRect.Height - panelHeight) * 0.5f);

            left = new Rectangle();
            left.Width = (int)(panelX);
            left.Height = movieRect.Height;

            right = new Rectangle();
            right.X = (int)(panelX + panelWidth);
            right.Width = (int)((movieRect.Width - panelWidth) * 0.5f);
            right.Height = movieRect.Height;

            return new Rectangle[] { top, bottom, left, right };
        }


        /// <summary>
        /// Adds up the completed single score.
        /// 
        /// �V���O�������̃X�R�A�����Z���܂��B
        /// </summary>
        public void AddSingleScore(long score)
        {
            result.SingleScore += score;
        }


        /// <summary>
        /// Adds up the completed double score.
        /// 
        /// �_�u�������̃X�R�A�����Z���܂��B
        /// </summary>
        public void AddDoubleScore(long score)
        {
            result.DoubleScore += score;
        }


        /// <summary>
        /// Performs processing if panels are completed in single.
        /// 
        /// �V���O���ő������ꍇ�̏������s���B
        /// </summary>
        public void PanelCompleteSingle(List<PanelData> list)
        {
            // Processing is performed only in Change. 
            // 
            // Change�ȊO�ł͏������s���܂���B
            if (setting.Style != StageSetting.StyleList.Change)
                return;

            // Processing is only performed in Normal Mode. 
            // 
            // �m�[�}�����[�h�ȊO�ł͏������s���܂���B
            if (setting.Mode != StageSetting.ModeList.Normal)
                return;

            // Adds up the score.
            // 
            // �X�R�A�����Z���܂��B
            AddSingleScore(ScoreSingle);

            // Creates the score sprite.
            // 
            // �X�R�A�̃X�v���C�g���쐬���܂��B
            foreach (PanelData panel in list)
            {
                // Creates the sprite.
                // 
                // �X�v���C�g���쐬���܂��B
                SpriteScorePopup sprite = CreateSpriteScore(panel, ScoreSingle);

                // Adds the sprite to the array.
                // 
                // �z��ɒǉ����܂��B
                scorePopupList.Add(sprite);
            }
        }


        /// <summary>
        /// Performs processing if panels are completed in double.
        /// 
        /// �p�l�����_�u���ő��������̏������s���܂��B
        /// </summary>
        public void PanelCompleteDouble(List<PanelData> list)
        {
            // Processing is performed only in Change.
            // 
            // Change�ȊO�ł͏������s���܂���B
            if (setting.Style != StageSetting.StyleList.Change)
                return;

            // Processing is performed only in Normal Mode.
            // 
            // �m�[�}�����[�h�ȊO�ł͏������s���܂���B
            if (setting.Mode != StageSetting.ModeList.Normal)
                return;

            // Adds up the score.
            // 
            // �X�R�A�����Z���܂��B
            AddDoubleScore(ScoreDouble * 2);

            // Creates the score sprite.
            // 
            // �X�R�A�̃X�v���C�g���쐬���܂��B
            foreach (PanelData panel in list)
            {
                // Creates the sprite.
                // 
                // �X�v���C�g���쐬���܂��B
                SpriteScorePopup sprite = CreateSpriteScore(panel, ScoreDouble);

                // Adds the sprite to the array.
                // 
                // �z��ɒǉ����܂��B
                scorePopupList.Add(sprite);
            }
        }


        /// <summary>
        /// Creates and returns the score sprite.
        /// 
        /// �X�R�A�̃X�v���C�g���쐬���ĕԂ��܂��B
        /// </summary>
        private SpriteScorePopup CreateSpriteScore(PanelData panel, int score)
        {
            SpriteScorePopup sprite = new SpriteScorePopup(Game);
            sprite.Initialize();

            // Sets the score initial position. 
            // 
            // �X�R�A�̏����ʒu��ݒ肵�܂��B
            sprite.Position = panel.Center + panelManager.DrawOffset;
            sprite.DefaultPosition = sprite.Position;

            // Sets the score target position.
            // 
            // �X�R�A�̈ړ����ݒ肵�܂��B
            Vector2 target;
            target = positions[PositionList.Score] + new Vector2(128, 32);
            sprite.TargetPosition = target;

            // Sets the score.
            // 
            // �X�R�A�̐ݒ�����܂��B
            sprite.Score = score;

            return sprite;
        }


        /// <summary>
        /// Creates and returns the component for the specified style.
        /// 
        /// �w�肵���X�^�C���̃R���|�[�l���g���쐬���ĕԂ��܂��B
        /// </summary>
        private StyleBase CreateStyleComponent()
        {
            return CreateStyleComponent(setting, cursor, panelManager);
        }


        /// <summary>
        /// Creates and returns the component for the specified style.
        /// 
        /// �w�肵���X�^�C���̃R���|�[�l���g���쐬���ĕԂ��܂��B
        /// </summary>
        private StyleBase CreateStyleComponent(
            StageSetting setting, Point cursor, PanelManager manager)
        {
            StyleBase component = null;

            if (setting.Style == StageSetting.StyleList.Change)
            {
                component = new ChangeComponent(Game, setting, cursor, manager);
            }
            else if (setting.Style == StageSetting.StyleList.Revolve)
            {
                component = new RevolveComponent(Game, setting, cursor, manager);
            }
            else if (setting.Style == StageSetting.StyleList.Slide)
            {
                component = new SlideComponent(Game, setting, cursor, manager);
            }

            return component;
        }

        
        /// <summary>
        /// Creates and returns the movie component.
        /// 
        /// ���[�r�[�R���|�[�l���g���쐬���ĕԂ��܂��B
        /// </summary>
        private PuzzleAnimation CreateMovie(AnimationInfo info)
        {
            return PuzzleAnimation.CreateAnimationComponent(Game, info);
        }


        /// <summary>
        /// Checks the pause.
        /// 
        /// �|�[�Y�̃`�F�b�N�����܂��B
        /// </summary>
        private bool CheckPause()
        {
            VirtualPadState virtualPad =
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            // Processing is performed only when start has been pressed. 
            // 
            // �X�^�[�g��������Ă��Ȃ��ꍇ�͏������s���܂���B
            if (!buttons.Start[VirtualKeyState.Push])
                return false;

            // Start has been pressed; Pause flag is set.
            // 
            // �X�^�[�g��������Ă����̂ŁA�|�[�Y�̃t���O��ݒ肵�܂��B
            isPause = true;

            // Initializes the pause screen cursor.
            // 
            // �|�[�Y��ʂ̃J�[�\���̏����ݒ�����܂��B
            pauseCursor = PauseCursor.ReturnToGame;

            // Sets the panel selection in the background so that it will not move.
            //
            // ���Ńp�l���I���������Ȃ��悤�ɐݒ肵�܂��B
            style.SelectEnabled = false;

            // Rests the sequence displayed on the pause screen.
            // 
            // �|�[�Y��ʂɕ\������V�[�P���X�����Z�b�g���܂��B
            seqRefStart.Replay();
            seqRefLoop.Replay();
            seqRefNaviStart.Replay();
            seqRefNaviLoop.Replay();
            seqRefReturnToGameOn.Replay();
            seqRefGoToTitleOn.Replay();

            // Sets the Navigate button.
            // 
            // �i�r�Q�[�g�{�^���̐ݒ���s���܂��B
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("B_Cancel"), false));
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));

            return true;
        }


        /// <summary>
        /// Checks the time limit.
        /// 
        /// �^�C�����~�b�g�̃`�F�b�N���s���܂��B
        /// </summary>
        private bool IsTimeLimit()
        {
            // The time limit judgment is performed only in Normal Mode.
            // 
            // �m�[�}�����[�h�ȊO�ł̓^�C�����~�b�g�̔�����s���܂���B
            if (setting.Mode != StageSetting.ModeList.Normal)
                return false;

            return (playTime >= setting.TimeLimit);
        }


        /// <summary>
        /// Uses the help item.
        /// 
        /// �w���v�A�C�e�����g�p���܂��B
        /// </summary>
        private bool UseHelpItem()
        {
            // Processing is not performed if no items are remaining.
            // 
            // �A�C�e���̎c�����������Ώ��������܂���B
            if (helpItemCount <= 0)
                return false;

            // Processing is not performed while items are in use.
            // 
            // �A�C�e���g�p���Ȃ珈�������܂���B
            if (helpItemTime > TimeSpan.Zero)
                return false;

            // Sets the item usage time.
            // 
            // �A�C�e���̎g�p���Ԃ�ݒ肵�܂��B
            helpItemTime = HelpItemTime;

            // In Normal Mode, reduces the usage repetitions.
            // 
            // �m�[�}�����[�h�Ȃ�g�p�񐔂����炵�܂��B
            if (setting.Mode == StageSetting.ModeList.Normal)
                helpItemCount--;

            return true;
        }
        #endregion
    }
}


