/*  CanvasEntity.cs
 *  Base class for Canvas Renderer entities
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
    public abstract class CanvasEntity : Component
    {
        private float _width = 128;
        private float _height = 128;

        private bool _visible = true;

        private Color _color = Color.White;
        private Vector2 _position;

        protected CanvasRenderer canvasRenderer;

        public float Width
        {
            get => _width;
            set
            {
                _width = value;

                canvasRenderer?.SetDirty();
            }
        }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;

                canvasRenderer?.SetDirty();
            }
        }
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                canvasRenderer?.SetDirty();
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;

                canvasRenderer?.SetDirty();
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                canvasRenderer?.SetDirty();
            }
        }

        public override void Start()
        {
            canvasRenderer = GameObject.GetComponent<CanvasRenderer>() as CanvasRenderer;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
