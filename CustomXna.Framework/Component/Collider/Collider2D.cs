/*  Collider2D.cs
 *  Parent class for 2D collider types - stores which collisions were recorded
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.10.01: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public abstract class Collider2D : Component
    {
        private HashSet<Collider2D> collisions = new HashSet<Collider2D>();

        protected Vector2 _offset;

        private bool transformChanged = true;

        protected float _leftX;
        protected float _rightX;
        protected float _topY;
        protected float _bottomY;

        internal float Left { get => _leftX; }
        internal float Right { get => _rightX; }
        internal float Top { get => _topY; }
        internal float Bottom { get => _bottomY; }

        public bool IsTrigger { get; set; }

        protected Vector2 _center;

        protected Vector2[] cornersWorld = new Vector2[4];

        /// <summary>
        /// The World center of the collider
        /// </summary>
        public Vector2 Center
        {
            get => _center;
        }

        /// <summary>
        /// The local offset of the collider geometry
        /// </summary>
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;

                if (GameObject != null && GameObject.ActiveSelf)
                {
                    UpdateBounds();
                    
                    _center = new Vector2(Transform.Position.X + _offset.X, Transform.Position.Y + _offset.Y);
                }
            }
        }

        public Collider2D()
        {
            
        }

        protected override void OnEnable()
        {
            _transform.TransformChanged += TransformChanged;

            TransformChanged(this, null);
        }

        protected override void OnDisable()
        {
            RemoveCollisions();

            _transform.TransformChanged -= TransformChanged;

            //GameObject.Game.Physics2DManager.RemoveSleepingCollider(this);
        }

        internal void RemoveCollisions()
        {
            // remove all collisions on the other collider
            foreach (Collider2D c in collisions)
            {
                // notify of collision exit
                c.GameObject.Transform.Root.OnCollisionExit2D(this);

                c.collisions.Remove(this);
            }

            collisions.Clear();
        }

        public override void Start()
        {
            _transform.TransformChanged += TransformChanged;

            UpdateBounds();

            _center = new Vector2(Transform.Position.X + _offset.X, Transform.Position.Y + _offset.Y);
        }

        protected void TransformChanged(object sender, EventArgs e)
        {
            transformChanged = true;

            UpdateBounds();

            _center = new Vector2(Transform.Position.X + _offset.X, Transform.Position.Y + _offset.Y);

            //GameObject.Game.Physics2DManager.AddAwakenedCollider(this);
        }

        public virtual void UpdateBounds()
        {
            
        }

        private static void NotifyComponentControllersOfCollision(GameObject gameObject, Collider2D collider, CollisionType collisionType)
        {
            switch (collisionType)
            {
                case CollisionType.CollisionEnter:
                    if (collider.IsTrigger)
                    {
                        gameObject.OnTriggerEnter2D(collider);
                    }
                    else
                    {
                        gameObject.OnCollisionEnter2D(collider);
                    }
                    break;
                case CollisionType.CollisionStay:
                    if (collider.IsTrigger)
                    {
                        gameObject.OnTriggerStay2D(collider);
                    }
                    else
                    {
                        gameObject.OnCollisionStay2D(collider);
                    }
                    break;
                case CollisionType.CollisionExit:
                    if (collider.IsTrigger)
                    {
                        gameObject.OnTriggerExit2D(collider);
                    }
                    else
                    {
                        gameObject.OnCollisionExit2D(collider);
                    }
                    break;
            }
        }

        private static void DoCollisionNotification(GameController game, Collider2D a, Collider2D b, bool collision)
        {
            if (game != null)
            {
                bool collisionExists = a.collisions.Contains(b);

                if (collisionExists && !collision) // do OnCollisionExit
                {
                    a.collisions.Remove(b);
                    b.collisions.Remove(a);

                    NotifyComponentControllersOfCollision(a.Transform.Root, b, CollisionType.CollisionExit);
                    NotifyComponentControllersOfCollision(b.Transform.Root, a, CollisionType.CollisionExit);
                }
                else if (collisionExists && collision) // do OnCollisionStay
                {
                    NotifyComponentControllersOfCollision(a.Transform.Root, b, CollisionType.CollisionStay);
                    NotifyComponentControllersOfCollision(b.Transform.Root, a, CollisionType.CollisionStay);
                }
                else if (!collisionExists && collision) // do OnCollisionEnter
                {
                    a.collisions.Add(b);
                    b.collisions.Add(a);

                    NotifyComponentControllersOfCollision(a.Transform.Root, b, CollisionType.CollisionEnter);
                    NotifyComponentControllersOfCollision(b.Transform.Root, a, CollisionType.CollisionEnter);
                }
            }
        }

        public static void DoCollisionChecks(GameController game)
        {
            if (game != null)
            {
                int colliderCount = game.ColliderComponents.Count;

                for (int i = 0; i < colliderCount; i++)
                {
                    // check if i is active
                    if (game.ColliderComponents[i].Enabled && game.ColliderComponents[i].transformChanged && !game.ColliderComponents[i].IsTrigger)
                    {
                        for (int j = 0; j < colliderCount; j++)
                        {
                            // check if j is active
                            if (i != j && game.ColliderComponents[j].Enabled)
                            {
                                // only do collision checks between objects that aren't both static
                                if ((!game.ColliderComponents[i].GameObject.Static || !game.ColliderComponents[j].GameObject.Static)
                                    && (game.CollisionLayer.IsLayerCollidable(game.ColliderComponents[i].GameObject.Layer, game.ColliderComponents[j].GameObject.Layer)
                                        || game.ColliderComponents[j].IsTrigger)
                                    && game.ColliderComponents[i].Transform.Root != game.ColliderComponents[j].Transform.Root)
                                {
                                    // check if the two colliders are colliding
                                    bool collision = false;

                                    // check if collider AABB (axis aligned bounding box) collide before checking the OBB (oriented bounding box) collision
                                    if (IsBoundingBoxColliding(game.ColliderComponents[i], game.ColliderComponents[j]))
                                    {
                                        // if the outer collider is a Circle collider
                                        if (game.ColliderComponents[i] is CircleCollider2D circleCollider2D_A)
                                        {
                                            // if the inner collider is a Circle collider >> check Circle vs Circle
                                            if (game.ColliderComponents[j] is CircleCollider2D circleCollider2D_B)
                                            {
                                                collision = IsColliding(circleCollider2D_A, circleCollider2D_B);
                                            }
                                            // if the inner collider is a Box collider >> check Box vs Circle
                                            else if (game.ColliderComponents[j] is BoxCollider2D boxCollider2D_B)
                                            {
                                                collision = IsColliding(boxCollider2D_B, circleCollider2D_A);
                                            }
                                        }
                                        // if the outer collider is a Box collider
                                        else if (game.ColliderComponents[i] is BoxCollider2D boxCollider2D_A)
                                        {
                                            // if the inner collider is a Box collider >> check Box vs Box
                                            if (game.ColliderComponents[j] is BoxCollider2D boxCollider2D_B)
                                            {
                                                collision = IsColliding(boxCollider2D_A, boxCollider2D_B);
                                            }
                                            // if the inner collider is a Circle collider >> check Box vs Circle
                                            else if (game.ColliderComponents[j] is CircleCollider2D circleCollider2D_B)
                                            {
                                                collision = IsColliding(boxCollider2D_A, circleCollider2D_B);
                                            }
                                        }
                                    }

                                    // notify controllers of collision enter or collision exit
                                    DoCollisionNotification(game, game.ColliderComponents[i] as Collider2D, game.ColliderComponents[j] as Collider2D, collision);
                                }
                            }

                            // if collider has become disabled during collision >> break the loop
                            if (!game.ColliderComponents[i].Enabled)
                            {
                                break;
                            }
                        }

                        // collisions have now been checked since the change in transform
                        game.ColliderComponents[i].transformChanged = false;
                    }
                }
            }
        }

        private static bool IsBoundingBoxColliding(Collider2D a, Collider2D b)
        {
            return a._leftX < b._rightX && a._rightX > b._leftX && a._topY > b._bottomY && a._bottomY < b._topY;
        }

        public static bool IsColliding(CircleCollider2D a, CircleCollider2D b)
        {
            return Math.Pow(b.Transform.Position.X - a.Transform.Position.X, 2) + Math.Pow(b.Transform.Position.Y - a.Transform.Position.Y, 2) <= Math.Pow(a.Radius + b.Radius, 2);
        }

        public static bool IsColliding(BoxCollider2D b, CircleCollider2D c)
        {
            //// Transform the point to the box space 
            Vector3 boxSpacePoint = Vector3.Transform(c.Transform.Position, Matrix.Invert(b.Transform.GetWorldMatrix()));

            float absCircleX = Math.Abs(boxSpacePoint.X);
            float absCircleY = Math.Abs(boxSpacePoint.Y);

            // Just apply an axis-aligned bounding box test - if the center of the circle is inside the box then return true
            if (absCircleX < b.Extents.X && absCircleY < b.Extents.Y)
                return true;

            // Now get into distance calculation to find if circle is inside corner/edge
            float deltaX = absCircleX - Math.Min(absCircleX, b.Extents.X);
            float deltaY = absCircleY - Math.Min(absCircleY, b.Extents.Y);

            if (deltaX * deltaX + deltaY * deltaY < c.Radius * c.Radius)
                return true;

            return false;
        }

        private static bool IsColliding(BoxCollider2D a, BoxCollider2D b)
        {
            // get the axis of each collider (only 2 axis per BoxCollider)
            Vector2[] axis =
            {
                // box A
                Vector2.Normalize(a.cornersWorld[2] - a.cornersWorld[1]),
                Vector2.Normalize(a.cornersWorld[0] - a.cornersWorld[1]),
                // box B
                Vector2.Normalize(b.cornersWorld[2] - b.cornersWorld[1]),
                Vector2.Normalize(b.cornersWorld[0] - b.cornersWorld[1])
            };

            // iterate through each axis
            for (int i = 0; i < 4; i++)
            {
                // record the min/max scalar values for each box per axis
                float?[] scalarMin = new float?[2];
                float?[] scalarMax = new float?[2];

                // iterate through each BoxCollider
                for (int j = 0; j < 2; j++)
                {
                    // iterate through the BoxCollider's corners
                    for (int k = 0; k < 4; k++)
                    {
                        // project individual corner onto axis
                        float scalar = Vector2.Dot(j == 0 ? a.cornersWorld[k] : b.cornersWorld[k], axis[i]);

                        // get only the min & max scalar values
                        if (scalarMin[j] == null || scalarMin[j] > scalar)
                        {
                            scalarMin[j] = scalar;
                        }

                        if (scalarMax[j] == null || scalarMax[j] < scalar)
                        {
                            scalarMax[j] = scalar;
                        }
                    }
                }

                // if one axis doesn't overlap then there's no collision - abort early to save on unnecessary checks
                if (scalarMin[1] > scalarMax[0] || scalarMax[1] < scalarMin[0])
                {
                    return false;
                }

                // check next axis or return true to there being a collision if all axis and corners collided
            }

            return true;
        }

        internal static bool IsColliding(BoxCollider2D a, Vector2[] rayPoints)
        {
            // get the axis of each collider (only 2 axis per BoxCollider)
            Vector2[] axis =
            {
                // box A
                Vector2.Normalize(a.cornersWorld[2] - a.cornersWorld[1]),
                Vector2.Normalize(a.cornersWorld[0] - a.cornersWorld[1]),
                // raycast
                //Vector2.Normalize(rayPoints[2] - rayPoints[1]),
                //Vector2.Normalize(rayPoints[0] - rayPoints[1])

                Vector2.Normalize(rayPoints[0] - rayPoints[1]),
                new Vector2(Vector2.Normalize(rayPoints[0] - rayPoints[1]).Y, -Vector2.Normalize(rayPoints[0] - rayPoints[1]).X)
            };
            
            // iterate through each axis
            for (int i = 0; i < 4; i++)
            {
                // record the min/max scalar values for each box per axis
                float?[] scalarMin = new float?[2];
                float?[] scalarMax = new float?[2];

                // iterate through BoxCollider or Ray
                for (int j = 0; j < 2; j++)
                {
                    // iterate through the BoxCollider's corners
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 points;

                        if (j == 0)
                        {
                            points = a.cornersWorld[k];
                        }
                        else
                        {
                            points = rayPoints[k % 2];
                        }

                        // project individual corner onto axis
                        float scalar = Vector2.Dot(points, axis[i]);

                        // get only the min & max scalar values
                        if (scalarMin[j] == null || scalarMin[j] > scalar)
                        {
                            scalarMin[j] = scalar;
                        }

                        if (scalarMax[j] == null || scalarMax[j] < scalar)
                        {
                            scalarMax[j] = scalar;
                        }
                    }
                }

                // if one axis doesn't overlap then there's no collision - abort early to save on unnecessary checks
                if (scalarMin[1] > scalarMax[0] || scalarMax[1] < scalarMin[0])
                {
                    return false;
                }

                // check next axis or return true to there being a collision if all axis and corners collided
            }

            return true;
        }
    }
}
