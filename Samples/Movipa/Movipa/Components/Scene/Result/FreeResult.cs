#region File Description
//-----------------------------------------------------------------------------
// FreeResult.cs
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
    /// Scene component displaying the Free Mode results.
    /// It inherits ResultBase and loads the content to be used,
    /// then performs main update processing.
    ///
    /// �t���[���[�h�̃��U���g��\������V�[���R���|�[�l���g�ł��B
    /// ResultBase���p�����A�g�p����R���e���g�̓ǂݍ��݂ƁA
    /// ���C���̍X�V�����̓��e���킯�Ă��܂��B
    /// </summary>
    public class FreeResult : ResultBase
    {
        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public FreeResult(Game game, StageResult stageResult)
            : base(game, stageResult)
        {
        }


        /// <summary>
        /// Initializes the Navigate button.
        /// 
        /// �i�r�Q�[�g�{�^���̏������������s���܂��B
        /// </summary>
        protected override void InitializeNavigate()
        {
            Navigate.Clear();
            Navigate.Add(new NavigateData(AppSettings("B_Title")));
            Navigate.Add(new NavigateData(AppSettings("A_Menu"), true));

            // Sets the Navigate button draw status.
            // 
            // �i�r�Q�[�g�{�^���̕`���Ԃ�ݒ�
            drawNavigate = true;

            base.InitializeNavigate();
        }


        /// <summary>
        /// Loads the content. 
        /// 
        /// �R���e���g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void LoadContent()
        {
            // Loads the sequence data.
            // 
            // �V�[�P���X�f�[�^�̓ǂݍ���
            string asset = "Layout/Result/result_Scene";
            sceneData = Content.Load<SceneData>(asset);
            seqStart = sceneData.CreatePlaySeqData("ResultFreeStart");
            seqPosition = sceneData.CreatePlaySeqData("PosFreeStart");

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Performs main update processing.
        /// 
        /// ���C���̍X�V�������s���܂��B
        /// </summary>
        protected override void UpdateMain(GameTime gameTime)
        {
            VirtualPadState virtualPad = 
                GameData.Input.VirtualPadStates[PlayerIndex.One];
            VirtualPadButtons buttons = virtualPad.Buttons;

            if (phase == Phase.Start)
            {
                // Sets to Select Processing when the sequence finishes.
                // 
                // �V�[�P���X���I��������A�I�������ɐݒ肵�܂��B
                if (!seqStart.IsPlay)
                {
                    phase = Phase.Select;
                }
            }
            else if (phase == Phase.Select)
            {
                if (buttons.A[VirtualKeyState.Push])
                {
                    // Performs menu transition when the A button is pressed.
                    // 
                    // A�{�^���������ꂽ�ꍇ�̓��j���[�ɑJ�ڂ��܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                    GameData.SceneQueue.Enqueue(new Menu.MenuComponent(Game));
                    GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
                }
                else if (buttons.B[VirtualKeyState.Push])
                {
                    // Performs title transition when the B button is pressed.
                    // 
                    // B�{�^���������ꂽ�ꍇ�̓^�C�g���ɑJ�ڂ��܂��B
                    GameData.Sound.PlaySoundEffect(Sounds.SoundEffectOkay);
                    GameData.SceneQueue.Enqueue(new Title(Game));
                    GameData.FadeSeqComponent.Start(FadeType.Normal, FadeMode.FadeOut);
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
        protected override string GetSequenceString(int id)
        {
            switch (id)
            {
                case 0:
                    return result.ClearTime.ToString().Substring(0, 8);
                case 1:
                    return string.Format("{0:000}", result.MoveCount);
            }

            return String.Empty;
        }
        #endregion

    }
}


