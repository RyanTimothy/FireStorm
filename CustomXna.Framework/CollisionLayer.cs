/*  CollisionLayer.cs
 *  Keeps the collision layers of what can collide with what
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
    public class CollisionLayer
    {
        private Dictionary<int, HashSet<int>> collisionLayers = new Dictionary<int, HashSet<int>>();

        public void AddCollisions(int layerA, int layerB)
        {
            if (!collisionLayers.ContainsKey(layerA))
            {
                collisionLayers.Add(layerA, new HashSet<int>());
            }

            if (!collisionLayers[layerA].Contains(layerB))
            {
                collisionLayers[layerA].Add(layerB);
            }

            if (!collisionLayers.ContainsKey(layerB))
            {
                collisionLayers.Add(layerB, new HashSet<int>());
            }

            if (!collisionLayers[layerB].Contains(layerA))
            {
                collisionLayers[layerB].Add(layerA);
            }
        }

        public bool IsLayerCollidable (int layerA, int layerB)
        {
            if (collisionLayers.ContainsKey(layerA))
            {
                if (collisionLayers[layerA].Contains(layerB))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
