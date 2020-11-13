#region File Description
//-----------------------------------------------------------------------------
// SaveFileLoader.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;

using Movipa.Util;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Performs an asynchronous search for the save
    /// file of the game. 
    /// This class is designed to execute the 
    /// Initialize method in a thread by inheriting 
    /// InitializeThread and invoking the associated 
    /// Run method. The save files are named SaveDataFile1.xml,
    /// SaveDataFile2.xml and SaveDataFile3.xml.
    /// 
    /// �Q�[���̃Z�[�u�t�@�C����񓯊��Ō������܂��B
    /// ���̃N���X��InitializeThread���p�����AInitializeThread��
    /// Run���\�b�h���Ăяo�����ƂŁAInitialize���\�b�h���X���b�h��
    /// ���s����悤�ɂȂ��Ă��܂��B
    /// �Z�[�u�t�@�C���̖��̂ɂ��ẮASaveDataFile1.xml�ASaveDataFile2.xml�A
    /// SaveDataFile3.xml�A�ƂȂ��Ă��܂��B
    /// </summary>
    public class SaveFileLoader : InitializeThread
    {
        #region Fields
        private SaveData[] gameSettings = null;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains settings information.
        /// 
        /// �ݒ�����擾���܂��B
        /// </summary>
        public SaveData[] GetGameSettings()
        {
            if (!Initialized)
                return null;

            return gameSettings;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SaveFileLoader(Game game, int cpu)
            : base(game, cpu)
        {
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Loads the save file.
        ///
        /// �Z�[�u�t�@�C���̓ǂݍ��݂��s���܂��B
        /// </summary>
        protected override void Initialize()
        {
            // Sets the CPU core.
            // 
            // CPU�R�A�̐ݒ�����܂��B
            SetCpuCore();

            // Sets the filename to be searched.
            // 
            // ��������t�@�C������ݒ肵�܂��B
            string[] saveFilePathList = {
                GameData.Storage.GetStoragePath("SaveDataFile1.xml"),
                GameData.Storage.GetStoragePath("SaveDataFile2.xml"),
                GameData.Storage.GetStoragePath("SaveDataFile3.xml"),
            };

            // Loads the file and adds it to the list.
            //
            // �t�@�C����ǂݍ��݁A���X�g�ɒǉ����܂��B
            List<SaveData> saveList = new List<SaveData>();
            foreach (string saveFilePath in saveFilePathList)
            {
                saveList.Add(SettingsSerializer.LoadSaveData(saveFilePath));
            }

            // Converts the list to an array.
            // 
            // ���X�g��z��ɕϊ����܂��B
            gameSettings = saveList.ToArray();

            base.Initialize();
        }
        #endregion
    }

}
