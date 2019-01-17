/*  CanvasButton.cs
 *  Buttons for Canvas Renderer
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
    public class CanvasButton : CanvasSprite
    {
        private bool _isHovered;
        private Texture2D _hoverTexture;
        private Texture2D _normalTexture;

        private event EventHandler<EventArgs> _clicked;

        public bool IsHovered
        {
            get => _isHovered;
            internal set
            {
                if (_isHovered != value)
                {
                    _isHovered = value;

                    if (_isHovered)
                    {
                        base.Texture = HoverTexture;
                    }
                    else
                    {
                        base.Texture = NormalTexture;
                    }
                }
            }
        }

        public Texture2D HoverTexture
        {
            get => _hoverTexture;
            set
            {
                _hoverTexture = value;

                if (IsHovered)
                {
                    canvasRenderer?.SetDirty();
                }
            }
        }

        public Texture2D NormalTexture
        {
            get => _normalTexture;
            set
            {
                _normalTexture = value;

                if (!IsHovered)
                {
                    canvasRenderer?.SetDirty();
                }
            }
        }

        public new Texture2D Texture
        {
            get => Texture;
        }

        public event EventHandler<EventArgs> Clicked
        {
            add
            {
                _clicked += value;
            }
            remove
            {
                _clicked -= value;
            }
        }

        public void Click()
        {
            _clicked?.Invoke(this, null);
        }

        public override void Start()
        {
            base.Texture = NormalTexture;
            base.Start();
        }
    }
}