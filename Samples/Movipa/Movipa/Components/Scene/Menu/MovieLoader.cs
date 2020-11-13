#region File Description
//-----------------------------------------------------------------------------
// MovieLoader.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Movipa.Components.Animation;
using MovipaLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Loads and initializes the movie animation 
    /// asynchronously. 
    /// This class is designed to execute the 
    /// Initialize method in a thread by inheriting 
    /// InitializeThread and invoking the associated Run method.
    /// 
    /// ���[�r�[�A�j���[�V�����̓ǂݍ��݂Ə�������񓯊��ōs���܂��B
    /// ���̃N���X��InitializeThread���p�����AInitializeThread��
    /// Run���\�b�h���Ăяo�����ƂŁAInitialize���\�b�h���X���b�h��
    /// ���s����悤�ɂȂ��Ă��܂��B
    /// </summary>
    public class MovieLoader : InitializeThread
    {
        #region Fields
        // Animation information 
        //
        // �A�j���[�V�������
        private AnimationInfo animationInfo;

        // Loaded movie objects
        //
        // �ǂݍ��܂ꂽ���[�r�[�I�u�W�F�N�g
        private PuzzleAnimation movie;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the movie objects that have been loaded. 
        ///
        /// �ǂݍ��܂ꂽ���[�r�[�I�u�W�F�N�g���擾���܂��B
        /// </summary>
        public PuzzleAnimation Movie
        {
            get
            {
                if (!Initialized)
                    return null;

                return movie;
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public MovieLoader(Game game, int cpu, AnimationInfo info)
            : base(game, cpu)
        {
            animationInfo = info;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Loads the movie.
        ///
        /// ���[�r�[�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void Initialize()
        {
            // Sets the CPU core.
            // 
            // CPU�R�A�̐ݒ�����܂��B
            SetCpuCore();

            // Loads the movie.
            // 
            // ���[�r�[��ǂݍ��݂܂��B
            movie = PuzzleAnimation.CreateAnimationComponent(Game, animationInfo);

            // Initializes the movie that has been loaded.
            //
            // �ǂݍ��񂾃��[�r�[�̏��������s���܂��B
            movie.Initialize();

            base.Initialize();
        }
        #endregion
    }
}
