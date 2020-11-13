#region File Description
//-----------------------------------------------------------------------------
// ThumbnailLoader.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Loads movie thumbnails asynchronously.
    /// The load list should be specified in the constructor.
    /// This class is designed to execute the Initialize method
    /// in a thread by inheriting InitializeThread and 
    /// invoking the associated Run method. 
    /// 
    /// ���[�r�[�̃T���l�C����񓯊��œǂݍ��݂܂��B
    /// �ǂݍ��ރ��X�g�̓R���X�g���N�^�Ɏw�肵�ĉ������B
    /// ���̃N���X��InitializeThread���p�����AInitializeThread��
    /// Run���\�b�h���Ăяo�����ƂŁAInitialize���\�b�h���X���b�h��
    /// ���s����悤�ɂȂ��Ă��܂��B
    /// </summary>
    public class ThumbnailLoader : InitializeThread
    {
        #region Fields
        // Asset list to load
        // 
        // �ǂݍ��ރA�Z�b�g���X�g
        private string[] list;

        // Loaded texture list
        // 
        // �ǂݍ��܂ꂽ�e�N�X�`�����X�g
        private List<Texture2D> textures;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the texture list.
        /// 
        /// �e�N�X�`���̃��X�g���擾���܂��B
        /// </summary>
        public List<Texture2D> Textures
        {
            get
            {
                if (!Initialized)
                    return null;

                return textures;
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public ThumbnailLoader(Game game, int cpu, string[] assetList)
            : base(game, cpu)
        {
            list = assetList;
            textures = new List<Texture2D>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Loads the asset list.
        ///
        /// �A�Z�b�g���X�g�̓ǂݍ��ݏ������s���܂��B
        /// </summary>
        protected override void Initialize()
        {
            // Sets the CPU core.
            // 
            // CPU�R�A�̐ݒ�����܂��B
            SetCpuCore();

            // Loads all assets in the list.
            // 
            // ���X�g�ɂ���A�Z�b�g��S�ēǂݍ��݂܂��B
            foreach (string asset in list)
            {
                Texture2D texture = Game.Content.Load<Texture2D>(asset);
                textures.Add(texture);
            }

            base.Initialize();
        }
        #endregion
    }
}
