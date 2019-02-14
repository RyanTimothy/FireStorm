/*  Physics2D.cs
 *  Handles raycast of all physics
 *  Unimplemented due to time: it was going to handle all collsions in a spatial hash for efficient collision checks
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
    public sealed class Physics2D
    {
        private GameController game;

        internal Physics2D()
        {
        }

        /// <summary>
        /// Physics2DManager handles all 2D collisions and 2D collision layers
        /// </summary>
        internal Physics2D(GameController game)
        {
            this.game = game;
        }

        internal Collider2D Raycast(GameController game, Vector2 origin, Vector2 direction, float maxDistance) //, float rayWidth)
        {
            Collider2D colliderHit = null;

            if (game != null)
            {
                Vector2 final = origin + direction * maxDistance;

                float left = Math.Min(origin.X, final.X);
                float right = Math.Max(origin.X, final.X);
                float bottom = Math.Min(origin.Y, final.Y);
                float top = Math.Max(origin.Y, final.Y);

                int colliderCount = game.ColliderComponents.Count;

                Vector2[] points = new Vector2[4];
                points[0] = origin;
                points[1] = final;

                for (int i = 0; i < colliderCount; i++)
                {
                    if (game.ColliderComponents[i] is BoxCollider2D boxCollider2D)
                    {
                        if (Collider2D.IsColliding(boxCollider2D, points))
                        {
                            if (colliderHit == null || Vector2.DistanceSquared(origin, boxCollider2D.Center) < Vector2.DistanceSquared(origin, colliderHit.Center))
                            {
                                colliderHit = boxCollider2D;
                            }
                        }
                    }
                }
            }

            return colliderHit;
        }
    }
}
