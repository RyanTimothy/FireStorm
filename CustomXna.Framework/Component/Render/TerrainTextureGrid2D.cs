/*  TerrainTextureGrid2D.cs
 *  Handles the drawing of the terrain grid mesh
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomXna.Framework
{
    public class TerrainTextureGrid2D : Renderer
    {
        private BasicEffect _basicEffect;
        private int _textureSetX = 1;
        private int _textureSetY = 1;

        private int gridWidth;
        private int gridHeight;
        private ushort?[,] _grid = new ushort?[1, 1];
        private Texture2D _texture;
        VertexPositionTexture[] vertexPositionTexture;

        public int TextureSetX
        {
            get => _textureSetX;
            set => _textureSetX = Math.Max(1, value); // must be minimum of 1
        }

        public int TextureSetY
        {
            get => _textureSetY;
            set => _textureSetY = Math.Max(1, value); // must be minimum of 1
        }

        /// <summary>
        /// The BasicEffect
        /// </summary>
        public BasicEffect BasicEffect
        {
            get;
            set; // TODO: Prevent nullable assignment
        }

        /// <summary>
        /// The texture set of the grid
        /// </summary>
        public Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
            }
        }

        public TerrainTextureGrid2D()
        {
            
        }

        public void SetGrid(ushort?[,] grid)
        {
            _grid = grid;
        }

        public override void Start()
        {
            if (GameObject.Game != null)
            {
                _basicEffect = new BasicEffect(GameObject.Game.GraphicsDevice)
                {
                    TextureEnabled = true,
                    Texture = Texture
                };

            }

            // 6 verts per texture square
            vertexPositionTexture = new VertexPositionTexture[_grid.GetLength(0) * _grid.GetLength(1) * 6];

            float texWidth = 1f / _textureSetX;
            float textHeight = 1f / _textureSetY;

            for (int x = 0; x < _grid.GetLength(1); x++)
            {
                for (int y = 0; y < _grid.GetLength(0); y++)
                {
                    if (_grid[y, x] != null && _grid[y, x] < TextureSetX * TextureSetY)
                    {
                        int i = (x * _grid.GetLength(1) + y) * 6;

                        int value = (int)_grid[y, x];

                        float texStartX = ((int)_grid[y, x] % TextureSetY) * texWidth;
                        float texStartY = ((int)_grid[y, x] / TextureSetY) * textHeight;

                        vertexPositionTexture[i] = new VertexPositionTexture(new Vector3(x, -y, 0), new Vector2(texStartX + 0.001f, texStartY + 0.001f));
                        vertexPositionTexture[i + 1] = new VertexPositionTexture(new Vector3(x + 1, -y, 0), new Vector2(texStartX + texWidth - 0.001f, texStartY + 0.001f));
                        vertexPositionTexture[i + 2] = new VertexPositionTexture(new Vector3(x, -(y + 1), 0), new Vector2(texStartX + 0.001f, texStartY + textHeight - 0.001f));

                        vertexPositionTexture[i + 3] = new VertexPositionTexture(new Vector3(x, -(y + 1), 0), new Vector2(texStartX + 0.001f, texStartY + textHeight - 0.001f));
                        vertexPositionTexture[i + 4] = new VertexPositionTexture(new Vector3(x + 1, -y, 0), new Vector2(texStartX + texWidth - 0.001f, texStartY + 0.001f));
                        vertexPositionTexture[i + 5] = new VertexPositionTexture(new Vector3(x + 1, -(y + 1), 0), new Vector2(texStartX + texWidth - 0.001f, texStartY + textHeight - 0.001f));
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float textureCoordinateWidth = 1f / _textureSetX;
            float textureCoordinateHeight = 1f / _textureSetY;

            _basicEffect.FogEnabled = Game.Camera.FogEnabled;
            _basicEffect.FogColor = Game.Camera.FogColor;
            _basicEffect.FogStart = Game.Camera.FogStart;
            _basicEffect.FogEnd = Game.Camera.FogEnd;

            _basicEffect.View = GameObject.Game.Camera.View;
            _basicEffect.Projection = GameObject.Game.Camera.Projection;
            _basicEffect.World = Transform.GetWorldMatrix();

            int primitiveCount = _grid.GetLength(0) * _grid.GetLength(1) * 2;

            // iterate through the passes that may be required to draw the primitive
            foreach (EffectPass effect in _basicEffect.CurrentTechnique.Passes)
            {
                // apply the effectpass for the current technique - must be done before assigning World position to the BasicEffect
                effect.Apply();

                // draw the mesh
                GameObject.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexPositionTexture, 0, primitiveCount);
            }
        }

        public int? GetTileType(int x, int y)
        {
            if (_grid != null && x >= 0 && y >= 0 && x < _grid.GetLength(1) && y < _grid.GetLength(0))
            {
                return _grid[y, x];
            }

            return null;
        }
    }
}