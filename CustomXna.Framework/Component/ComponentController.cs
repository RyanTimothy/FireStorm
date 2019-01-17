/*  ComponentController.cs
 *  ComponentControllers is the base class from which every GameObject's components script derives. 
 *  The ComponentController handles Start, Update, and various events.
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.9.24: Created
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
    public abstract class ComponentController : Component, IGameComponent, IUpdateable, IComparable<ComponentController>
    {
        private int _updateOrder;
        private bool _initialized = false;

        private event EventHandler<EventArgs> _updateOrderChanged;

        #region Properties

        public int UpdateOrder
        {
            get
            {
                return _updateOrder;
            }
            set
            {
                _updateOrder = value;
                _updateOrderChanged?.Invoke(this, null);

                OnUpdateOrderChanged(this, null);
            }
        }

        public bool Initialized { get => _initialized; }

        public void Initialize()
        {
            if (!_initialized)
            {
                LoadContent();
                _initialized = true;

                Start();
            }
        }
        #endregion

        #region IUpdateable EventHandlers - EnableChanged & UpdateOrder
        event EventHandler<EventArgs> IUpdateable.EnabledChanged
        {
            add
            {
                _enabledChanged += value;
            }
            remove
            {
                _enabledChanged -= value;
            }
        }

        event EventHandler<EventArgs> IUpdateable.UpdateOrderChanged
        {
            add
            {
                _updateOrderChanged += value;
            }
            remove
            {
                _updateOrderChanged -= value;
            }
        }
        #endregion

        public ComponentController()
        {

        }

        public Collider2D Raycast2D(Vector2 origin, Vector2 direction, float maxDistance = 100) //, float rayWidth = 0)
        {
            return Game.Physics2D.Raycast(Game, origin, direction, maxDistance); //, Math.Max(0, rayWidth));
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Collider2D has entered collision with another Collider2D
        /// </summary>
        public virtual void OnCollisionEnter2D(Collider2D sender)
        {

        }

        /// <summary>
        /// Collider2D has exited collision with another Collider2D
        /// </summary>
        public virtual void OnCollisionExit2D(Collider2D sender)
        {

        }

        /// <summary>
        /// Collider2D has a continued collision with Collider
        /// </summary>
        public virtual void OnCollisionStay2D(Collider2D sender)
        {

        }

        /// <summary>
        /// Collider2D has entered trigger collision with another Collider2D
        /// </summary>
        public virtual void OnTriggerEnter2D(Collider2D sender)
        {

        }

        /// <summary>
        /// Collider2D has exited trigger collision with another Collider2D
        /// </summary>
        public virtual void OnTriggerExit2D(Collider2D sender)
        {

        }

        /// <summary>
        /// Collider2D has a continued trigger collision with another Collider2D
        /// </summary>
        public virtual void OnTriggerStay2D(Collider2D sender)
        {

        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void LoadGraphicsContent(bool loadContent)
        {

        }

        protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
        {

        }

        #region IComparable<ComponentController> Members

        public int CompareTo(ComponentController other)
        {
            return other.UpdateOrder - this.UpdateOrder;
        }
        
        #endregion
        
    }
}