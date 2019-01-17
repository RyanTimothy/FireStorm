/*  CanvasText.cs
 *  Handles the text rendering for the Canvas Renderer
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class CanvasText : CanvasEntity
    {
        private SpriteFont _spriteFont;
        private string _text;

        public SpriteFont SpriteFont
        {
            get => _spriteFont;
            set
            {
                _spriteFont = value;

                canvasRenderer?.SetDirty();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                canvasRenderer?.SetDirty();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible && _spriteFont != null)
            {
                spriteBatch?.DrawString(_spriteFont, _text, Position, Color);
            }
        }
    }
}