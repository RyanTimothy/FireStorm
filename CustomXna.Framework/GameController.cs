/*  GameController.cs
 *  Handles the management of the MonoObjects and the Game
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.11.01: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public abstract class GameController : Game
    {
        public GraphicsDeviceManager Graphics { get; protected set; }
        public SpriteBatch SpriteBatch { get; protected set; }
        protected Viewport Viewport { get; set; }
        public Camera Camera { get; protected set; }

        private Scene _scene;
        public Scene Scene { get => _scene; }

        private List<MonoObject> monoObjects = new List<MonoObject>();

        public List<GameObject> GameObjectDisposal { get; } = new List<GameObject>();
        public List<Component> ComponentDisposal { get; } = new List<Component>();
        // TODO: Think about having GameObject & Component extend from same class to avoid having TWO disposal lists

        public List<CanvasRenderer> CanvasRenderers { get; } = new List<CanvasRenderer>();
        public List<Collider2D> ColliderComponents { get; } = new List<Collider2D>();
        public List<Component> CreatedGameObjectComponents { get; } = new List<Component>();

        /// <summary>
        /// The Collision Layer is where you set specific layer collisions - what can collide with what
        /// </summary>
        public CollisionLayer CollisionLayer { get; } = new CollisionLayer();

        /// <summary>
        /// Handles the Physics2D and contains helper methods
        /// </summary>
        public Physics2D Physics2D { get; }

        public Type ActivevSceneType { get; set; }

        public GameController()
        {
            Physics2D = new Physics2D(this);
        }

        public void SetScene<T>() where T : Scene, new()
        {
            // dispose any creating game objects
            for (int i = CreatedGameObjectComponents.Count - 1; i >= 0; i--)
            {
                if (!CreatedGameObjectComponents[i].GameObject.DontDestroy)
                {
                    CreatedGameObjectComponents.RemoveAt(i);
                }
            }

            // add all GameObjects without the DontDestroy boolean set to the disposal list
            foreach (GameObject g in monoObjects.OfType<GameObject>())
            {
                if (!g.DontDestroy && !GameObjectDisposal.Contains(g))
                {
                    GameObjectDisposal.Add(g);
                }
            }

            // unload all content from previous scene
            _scene?.UnloadContent();

            // dispose all monoObjects to clear the scene
            DisposeMonoObjects();

            // instantiate Scene Type
            _scene = new T()
            {
                Game = this
            };

            // call OnSceneChange
            OnSceneChange();

            // assign the SceneType
            ActivevSceneType = typeof(T);

            // load all scene content
            _scene.LoadContent();

            // add all MonoObjects to the Components lists
            AddMonoObjects();

            // call Scene Start after all objects have been instantiated
            _scene.Start();
        }

        protected virtual void OnSceneChange()
        {
            
        }

        internal void AddMonoObject(MonoObject monoObject)
        {
            if (monoObject != null && !monoObjects.Contains(monoObject))
            {
                monoObjects.Add(monoObject);
            }
        }

        /// <summary>
        /// Removes MonoObject from game
        /// </summary>
        internal void RemoveMonoObject(MonoObject monoObject)
        {
            if (monoObject != null && monoObjects.Contains(monoObject))
            {
                monoObjects.Remove(monoObject);
            }
        }

        /// <summary>
        /// Returns the first loaded GameObject by tag and returns it. If no GameObject is found, null is returned.
        /// </summary>
        internal GameObject FindGameObjectByTag(int tag)
        {
            return monoObjects.FirstOrDefault(g => g is GameObject && ((GameObject)g).Tag == tag) as GameObject;
        }

        /// <summary>
        /// Returns all GameObjects by tag and returns them. If no GameObjects are found, null is returned.
        /// </summary>
        internal GameObject[] FindGameObjectsByTag(int tag)
        {
            return monoObjects.Where(g => g is GameObject && ((GameObject)g).Tag == tag).Cast<GameObject>().ToArray();
        }

        /// <summary>
        /// Returns a list of MonoObjects of type in the Game
        /// </summary>
        public T[] FindObjectsByType<T>() where T : MonoObject
        {
            return monoObjects.OfType<T>().ToArray();
        }

        /// <summary>
        /// Returns the first created MonoObjects of type in the Game
        /// </summary>
        public T FindObjectByType<T>() where T : MonoObject
        {
            return monoObjects.OfType<T>().FirstOrDefault();
        }

        public Vector3 CursorToViewportWorld(MouseState mouseState)
        {
            // source: https://stackoverflow.com/questions/11503226/c-sharp-xna-mouse-position-projected-to-3d-plane

            Vector3 nearScreenPoint = new Vector3(mouseState.X, mouseState.Y, 0);
            Vector3 farScreenPoint = new Vector3(mouseState.X, mouseState.Y, 1);

            Vector3 nearWorldPoint = Viewport.Unproject(nearScreenPoint, Camera.Projection, Camera.View, Matrix.Identity);
            Vector3 farWorldPoint = Viewport.Unproject(farScreenPoint, Camera.Projection, Camera.View, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Viewport = GraphicsDevice.Viewport;
        }

        protected override void LoadContent()
        {
            
        }

        protected override void UnloadContent()
        {
            Content.Unload();

            // TODO: Unload any non ContentManager content here
        }

        public abstract void ResetSamplerStates();

        protected override void Update(GameTime gameTime)
        {
            // run the updates for each game object component
            base.Update(gameTime);

            // run the collision checks
            Collider2D.DoCollisionChecks(this);

            // dispose MonoObjects
            DisposeMonoObjects();

            // add MonoObjects
            AddMonoObjects();

            // draw the render targets If they're dirty
            for (int i = 0; i < CanvasRenderers.Count; i++)
            {
                CanvasRenderers[i].DrawRenderTarget2D();
            }
        }

        private void AddMonoObjects()
        {
            // add the newly created game components to the game
            if (CreatedGameObjectComponents.Count > 0)
            {
                for (int i = 0; i < CreatedGameObjectComponents.Count; i++)
                {
                    if (CreatedGameObjectComponents[i] is IGameComponent)
                    {
                        Components.Add(CreatedGameObjectComponents[i] as IGameComponent);

                        if (CreatedGameObjectComponents[i] is CanvasRenderer)
                        {
                            CanvasRenderers.Add(CreatedGameObjectComponents[i] as CanvasRenderer);
                        }
                    }
                    else
                    {
                        if (CreatedGameObjectComponents[i] is Collider2D)
                        {
                            ColliderComponents.Add(CreatedGameObjectComponents[i] as Collider2D);
                        }

                        CreatedGameObjectComponents[i].Start();
                    }
                }

                CreatedGameObjectComponents.Clear();
            }
        }

        private void DisposeMonoObjects()
        {
            // run the GameObject removal
            if (GameObjectDisposal.Count > 0)
            {
                for (int i = GameObjectDisposal.Count - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediately(GameObjectDisposal[i]);
                }

                GameObjectDisposal.Clear();
            }

            // run the Component removal
            if (ComponentDisposal.Count > 0)
            {
                for (int i = ComponentDisposal.Count - 1; i >= 0; i--)
                {
                    Component.DestroyImmediately(ComponentDisposal[i]);
                }

                ComponentDisposal.Clear();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}