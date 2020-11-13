#region File Description
//-----------------------------------------------------------------------------
// FadeSeqComponent.cs
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
using SceneDataLibrary;
#endregion

namespace Movipa.Components
{
    #region Public Types
    /// <summary>
    /// Fade status
    /// 
    /// �t�F�[�h���
    /// </summary>
    public enum FadeMode
    {
        /// <summary>
        /// Fade-in
        /// 
        /// �t�F�[�h�C��
        /// </summary>
        FadeIn,

        /// <summary>
        /// Fade-out
        /// 
        /// �t�F�[�h�A�E�g
        /// </summary>
        FadeOut,

        /// <summary>
        /// Stop fade
        /// 
        /// �t�F�[�h��~
        /// </summary>
        None
    }

    /// <summary>
    /// Fade type
    /// 
    /// �t�F�[�h���
    /// </summary>
    public enum FadeType
    {
        /// <summary>
        /// Normal fade
        /// 
        /// �ʏ�t�F�[�h
        /// </summary>
        Normal,
        
        /// <summary>
        /// Rectangle fade
        /// 
        /// ��`�t�F�[�h
        /// </summary>
        RotateBox,
        
        /// <summary>
        /// Gonzales fade
        /// 
        /// �S���U���X�t�F�[�h
        /// </summary>
        Gonzales,
    }
    #endregion

    /// <summary>
    /// The component that draws the fade.
    /// The fade animation involves loading the Layout 
    /// sequence and managing and drawing the in and out separately.
    /// To add more fade types, first add animations to the sequence
    /// used for fade processing, then add the Fadetype item, then
    /// load the corresponding animation. 
    /// 
    /// �t�F�[�h�̕`�������R���|�[�l���g�ł��B
    /// �t�F�[�h�̃A�j���[�V�����ɂ�Layout�̃V�[�P���X��ǂݍ��݁A
    /// �C���ƃA�E�g��ʂɊǗ����ĕ`�悵�Ă��܂��B
    /// �t�F�[�h�̎�ނ𑝂₵�����ꍇ�́A���O�Ƀt�F�[�h�Ɏg�p����
    /// �V�[�P���X�ɃA�j���[�V������ǉ����AFadeType�̍��ڂ�ǉ����A
    /// �Ή�����A�j���[�V������ǂݍ��݂܂��B
    /// </summary>
    public class FadeSeqComponent : SceneComponent
    {
        #region Private Types
        /// <summary>
        /// Fade-in sequence name
        /// 
        /// �t�F�[�h�C���̃V�[�P���X��
        /// </summary>
        private const string SeqFadeInName = "FadeIn";

        /// <summary>
        /// Fade-out sequence name
        /// 
        /// �t�F�[�h�A�E�g�̃V�[�P���X��
        /// </summary>
        private const string SeqFadeOutName = "FadeOut";
        #endregion

        #region Fields
        private Dictionary<FadeType, SceneData> sceneList;
        private Dictionary<FadeType, Dictionary<FadeMode, SequencePlayData>> seqList;
        private SequencePlayData curSeqData = null;
        private FadeMode fadeMode = FadeMode.None;
        private float count = 0.0f;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the fade status.
        /// 
        /// �t�F�[�h��Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public FadeMode FadeMode
        {
            get { return fadeMode; }
            set { fadeMode = value; }
        }


        /// <summary>
        /// Obtains the fade count.
        /// 
        /// �t�F�[�h�J�E���g���擾���܂��B
        /// </summary>
        public float Count
        {
            get { return count; }
        }


        /// <summary>
        /// Obtains the playback status.
        /// 
        /// �Đ���Ԃ��擾���܂��B
        /// </summary>
        public bool IsPlay
        {
            get { return curSeqData.IsPlay; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public FadeSeqComponent(Game game)
            : base(game)
        {
        }


        /// <summary>
        /// Executes content load processing.
        /// 
        /// �R���e���g�̓ǂݍ��ݏ��������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            sceneList = new Dictionary<FadeType, SceneData>();
            seqList = new Dictionary<FadeType, Dictionary<FadeMode, SequencePlayData>>();

            // Loads the sequence data.
            // 
            // �V�[�P���X�f�[�^��ǂݍ��݂܂��B
            addFadeScene(FadeType.Normal, "Layout/Fade/Normal_Scene");
            addFadeScene(FadeType.RotateBox, "Layout/Fade/RotateBox_Scene");
            addFadeScene(FadeType.Gonzales, "Layout/Fade/Gonzales_Scene");

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the sequence. 
        /// 
        /// �V�[�P���X�̍X�V�������s���܂��B
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (curSeqData != null)
            {
                curSeqData.Update(gameTime.ElapsedGameTime);
                count += 1.0f;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the sequence.
        /// 
        /// �V�[�P���X�̕`�揈�����s���܂��B
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (curSeqData != null)
            {
                Batch.Begin();
                curSeqData.Draw(Batch, null);
                Batch.End();
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Starts the fade processing.
        /// 
        /// �t�F�[�h�������J�n���܂��B
        /// </summary>
        /// <param name="type">Fade Type</param>
        ///
        /// <param name="type">�t�F�[�h�̎��</param>

        /// <param name="mode">Fade Status</param>
        /// 
        /// <param name="mode">�t�F�[�h�̏��</param>
        public void Start(FadeType type, FadeMode mode)
        {
            // No processing is performed when Stop status is specified.
            // 
            // ��Ԃ���~�Ŏw�肳�ꂽ�ꍇ�͏������s���܂���B
            if (mode == FadeMode.None)
            {
                return;
            }

            // Sets the current fade status.
            // 
            // ���݂̃t�F�[�h��Ԃ�ݒ肵�܂��B
            FadeMode = mode;

            // Initializes the count. 
            // 
            // �J�E���g�����������܂��B
            count = 0.0f;

            // Replaces the sequence with the specified one.
            // 
            // �V�[�P���X���w��̂��̂ɍ����ւ��܂��B
            curSeqData = seqList[type][mode];
            curSeqData.SequenceData.SequenceGroupList[0].Replay();

            // Updates the first frame once.
            // 
            // �ŏ��̃t���[���Ɉ�x�X�V���܂��B
            curSeqData.Update(new TimeSpan());
        }


        /// <summary>
        /// Adds the specified sequence to the array.
        /// 
        /// �w�肳�ꂽ�V�[�P���X��z��ɒǉ����܂��B
        /// </summary>
        /// <param name="type">Fade Type</param>
        ///  
        /// <param name="type">�t�F�[�h�^�C�v</param>
        /// <param name="asset">Sequence Asset Name</param>
        ///  
        /// <param name="asset">�V�[�P���X�̃A�Z�b�g��</param>
        private void addFadeScene(FadeType type, string asset)
        {
            // Loads the scene data.
            // 
            // �V�[���f�[�^��ǂݍ��݂܂��B
            SceneData scene = Content.Load<SceneData>(asset);
            sceneList.Add(type, scene);

            // Loads the sequence from the scene data.
            // 
            // �V�[���f�[�^����V�[�P���X��ǂݍ��݂܂��B
            Dictionary<FadeMode, SequencePlayData> fadeList = 
                new Dictionary<FadeMode, SequencePlayData>();
            fadeList.Add(FadeMode.FadeIn, scene.CreatePlaySeqData(SeqFadeInName));
            fadeList.Add(FadeMode.FadeOut, scene.CreatePlaySeqData(SeqFadeOutName));
            seqList.Add(type, fadeList);
        }
        #endregion
    }
}


