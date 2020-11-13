#region File Description
//-----------------------------------------------------------------------------
// Particle.cs
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

namespace Movipa.Components.Animation
{
    /// <summary>
    /// This class handles particle information.
    ///
    /// �p�[�e�B�N���̏��������܂��B
    /// </summary>
    public class Particle
    {
        #region Fields
        /// <summary>
        /// Fall velocity
        ///
        /// �������x
        /// </summary>
        public static readonly Vector3 Gravity = new Vector3(0, -0.1f, 0);

        private Vector3 position;
        private Vector3 velocity;
        private bool enable;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the position.
        ///
        /// �ʒu���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// Obtains or sets the distance.
        ///
        /// �ړ��ʂ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }


        /// <summary>
        /// Obtains or sets the enabled status.
        ///
        /// �L����Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        #endregion
    }
}

