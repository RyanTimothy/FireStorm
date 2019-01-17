/*  Renderer.cs
 *  Base class and general functionality for all renderers. It's what makes an object appear on screen.
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.9.24: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public abstract class Renderer : Component, IGameComponent, IDrawable
    {
        private int _drawOrder;
        private bool _initialized = false;
        private bool _visible = true;

        private event EventHandler<EventArgs> _drawOrderChanged;
        protected event EventHandler<EventArgs> _visibleChanged;

        public bool Visible
        {
            get { return ActiveAndVisible; }
            set
            {
                if (value != _visible)
                {
                    _visible = value;

                    _visibleChanged?.Invoke(this, null);

                    OnVisibleChanged(this, null);
                }
            }
        }

        public bool ActiveAndVisible
        {
            get
            {
                return _visible && GameObject.ActiveInGame;
            }
        }

        internal override void GameObjectActiveStateChanged()
        {
            _visibleChanged?.Invoke(this, null);

            base.GameObjectActiveStateChanged();
        }

        event EventHandler<EventArgs> IDrawable.DrawOrderChanged
        {
            add
            {
                _drawOrderChanged += value;
            }
            remove
            {
                _drawOrderChanged -= value;
            }
        }

        event EventHandler<EventArgs> IDrawable.VisibleChanged
        {
            add
            {
                _visibleChanged += value;
            }
            remove
            {
                _visibleChanged -= value;
            }
        }

        public event EventHandler<EventArgs> VisibleChanged
        {
            add
            {
                _visibleChanged += value;
            }
            remove
            {
                _visibleChanged -= value;
            }
        }

        public bool Initialized { get => _initialized; }

        public Renderer() : base()
        {
        }

        #region IGameComponent Members
        /// <summary>
        /// Initialize game component when added to Games' Components list - IGameComponent
        /// </summary>
        public void Initialize()
        {
            if (!_initialized)
            {
                LoadContent();
                _initialized = true;

                Start();
            }
        }

        protected virtual void LoadContent()
        {
        }
        #endregion

        #region IDrawable Members

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                _drawOrderChanged?.Invoke(this, null);

                OnDrawOrderChanged(this, null);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
        {
        }

        protected virtual void OnVisibleChanged(object sender, EventArgs args)
        {
        }

        #endregion
    }
}
