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
        //private readonly Dictionary<int, HashSet<Collider2D>> _bucket = new Dictionary<int, HashSet<Collider2D>>();

        //private readonly HashSet<Collider2D> awakeColliders = new HashSet<Collider2D>();

        //private Vector2 _topLeft;
        //private Vector2 _bottomRight;

        //private Vector2 _worldBoundsCenter;
        //private Vector2 _worldBoundsExtents = new Vector2(256, 256);
        //private Point _worldBucketSize;
        //private int _worldSubdivisions = 32;

        //private Vector2 _cellSize;

        private GameController game;

        ///// <summary>
        ///// The world bounds of collision detection - anything beyond these bounds will not register collision
        ///// </summary>
        //public Vector2 WorldBoundsExtents
        //{
        //    get => _worldBoundsExtents;
        //    set
        //    {
        //        _worldBoundsExtents = new Vector2(Math.Abs(value.X), Math.Abs(value.Y));

        //        GetWorldPositions();
        //        GetCellSize();
        //    }
        //}

        ///// <summary>
        ///// The center of the world bounds extents
        ///// </summary>
        //public Vector2 WorldBoundsCenter
        //{
        //    get => _worldBoundsCenter;
        //    set
        //    {
        //        _worldBoundsCenter = value;

        //        GetWorldPositions();
        //    }
        //}

        ///// <summary>
        ///// The amount of divisions in the spatial hash - less divisions is more efficient for larger Colliders and higher is more efficient for smaller colliders
        ///// </summary>
        //public int WorldSubdivisions
        //{
        //    get => _worldSubdivisions;
        //    set
        //    {
        //        _worldSubdivisions = Math.Max(1, value);

        //        //GetCellSize();
        //    }
        //}

        internal Physics2D()
        {
        }

        /// <summary>
        /// Physics2DManager handles all 2D collisions and 2D collision layers
        /// </summary>
        internal Physics2D(GameController game)
        {
            this.game = game;

            //GetCellSize();
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
            
                //Vector2 leftHand = Vector2.Normalize(new Vector2(origin.Y, -origin.X));
                //Vector2 rightHand = Vector2.Normalize(new Vector2(-origin.Y, origin.X));

                //points[0] = origin + leftHand * (rayWidth * 0.5f);
                //points[1] = origin + rightHand * (rayWidth * 0.5f);

                //points[2] = final + rightHand * (rayWidth * 0.5f);
                //points[3] = final + leftHand * (rayWidth * 0.5f);

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

        //private void AddToCell(Collider2D collider2D)
        //{
        //    if (collider2D.Right > _topLeft.X && collider2D.Left < _bottomRight.X && collider2D.Bottom < _topLeft.Y && collider2D.Top > _bottomRight.Y)
        //    {
        //        int x = (int)Math.Floor(_topLeft.X - collider2D.Center.X / _cellSize.X);
        //        int y = (int)Math.Floor(_topLeft.Y - collider2D.Center.Y / _cellSize.Y);

        //        int i = x + y * _worldBucketSize.X;

        //        if (!_bucket.ContainsKey(i))
        //        {
        //            _bucket.Add(i, new HashSet<Collider2D>());
        //        }

        //        _bucket[i].Add(collider2D);
        //    }
        //}

        //internal void AddAwakenedCollider(Collider2D collider2D)
        //{
        //    if (!awakeColliders.Contains(collider2D))
        //    {
        //        awakeColliders.Add(collider2D);
        //    }

        //    AddToCell(collider2D);
        //}

        //internal void RemoveSleepingCollider(Collider2D collider2D)
        //{
        //    awakeColliders.Remove(collider2D);
        //}

        //internal void RemoveCollider2D(Collider2D collider2D)
        //{
        //    for (int i = 0; i < _bucket.Count; i++)
        //    {
        //        _bucket[i].Remove(collider2D);
        //    }
        //}

        //private void GetWorldPositions()
        //{
        //    _topLeft = new Vector2(_worldBoundsCenter.X - _worldBoundsExtents.X, _worldBoundsCenter.Y + _worldBoundsExtents.Y);
        //    _bottomRight = new Vector2(_worldBoundsCenter.X + _worldBoundsExtents.X, _worldBoundsCenter.Y - _worldBoundsExtents.Y);
        //}

        //private void GetCellSize()
        //{
        //    _cellSize = new Vector2((_worldBoundsExtents.X * 2) / _worldSubdivisions, (_worldBoundsExtents.Y * 2) / _worldSubdivisions);

        //    _worldBucketSize = new Point((int)(_worldBoundsExtents.X / _worldSubdivisions), (int)(_worldBoundsExtents.Y / _worldSubdivisions));
        //}
    }
}