#region File Description
//-----------------------------------------------------------------------------
// StyleBase.cs
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

using Movipa.Util;
using MovipaLibrary;
using SceneDataLibrary;
#endregion

namespace Movipa.Components.Scene.Puzzle.Style
{
    /// <summary>
    /// Panel selection class.
    /// This class implements panel update and drawing functions.
    /// DrawCursor (used for drawing the cursor) and RandomShuffle (used for shuffling)
    /// must be defined in the inheritance target for this class. 
    /// 
    /// �p�l���I���̃N���X�ł��B
    /// ���̃N���X�ɂ̓p�l���̍X�V�ƕ`��̋@�\���������Ă��܂��B
    /// �N���X�̌p����ł̓J�[�\����`�悷��DrawCursor�ƁA
    /// �V���b�t�����Ɏg�p�����RandomShuffle���`����K�v������܂��B
    /// </summary>
    public abstract class StyleBase : SceneComponent
    {
        #region Public Types
        /// <summary>
        /// Cursor draw position
        /// 
        /// �J�[�\���`��ʒu
        /// </summary>
        [Flags]
        public enum PanelTypes
        {
            /// <summary>
            /// Left up
            /// 
            /// ����
            /// </summary>
            LeftUp = 1,

            /// <summary>
            /// Left down 
            /// 
            /// ����
            /// </summary>
            LeftDown = 2,

            /// <summary>
            /// Right up
            /// 
            /// �E��
            /// </summary>
            RightUp = 4,

            /// <summary>
            /// Right down
            /// 
            /// �E��
            /// </summary>
            RightDown = 8,

            /// <summary>
            /// All
            /// 
            /// �S��
            /// </summary>
            All = LeftUp | LeftDown | RightUp | RightDown
        }
        #endregion

        #region Fields
        /// <summary>
        /// Active panel color 
        /// 
        /// ���쒆�̃p�l���̐F
        /// </summary>
        protected readonly Color ActivePanelColor = new Color(0xff, 0xff, 0xff, 0x7f);

        // Movie texture
        // 
        // ���[�r�[�e�N�X�`��
        private Texture2D movieTexture = null;

        // Select enabled status
        // 
        // �I���ۏ��
        private bool selectEnabled = false;

        // Panel after-image list
        // 
        // �p�l���̎c�����X�g
        private LinkedList<PanelAfterImage> panelAfterImageList;

        // Cursor
        // 
        // �J�[�\��
        protected Point cursor;

        // Cursor texture
        // 
        // �J�[�\���e�N�X�`��
        protected Texture2D cursorTexture;

        // Stage settings information 
        // 
        // �X�e�[�W�ݒ���
        protected StageSetting stageSetting;

        // Panel management class
        // 
        // �p�l���Ǘ��N���X
        protected PanelManager panelManager;

        // Move count
        // 
        // �ړ���
        protected UInt32 moveCount;

        // Glass texture
        // 
        // �K���X�e�N�X�`��
        private Texture2D glassTexture;

        // Lighting texture
        // 
        // ���C�e�B���O�e�N�X�`��
        private Texture2D glassLightingTexture;

        // Glass texture rectangle
        // 
        // �K���X�e�N�X�`���̋�`
        private Rectangle glassRect;

        // Lighting texture rectangle
        // 
        // ���C�e�B���O�e�N�X�`���̋�`
        private Rectangle glassLightingRect;

        // Glass texture draw color
        // 
        // �K���X�e�N�X�`���̕`��F
        private Color glassColor;

        // Primitive draw class
        // 
        // ��{�`��N���X
        private PrimitiveDraw2D primitiveDraw;
        #endregion

        #region Properties
        /// <summary>
        /// Obtains or sets the movie animation texture.
        /// 
        /// ���[�r�[�A�j���[�V�����̃e�N�X�`�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public Texture2D MovieTexture
        {
            get { return movieTexture; }
            set { movieTexture = value; }
        }


        /// <summary>
        /// Obtains or sets the select enabled status.
        /// 
        /// �I���ۏ�Ԃ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool SelectEnabled
        {
            get { return selectEnabled; }
            set { selectEnabled = value; }
        }


        /// <summary>
        /// Obtains or sets the panel after-image list.      
        /// 
        /// �c���p�l���̃��X�g���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public LinkedList<PanelAfterImage> PanelAfterImageList
        {
            get { return panelAfterImageList; }
        }


        /// <summary>
        /// Obtains the move count. 
        /// 
        /// �ړ��񐔂��擾���܂��B
        /// </summary>
        public UInt32 MoveCount
        {
            get { return moveCount; }
        }


        /// <summary>
        /// Obtains the panel action status.
        /// 
        /// �p�l�������Ԃ��擾���܂��B
        /// </summary>
        public bool IsPanelAction
        {
            get { return GetPanelAction(); }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the instance.
        /// 
        /// �C���X�^���X�����������܂��B
        /// </summary>
        protected StyleBase(Game game, StageSetting setting, Point cursor, 
            PanelManager manager)
            : base(game)
        {
            this.cursor = cursor;
            stageSetting = setting;
            panelManager = manager;

            // Creates the panel after-image array.
            // 
            // �p�l���̎c���̔z����쐬���܂��B
            panelAfterImageList = new LinkedList<PanelAfterImage>();

            // Creates the line drawing class.
            // 
            // ���C���`��̃N���X���쐬���܂��B
            primitiveDraw = new PrimitiveDraw2D(game.GraphicsDevice);
        }


        /// <summary>
        /// Loads the content.
        /// 
        /// �R���e���g��ǂݍ��݂܂��B
        /// </summary>
        protected override void LoadContent()
        {
            string asset;
            
            // Loads the glass texture drawn at completion.
            // 
            // �������ɕ`�悷��K���X�̃e�N�X�`����ǂݍ��݂܂��B
            asset = "Textures/Puzzle/glass";
            glassTexture = Content.Load<Texture2D>(asset);

            // Loads the reflected light texture.
            // 
            // ���ˌ��̃e�N�X�`����ǂݍ��݂܂��B
            asset = "Textures/Puzzle/glass_light";
            glassLightingTexture = Content.Load<Texture2D>(asset);


            glassRect = new Rectangle();
            glassRect.Width = glassTexture.Width;
            glassRect.Height = glassTexture.Height;

            glassLightingRect = new Rectangle();
            glassLightingRect.Width = glassLightingTexture.Width;
            glassLightingRect.Height = glassLightingTexture.Height;

            glassColor = new Color(0xff, 0xff, 0xff, 0x4f);

            base.LoadContent();
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates all panels.
        /// 
        /// �S�Ẵp�l���̍X�V�������s���܂��B
        /// </summary>
        protected void UpdatePanels(GameTime gameTime)
        {
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                for (int y = 0; y < stageSetting.Divide.Y; y++)
                {
                    UpdatePanel(panelManager.GetPanel(x, y));
                }
            }

            // Updates the panel after-image. 
            // 
            // �c���̃p�l�����X�V���܂��B
            UpdateAfterImage(gameTime);
        }


        /// <summary>
        /// Updates the panel.
        ///
        /// �p�l���̍X�V�������s���܂��B
        /// </summary>
        protected virtual void UpdatePanel(PanelData panel)
        {
            // Updates panel rotation.
            // 
            // �p�l���̉�]�����̍X�V���s���܂��B
            UpdatePanelRotate(panel);

            // Moves the panel.
            // 
            // �p�l���̈ړ��������s���܂��B
            UpdatePanelMove(panel);

            // Updates the panel reflected light.
            // 
            // �p�l���̔��ˌ��̍X�V�������s���܂��B
            UpdatePanelGlass(panel);

            // Creates the after-image if the panel is in active status.
            // 
            // �p�l���������Ԃ̏ꍇ�͎c�����쐬���܂��B
            if (panel.Status != PanelData.PanelStatus.None)
            {
                // Creates the after-image panel.
                // 
                // �c���̃p�l�����쐬���܂��B
                PanelAfterImage afterimage = CreateAfterImage(panel);

                // Adds the after-image to the array.
                // 
                // �c����z��ɒǉ����܂��B
                PanelAfterImageList.AddLast(afterimage);
            }

        }


        /// <summary>
        /// Updates the panel rotation.
        /// 
        /// �p�l���̉�]�����̍X�V���s���܂��B
        /// </summary>
        private static void UpdatePanelRotate(PanelData panel)
        {
            // Processing is not performed if the panel status is not Rotate.
            // 
            // �X�e�[�^�X����]�ł͖����ꍇ�͏������s���܂���B
            if (panel.Status != PanelData.PanelStatus.RotateLeft &&
                panel.Status != PanelData.PanelStatus.RotateRight)
                return;


            if (panel.Status == PanelData.PanelStatus.RotateLeft)
            {
                panel.Rotate += 10;
                panel.Rotate %= 360;
            }
            else if (panel.Status == PanelData.PanelStatus.RotateRight)
            {
                panel.Rotate += 350;
                panel.Rotate %= 360;
            }

            // Rotation completed; status is returned.
            // 
            // ��]���I�������̂ŃX�e�[�^�X��߂��܂��B
            if (Math.Abs(panel.Rotate - panel.ToRotate) < 10)
            {
                panel.Status = PanelData.PanelStatus.None;
                panel.Rotate = panel.ToRotate;
            }
        }


        /// <summary>
        /// Moves the panel.
        /// 
        /// �p�l���̈ړ��������s���܂��B
        /// </summary>
        private static void UpdatePanelMove(PanelData panel)
        {
            // Processing is not performed if the panel status is not Move.
            // 
            // �X�e�[�^�X���ړ��ł͂Ȃ��ꍇ�͏������s���܂���B
            if (panel.Status != PanelData.PanelStatus.Move)
                return;

            if (panel.MoveCount > 0)
            {
                // Moves the panel.
                // 
                // �p�l���̈ړ��������s���܂��B
                panel.MoveCount = MathHelper.Clamp(panel.MoveCount - 0.1f, 0.0f, 1.0f);

                Vector2 fromPosition = panel.FromPosition;
                Vector2 toPosition = panel.ToPosition;
                float amount = panel.MoveCount;
                panel.Position = Vector2.Lerp(toPosition, fromPosition, amount);
            }
            else
            {
                // Movement completed; status is returned.
                // 
                // �ړ������������̂ŃX�e�[�^�X��߂��܂��B
                panel.Status = PanelData.PanelStatus.None;
                panel.Position = panel.ToPosition;
            }
        }


        /// <summary>
        /// Updates the panel reflected light.
        /// 
        /// �p�l���̔��ˌ��̍X�V�������s���܂��B
        /// </summary>
        private void UpdatePanelGlass(PanelData panel)
        {
            // Processing is not performed if the panel status is Active.
            // 
            // �X�e�[�^�X�������Ԃ̏ꍇ�͏������s���܂���B
            if (panel.Status != PanelData.PanelStatus.None)
                return;

            // Returns the panel draw color.
            // 
            // �p�l���̕`��F��߂��܂��B
            panel.Color = Color.White;
            panel.Flush += 32;

            // Generates glass light at random.
            // 
            // �����_���ŃK���X�̃��C�g�𔭐������܂��B
            if (panel.Enabled == false)
            {
                if (panel.Flush > 256 && (Random.Next() % 200) == 0)
                {
                    panel.Flush = -256;
                }
            }
        }


        /// <summary>
        /// Updates the panel after-image.
        /// 
        /// �c���̃p�l�����X�V���܂��B
        /// </summary>
        protected void UpdateAfterImage(GameTime gameTime)
        {
            LinkedListNode<PanelAfterImage> node = PanelAfterImageList.First;
            while (node != null)
            {
                PanelAfterImage afterimage = node.Value;
                LinkedListNode<PanelAfterImage> removeNode = node;

                // Updates the after-image.
                // 
                // �c���̍X�V�������s���܂��B
                afterimage.Update(gameTime);

                // Moves to the next node. 
                // 
                // ���̃m�[�h�ֈړ����܂��B
                node = node.Next;

                // Deletes the after image from the array if released.
                // 
                // �J���������s���Ă�����z�񂩂�폜���܂��B
                if (afterimage.Disposed)
                {
                    PanelAfterImageList.Remove(removeNode);
                }
            }
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the panel.
        /// 
        /// �p�l���̕`����s���܂��B
        /// </summary>
        public void DrawPanels(GameTime gameTime)
        {
            // Processing is not performed if there is no texture.
            // 
            // �e�N�X�`���������ꍇ�͏������s���܂���B
            if (MovieTexture == null)
                return;

            // Draws Normal status panels.
            // 
            // �ʏ��Ԃ̃p�l����`�悵�܂��B
            DrawNormalPanel();

            // Draws the panel frame.
            // 
            // �p�l���̃t���[����`�悵�܂��B
            DrawPanelFrame();

            // Draws the panel after-image.
            // 
            // �c���̃p�l����`�悵�܂��B
            DrawEffectPanel(gameTime, Batch);

            // Draws panels undergoing change.
            // 
            // �ω����̃p�l���`���`�悵�܂��B
            DrawActivePanel();
        }


        /// <summary>
        /// Draws Normal status panels.
        /// 
        /// �ʏ��Ԃ̃p�l����`�悵�܂��B
        /// </summary>
        private void DrawNormalPanel()
        {
            Batch.Begin();
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                for (int y = 0; y < stageSetting.Divide.Y; y++)
                {
                    // Obtains the panel.
                    // 
                    // �p�l�����擾���܂��B
                    PanelData panel = panelManager.GetPanel(x, y);

                    // Panels undergoing change are not drawn.
                    // 
                    // �ω����̃p�l���͕`�揈�����s���܂���B
                    if (panel.Status != PanelData.PanelStatus.None)
                    {
                        continue;
                    }

                    // Draws Normal status panels.
                    // 
                    // �ʏ��Ԃ̃p�l����`�悵�܂��B
                    DrawPanel(panel);

                    // Draws glass texture for completed panels.
                    // 
                    // �������ꂽ�p�l���̃K���X�e�N�X�`����`�悵�܂��B
                    DrawPanelGlass(panel);
                }
            }
            Batch.End();

        }


        /// <summary>
        /// Draws Normal status panels.
        /// 
        /// �ʏ��Ԃ̃p�l����`�悵�܂��B
        /// </summary>
        private void DrawPanel(PanelData panel)
        {
            Batch.Draw(
                MovieTexture,
                panel.Center + panelManager.DrawOffset,
                panel.SourceRectangle,
                panel.Color,
                MathHelper.ToRadians(panel.Rotate),
                panel.Origin,
                1.0f,
                SpriteEffects.None,
                0.0f);
        }


        /// <summary>
        /// Draws glass texture for completed panels.
        /// 
        /// �������ꂽ�p�l���̃K���X�e�N�X�`����`�悵�܂��B
        /// </summary>
        private void DrawPanelGlass(PanelData panel)
        {
            // Drawing is not performed if the panel is not completed.
            // 
            // �p�l�����܂���������Ă��Ȃ���ԂȂ�`�揈�����s���܂���B
            if (panel.Enabled)
                return;

            Rectangle panelRect = panel.RectanglePosition;
            panelRect.X += (int)panelManager.DrawOffset.X;
            panelRect.Y += (int)panelManager.DrawOffset.Y;


            Texture2D texture;

            // Draws the glass.
            // 
            // �K���X��`�悵�܂��B
            texture = glassTexture;
            Batch.Draw(texture, panelRect, glassRect, glassColor);

            // Draws the lighting effect.
            // 
            // ���C�e�B���O�G�t�F�N�g��`�悵�܂��B
            glassLightingRect.X = (int)panel.Flush;
            texture = glassLightingTexture;
            Batch.Draw(texture, panelRect, glassLightingRect, Color.White);
        }


        /// <summary>
        /// Draws the active panel.
        /// 
        /// ���쒆�̃p�l����`�悵�܂��B
        /// </summary>
        private void DrawActivePanel()
        {
            // Performs drawing via addition.
            // 
            // ���Z�ŕ`����s���܂��B
            Batch.Begin(SpriteBlendMode.AlphaBlend);
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                for (int y = 0; y < stageSetting.Divide.Y; y++)
                {
                    // Obtains the panel.
                    // 
                    // �p�l�����擾���܂��B
                    PanelData panel = panelManager.GetPanel(x, y);

                    // Stopped panels are not drawn.
                    // 
                    // ��~���̃p�l���͕`�揈�����s���܂���B
                    if (panel.Status == PanelData.PanelStatus.None)
                    {
                        continue;
                    }

                    // Draws the active panel.
                    // 
                    // ���쒆�̃p�l����`�悵�܂��B
                    DrawPanel(panel);
                }
            }
            Batch.End();
        }


        /// <summary>
        /// Draws the after-image.
        /// 
        /// �c���̕`����s���܂��B
        /// </summary>
        private void DrawEffectPanel(GameTime gameTime, SpriteBatch batch)
        {
            // Performs drawing via addition.
            // 
            // ���Z�ŕ`�揈�����s���܂��B
            Batch.Begin(SpriteBlendMode.Additive);
            foreach (PanelAfterImage afterimage in PanelAfterImageList)
            {
                afterimage.Draw(gameTime, batch);
            }
            Batch.End();

        }


        /// <summary>
        /// Draws lines in the panel rectangle.
        /// 
        /// �p�l���̋�`�Ƀ��C����`�悵�܂��B
        /// </summary>
        private void DrawPanelFrame()
        {
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                for (int y = 0; y < stageSetting.Divide.Y; y++)
                {
                    // Obtains the panel.
                    // 
                    // �p�l�����擾���܂��B
                    PanelData panel = panelManager.GetPanel(x, y);

                    // Panels undergoing change are not drawn.
                    // 
                    // �ω����̃p�l���͕`�揈�����s���܂���B
                    if (panel.Status != PanelData.PanelStatus.None)
                    {
                        continue;
                    }

                    // Draws the lines.
                    // 
                    // ���C����`�悵�܂��B
                    DrawPanelFrame(panel);
                }
            }
        }


        /// <summary>
        /// Draws lines in the panel rectangle.
        /// 
        /// �p�l���̋�`�Ƀ��C����`�悵�܂��B
        /// </summary>
        private void DrawPanelFrame(PanelData panel)
        {
            Vector2 panelPosition = panel.Position + panelManager.DrawOffset;
            Color color = new Color(0xff, 0xff, 0xff, 0x20);
            Vector4 rect = new Vector4();
            rect.X = panelPosition.X;
            rect.Y = panelPosition.Y;
            rect.Z = panel.Size.X;
            rect.W = panel.Size.Y;

            primitiveDraw.DrawRect(null, rect, color);
        }


        /// <summary>
        /// Abstract method for cursor drawing.
        /// 
        /// �J�[�\���`��̒��ۃ��\�b�h�ł��B
        /// </summary>
        public abstract void DrawCursor(GameTime gameTime);


        /// <summary>
        /// Draws the cursor.
        /// 
        /// �J�[�\���`�揈�����s���܂��B
        /// </summary>
        protected void DrawCursor(
            GameTime gameTime, Rectangle panelSize, Color color, PanelTypes panelType)
        {
            Color[] colors = {
                color,
                color,
                color,
                color
            };

            DrawCursor(gameTime, panelSize, colors, panelType);
        }


        /// <summary>
        /// Draws the cursor by specifying the four corners.
        /// 
        /// �l�����w�肵�ăJ�[�\����`�悵�܂��B
        /// </summary>
        protected virtual void DrawCursor(GameTime gameTime,
            Rectangle panelSize, Color[] colors, PanelTypes panelType)
        {
            // Processing is not performed if the cursor texture is not specified.
            // 
            // �J�[�\���̃e�N�X�`�����w�肳��Ă��Ȃ��ꍇ�͏������s���܂���B
            if ((cursorTexture == null) || cursorTexture.IsDisposed)
            {
                return;
            }

            // Specifies the cursor size at the four corners.
            // 
            // �l���ɂ���J�[�\���̃T�C�Y���w�肵�܂��B
            Rectangle cursorSize = new Rectangle(0, 0, 48, 48);

            // Calculates the drawing coordinates at the four corners 
            // based on the panel rectangle.
            // 
            // �p�l���̋�`����A�l���̕`����W���v�Z���܂��B
            Vector2[] drawPosition = {
                // Left up 
                // 
                // ����
                new Vector2(
                    panelSize.X,
                    panelSize.Y),
                // Left down
                // 
                // ����
                new Vector2(
                    panelSize.X,
                    panelSize.Y + panelSize.Height - cursorSize.Height),
                // Right up 
                // 
                // �E��
                new Vector2(
                    panelSize.X + panelSize.Width - cursorSize.Width,
                    panelSize.Y),
                // Right down
                // 
                // �E��
                new Vector2(
                    panelSize.X + panelSize.Width - cursorSize.Width,
                    panelSize.Y + panelSize.Height - cursorSize.Height)
            };

            // Sets the texture source coordinates.
            // 
            // �e�N�X�`���̓]�������W��ݒ肵�܂��B
            int cursorType = (cursorSize.Height * 2) * 0;
            Rectangle[] cursorArea = {
                // Left up 
                // 
                // ����
                new Rectangle(0, cursorType,
                    cursorSize.Width, cursorSize.Height),
                // Left down
                // 
                // ����
                new Rectangle(0, cursorType + cursorSize.Height,
                    cursorSize.Width, cursorSize.Height),
                // Right up 
                // 
                // �E��
                new Rectangle(cursorSize.Width, cursorType,
                    cursorSize.Width, cursorSize.Height),
                // Right down
                // 
                // �E��
                new Rectangle(cursorSize.Width, cursorType + cursorSize.Height,
                    cursorSize.Width, cursorSize.Height)
            };

            // Draws the cursor at the designated flag location.
            // 
            // �w�肳�ꂽ�t���O�̉ӏ��̃J�[�\����`�悵�܂��B
            for (int i = 0; i < drawPosition.Length; i++)
            {
                // Processing is skipped if the draw cursor
                // flag is not specified.
                // 
                // �`�悷��J�[�\���̃t���O���w�肳��Ă��Ȃ����
                // �������X�L�b�v���܂��B
                if (((int)panelType & (1 << i)) == 0)
                    continue;

                Vector2 position;

                // Draws the cursor frame.
                // 
                // �J�[�\���̊O�g��`�悵�܂��B
                position = drawPosition[i] + panelManager.DrawOffset;
                Batch.Draw(cursorTexture, position, cursorArea[i], colors[i]);

                // Offsets the source coordinates.
                // 
                // �]�����̍��W�����炵�܂��B
                cursorArea[i].X += 96;

                // Changes the draw color.
                // 
                // �`��F��ύX���܂��B
                Vector4 color = colors[i].ToVector4();
                float radian = (float)gameTime.TotalGameTime.TotalSeconds;
                float alpha = Math.Abs((float)Math.Sin(radian));
                color.W = alpha;

                // Draws the internal cursor frame. 
                // 
                // �J�[�\���̓��g��`�悵�܂��B
                position = drawPosition[i] + panelManager.DrawOffset;
                Batch.Draw(cursorTexture, position, cursorArea[i], new Color(color));
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Abstract method for panel shuffle.
        /// Should be written in the inheritance target.
        ///
        /// �p�l���V���b�t���̒��ۃ��\�b�h�ł��B
        /// �p����ŋL�q���Ă��������B
        /// </summary>
        public abstract bool RandomShuffle();


        /// <summary>
        /// Obtains the panel action status.
        /// 
        /// �p�l���̓����Ԃ��擾���܂��B
        /// </summary>
        private bool GetPanelAction()
        {
            for (int x = 0; x < stageSetting.Divide.X; x++)
            {
                for (int y = 0; y < stageSetting.Divide.Y; y++)
                {
                    if (panelManager.GetPanel(x, y).Status !=
                        PanelData.PanelStatus.None)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Creates the panel after-image.
        /// 
        /// �c���̃p�l�����쐬���܂��B
        /// </summary>
        private PanelAfterImage CreateAfterImage(PanelData panel)
        {
            PanelAfterImage afterimage = new PanelAfterImage(Game);
            afterimage.Texture = MovieTexture;
            afterimage.Position = panel.Center + panelManager.DrawOffset;
            afterimage.Size = panel.Size;
            afterimage.Origin = panel.Origin;
            afterimage.Rotate = MathHelper.ToRadians(panel.Rotate);
            afterimage.TexturePosition = panel.TexturePosition;

            return afterimage;
        }
        #endregion
    }
}


