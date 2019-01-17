/*  Scene.cs
 *  Use this for loading/unloading each level
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public abstract class Scene : MonoObject
    {
        protected internal Scene()
        {
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void UnloadContent()
        {
        }
    }
}