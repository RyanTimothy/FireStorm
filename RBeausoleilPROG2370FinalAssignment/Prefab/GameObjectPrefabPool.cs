/*  GameObjectPoolManager.cs
 *  Pool manager for handling GameObjects
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.10.26: Created
 */
using CustomXna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public class GameObjectPrefabPool
    {
        private readonly Dictionary<PrefabType, Stack<GameObject>> pool = new Dictionary<PrefabType, Stack<GameObject>>();
        private int _maxPooled = 50;
        private readonly GameController game;

        public int MaxPooled { get; set; }

        public GameObjectPrefabPool(GameController game)
        {
            this.game = game;
        }

        public bool ContainsType(PrefabType type)
        {
            return pool.ContainsKey(type);
        }

        /// <summary>
        /// Return GameObject to pool
        /// </summary>
        public void Return(PrefabType type, GameObject gameObject)
        {
            if (gameObject != null)
            {
                if (!pool.ContainsKey(type))
                {
                    pool.Add(type, new Stack<GameObject>());
                }

                // if pool doesn't already contain this gameObject
                if (!pool[type].Contains(gameObject))
                {
                    // if less than max, keep object
                    if (pool[type].Count < _maxPooled)
                    {
                        gameObject.Transform.Parent = null;
                        gameObject.SetActive(false);

                        // set the gameObject to not destroy when its stored inside
                        GameObject.DontDestroyOnLoad(gameObject, true);
                        pool[type].Push(gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(gameObject);
                    }
                }
                else // if pool already contains this gameObject, set it's active state to false just to be sure
                {
                    gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Take GameObject from pool - returns null if Type prefab doesn't exist
        /// </summary>
        public GameObject Take(PrefabType type)
        {
            GameObject gameObject = null;

            if (!pool.ContainsKey(type))
            {
                pool.Add(type, new Stack<GameObject>());
            }

            // pull from pool
            if (pool[type].Count > 0)
            {
                gameObject = pool[type].Pop();

                GameObject.DontDestroyOnLoad(gameObject, false);
            }
            else // create GameObject
            {
                gameObject = GameObjectPrefabManager.InstantiateType(type, game);
            }

            return gameObject;
        }

        /// <summary>
        /// Returns if pool contains the PrefabType
        /// </summary>
        public bool ContainsPrefabType(PrefabType type)
        {
            return pool.ContainsKey(type);
        }

        /// <summary>
        /// Initialize Prefab - preload
        /// </summary>
        public void InitializePrefab(PrefabType prefab)
        {
            if (!pool.ContainsKey(prefab))
            {
                pool.Add(prefab, new Stack<GameObject>());
            }

            GameObject gameObject = GameObjectPrefabManager.InstantiateType(prefab, game);

            if (gameObject != null)
            {
                gameObject.SetActive(false);

                pool[prefab].Push(gameObject);
            }
        }
    }
}