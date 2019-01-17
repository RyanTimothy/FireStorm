/*  GameObject.cs
 *  GameObject class for entities in a Game's Level - has a Transform which contains Position/Rotation/Scale
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.09.24: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class GameObject : MonoObject, IEquatable<GameObject>
    {
        private List<Component> Components = new List<Component>();

        private bool _static;
        private bool _active = true;
        private bool _dontDestroy;

        /// <summary>
        /// Don't Destroy on Scene change
        /// </summary>
        internal bool DontDestroy
        {
            get
            {
                if (Transform.Parent != null)
                {
                    return _dontDestroy || Transform.Parent.DontDestroy;
                }

                return _dontDestroy;
            }
            set => _dontDestroy = value;
        }

        /// <summary>
        /// A static GameObject cannot change position or rotate - setting a GameObject to static within a non-static object will remove it from the parent
        /// </summary>
        public bool Static
        {
            get => _static;
            set
            {
                if (_static != value)
                {
                    _static = value;

                    foreach (GameObject g in Transform.Children)
                    {
                        g.Static = value;
                    }

                    // TODO: remove GameObject from parent if parent is non-static
                }
            }
        }

        public bool ActiveSelf { get => _active; }

        /// <summary>
        /// Defines whether the GameObject is active in the Game - A deactivated Parent will make an active child inactive
        /// </summary>
        public bool ActiveInGame
        {
            get
            {
                if (Transform.Parent != null)
                {
                    return _active && Transform.Parent.ActiveInGame;
                }

                return _active;
            }
            internal set
            {
                if (value != _active)
                {
                    _active = value;
                }
            }
        }

        /// <summary>
        /// The layer the GameObject is in
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// The Transform attached to this GameObject
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// The tag of this GameObject
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// GameObject empty constructor will not add it to a Game's collection - use GameObject.Instantiate
        /// </summary>
        public GameObject()
        {
            // create a Transform for every GameObject
            Transform = new Transform
            {
                GameObject = this,  //assign this GameObject to the Transform
                Root = this         //assign this as its own GameObject root upon creation
            };
        }

        /// <summary>
        /// Create a GameObject with name
        /// </summary>
        public static GameObject Instantiate(GameController game, int tag = -1)
        {
            GameObject gameObject = new GameObject()
            {
                Tag = tag,
                Game = game
            };

            if (game != null)
            {
                game.AddMonoObject(gameObject);
            }

            return gameObject;
        }

        /// <summary>
        /// Adds a component class to the GameObject - must inherit ComponentController
        /// </summary>
        public Component AddComponent<T>() where T : Component, new()
        {
            Component c = new T()
            {
                GameObject = this,
                Transform = Transform,
                Game = Game
            };

            if (Game != null)
            {
                // add to Created list which won't be added to game's components until Update starts a new cycle
                Game.CreatedGameObjectComponents.Add(c);
                Game.AddMonoObject(c);
            }

            Components.Add(c);

            return c;
        }

        /// <summary>
        /// Returns the first component of class type in GameObject, null if none
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Returns the components of class type in GameObject
        /// </summary>
        public T[] GetComponents<T>() where T : Component
        {
            return Components.OfType<T>().ToArray();
        }

        /// <summary>
        /// Returns the first component of class type in all children GameObjects, null if it none
        /// </summary>
        public T GetComponentInChildren<T>() where T : Component
        {
            int childrenCount = Transform.Children.Count;

            T component;

            for (int i = 0; i < childrenCount; i++)
            {
                component = Transform.Children[i].GetComponent<T>();

                if (component != null)
                {
                    return component;
                }

                component = Transform.Children[i].GetComponentInChildren<T>();

                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// Activates/Deactivates the GameObject
        /// </summary>
        public void SetActive(bool value)
        {
            if (_active != value)
            {
                _active = value;

                int componentCount = Components.Count;

                for (int i = 0; i < componentCount; i++)
                {
                    Components[i].GameObjectActiveStateChanged();
                }

                int childrenCount = Transform.Children.Count;

                for (int i = 0; i < childrenCount; i++)
                {
                    Transform.Children[i].SetActive(value);
                }

            }
        }

        public static void DontDestroyOnLoad(GameObject gameObject, bool dontDestroy = true)
        {
            if (gameObject != null)
            {
                gameObject.DontDestroy = dontDestroy;
            }
        }

        /// <summary>
        /// Removes GameObject, children and their components
        /// </summary>
        public static void Destroy(GameObject gameObject)
        {
            // adds GameObject to the disposal which runs at end of Game's update to avoid conflict
            gameObject?.Game?.GameObjectDisposal.Add(gameObject);

            int componentCount = gameObject.Components.Count;

            for (int i = componentCount - 1; i >= 0; i--)
            {
                gameObject.Game.RemoveMonoObject(gameObject.Components[i]);
            }

            // destroy its children GameObjects too
            int childrenCount = gameObject.Transform.Children.Count;

            for (int i = childrenCount - 1; i >= 0; i--)
            {
                Destroy(gameObject.Transform.Children[i]);
            }

            // remove the GameObject from Game's collection of GameObjects
            gameObject.Game.RemoveMonoObject(gameObject);
        }

        /// <summary>
        /// Immediately Removes GameObject, children and their components - Not recommended to use, use Destroy instead.
        /// </summary>
        internal static void DestroyImmediately(GameObject gameObject)
        {
            if (gameObject != null)
            {
                int componentCount = gameObject.Components.Count;

                for (int i = componentCount - 1; i >= 0; i--)
                {
                    Component.DestroyImmediately(gameObject.Components[i]);
                }

                // destroy its children GameObjects too
                int childrenCount = gameObject.Transform.Children.Count;

                for (int i = childrenCount - 1; i >= 0; i--)
                {
                    DestroyImmediately(gameObject.Transform.Children[i]);
                }

                gameObject.Transform.Parent = null;

                // remove the GameObject from Game's collection of GameObjects
                gameObject.Game.RemoveMonoObject(gameObject);
            }
        }

        /// <summary>
        /// Collider2D has entered collision with another Collider2D
        /// </summary>
        public void OnCollisionEnter2D(Collider2D sender)
        {
            // notify the Component Controllers of collision
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnCollisionEnter2D(sender);
                }
            }

            // notify the children GameObjects of collision
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnCollisionEnter2D(sender);
            }
        }

        /// <summary>
        /// Collider2D has exited collision with another Collider2D
        /// </summary>
        public void OnCollisionExit2D(Collider2D sender)
        {
            // notify the Component Controllers of collision
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnCollisionExit2D(sender);
                }
            }

            // notify the children GameObjects of collision
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnCollisionExit2D(sender);
            }
        }

        /// <summary>
        /// Collider2D is still within a collision with another Collider2D
        /// </summary>
        public void OnCollisionStay2D(Collider2D sender)
        {
            // notify the Component Controllers of collision
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnCollisionStay2D(sender);
                }
            }

            // notify the children GameObjects of collision
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnCollisionStay2D(sender);
            }
        }

        /// <summary>
        /// Collider2D has entered trigger collision with another Collider2D
        /// </summary>
        public void OnTriggerEnter2D(Collider2D sender)
        {
            // notify the Component Controllers of trigger entry
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnTriggerEnter2D(sender);
                }
            }

            // notify the children GameObjects of trigger entry
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnTriggerEnter2D(sender);
            }
        }

        /// <summary>
        /// Collider2D is still within a trigger collision with another Collider2D
        /// </summary>
        public void OnTriggerStay2D(Collider2D sender)
        {
            // notify the Component Controllers of trigger stay
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnTriggerStay2D(sender);
                }
            }

            // notify the children GameObjects of trigger entry
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnTriggerStay2D(sender);
            }
        }

        /// <summary>
        /// Collider2D has exited trigger collision with another Collider2D
        /// </summary>
        public void OnTriggerExit2D(Collider2D sender)
        {
            // notify the Component Controllers of trigger stay
            int componentCount = Components.Count;

            for (int i = 0; i < componentCount; i++)
            {
                if (Components[i] is ComponentController)
                {
                    (Components[i] as ComponentController).OnTriggerExit2D(sender);
                }
            }

            // notify the children GameObjects of trigger entry
            int childrenCount = Transform.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                Transform.Children[i].OnTriggerExit2D(sender);
            }
        }

        public bool Equals(GameObject other)
        {
            return this == other;
        }
    }
}
