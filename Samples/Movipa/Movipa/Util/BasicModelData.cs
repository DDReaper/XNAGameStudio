#region File Description
//-----------------------------------------------------------------------------
// BasicModelData.cs
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
    /// Manages the basic model data.
    /// Includes parameters available in BasicEffect. 
    /// The values of the parameters can be utilized in drawing. Note that 
    /// models without BasicEffect cannot be drawn.
    /// 
    /// �W���̃��f���f�[�^���Ǘ����܂��B
    /// BasicEffect�ň�����p�����[�^���p�ӂ���Ă���A
    /// �`�掞�ɂ��̒l�𔽉f�����邱�Ƃ��o���܂��B
    /// �������ABasicEffect�������Ȃ����f���͕`�悷�邱�Ƃ��o���܂���B
    /// </summary>
    public class BasicModelData : ModelData
    {
        #region Public Types
        /// <summary>
        /// Manages light parameters including light color,
        /// direction and reflection color.
        /// 
        /// ���C�g�̃p�����[�^���Ǘ����܂��B
        /// ���C�g�̐F�ƕ����A���˂̐F�Ȃǂ̃p�����[�^������܂��B
        /// </summary>
        public class DirectionalLight
        {
            #region Fields
            private Vector3 diffuseColor = Vector3.Zero;
            private Vector3 direction = Vector3.Up;
            private bool enabled = false;
            private Vector3 specularColor = Vector3.Zero;
            #endregion

            #region Properties
            /// <summary>
            /// Obtains or sets diffuse reflection light.
            /// 
            /// �g�U���ˌ����擾�܂��͐ݒ肵�܂��B
            /// </summary>
            public Vector3 DiffuseColor
            {
                get { return diffuseColor; }
                set { diffuseColor = value; }
            }


            /// <summary>
            /// Obtains or sets the light direction. 
            /// 
            /// ���C�g�̌������擾�܂��͐ݒ肵�܂��B
            /// </summary>
            public Vector3 Direction
            {
                get { return direction; }
                set { direction = value; }
            }


            /// <summary>
            /// Obtains or sets the light enabled status.
            /// 
            /// ���C�g�̗L����Ԃ��擾�܂��͐ݒ肵�܂��B
            /// </summary>
            public bool Enabled
            {
                get { return enabled; }
                set { enabled = value; }
            }


            /// <summary>
            /// Obtains or sets the specular color. 
            /// 
            /// �X�y�L�����̐F���擾�܂��͐ݒ肵�܂��B
            /// </summary>
            public Vector3 SpecularColor
            {
                get { return specularColor; }
                set { specularColor = value; }
            }
            #endregion
        }
        #endregion

        #region Fields
        protected float alpha;
        protected Vector3 ambientLightColor;
        protected Vector3 diffuseColor;
        protected Vector3 emissiveColor;
        protected bool fogEnabled;
        protected Vector3 fogColor;
        protected float fogStart;
        protected float fogEnd;
        protected bool lightingEnabled;
        protected bool preferPerPixelLighting;
        protected Vector3 specularColor;
        protected float specularPower;
        protected DirectionalLight directionalLight0;
        protected DirectionalLight directionalLight1;
        protected DirectionalLight directionalLight2;
        protected Texture2D texture;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the diffuse reflection light. 
        /// 
        /// �g�U���ˌ����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 AmbientLightColor
        {
            get { return ambientLightColor; }
            set { ambientLightColor = value; }
        }


        /// <summary>
        /// Obtains or sets the diffuse reflection light. 
        /// 
        /// �g�U���ˌ����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }


        /// <summary>
        /// Obtains or sets the brightness. 
        /// 
        /// �P�x���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 EmissiveColor
        {
            get { return emissiveColor; }
            set { emissiveColor = value; }
        }


        /// <summary>
        /// Obtains or sets the alpha value.
        /// 
        /// �A���t�@�l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }


        /// <summary>
        /// Obtains or sets the fog enabled status.
        /// 
        /// �t�H�O�̗L����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool FogEnabled
        {
            get { return fogEnabled; }
            set { fogEnabled = value; }
        }


        /// <summary>
        /// Obtains or sets the fog color.
        /// 
        /// �t�H�O�̐F���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 FogColor
        {
            get { return fogColor; }
            set { fogColor = value; }
        }


        /// <summary>
        /// Obtains or sets the fog start position.
        /// 
        /// �t�H�O�̊J�n�ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float FogStart
        {
            get { return fogStart; }
            set { fogStart = value; }
        }


        /// <summary>
        /// Obtains or sets the fog end position.
        /// 
        ///�t�H�O�̏I���ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float FogEnd
        {
            get { return fogEnd; }
            set { fogEnd = value; }
        }


        /// <summary>
        /// Obtains or sets the lighting enabled status.
        /// 
        /// ���C�e�B���O�̗L����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool LightingEnabled
        {
            get { return lightingEnabled; }
            set { lightingEnabled = value; }
        }


        /// <summary>
        /// Obtains or sets the PreferPerPixel lighting enabled status.
        /// 
        /// �Ȃ�Ƃ����C�e�B���O�̗L����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool PreferPerPixelLighting
        {
            get { return preferPerPixelLighting; }
            set { preferPerPixelLighting = value; }
        }


        /// <summary>
        /// Obtains or sets the specular color.
        /// 
        /// �X�y�L�����̐F���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }


        /// <summary>
        /// Obtains or sets the specular power.
        /// 
        /// �X�y�L�����̋������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public float SpecularPower
        {
            get { return specularPower; }
            set { specularPower = value; }
        }


        /// <summary>
        /// Obtains or sets the Light0 parameters.
        /// 
        /// ���C�g0�̃p�����[�^���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public DirectionalLight DirectionalLight0
        {
            get { return directionalLight0; }
            set { directionalLight0 = value; }
        }


        /// <summary>
        /// Obtains or sets the Light1 parameters.
        /// 
        /// ���C�g1�̃p�����[�^���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public DirectionalLight DirectionalLight1
        {
            get { return directionalLight1; }
            set { directionalLight1 = value; }
        }


        /// <summary>
        /// Obtains or sets the Light2 parameters.
        /// 
        /// ���C�g2�̃p�����[�^���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public DirectionalLight DirectionalLight2
        {
            get { return directionalLight2; }
            set { directionalLight2 = value; }
        }

        
        /// <summary>
        /// Obtains or sets the texture.
        /// 
        /// �e�N�X�`�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        public BasicModelData(Model model)
            : base(model)
        {
            // Sets the initial values of BasicEffect.
            // 
            // BasicEffect�̏����l��ݒ肵�܂��B
            alpha = 1.0f;
            ambientLightColor = new Vector3(0, 0, 0);
            diffuseColor = new Vector3(0.5882353f, 0.5882353f, 0.5882353f);
            emissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
            fogEnabled = false;
            fogColor = new Vector3(0, 0, 0);
            fogStart = 0;
            fogEnd = 1;
            lightingEnabled = false;
            preferPerPixelLighting = false;
            specularColor = new Vector3(0, 0, 0);
            specularPower = 1.99999988f;
            directionalLight0 = new DirectionalLight();
            directionalLight1 = new DirectionalLight();
            directionalLight2 = new DirectionalLight();
            texture = null;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Sets the effect parameters.
        /// 
        /// �G�t�F�N�g�̃p�����[�^��ݒ肵�܂��B
        /// </summary>
        protected override void SetEffectParameters(
            Effect effect, Matrix world, Matrix view, Matrix projection)
        {
            BasicDirectionalLight directionalLight;

            // Processing is not performed if unable to convert to BasicEffect.
            // 
            // BasicEffect�ɕϊ��ł��Ȃ��ꍇ�͏������s��Ȃ��悤�ɂ��܂��B
            BasicEffect basicEffect = effect as BasicEffect;
            if (basicEffect == null)
                return;

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;

            // Registers the lighting settings.
            // 
            // ���C�e�B���O�̐ݒ�����܂��B
            basicEffect.Alpha = Alpha;
            basicEffect.LightingEnabled = LightingEnabled;
            basicEffect.PreferPerPixelLighting = PreferPerPixelLighting;
            basicEffect.AmbientLightColor = AmbientLightColor;
            basicEffect.DiffuseColor = DiffuseColor;
            basicEffect.EmissiveColor = EmissiveColor;
            basicEffect.SpecularColor = SpecularColor;
            basicEffect.SpecularPower = SpecularPower;

            // Registers the DirectionLight0 settings.
            // 
            // DirectionalLight0�̐ݒ�����܂��B
            directionalLight = basicEffect.DirectionalLight0;
            directionalLight.Enabled = DirectionalLight0.Enabled;
            directionalLight.DiffuseColor = DirectionalLight0.DiffuseColor;
            directionalLight.Direction = DirectionalLight0.Direction;
            directionalLight.SpecularColor = DirectionalLight0.SpecularColor;

            // Registers the DirectionLight1 settings.
            // 
            // DirectionalLight1�̐ݒ�����܂��B
            directionalLight = basicEffect.DirectionalLight1;
            directionalLight.Enabled = DirectionalLight1.Enabled;
            directionalLight.DiffuseColor = DirectionalLight1.DiffuseColor;
            directionalLight.Direction = DirectionalLight1.Direction;
            directionalLight.SpecularColor = DirectionalLight1.SpecularColor;

            // Registers the DirectionLight2 settings.
            // 
            // DirectionalLight2�̐ݒ�����܂��B
            directionalLight = basicEffect.DirectionalLight2;
            directionalLight.Enabled = DirectionalLight2.Enabled;
            directionalLight.DiffuseColor = DirectionalLight2.DiffuseColor;
            directionalLight.Direction = DirectionalLight2.Direction;
            directionalLight.SpecularColor = DirectionalLight2.SpecularColor;


            // Registers the fog settings.
            // 
            // �t�H�O�̐ݒ�����܂��B
            basicEffect.FogEnabled = FogEnabled;
            basicEffect.FogColor = FogColor;
            basicEffect.FogStart = FogStart;
            basicEffect.FogEnd = FogEnd;

            // Registers the texture settings.
            // 
            // �e�N�X�`���̐ݒ�����܂��B
            if (Texture != null)
                basicEffect.Texture = Texture;
        }


        /// <summary>
        /// To work around an XNA Game Studio 2.0 bug, textures from render targets
        /// or resolve targets must be cleared manually, or they can interfere with
        /// automatic restoration after the graphics device is lost.
        /// </summary>
        protected override void ClearTexturesFromEffects(Effect effect)
        {
            // Processing is not performed if unable to convert to BasicEffect.
            // 
            // BasicEffect�ɕϊ��ł��Ȃ��ꍇ�͏������s��Ȃ��悤�ɂ��܂��B
            if (Texture != null)
            {
                BasicEffect basicEffect = effect as BasicEffect;
                if (basicEffect != null)
                {
                    basicEffect.Texture = null;
                }
            }
        }

        #endregion
    }
}