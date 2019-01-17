/*  CanvasRenderer.cs
 *  Canvas Renderer for UI
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public sealed class CanvasRenderer : Renderer
    {
        private RenderTarget2D renderTarget2D;

        private BasicEffect _basicEffect;
        private VertexPositionTexture[] vertexPositionTexture;

        private Vector2 _halfExtents = new Vector2(64, 64);

        private int _bufferWidth = 128;
        private int _bufferHeight = 128;

        private float _width = 10;
        private float _height = 10;

        private float _masterAlpha = 1;

        private bool isDirty;

        /// <summary>
        /// The master alpha of the particles between 0 and 1 (Zero is invisible)
        /// </summary>
        public float MasterAlpha
        {
            get => _masterAlpha;
            set => _masterAlpha = Math.Max(0, Math.Min(1, value));
        }

        public int BufferWidth
        {
            get => _bufferWidth;
            set
            {
                _bufferWidth = Math.Abs(value);
                GenerateRenderTarget2D();
            }
        }

        public int BufferHeight
        {
            get => _bufferHeight;
            set
            {
                _bufferHeight = Math.Abs(value);
                GenerateRenderTarget2D();
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = Math.Abs(value);
                _halfExtents.X = _width / 2;
                GenerateVertexPositionTexture();
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = Math.Abs(value);
                _halfExtents.Y = _height / 2;
                GenerateVertexPositionTexture();
            }
        }

        public Vector2 HalfExtents { get => _halfExtents; }

        public bool IsDirty { get => isDirty; }

        public RenderMode RenderMode { get; set; }

        public Color ClearColor { get; set; } = Color.Transparent;

        private static readonly Vector3[] vertexPositions =
        {
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),  
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0)
        };

        public CanvasRenderer() : base()
        {
        }

        internal void SetDirty()
        {
            isDirty = true;
        }

        private void GenerateRenderTarget2D()
        {
            renderTarget2D = new RenderTarget2D(Game.GraphicsDevice, _bufferWidth, _bufferHeight, false, Game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        private void GenerateVertexPositionTexture()
        {
            Vector3 vertexTileRatio = new Vector3(_width, _height, 0);

            vertexPositionTexture = new VertexPositionTexture[6]
                {
                    new VertexPositionTexture { Position = vertexPositions[0] * vertexTileRatio, TextureCoordinate = new Vector2(0, 0) },
                    new VertexPositionTexture { Position = vertexPositions[1] * vertexTileRatio, TextureCoordinate = new Vector2(1, 0) },
                    new VertexPositionTexture { Position = vertexPositions[2] * vertexTileRatio, TextureCoordinate = new Vector2(1, 1) },
                    new VertexPositionTexture { Position = vertexPositions[3] * vertexTileRatio, TextureCoordinate = new Vector2(0, 0) },
                    new VertexPositionTexture { Position = vertexPositions[4] * vertexTileRatio, TextureCoordinate = new Vector2(1, 1) },
                    new VertexPositionTexture { Position = vertexPositions[5] * vertexTileRatio, TextureCoordinate = new Vector2(0, 1) }
                };
        }

        public override void Start()
        {
            GenerateRenderTarget2D();
            GenerateVertexPositionTexture();

            _basicEffect = new BasicEffect(GameObject.Game.GraphicsDevice)
            {
                TextureEnabled = true,
                Texture = renderTarget2D
            };

            SetDirty();
        }

        public void DrawRenderTarget2D()
        {
            if (Initialized && isDirty)
            {
                CanvasEntity[] canvasEntities = GameObject.GetComponents<CanvasEntity>();

                if (canvasEntities.Length > 0)
                {
                    Visible = true;

                    RenderTargetBinding[] originRenderTargets = Game.GraphicsDevice.GetRenderTargets();

                    // draw to the UI Components to the RenderTarget2D
                    Game.GraphicsDevice.SetRenderTarget(renderTarget2D);
                    Game.GraphicsDevice.Clear(ClearColor);

                    Game.SpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp);
                    for (int i = 0; i < canvasEntities.Length; i++)
                    {
                        canvasEntities[i].Draw(Game.SpriteBatch);
                    }
                    Game.SpriteBatch.End();

                    Game.GraphicsDevice.SetRenderTargets(originRenderTargets);
                    Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    Game.ResetSamplerStates();

                    isDirty = false;
                }
                else
                {
                    Visible = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (renderTarget2D != null)
            {
                //Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                if (RenderMode == RenderMode.WorldSpace)
                {
                    Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                }
                else if (RenderMode == RenderMode.ScreenSpaceOverlay)
                {
                    Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
                }

                // set the BasicEffect's View & Projection from the Game Camera
                _basicEffect.View = Game.Camera.View;
                _basicEffect.Projection = Game.Camera.Projection;
                _basicEffect.World = Transform.GetWorldMatrix();

                _basicEffect.Alpha = _masterAlpha;

                // iterate through the passes that may be required to draw the primitive
                foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                {
                    // apply the effectpass for the current technique - must be done before assigning World position to the BasicEffect
                    pass.Apply();

                    // draw the billboard particle
                    Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionTexture, 0, 2);
                }

                GameObject.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GameObject.Game.GraphicsDevice.BlendState = BlendState.Opaque;
            }
        }
    }
}