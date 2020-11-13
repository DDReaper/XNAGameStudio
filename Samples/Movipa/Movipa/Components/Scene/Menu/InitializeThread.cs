#region File Description
//-----------------------------------------------------------------------------
// InitializeThread.cs
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
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>     
    /// Class used for asynchronous initialization.
    /// This class is inherited. 
    /// The initialize method is defined in the inherited 
    /// target class with description of initialization processing.
    /// When the Run method is called, the Initialize method is
    /// executed in the thread, thus enabling asynchronous initialization.
    /// The executing CPU core may be changed in Xbox360 only.
    /// When initialization is completed, the Initialized property becomes True.
    /// 
    /// �񓯊��ŏ�����������ׂ̃N���X�ł��B
    /// ���̃N���X�͌p�����Ďg�p���܂��B
    /// �p����̃N���X��Initialize���\�b�h���`���A�������̏�����
    /// �L�q���܂��BRun���\�b�h���ĂԂƁA�X���b�h��Initialize���\�b�h��
    /// ���s����A�񓯊��ŏ��������s�����Ƃ��o���܂��B
    /// Xbox360�ł̂݁A���s������CPU�̃R�A��ύX���邱�Ƃ��o���܂��B
    /// ���������I��������AInitialized�v���p�e�B��True�ɂȂ�܂��B
    /// </summary>
    public class InitializeThread
    {
        #region Fields
        // CPU core
        //
        // cpu�R�A
        private int cpuId;

        // Initialized flag
        //  
        // �������ς݃t���O
        private bool initialized = false;

        // Load thread
        //
        //�ǂݍ��݃X���b�h
        private Thread thread;

        // Game object transferred to movie
        //
        // ���[�r�[�ɓn��Game�I�u�W�F�N�g
        private Game game;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains the Game instance.
        /// 
        /// Game�C���X�^���X���擾���܂��B
        /// </summary>
        public Game Game
        {
            get { return game; }
        }

        /// <summary>
        /// Obtains the initialization status.
        ///
        /// ��������Ԃ��擾���܂��B
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="game">Game�C���X�^���X</param>
        /// <param name="cpu">�g�p����CPU�R�A</param>
        public InitializeThread(Game game, int cpu)
        {
            this.game = game;
            cpuId = cpu;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Loads the movie asynchronously.
        ///
        /// ���[�r�[��񓯊��œǂݍ��݂܂��B 
        /// </summary>
        public void Run()
        {
            // Start the thread.
            // 
            // �X���b�h�̊J�n
            thread = new Thread(new ThreadStart(this.Initialize));
            thread.Start();
        }


        /// <summary>
        /// Forcibly terminates the thread.
        ///
        /// �X���b�h�������I�����܂��B
        /// </summary>
        public void Abort()
        {
            if (thread != null)
                thread.Abort();
        }


        /// <summary>
        /// Waits until the thread terminates.
        ///
        /// �X���b�h���I������܂őҋ@���܂��B
        /// </summary>
        public void Join()
        {
            if (thread != null)
                thread.Join();
        }

        
        /// <summary>
        /// Loads the moview.
        ///
        /// ���[�r�[��ǂݍ��݂܂��B 
        /// </summary>
        protected virtual void Initialize()
        {
            // Sets the initialized flag.
            // 
            // �������ς݃t���O��ݒ肷��
            initialized = true;
        }


        /// <summary>
        /// Sets the CPU core processed in the thread.
        ///
        /// �X���b�h�ŏ������s��CPU�R�A�̐ݒ�����܂��B
        /// </summary>
        protected void SetCpuCore()
        {
#if XBOX360
            Thread.CurrentThread.SetProcessorAffinity(cpuId);
#endif
        }
        #endregion
    }
}
