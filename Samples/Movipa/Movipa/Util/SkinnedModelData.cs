#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelData.cs
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

using SkinnedModel;
using Movipa.Util;
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Contains the skin model parameters.
    /// Inherits the ModelData class and expands the skin animation parameters.
    /// Includes a dedicated Draw method since BasicEffect is not used for drawing. 
    /// 
    /// �X�L�����f���̃p�����[�^�������܂��B
    /// ModelData�N���X���p�����A�X�L���A�j���[�V�����̃p�����[�^���g�����Ă��܂��B
    /// �`��ɂ�BasicEffect���g�p���Ȃ��̂ŁA��p��Draw���\�b�h�������܂��B
    /// </summary>
    public class SkinnedModelData : ModelData
    {
        #region Fields
        private AnimationPlayer animationPlayer;
        private AnimationClip animationClip;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the animation player.
        /// 
        /// �A�j���[�V�����v���C���[���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public AnimationPlayer AnimationPlayer
        {
            get { return animationPlayer; }
            set { animationPlayer = value; }
        }


        /// <summary>
        /// Obtains or sets the animation clip.
        ///
        /// �A�j���[�V�����N���b�v���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public AnimationClip AnimationClip
        {
            get { return animationClip; }
            set { animationClip = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public SkinnedModelData(Model model, string animationClipName)
            : base(model)
        {
            this.model = model;
            SkinningData skinningData = Model.Tag as SkinningData;
            AnimationPlayer = new AnimationPlayer(skinningData);
            AnimationClip = skinningData.AnimationClips[animationClipName];
            AnimationPlayer.StartClip(AnimationClip); 
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the skin model.
        /// 
        /// �X�L�����f����`�悵�܂��B
        /// </summary>
        public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Draw(world, view, projection, true, null, null, null, null, null);
        }



        /// <summary>
        /// Draws the skin model.
        /// 
        /// �X�L�����f����`�悵�܂��B
        /// </summary>
        public void Draw(
            Matrix world, Matrix view, Matrix projection, bool lightingEnabled)
        {
            Draw(world, view, projection, lightingEnabled, null, null, null, null, null);
        }


        /// <summary>
        /// Draws the skin model.
        /// 
        /// �X�L�����f����`�悵�܂��B
        /// </summary>
        public void Draw(
            Matrix world,
            Matrix view,
            Matrix projection,
            bool lightingEnabled,
            Vector3? light1Color,
            Vector3? light2Color)
        {
            Draw(
                world,
                view, 
                projection,
                lightingEnabled,
                light1Color,
                null,
                light2Color,
                null, 
                null);
        }


        /// <summary>
        /// Draws the skin model.
        /// 
        /// �X�L�����f����`�悵�܂��B
        /// </summary>
        public void Draw(
            Matrix world,
            Matrix view, 
            Matrix projection,
            bool lightingEnabled,
            Vector3? light1Color, 
            Vector3? light1Direction, 
            Vector3? light2Color,
            Vector3? light2Direction,
            float? ambientColor)
        {
            Matrix[] bones = GetBones(world);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    // Sets the effect parameters.
                    // 
                    // �G�t�F�N�g�̃p�����[�^��ݒ肵�܂��B
                    EffectParameterCollection parameters = effect.Parameters;

                    parameters["Bones"].SetValue(bones);
                    parameters["View"].SetValue(view);
                    parameters["Projection"].SetValue(projection);

                    parameters["LightingEnabled"].SetValue(lightingEnabled);

                    if (light1Color != null && light1Color.HasValue)
                        parameters["Light1Color"].SetValue(light1Color.Value);

                    if (light1Direction != null && light1Direction.HasValue)
                        parameters["Light1Direction"].SetValue(light1Direction.Value);

                    if (light2Color != null && light2Color.HasValue)
                        parameters["Light2Color"].SetValue(light2Color.Value);

                    if (light2Direction != null && light2Direction.HasValue)
                        parameters["Light2Direction"].SetValue(light2Direction.Value);

                    if (ambientColor != null && ambientColor.HasValue)
                        parameters["AmbientColor"].SetValue(ambientColor.Value);
                }

                mesh.Draw();
            }
        }
        #endregion

        #region Helper Methods


        /// <summary>
        /// Obtains the bones.
        /// 
        /// �{�[�����擾���܂��B
        /// </summary>
        private Matrix[] GetBones(Matrix world)
        {
            Matrix[] bones = AnimationPlayer.GetSkinTransforms();

            Matrix worldMatrix = world *
            Matrix.CreateScale(Scale) *
            Matrix.CreateRotationX(Rotate.X) *
            Matrix.CreateRotationY(Rotate.Y) *
            Matrix.CreateRotationZ(Rotate.Z) *
            Matrix.CreateTranslation(Position);

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] = bones[i] * worldMatrix;
            }

            return bones;
        }
        #endregion
    }
}