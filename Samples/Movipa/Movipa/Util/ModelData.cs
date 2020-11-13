#region File Description
//-----------------------------------------------------------------------------
// ModelData.cs
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
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// 3D�̃��f���f�[�^���Ǘ����܂��B
    /// ���f���f�[�^�ƈʒu�ƃX�P�[���̏��������A
    /// �ŏ����̕`��@�\��񋟂��܂��B
    /// </summary>
    public abstract class ModelData : PrimitiveRenderState, IDisposable
    {
        #region Fields
        // �J���ς݃t���O
        private bool disposed = false;

        // 3D���f���f�[�^
        protected Model model = null;

        // �ʒu
        protected Vector3 position = Vector3.Zero;
        protected Vector3 rotate = Vector3.Zero;
        protected Vector3 yawPitchRoll = Vector3.Zero;

        // �X�P�[��
        protected float scale = 1.0f;

        // �{�[���}�g���b�N�X
        private Matrix[] boneTransforms;
        #endregion

        #region Properties
        /// <summary>
        /// �J����Ԃ��擾���܂��B
        /// </summary>
        public bool IsDisposed
        {
            get { return disposed; }
        }


        /// <summary>
        /// ���f�����擾���܂��B
        /// </summary>
        public Model Model
        {
            get { return model; }
        }


        /// <summary>
        /// �ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// ��]���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }


        /// <summary>
        /// ���[�ƃs�b�`�ƃ��[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 YawPitchRoll
        {
            get { return yawPitchRoll; }
            set { yawPitchRoll = value; }
        }


        /// <summary>
        /// ���[���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Yaw
        {
            get { return yawPitchRoll.X; }
            set { yawPitchRoll.X = value; }
        }


        /// <summary>
        /// �s�b�`���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Pitch
        {
            get { return yawPitchRoll.Y; }
            set { yawPitchRoll.Y = value; }
        }


        /// <summary>
        /// ���[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Roll
        {
            get { return yawPitchRoll.Z; }
            set { yawPitchRoll.Z = value; }
        }


        /// <summary>
        /// �X�P�[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new ModelData object.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        protected ModelData(Model model)
        {
            this.model = model;

            boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        }

        #endregion


        #region IDisposable Members

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
                disposed = true;
            }
        }

        #endregion


        #region Draw Methods
        /// <summary>
        /// 3D���f����`�悵�܂��B
        /// </summary>
        public virtual void Draw(Matrix view, Matrix projection)
        {
            Draw(Matrix.Identity, view, projection);
        }


        /// <summary>
        /// 3D���f����`�悵�܂��B
        /// </summary>
        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                // �G�t�F�N�g�̃p�����[�^��ݒ肵�܂��B
                SetEffectParameters(mesh, world, view, projection);

                // ���f����`�悵�܂��B
                mesh.Draw();

                ClearTexturesFromEffects(mesh);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// ���[���h�}�g���b�N�X���擾���܂��B
        /// </summary>
        public Matrix GetWorldMatrix(ModelMesh mesh, Matrix world)
        {
            if (model == null || mesh == null)
                return Matrix.Identity;

            return boneTransforms[mesh.ParentBone.Index] *
                                Matrix.CreateScale(Scale) *
                                Matrix.CreateRotationX(Rotate.X) *
                                Matrix.CreateRotationY(Rotate.Y) *
                                Matrix.CreateRotationZ(Rotate.Z) *
                                Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll) *
                                Matrix.CreateTranslation(Position) *
                                world;
        }


        /// <summary>
        /// �G�t�F�N�g�̃p�����[�^��ݒ肵�܂��B
        /// </summary>
        protected virtual void SetEffectParameters(
            ModelMesh mesh, Matrix world, Matrix view, Matrix projection)
        {
            foreach (Effect effect in mesh.Effects)
            {
                SetEffectParameters(effect, GetWorldMatrix(mesh, world), view, 
                    projection);
            }
        }


        /// <summary>
        /// �G�t�F�N�g�̃p�����[�^��ݒ肵�܂��B
        /// </summary>
        protected virtual void SetEffectParameters(
            Effect effect, Matrix world, Matrix view, Matrix projection)
        {
            BasicEffect basicEffect = effect as BasicEffect;
            if (basicEffect != null)
            {
                // BasicEffect�̃p�����[�^��ݒ肵�܂��B

                basicEffect.World = world;
                basicEffect.View = view;
                basicEffect.Projection = projection;

                basicEffect.EnableDefaultLighting();
                basicEffect.PreferPerPixelLighting = true;
            }
            else
            {
                // ���̑���Effect���ݒ肳��Ă���̂ŁA
                // Effect�̖��O��"World", "View", "Projection"��
                // ��������T���āA�l��ݒ肵�܂��B

                foreach (EffectParameter effectParameter in effect.Parameters)
                {
                    if (effectParameter.Name == "World")
                    {
                        effectParameter.SetValue(world);
                    }
                    else if (effectParameter.Name == "View")
                    {
                        effectParameter.SetValue(view);
                    }
                    else if (effectParameter.Name == "Projection")
                    {
                        effectParameter.SetValue(projection);
                    }
                }
            }
        }


        /// <summary>
        /// To work around an XNA Game Studio 2.0 bug, textures from render targets
        /// or resolve targets must be cleared manually, or they can interfere with
        /// automatic restoration after the graphics device is lost.
        /// </summary>
        private void ClearTexturesFromEffects(ModelMesh mesh)
        {
            foreach (Effect effect in mesh.Effects)
            {
                ClearTexturesFromEffects(effect);
            }
        }


        /// <summary>
        /// To work around an XNA Game Studio 2.0 bug, textures from render targets
        /// or resolve targets must be cleared manually, or they can interfere with
        /// automatic restoration after the graphics device is lost.
        /// </summary>
        protected virtual void ClearTexturesFromEffects(Effect effect) { }


        #endregion

    }
}
