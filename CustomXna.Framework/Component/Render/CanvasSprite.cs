/*  CanvasSprite.cs
 *  Sprite class for Canvas Renderer
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
    public class CanvasSprite : CanvasEntity
    {
        private Texture2D _texture;

        public Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;

                canvasRenderer?.SetDirty();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && _texture != null)
            {
                spriteBatch?.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height), Color);
            }
        }
    }
}