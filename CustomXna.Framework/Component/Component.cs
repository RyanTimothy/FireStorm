/*  Component.cs
 *  Base class for everything attached to GameObjects.
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
    public abstract class Component : MonoObject, IDisposable
    {
        private bool _enabled = true;

        protected Transform _transform;
        protected GameObject _gameObject;

        protected event EventHandler<EventArgs> _enabledChanged;

        /// <summary>
        /// The Transform attached to this GameObject
        /// </summary>
        public Transform Transform { get => _transform; internal set => _transform = value; }

        public GameObject GameObject
        {
            get => _gameObject;
            internal set => _gameObject = value;
        }

        public bool ActiveAndEnabled
        {
            get
            {
                return _enabled && GameObject.ActiveInGame;
            }
        }

        /// <summary>
        /// When Enabled it will perform updates or drawing or collisions
        /// </summary>
        public bool Enabled
        {
            get
            {
                return ActiveAndEnabled;
            }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;

                    GameObjectActiveStateChanged();
                }
            }
        }

        internal virtual void GameObjectActiveStateChanged()
        {
            _enabledChanged?.Invoke(this, null);

            if (ActiveAndEnabled)
            {
                OnEnable();
            }
            else if (!ActiveAndEnabled)
            {
                OnDisable();
            }
        }

        /// <summary>
        /// Called when ComponentController is enabled
        /// </summary>
        protected virtual void OnEnable()
        {

        }

        /// <summary>
        /// Called when ComponentController is disabled
        /// </summary>
        protected virtual void OnDisable()
        {

        }

        public virtual void Start()
        {

        }

        /// <summary>
        /// Shuts down the component.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// Shuts down the component.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static void Destroy(Component component)
        {
            if (component != null)
            {
                component.Enabled = false;

                component.GameObject.Game.ComponentDisposal.Add(component);
            }
        }

        internal static void DestroyImmediately(Component component)
        {
            if (component != null)
            {
                if (component is IGameComponent)
                {
                    component.Game.Components.Remove(component as IGameComponent);
                }
                else if (component is Collider2D)
                {
                    (component as Collider2D).RemoveCollisions();

                    component.Game.ColliderComponents.Remove(component as Collider2D);
                }
                else if (component is CanvasRenderer)
                {
                    component.Game.CanvasRenderers.Remove(component as CanvasRenderer);
                }

                component.Game.RemoveMonoObject(component);
            }
        }
    }
}