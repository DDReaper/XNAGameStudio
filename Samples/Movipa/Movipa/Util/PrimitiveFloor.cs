#region File Description
//-----------------------------------------------------------------------------
// PrimitiveFloor.cs
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
    /// Draws the floor plane.
    /// The floor is initially centered on Vector 3(0, 0, 0).
    /// Use SetPositionOffset to shift the floor.
    /// 
    /// ���ʂ̏���`�悵�܂��B
    /// ������Ԃł�Vector3(0, 0, 0)�𒆐S�Ƃ��A�����쐬���܂��B
    /// �����ړ�������ɂ�SetPositionOffset���g�p���Ă��������B
    /// </summary>
    public class PrimitiveFloor : PrimitiveRenderState, IDisposable
    {
        #region Fields
        private BasicEffect basicEffect;
        private VertexDeclaration vertexDeclaration;
        private VertexPositionColor[] vertices;
        private VertexBuffer vertexBuffer;
        private int totalSurface;
        private int totalVertices;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        /// <param name="width">Number of horizontal tiles</param>
        ///  s
        /// <param name="width">���̃^�C������</param>

        /// <param name="height">Number of vertical tiles</param>
        ///  
        /// <param name="height">�c�̃^�C������</param>

        /// <param name="scale">Tile size</param>
        ///  
        /// <param name="scale">�^�C���̃T�C�Y</param>

        /// <param name="color">Tile color</param>
        ///  
        /// <param name="color">�^�C���̐F</param>
        public PrimitiveFloor(
            GraphicsDevice graphics, int width, int height, float scale, Color color)
        {
            // Creates the BasicEffect.
            // 
            // BasicEffect���쐬���܂��B
            basicEffect = new BasicEffect(graphics, null);

            // Sets the vertex buffer.
            // 
            // ���_�o�b�t�@��ݒ肵�܂��B
            VertexElement[] elements = VertexPositionColor.VertexElements;
            vertexDeclaration = new VertexDeclaration(graphics, elements);

            // Creates the floor vertex.
            // 
            // ���̒��_���쐬���܂��B
            CreateFloor(graphics, width, height, scale, color);

            // Registers the fog settings.
            // 
            // �t�H�O�̐ݒ���s���܂��B
            SetFogMode(true, Vector3.Zero, 0.0f, 1000.0f);
        }


        /// <summary>
        /// Initializes the instance.
        /// Creates grey tiles.
        ///
        /// �C���X�^���X�����������܂��B
        /// �O���[�̃^�C�����쐬���܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        /// <param name="width">Number of horizontal tiles</param>
        ///  
        /// <param name="width">���̃^�C������</param>

        /// <param name="height">Number of vertical tiles </param>
        ///  
        /// <param name="height">�c�̃^�C������</param>

        /// <param name="scale">Tile size</param>
        ///  
        /// <param name="scale">�^�C���̃T�C�Y</param>

        public PrimitiveFloor(
            GraphicsDevice graphics, int width, int height, float scale)
            : this(graphics, width, height, scale, Color.Gray)
        {
        }


        /// <summary>
        /// Initializes the instance. 
        /// Creates tiles of side length 50 in grey.
        /// 
        /// �C���X�^���X�����������܂��B
        /// �O���[�ŁA��ӂ�50�̃^�C�����쐬���܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        /// <param name="width">Number of horizontal tiles</param>
        ///  
        /// <param name="width">���̃^�C������</param>

        /// <param name="height">Number of vertical tiles</param>
        ///  
        /// <param name="height">�c�̃^�C������</param>
        public PrimitiveFloor(GraphicsDevice graphics, int width, int height)
            : this(graphics, width, height, 50.0f, Color.Gray)
        {
        }


        /// <summary>
        /// Initializes the instance.
        /// Creates tiles of side length 50 in grey.
        /// 
        /// �C���X�^���X�����������܂��B
        /// �O���[�ŁA��ӂ�50�̃^�C�����쐬���܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        /// <param name="tiles">Number of tiles</param>
        ///  
        /// <param name="tiles">�^�C������</param>
        public PrimitiveFloor(GraphicsDevice graphics, Point tiles)
            : this(graphics, tiles.X, tiles.Y, 50.0f, Color.Gray)
        {
        }


        /// <summary>
        /// Initializes the instance.
        /// Creates horizontal and vertical sides of 25 tiles 
        /// with side length 50 in grey.
        /// 
        /// �C���X�^���X�����������܂��B
        /// �O���[�ŁA��ӂ�50�̃^�C�����c��25�����쐬���܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        public PrimitiveFloor(GraphicsDevice graphics)
            : this(graphics, 25, 25, 50.0f, Color.Gray)
        {
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the floor.
        /// 
        /// ����`�悵�܂��B
        /// </summary>
        public void Draw(Matrix projection, Matrix view)
        {
            // Registers the projection settings.
            // 
            // �v���W�F�N�V�����̐ݒ�����܂��B
            basicEffect.Projection = projection;

            // Registers the view settings.
            // 
            // �r���[�̐ݒ�����܂��B
            basicEffect.View = view;

           // Draws the floor.
           // 
           // ����`�悵�܂��B
            DrawFloor(basicEffect);
        }


        /// <summary>
        /// Draws the floor.
        /// 
        /// ����`�悵�܂��B
        /// </summary>
        private void DrawFloor(BasicEffect basicEffect)
        {
            GraphicsDevice graphics = basicEffect.GraphicsDevice;
            VertexStream stream = graphics.Vertices[0];

            // Sets the definitions of the vertex data to be drawn.
            // 
            // �`�悷�钸�_�f�[�^�̒�`��ݒ肵�܂��B
            graphics.VertexDeclaration = vertexDeclaration;

            // Sets the vertex buffer.
            // 
            // ���_�o�b�t�@��ݒ肵�܂��B
            stream.SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);

            // Enables the vertex color.
            // 
            // ���_�J���[��L���ɂ��܂��B
            basicEffect.VertexColorEnabled = true;

            // Begins using the effect.
            // 
            // �G�t�F�N�g�̎g�p���J�n���܂��B
            basicEffect.Begin();

            // Repeats drawing for the number of passes.
            // 
            // �p�X�̐������J��ւ����`�悵�܂��B
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // Begins the pass.
                // 
                // �p�X���J�n���܂��B
                pass.Begin();

                // Draws with TriangleList.
                // 
                // TriangleList �ŕ`�悵�܂��B
                basicEffect.GraphicsDevice.DrawPrimitives(
                    PrimitiveType.TriangleList, 0, totalSurface * 2);

                // Finishes the pass.
                // 
                // �p�X���I�����܂��B
                pass.End();
            }

            // Finishes using the effect.
            // 
            // �G�t�F�N�g�̎g�p���I�����܂��B
            basicEffect.End();

        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates floor vertex information.
        /// 
        /// ���̒��_�����쐬���܂��B
        /// </summary>
        /// <param name="graphics">GraphicsDevice</param>
        /// <param name="width">Number of horizontal tiles</param>
        ///  
        /// <param name="width">���̃^�C������</param>

        /// <param name="height">Number of vertical tiles</param>
        ///  
        /// <param name="height">�c�̃^�C������</param>

        /// <param name="scale">Tile size</param>
        ///  
        /// <param name="scale">�^�C���̃T�C�Y</param>

        /// <param name="color">Tile color</param>
        ///  
        /// <param name="color">�^�C���̐F</param>

        private void CreateFloor(
            GraphicsDevice graphics, int width, int height, float scale, Color color)
        {
            // Calculates the base position.
            // 
            // ��_���v�Z���܂��B
            Vector3 basePosition = new Vector3();
            basePosition.X = -((float)width / 2) * scale;
            basePosition.Z = -((float)height / 2) * scale;

            // Calculates the number of surfaces.
            // 
            // �ʂ̐����v�Z���܂��B
            totalSurface = (width * height);

            // Calculates the number of vertices.
            // 
            // ���_�̐����v�Z���܂��B
            totalVertices = totalSurface * 6;

            // Sets the floor color.
            // 
            // ���̐F��ݒ肵�܂��B
            Color[] colors = new Color[]
            {
                color,
                new Color(color.ToVector3() * 0.8f),
            };

            // Sets the vertex position differential.
            // 
            // ���_�̈ʒu�̍�����ݒ肵�܂��B
            Vector3[] offsets = new Vector3[] {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(scale, 0.0f, 0.0f),
                new Vector3(scale, 0.0f, scale),
                new Vector3(scale, 0.0f, scale),
                new Vector3(0.0f, 0.0f, scale),
                new Vector3(0.0f, 0.0f, 0.0f),
            };

            // Sets the vertex.
            // 
            // ���_��ݒ肵�܂��B
            vertices = new VertexPositionColor[totalVertices];
            int colorCount = 0;
            int vertexCount = 0;
            for (int x = 0; x < width; x++)
            {
                // Switches the tile color arrangement.
                // 
                // �^�C���̐F����т����ւ��܂��B
                colorCount = ((x % 2) == 0) ? 0 : 1;

                for (int y = 0; y < height; y++)
                {
                    // Sets the vertex position.
                    // 
                    // ���_�̈ʒu��ݒ肵�܂��B
                    Vector3 position = basePosition;
                    position.X += x * scale;
                    position.Z += y * scale;

                    // Obtains the vertex color.
                    // 
                    // ���_�̐F���擾���܂��B
                    Color floorColor = colors[(colorCount++ % 2)];

                    // Sets the vertex.
                    // 
                    // ���_��ݒ肵�܂��B
                    foreach (Vector3 offset in offsets)
                    {
                        vertices[vertexCount++] = 
                            new VertexPositionColor(position + offset, floorColor);
                    }
                }
            }

            // Creates a vertex buffer for TriangleList.
            // 
            // TriangleList �p���_�o�b�t�@�쐬���܂��B
            int sizeInBytes = VertexPositionColor.SizeInBytes * totalVertices;
            vertexBuffer = new VertexBuffer(graphics, sizeInBytes, BufferUsage.None);

            // Writes vertex data to the vertex buffer.
            // 
            // ���_�f�[�^�𒸓_�o�b�t�@�ɏ������݂܂��B
            vertexBuffer.SetData<VertexPositionColor>(vertices);
        }


        /// <summary>
        /// Registers the fog settings.
        /// 
        /// �t�H�O�̐ݒ�����܂��B
        /// </summary>
        public void SetFogMode(bool fogEnabled)
        {
            if (fogEnabled)
            {
                // Sets the fog parameters.
                // 
                // �t�H�O�̃p�����[�^��ݒ肵�܂��B
                SetFogMode(true, Vector3.Zero, 0.0f, 1000.0f);
            }
            else
            {
                // Disables the fog settings.
                // 
                // �t�H�O�̐ݒ�𖳌��ɂ��邵�܂��B
                basicEffect.FogEnabled = fogEnabled;
            }
        }


        /// <summary>
        /// Registers the fog settings.
        /// 
        /// �t�H�O�̐ݒ�����܂��B
        /// </summary>
        /// <param name="fogEnabled">Enabled flag</param>
        ///  
        /// <param name="fogEnabled">�L���t���O</param>

        /// <param name="fogColor">Fog color</param>
        ///  
        /// <param name="fogColor">�t�H�O�̐F</param>

        /// <param name="fogStart">Fog start position</param>
        ///  
        /// <param name="fogStart">�t�H�O�̊J�n�ʒu</param>

        /// <param name="fogEnd">Fog end position</param>
        ///  
        /// <param name="fogEnd">�t�H�O�̏I���ʒu</param>
        public void SetFogMode(
            bool fogEnabled, Vector3 fogColor, float fogStart, float fogEnd)
        {
            // Sets the fog parameters.
            // 
            // �t�H�O�̃p�����[�^��ݒ肵�܂��B
            basicEffect.FogEnabled = fogEnabled;
            basicEffect.FogColor = fogColor;
            basicEffect.FogStart = fogStart;
            basicEffect.FogEnd = fogEnd;
        }



        /// <summary>
        /// Shifts the floor position.
        /// 
        /// ���̈ʒu���ړ����܂��B
        /// </summary>
        public void SetPositionOffset(Vector3 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position += offset;
            }
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
                if (basicEffect != null)
                {
                    basicEffect.Dispose();
                    basicEffect = null;
                }
                if (vertexDeclaration != null)
                {
                    vertexDeclaration.Dispose();
                    vertexDeclaration = null;
                }
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }
                disposed = true;
            }
        }

        #endregion
    }
}