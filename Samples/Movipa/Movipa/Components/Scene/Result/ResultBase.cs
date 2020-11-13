#region File Description
//-----------------------------------------------------------------------------
// ResultBase.cs
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

using Movipa.Components.Input;
using Movipa.Util;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Result
{
    /// <summary>
    /// Conducts basic result processing. This class is inherited.
    /// This class loads common assets, and implements update and draw 
    /// functions. Fade processing is performed in the Update method, while
    /// main update processing is written in the inherited UpdateMain. 
    /// The text string used for drawing is defined in the inherited
    /// GetSequenceString and drawn with DrawSequenceString.
    ///
    /// ���U���g�̊�{�������s���܂��B���̃N���X�͌p�����ė��p���܂��B
    /// ���̃N���X�ł́A���ʂ̃A�Z�b�g��ǂݍ��݁A�X�V�ƕ`����s���@�\��
    /// �������Ă��܂��B�t�F�[�h�̏�����Update���\�b�h���ōs���A
    /// ���C���̍X�V�����͌p�����UpdateMain�ɋL�q���܂��B
    /// �`��Ɏg�p���镶����͌p�����GetSequenceString�Őݒ肵�A
    /// DrawSequenceString�ŕ`�悵�Ă��܂��B
    /// </summary>
    public class ResultBase : SceneComponent
    {
        #region Private Types
        /// <summary>
        /// Processing status
        /// 
        /// �������
        /// </summary>
        protected enum Phase
        {
            /// <summary>
            /// Start animation in progress
            /// 
            /// �J�n�A�j���[�V������
            /// </summary>
            Start,

            /// <summary>
            /// Selected status after animation finishes
            /// 
            /// �A�j���[�V�����I����̑I�����
            /// </summary>
            Select
        }
        #endregion


        #region Fields
        // Processing status
        // 
        // �������
        protected Phase phase;

        // Stage completion result
        // 
        // �X�e�[�W�̃N���A����
        protected StageResult result;

        // BackgroundMusic cue
        // 
        // BackgroundMusic�̃L���[
        protected Cue bgm;

        // Camera
        // 
        // �J����
        protected readonly Vector3 cameraPosition;
        protected readonly Vector3 cameraLookAt;

        // Background sphere model data
        // 
        // �w�i�̋��̃��f���f�[�^
        protected BasicModelData[] spheres;

        // Background texture 
        // 
        // �w�i�e�N�X�`��
        protected Texture2D wallpaperTexture;

        // Layout
        protected SceneData sceneData;
        protected SequencePlayData seqStart;
        protected SequencePlayData seqPosition;

        // Navigate button draw flag
        // 
        // �i�r�Q�[�g�{�^���̕`��t���O
        protected bool drawNavigate;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public ResultBase(Game game, StageResult stageResult)
            : base(game)
        {
            result = stageResult;

            cameraPosition = new Vector3(0.0f, 0.0f, 200.0f);
            cameraLookAt = Vector3.Zero;
        }


        /// <summary>
        /// Performs initialization processing.
        /// 
        /// �������������s���܂��B
        /// </summary>
        public override void Initialize()
        {
            // Sets the initial processing status.
            // 
            // �����̏�����Ԃ�ݒ肵�܂��B
            phase = Phase.Start;

            // Sets the Navigate button draw status.
            // 
            // �i�r�Q�[�g�{�^���̕`���Ԃ�ݒ�
            drawNavigate = false;

            // Sets the Fade-in.
            // 
            // �t�F�[�h�C���̐ݒ���s���܂��B
            GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeIn);

            // Plays the BackgroundMusic and obtains the Cue.
            // 
            // BackgroundMusic���Đ����ACue���擾���܂��B
            bgm = GameData.Sound.PlayBackgroundMusic(Sounds.GameClearBackgroundMusic);

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
            // �w�i�e�N�X�`����ǂݍ��݂܂��B
            string asset = "Textures/Wallpaper/Wallpaper_005";
            wallpaperTexture = Content.Load<Texture2D>(asset);

            // Loads and sets the sphere model.
            // 
            // ���̃��f���̓ǂݍ��݂Ɛݒ�����܂��B
            spheres = new BasicModelData[2];
            spheres[0] = new BasicModelData(Content.Load<Model>("Models/sphere01"));
            spheres[0].Scale = 0.9f;
            spheres[1] = new BasicModelData(Content.Load<Model>("Models/sphere02"));
            spheres[1].Scale = 0.88f;

            base.LoadContent();
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
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            // Updates the sphere model.
            // 
            // ���̃��f���̍X�V�������s���܂��B
            UpdateModels();

            if (GameData.FadeSeqComponent.FadeMode == FadeMode.FadeIn)
            {
                // Changes the processing status after the fade-in finishes.
                // 
                // �t�F�[�h�C�����I��������A������Ԃ�ύX���܂��B
                if (!GameData.FadeSeqComponent.IsPlay)
                {
                    GameData.FadeSeqComponent.FadeMode = FadeMode.None;
                }
            }
            else if (GameData.FadeSeqComponent.FadeMode == FadeMode.None)
            {
                // Performs main update processing.
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

            // Updates the sequence except during fade-in.
            // 
            // �t�F�[�h�C���ȊO�ŁA�V�[�P���X�̍X�V�������s���܂��B
            if (GameData.FadeSeqComponent.FadeMode != FadeMode.FadeIn)
            {
                UpdateSequence(gameTime);
            }


            base.Update(gameTime);
        }


        /// <summary>
        /// Performs update except during a fade.
        /// 
        /// �t�F�[�h�������ȊO�̍X�V�������s���܂��B
        /// </summary>
        protected virtual void UpdateMain(GameTime gameTime)
        {
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
            // �t�F�[�h�A�E�g���I��������A�J���������s���܂��B
            if (!GameData.FadeSeqComponent.IsPlay)
            {
                Dispose();
            }
        }


        /// <summary>
        /// Updates the model.
        /// 
        /// ���f���̍X�V�������s���܂��B
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateModels()
        {
            Vector3 rotate;

            rotate = spheres[0].Rotate;
            rotate.Y += MathHelper.ToRadians(0.1f);
            spheres[0].Rotate = rotate;

            rotate = spheres[1].Rotate;
            rotate.Y -= MathHelper.ToRadians(0.03f);
            spheres[1].Rotate = rotate;
        }


        /// <summary>
        /// Updates the sequence.
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        private void UpdateSequence(GameTime gameTime)
        {
            seqStart.Update(gameTime.ElapsedGameTime);
            seqPosition.Update(gameTime.ElapsedGameTime);
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
            // Draws the background.
            // 
            // �w�i�̕`����s���܂��B
            Batch.Begin();
            Batch.Draw(wallpaperTexture, Vector2.Zero, Color.White);
            Batch.End();

            // Clears the depth buffer and draws the sphere model.
            // 
            // �[�x�o�b�t�@���N���A���A���̃��f����`�悵�܂��B
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSpheres();

            Batch.Begin();

            // Draws the sequence.
            // 
            // �V�[�P���X��`�悵�܂��B
            seqStart.Draw(Batch, null);

            // Draws the text string.
            // 
            // �������`�悵�܂��B
            DrawSequenceString();

            // Draws the Navigate button.
            // 
            // �i�r�Q�[�g�{�^����`�悵�܂��B
            if (drawNavigate)
            {
                DrawNavigate(gameTime, false);
            }

            Batch.End();

            base.Draw(gameTime);
        }

        
        /// <summary>
        /// Draws the cursor sphere.
        /// 
        /// �J�[�\���̋��̂�`�悵�܂��B
        /// </summary>
        private void DrawSpheres()
        {
            GraphicsDevice graphicsDevice = GraphicsDevice;
            Matrix view;
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
            for (int i = 0; i < spheres.Length; i++)
            {
                spheres[i].SetRenderState(graphicsDevice, SpriteBlendMode.Additive);
                spheres[i].Draw(view, GameData.Projection);
            }
        }


        /// <summary>
        /// Draws the sequence text string.
        /// 
        /// �V�[�P���X�̕������`�悵�܂��B
        /// </summary>
        private void DrawSequenceString()
        {
            SpriteFont font = LargeFont;
            SequenceBankData sequenceBank = seqPosition.SequenceData;

            for (int i = 0; i < sequenceBank.SequenceGroupList.Count; i++)
            {
                SequenceGroupData seqBodyData = sequenceBank.SequenceGroupList[i];
                SequenceObjectData seqPartsData = seqBodyData.CurrentObjectList;

                // Processing is skipped if the parts data cannot be obtained.
                // 
                // �p�[�c�f�[�^���擾�ł��Ȃ��ꍇ�͏������X�L�b�v���܂��B
                if (seqPartsData == null)
                {
                    continue;
                }

                List<PatternObjectData> patternObjects = seqPartsData.PatternObjectList;
                foreach (PatternObjectData patPartsData in patternObjects)
                {
                    DrawData putInfoData = patPartsData.InterpolationDrawData;
                    Color color = putInfoData.Color;
                    Point point = putInfoData.Position;
                    Vector2 position = new Vector2(point.X, point.Y);

                    // Obtains the drawn text string.
                    // 
                    // �`�敶������擾���܂��B
                    string text = GetSequenceString(i);

                    // Sets the draw position to right aligned.
                    // 
                    // �`��ʒu���E�񂹂ɐݒ肵�܂��B
                    position.X -= font.MeasureString(text).X;

                    Batch.DrawString(font, text, position, color);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns the text string to display in the sequence.
        /// 
        /// �V�[�P���X�ɕ\�����镶�����Ԃ��܂��B
        /// </summary>
        protected virtual string GetSequenceString(int id)
        {
            return String.Empty;
        }
        #endregion

    }
}


