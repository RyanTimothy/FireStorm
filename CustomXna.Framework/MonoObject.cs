/*  MonoObject.cs
 *  The base class for all entities in the game world
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class MonoObject
    {
        /// <summary>
        /// The Game the GameObject is in 
        /// </summary>
        public GameController Game { get; internal set; }

        /// <summary>
        /// Returns the first loaded GameObject by tag and returns it. If no GameObject is found, null is returned.
        /// </summary>
        public GameObject FindGameObjectByTag(int tag)
        {
            if (Game != null)
            {
                return Game.FindGameObjectByTag(tag);
            }

            return null;
        }

        /// <summary>
        /// Returns the first loaded GameObject by tag and returns it. If no GameObject is found, null is returned.
        /// </summary>
        public GameObject[] FindGameObjectsByTag(int tag)
        {
            if (Game != null)
            {
                return Game.FindGameObjectsByTag(tag);
            }

            return null;
        }

        /// <summary>
        /// Returns the first created MonoObjects of type in the Game
        /// </summary>
        public T FindObjectByType<T>() where T : MonoObject
        {
            return Game.FindObjectByType<T>();
        }

        /// <summary>
        /// Returns a list of MonoObjects of type in the Game
        /// </summary>
        public T[] FindObjectsByType<T>() where T : MonoObject
        {
            return Game.FindObjectsByType<T>().ToArray();
        }
    }
}