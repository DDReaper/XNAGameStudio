#region File Description
//-----------------------------------------------------------------------------
// SelectMovie.cs
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
using Microsoft.Xna.Framework.Graphics;

using Movipa.Components.Input;
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Menu item for processing movie selection.
    /// It inherits MenuBase and expands menu compilation processing.
    /// This class uses threads for asynchronous loading of movies and for 
    /// loading of thumbnail images. A loading status icon is displayed 
    /// during asynchronous movie loading.
    /// 
    /// ���[�r�[�I�����������郁�j���[���ڂł��B
    /// MenuBase���p�����A���j���[���\�����鏈�����g�����Ă��܂��B
    /// ���̃N���X�́A�X���b�h���g�p���Ĕ񓯊��Ń��[�r�[�̓ǂݍ��݂ƁA
    /// �T���l�C���摜�̓ǂݍ��݂��s���Ă��܂��B
    /// �񓯊��Ń��[�r�[��ǂݍ���ł��鎞�́A���̏�Ԃ������A�C�R����
    /// �`�悵�Ă��܂��B
    /// </summary>
    public class SelectMovie : MenuBase
    {
        #region Private Types
        /// <summary>
        /// Processing status
        /// 
        /// �������
        /// </summary>
        private enum Phase
        {
            /// <summary>
            /// Initialization
            /// 
            /// ������
            /// </summary>
            Loading,

            /// <summary>
            /// Start
            ///
            /// �J�n���o
            /// </summary>
            Start,

            /// <summary>
            /// Select
            ///
            /// �I��
            /// </summary>
            Select,

            /// <summary>
            /// Selected
            ///
            /// �I�����o
            /// </summary>
            Selected,
        }
        #endregion

        #region Fields
        // Thumbnail size
        // 
        // �T���l�C���̃T�C�Y
        private readonly Vector2 ThumbnailSize;

        // Number of panels displaying thumbnails
        // 
        // �T���l�C����\������p�l����
        private const int ThumbnailPanel = 7;

        // Movie draw position
        // 
        // ���[�r�[��`�悷��ʒu
        private readonly Vector2 PositionThumbnail;
        private readonly Rectangle MoviePreviewRect;

        // Processing details
        //
        // �������e
        private Phase phase;

        // Cursor position
        //
        // �J�[�\���ʒu
        private int cursor;

        // Sequence
        //
        // �V�[�P���X
        private SequencePlayData seqStart;
        private SequencePlayData seqLoop;
        private SequencePlayData seqMovieWindow;
        private SequencePlayData seqMovieSelect;
        private SequencePlayData seqLeft;
        private SequencePlayData seqRight;
        private SequencePlayData seqLoading;
        private SequencePlayData seqPosMovieTitle;
        private SequencePlayData seqPosMovieCount;

        // Thumbnail texture list
        // 
        // �T���l�C���̃e�N�X�`�����X�g
        private List<Texture2D> thumbTextures;

        // Thumbnail sprite list 
        //
        // �T���l�C���̃X�v���C�g���X�g
        private List<ThumbnailSprite> thumbSprite;

        // CPU core for load processing
        //
        // �ǂݍ��ݏ�����������CPU�R�A
        private int cpuId;

        // Thumbnail loading class
        // 
        // �T���l�C����ǂݍ��ރN���X
        private ThumbnailLoader thumbLoader;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SelectMovie(Game game, MenuData data)
            : base(game, data)
        {
            // Loads the position.
            // 
            // �|�W�V������ǂݍ��݂܂��B
            PatternGroupData patternGroup = 
                data.sceneData.PatternGroupDictionary["Pos_PosMovie"];
            Point point;
            point = patternGroup.PatternObjectList[0].Position;
            MoviePreviewRect = new Rectangle(point.X, point.Y, 640, 360);

            point = patternGroup.PatternObjectList[1].Position;
            PositionThumbnail = new Vector2(point.X, point.Y);

            // Sets the thumbnail size.
            //
            // �T���l�C���̃T�C�Y��ݒ肵�܂��B
            ThumbnailSize = new Vector2(256.0f, 144.0f);
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������̏������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Initializes the processing status.
            // 
            // ������Ԃ̏����ݒ�����܂��B
            phase = Phase.Loading;

            // Initializes the sequence.
            //
            // �V�[�P���X�̏��������s���܂��B
            InitializeSequence();

            // Creates a thumbnail list for loading.
            // 
            // �ǂݍ��ރT���l�C���̃��X�g���쐬���܂��B
            thumbTextures = new List<Texture2D>();
            List<string> thumbAssetList = new List<string>();
            foreach (string infoPath in GameData.MovieList)
            {
                // Obtains the path to Info, then replaces the asset name 
                // with the Thumbnail and adds it to the list.
                //
                // Info�܂ł̃p�X���擾���A�A�Z�b�g����Thumbnail��
                // �u�������ă��X�g�ɒǉ����܂��B
                int length = infoPath.LastIndexOf("/");
                string path = infoPath.Substring(0, length);
                string asset = path + "/Thumbnail";
                thumbAssetList.Add(asset);
            }

            // Begins asynchronous thumbnail loading.
            // 
            // �T���l�C���̔񓯊��ǂݍ��݂��J�n���܂��B
            thumbLoader = new ThumbnailLoader(Game, 3, thumbAssetList.ToArray());
            thumbLoader.Run();

            // Creates thumbnail sprite.
            //
            // �T���l�C���̃X�v���C�g���쐬���܂��B
            thumbSprite = new List<ThumbnailSprite>();
            for (int i = 0; i < ThumbnailPanel; i++)
            {
                ThumbnailSprite sprite = new ThumbnailSprite(Game);

                sprite.Id = i;
                sprite.Position = PositionThumbnail;

                Vector2 target = PositionThumbnail;
                target.X += ThumbnailSize.X * (i - (ThumbnailPanel >> 1));
                sprite.TargetPosition = target;

                // Coordinate correction 
                // 
                // ���W�␳
                sprite.Position -= ThumbnailSize * 0.5f;
                sprite.TargetPosition -= ThumbnailSize * 0.5f;

                // Sets initial texture.
                // 
                // �����e�N�X�`���̐ݒ�
                int textureId = cursor + i;
                textureId += GameData.MovieList.Count - (ThumbnailPanel >> 1);
                textureId %= GameData.MovieList.Count;
                sprite.TextureId = textureId;

                thumbSprite.Add(sprite);
            }

            // Starts movie loading.
            // 
            // ���[�r�[�̓ǂݍ��ݏ������J�n���܂��B
            SetMovie(false);

            base.Initialize();
        }


        /// <summary>
        /// Initializes the navigate.
        /// 
        /// �i�r�Q�[�g�̏����������܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("B_Cancel"), false));
            Navigate.Add(new NavigateData(AppSettings("A_Ok"), true));
        }


        /// <summary>
        /// Initializes the sequence.
        /// 
        /// �V�[�P���X�̏��������s���܂��B
        /// </summary>
        private void InitializeSequence()
        {
            seqStart = Data.sceneData.CreatePlaySeqData("MovieStart");
            seqLoop = Data.sceneData.CreatePlaySeqData("MovieLoop");
            seqMovieWindow = Data.sceneData.CreatePlaySeqData("MovieWindow");
            seqMovieSelect = Data.sceneData.CreatePlaySeqData("MovieSelect");
            seqLeft = Data.sceneData.CreatePlaySeqData("Left");
            seqRight = Data.sceneData.CreatePlaySeqData("Right");
            seqPosMovieTitle = Data.sceneData.CreatePlaySeqData("PosMovieTitle");
            seqPosMovieCount = Data.sceneData.CreatePlaySeqData("PosMovieCount");
            seqLoading = Data.sceneData.CreatePlaySeqData("Loading");

            seqStart.Replay();
            seqPosMovieTitle.Replay();
            seqPosMovieCount.Replay();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs update processing.
        /// 
        /// �X�V�������s���܂��B
        /// </summary>
        public override MenuBase UpdateMain(GameTime gameTime)
        {
            // Updates the sequence.
            // 
            // �V�[�P���X�̍X�V
            UpdateSequence(gameTime);

            if (phase == Phase.Loading)
            {
                // Performs update during loading.
                // 
                // �ǂݍ��ݒ��̍X�V�������s���܂��B
                return UpdateLoading();
            }
            else if (phase == Phase.Start)
            {
                // Waits until start animation has finished.
                // 
                // �J�n�A�j���[�V�������I������܂őҋ@
                if (!seqStart.SequenceData.IsPlay)
                {
                    // To movie selection
                    // 
                    // ���[�r�[�I��������
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                // Performs update during selection.
                // 
                // �I�𒆂̍X�V�������s���܂��B
                return UpdateSelect();
            }
            else if (phase == Phase.Selected)
            {
                // Waits until selected animation has finished.
                // 
                // �I���A�j���[�V�������I������܂őҋ@
                if (!seqMovieSelect.SequenceData.IsPlay)
                {
                    // To division setting
                    // 
                    // �������ݒ��
                    return CreateMenu(Game, MenuType.SelectDivide, Data);
                }
            }



            return null;
        }


        /// <summary>
        /// Performs update during loading.
        /// 
        /// �ǂݍ��ݒ��̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateLoading()
        {
            // Waits until thumbnail and movie loading has finished.
            // 
            // �T���l�C���ƃ��[�r�[�̓ǂݍ��ݏ������I������܂őҋ@���܂��B
            if (thumbTextures == null || !thumbLoader.Initialized)
                return null;

            // Obtains the texture list.
            //
            // �e�N�X�`�����X�g���擾���܂��B
            thumbTextures = thumbLoader.Textures;

            // Sets the texture for the thumbnail sprite.
            //
            // �T���l�C���̃X�v���C�g�Ƀe�N�X�`�����Z�b�g���܂��B
            SetThumbnailTexture();

            // Replays the sequence animation.
            //
            // �V�[�P���X�̃A�j���[�V���������v���C���܂��B
            seqStart.Replay();

            // Sets the processing status to the start animation.
            //
            // ������Ԃ��J�n�A�j���[�V�����ɐݒ肵�܂��B
            phase = Phase.Start;

            return null;
        }


        /// <summary>
        /// Performs update during selection.
        /// 
        /// �I�𒆂̍X�V�������s���܂��B
        /// </summary>
        private MenuBase UpdateSelect()
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;
            VirtualPadDPad dPad = virtualPad.DPad;
            VirtualPadDPad leftStick = virtualPad.ThumbSticks.Left;

            if (buttons.A[VirtualKeyState.Push])
            {
                // Performs Enter key processing.
                // 
                // ����L�[�������ꂽ�Ƃ��̏������s���܂��B
                InputSelectKey();
            }
            else if (buttons.B[VirtualKeyState.Push])
            {
                GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCancel);
                return CreateMenu(Game, MenuType.SelectStyle, Data);
            }
            else if (InputState.IsPush(dPad.Left, leftStick.Left))
            {
                InputLeftKey();
            }
            else if (InputState.IsPush(dPad.Right, leftStick.Right))
            {
                InputRightKey();
            }
            
            return null;
        }


        /// <summary>
        /// Performs Enter key processing.
        /// 
        /// ����L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputSelectKey()
        {
            Data.StageSetting.Movie = GameData.MovieList[cursor];
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);

            seqMovieSelect.Replay();
            phase = Phase.Selected;
        }


        /// <summary>
        /// Performs Left key processing.
        /// 
        /// ���L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputLeftKey()
        {
            // Sets the previous cursor position.
            // 
            // �O�̃J�[�\���ʒu�ɐݒ肵�܂��B
            cursor = CursorPrev();

            // Sets the thumbnail.
            //
            // �T���l�C����ݒ肵�܂��B
            SetPreviousThumbnail();

            // Plays the cursor movement SoundEffect.
            //
            // �J�[�\���ړ���SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            seqLeft.Replay();
        }


        /// <summary>
        /// Performs Right key processing.
        /// 
        /// �E�L�[�������ꂽ�Ƃ��̏������s���܂��B
        /// </summary>
        private void InputRightKey()
        {
            // Sets the next cursor position.
            // 
            // ���̃J�[�\���ʒu�ɐݒ肵�܂��B
            cursor = CursorNext();

            // Sets the thumbnail.
            // 
            // �T���l�C����ݒ肵�܂��B
            SetNextThumbnail();

            // Places the cursor movement SoundEffect.
            // 
            // �J�[�\���ړ���SoundEffect���Đ����܂��B
            GameData.Sound.PlaySoundEffect(Sounds.SoundEffectCursor1);

            // Replays the sequence.
            // 
            // �V�[�P���X�����v���C���܂��B
            seqRight.Replay();
        }


        /// <summary>
        /// Updates the sequence.
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSequence(GameTime gameTime)
        {
            // Updates the sequence except during loading.
            // 
            // �ǂݍ��ݒ��ȊO�̏�ʂŁA�V�[�P���X���X�V���܂��B
            if (phase != Phase.Loading)
            {
                seqStart.Update(gameTime.ElapsedGameTime);
                seqLoop.Update(gameTime.ElapsedGameTime);
                seqMovieWindow.Update(gameTime.ElapsedGameTime);
                seqMovieSelect.Update(gameTime.ElapsedGameTime);
                seqLeft.Update(gameTime.ElapsedGameTime);
                seqRight.Update(gameTime.ElapsedGameTime);
                seqPosMovieTitle.Update(gameTime.ElapsedGameTime);
                seqPosMovieCount.Update(gameTime.ElapsedGameTime);

                foreach (Sprite sprite in thumbSprite)
                {
                    sprite.Update(gameTime);
                }
            }

            seqLoading.Update(gameTime.ElapsedGameTime);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Performs render processing.
        /// 
        /// �`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            // Draws the movie window.
            // 
            // ���[�r�[�̃E�B���h�E��`�悵�܂��B
            batch.Begin();
            seqMovieWindow.Draw(batch, null);
            batch.End();

            // Draws the movie.
            // 
            // ���[�r�[��`�悵�܂��B
            DrawMovie(batch);

            batch.Begin();

            // Draws the thumbnail.
            //
            // �T���l�C����`�悵�܂��B
            DrawThumbnail(gameTime, batch);

            // Draws the sequence.
            //
            // �V�[�P���X��`�悵�܂��B
            DrawSequence(gameTime, batch);

            batch.End();
        }


        /// <summary>
        /// Draws the movie.
        /// 
        /// ���[�r�[��`�悵�܂��B
        /// </summary>
        private void DrawMovie(SpriteBatch batch)
        {
            // Drawing is not performed during loading.
            // 
            // �ǂݍ��ݒ��͕`�揈�����s���܂���B
            if (phase == Phase.Loading)
                return;

            if (Data.movieTexture != null)
            {
                // Draws the movie with no alpha.
                // 
                // ���[�r�[���A���t�@�l�����ŕ`�悵�܂��B
                batch.Begin(SpriteBlendMode.None);
                batch.Draw(Data.movieTexture, MoviePreviewRect, Color.White);
                batch.End();
            }
            else
            {
                // Draws the load icon if the movie has not been
                // loaded.
                // 
                // ���[�r�[���܂��ǂݍ��܂�Ă��Ȃ��ꍇ��
                // ���[�h�A�C�R����`�悵�܂��B
                batch.Begin();
                seqLoading.Draw(batch, null);
                batch.End();
            }
        }


        /// <summary>
        /// Draws the thumbnail.
        /// 
        /// �T���l�C����`�悵�܂��B
        /// </summary>
        private void DrawThumbnail(GameTime gameTime, SpriteBatch batch)
        {
            foreach (Sprite sprite in thumbSprite)
            {
                sprite.Draw(gameTime, batch);
            }
        }


        /// <summary>
        /// Draws the sequence.
        /// 
        /// �V�[�P���X��`�悵�܂��B
        /// </summary>
        private void DrawSequence(GameTime gameTime, SpriteBatch batch)
        {
            if (phase == Phase.Start)
            {
                seqStart.Draw(batch, null);
            }
            else if (phase == Phase.Select)
            {
                seqLoop.Draw(batch, null);
                seqLeft.Draw(batch, null);
                seqRight.Draw(batch, null);

                // Draws the navigate button.
                // 
                // �i�r�Q�[�g�{�^����`�悵�܂��B
                DrawNavigate(gameTime, batch, false);
            }
            else if (phase == Phase.Selected)
            {
                seqLoop.Draw(batch, null);
                seqMovieSelect.Draw(batch, null);
                seqLeft.Draw(batch, null);
                seqRight.Draw(batch, null);
            }

            // Draws the movie title.
            // 
            // ���[�r�[�̃^�C�g����`�悵�܂��B
            DrawMovieTitle(batch);

            // Draws the movie number. 
            // 
            // ���[�r�[�̔ԍ���`�悵�܂��B
            DrawMovieCount(batch);
        }


        /// <summary>
        /// Draws the movie title.
        /// 
        /// ���[�r�[�̃^�C�g����`�悵�܂��B
        /// </summary>
        private void DrawMovieTitle(SpriteBatch batch)
        {
            // Drawing is not performed if the movie is Null.
            // 
            // ���[�r�[��null�̏ꍇ�͕`�揈�����s���܂���B
            if (Data.movie == null)
                return;


            SequenceBankData seqData = seqPosMovieTitle.SequenceData;
            foreach (SequenceGroupData seqBodyData in seqData.SequenceGroupList)
            {
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;
                if (seqPartsData == null)
                {
                    continue;
                }

                List<PatternObjectData> list = seqPartsData.PatternObjectList;
                foreach (PatternObjectData patPartsData in list)
                {
                    DrawData putInfoData = patPartsData.InterpolationDrawData;
                    SpriteFont font = LargeFont;
                    Color color = putInfoData.Color;
                    string text = Data.movie.Info.Name;

                    // Centers the position.
                    //
                    // �ʒu���Z���^�����O���܂��B
                    Point point = putInfoData.Position;
                    Vector2 position = new Vector2(point.X, point.Y);
                    position -= font.MeasureString(text) * 0.5f;

                    batch.DrawString(font, text, position, color);
                }
            }
        }


        /// <summary>
        /// Draws the movie number.
        /// 
        /// ���[�r�[�̔ԍ���`�悵�܂��B
        /// </summary>
        private void DrawMovieCount(SpriteBatch batch)
        {
            SequenceBankData seqData = seqPosMovieCount.SequenceData;
            foreach (SequenceGroupData seqBodyData in seqData.SequenceGroupList)
            {
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;
                if (seqPartsData == null)
                {
                    continue;
                }

                List<PatternObjectData> list = seqPartsData.PatternObjectList;
                foreach (PatternObjectData patPartsData in list)
                {
                    DrawData putInfoData = patPartsData.InterpolationDrawData;
                    SpriteFont font = LargeFont;
                    Color color = putInfoData.Color;

                    int current = cursor + 1;
                    int total = GameData.MovieList.Count;
                    string format = "{0:00}/{1:00}";
                    string text = string.Format(format, current, total);

                    Point point = putInfoData.Position;
                    Vector2 position = new Vector2(point.X, point.Y);

                    batch.DrawString(font, text, position, color);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Starts loading the movie.
        ///
        /// ���[�r�[�̓ǂݍ��ݏ������J�n���܂��B
        /// </summary>
        /// <param name="join">
        /// Waiting for movie to finish loading
        ///
        /// ���[�r�[�̓ǂݍ��ݏI���҂�
        /// </param>
        private void SetMovie(bool join)
        {
            // Performs release processing if 
            // movie object already exists.
            // 
            // ���Ƀ��[�r�[�I�u�W�F�N�g�����݂��Ă���ꍇ��
            // �J���������s���܂��B
            if (Data.movie != null)
            {
                Data.movie.Dispose();
                Data.movie = null;
            }

            Data.movieTexture = null;

            // Force quits if movie is already loading.
            // 
            // ���[�r�[�����ɓǂݍ��ݒ��Ȃ狭���I�������܂��B
            if (Data.movieLoader != null && !Data.movieLoader.Initialized)
            {
                Data.movieLoader.Abort();

                // Performs release processing.
                // 
                // �J���������s���܂��B
                if (Data.movieLoader.Movie != null)
                {
                    Data.movieLoader.Movie.Dispose();
                }

                Data.movieLoader = null;
            }

            // Loads movie asynchronously. 
            // For Xbox 360, set CPU core to execute thread.
            // 
            // ���[�r�[��񓯊��œǂݍ��݂܂��B
            // Xbox 360�̏ꍇ�̓X���b�h�����s����CPU�̃R�A��ݒ肵�܂��B
            AnimationInfo animationInfo =
                Content.Load<AnimationInfo>(GameData.MovieList[cursor]);
            Data.movieLoader = new MovieLoader(Game, 3 + cpuId, animationInfo);
            Data.movieLoader.Run();

            // Changes the next CPU core to be used.
            //
            // ����g�p����CPU�R�A��ύX���܂��B
            cpuId = (cpuId + 1) % 3;

            // If set to synchronous loading, wait for completion.
            // 
            // �����ǂݍ��݂��ݒ肳��Ă�����I������܂őҋ@���܂��B
            if (join)
            {
                Data.movieLoader.Join();
            }

            // Replays the movie title display sequence.
            // 
            // ���[�r�[�^�C�g���\���̃V�[�P���X�����v���C���܂��B
            if (seqPosMovieTitle != null)
            {
                seqPosMovieTitle.Replay();
            }

        }


        /// <summary>
        /// Shifts the thumbnail one back.
        /// 
        /// �T���l�C������O�Ɉړ����܂��B
        /// </summary>
        private void SetPreviousThumbnail()
        {
            ThumbnailSprite frontSprite = thumbSprite[0];
            ThumbnailSprite tailSprite = thumbSprite[thumbSprite.Count - 1];

            Vector2 frontPosition = frontSprite.TargetPosition;

            tailSprite.Position = tailSprite.TargetPosition =
                frontPosition - new Vector2(ThumbnailSize.X, 0);

            int textureId = cursor;
            textureId += GameData.MovieList.Count - (ThumbnailPanel >> 1);
            textureId %= GameData.MovieList.Count;
            tailSprite.TextureId = textureId;

            thumbSprite.Remove(tailSprite);
            thumbSprite.Insert(0, tailSprite);

            foreach (ThumbnailSprite sprite in thumbSprite)
            {
                Vector2 position = sprite.TargetPosition;
                position.X += ThumbnailSize.X;
                sprite.TargetPosition = position;
            }

            SetThumbnailTexture();

            SetMovie(false);
        }


        /// <summary>
        /// Shifts the thumbnail one forward.
        ///
        /// �T���l�C��������ֈړ����܂��B
        /// </summary>
        private void SetNextThumbnail()
        {
            ThumbnailSprite frontSprite = thumbSprite[0];
            ThumbnailSprite tailSprite = thumbSprite[thumbSprite.Count - 1];

            Vector2 tailPosition = tailSprite.TargetPosition;

            int textureId = cursor;
            textureId += (ThumbnailPanel >> 1) - 1;
            textureId %= GameData.MovieList.Count;
            tailSprite.TextureId = textureId;

            frontSprite.Position = frontSprite.TargetPosition = 
                tailPosition + new Vector2(ThumbnailSize.X, 0);

            thumbSprite.Remove(frontSprite);
            thumbSprite.Add(frontSprite);

            foreach (ThumbnailSprite sprite in thumbSprite)
            {
                Vector2 position = sprite.TargetPosition;
                position.X -= ThumbnailSize.X;
                sprite.TargetPosition = position;
            }

            SetThumbnailTexture();

            SetMovie(false);
        }


        /// <summary>
        /// Sets the texture for the thumbnail sprite.
        /// 
        /// �T���l�C���̃X�v���C�g�Ƀe�N�X�`����ݒ肵�܂��B
        /// </summary>
        private void SetThumbnailTexture()
        {
            // Processing is not performed if there is no texture list.
            // 
            // �e�N�X�`�����X�g�������ꍇ�͏��������܂���B
            if (thumbTextures == null)
            {
                return;
            }

            // Sets the texture.
            // 
            // �e�N�X�`����ݒ肵�܂��B
            foreach (ThumbnailSprite sprite in thumbSprite)
            {
                sprite.Texture = thumbTextures[sprite.TextureId];
            }
        }


        /// <summary>
        /// Returns the previous cursor position.
        /// 
        /// �O�̃J�[�\���ʒu��Ԃ��܂��B
        /// </summary>
        private int CursorPrev()
        {
            return ((cursor - 1) < 0) ? GameData.MovieList.Count - 1 : cursor - 1;
        }


        /// <summary>
        /// Returns the next cursor position.
        ///
        /// ���̃J�[�\���ʒu��Ԃ��܂��B
        /// </summary>
        private int CursorNext()
        {
            return (cursor + 1) % GameData.MovieList.Count;
        }
        #endregion
    }
}
