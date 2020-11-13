#region File Description
//-----------------------------------------------------------------------------
// MenuData.cs
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
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

using Movipa.Components.Animation;
using Movipa.Components.Scene.Puzzle;
using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Menu
{
    /// <summary>
    /// Stores common data used in menus.
    /// 
    /// ���j���[�Ŏg�p���鋤�ʃf�[�^���i�[���܂��B
    /// </summary>
    public class MenuData : IDisposable
    {
        #region Fields
        /// <summary>
        /// Cursor sphere rotation speed
        /// 
        /// �J�[�\���̋��̂̉�]���x
        /// </summary>
        public readonly float CursorSphereRotate;

        /// <summary>
        /// Cursor sphere default size
        /// 
        /// �J�[�\���̋��̂̃f�t�H���g�T�C�Y
        /// </summary>
        public readonly float CursorSphereSize;

        /// <summary>
        /// Small cursor sphere 
        /// 
        /// �J�[�\���̋��̂̏������T�C�Y
        /// </summary>
        public readonly float CursorSphereMiniSize;

        /// <summary>
        /// Enlarge value when cursor sphere selected
        /// 
        /// �J�[�\���̋��̂̑I�����Ɋg�傷��l
        /// </summary>
        public readonly float CursorSphereZoomSpeed;

        /// <summary>
        /// Change value during cursor sphere fade
        ///
        /// �J�[�\���̋��̂̃t�F�[�h���ɕω�����l
        /// </summary>
        public readonly float CursorSphereFadeSpeed;

        /// <summary>
        /// Camera position viewing sphere
        ///
        /// ���̂�����J�����ʒu
        /// </summary>
        public readonly Vector3 CameraPosition;
        
        /// <summary>
        /// Menu scene data
        ///
        /// ���j���[�̃V�[���f�[�^
        /// </summary>
        public SceneData sceneData;

        /// <summary>
        /// Background and selected status sphere
        ///
        /// �w�i�ƑI����Ԃ̋���
        /// </summary>
        public BasicModelData[][] Spheres;

        /// <summary>
        /// Movie animation
        /// 
        /// ���[�r�[�A�j���[�V����
        /// </summary>
        public PuzzleAnimation movie;

        /// <summary>
        /// Movie loader
        ///
        /// ���[�r�[���[�_
        /// </summary>
        public MovieLoader movieLoader;

        /// <summary>
        /// Movie texture
        ///
        /// ���[�r�[�e�N�X�`��
        /// </summary>
        public Texture2D movieTexture;

        /// <summary>
        /// Split preview render target
        ///
        /// �����v���r���[�̃����_�[�^�[�Q�b�g
        /// </summary>
        public RenderTarget2D DividePreview;

        /// <summary>
        /// Split texture
        /// 
        /// �����e�N�X�`��
        /// </summary>
        public Texture2D divideTexture;

        /// <summary>
        /// Style animation render target
        ///
        /// �X�^�C���A�j���[�V�����̃����_�[�^�[�Q�b�g
        /// </summary>
        public RenderTarget2D StyleAnimation;

        /// <summary>
        /// Style animation texture
        ///
        /// �X�^�C���A�j���[�V�����̃e�N�X�`��
        /// </summary>
        public Texture2D StyleAnimationTexture;

        /// <summary>
        /// Style animation texture
        ///
        /// �X�^�C���A�j���[�V�����̃e�N�X�`��
        /// </summary>
        public SequencePlayData SeqStyleAnimation;

        /// <summary>
        /// Stage settings information 
        ///
        /// �X�e�[�W�̐ݒ���
        /// </summary>
        public StageSetting StageSetting;

        /// <summary>
        /// Panel management class 
        ///
        /// �p�l���̊Ǘ��N���X
        /// </summary>
        public PanelManager PanelManager;

        /// <summary>
        /// Primitive drawing class
        ///
        /// ��{�`��N���X
        /// </summary>
        public PrimitiveDraw2D primitiveDraw;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        ///
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public MenuData(Game game)
        {
            CursorSphereRotate = MathHelper.ToRadians(1.0f);
            CursorSphereSize = 0.4f;
            CursorSphereMiniSize = 0.18f;
            CursorSphereZoomSpeed = 0.01f;
            CursorSphereFadeSpeed = 0.1f;

            CameraPosition = new Vector3(0.0f, 0.0f, 200.0f);
            
            StageSetting = new StageSetting();
            PanelManager = new PanelManager(game);
            primitiveDraw = new PrimitiveDraw2D(game.GraphicsDevice);
        }
        #endregion

        #region IDisposable Members

        private bool disposed = false;
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources.
        /// 
        /// �S�Ẵ��\�[�X���J�����܂��B
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                if (primitiveDraw != null)
                {
                    primitiveDraw.Dispose();
                    primitiveDraw = null;
                }
                if (DividePreview != null && !DividePreview.IsDisposed)
                {
                    DividePreview.Dispose();
                    DividePreview = null;
                }
                if (StyleAnimation != null && !StyleAnimation.IsDisposed)
                {
                    StyleAnimation.Dispose();
                    StyleAnimation = null;
                }
                disposed = true;
            }
        }

        #endregion

    }
}
