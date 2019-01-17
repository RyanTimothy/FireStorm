/*  RigidBody2D.cs
 *  Handles the basic physics of the tank - momentum and angular velocity; very primitive and simply reverts the tank back to its origin upon collision
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using System;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class RigidBody2D : ComponentController
    {
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private float _linearDrag;
        private float _maxVelocity = 20;
        private float _angularDrag = 20;

        public Vector2 Velocity { get; set; }
        public float AngularVelocity { get; set; }

        public float MaxVelocity
        {
            get => _maxVelocity;
            set
            {
                _maxVelocity = Math.Abs(value);
            }
        }

        /// <summary>
        /// Drag slows the object down due to friction with the air or water that surrounds it. Between 0 - 1000.
        /// </summary>
        public float LinearDrag
        {
            get => _linearDrag;
            set
            {
                _linearDrag = Math.Min(Math.Abs(value), 1000);
            }
        }

        /// <summary>
        /// Angular drag slows the rotational velocity with the air or water that surrounds it. Between 0 - 100
        /// </summary>
        public float AngularDrag
        {
            get => _angularDrag;
            set => _angularDrag = Math.Min(Math.Abs(value), 100);
        }

        public override void Start()
        {
            previousPosition = Transform.Position;
            previousRotation = Transform.Rotation;
        }

        public void AddVelocity(Vector2 vector2)
        {
            Velocity = vector2;
        }

        public void AddTorque(float torque)
        {
            AngularVelocity += torque;
        }

        private Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            // if the Square Magnitude of the Vector2 is higher than MaxLength >> return the normalized Vector multiplied by MaxLength
            float squareMagnitude = vector.X * vector.X + vector.Y * vector.Y;

            if (squareMagnitude > maxLength * maxLength)
            {
                return Vector2.Normalize(vector) * maxLength;
            }

            // return vector unchanged
            return vector;
        }

        public override void OnCollisionEnter2D(Collider2D sender)
        {
            if (sender is Collider2D c && c.GameObject.Static)
            {
                Transform.Position = previousPosition;
                Transform.Rotation = previousRotation;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
            }
        }

        public override void OnCollisionStay2D(Collider2D sender)
        {
            if (sender is Collider2D c && c.GameObject.Static)
            {
                Transform.Position = previousPosition;
                Transform.Rotation = previousRotation;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // get the previous Position and Rotation - our quick hack of reverting back to origin if Collision detected
            previousPosition = Transform.Position;
            previousRotation = Transform.Rotation;

            Velocity = ClampMagnitude(Velocity, _maxVelocity * deltaTime);

            Transform.Position += Transform.Right * Velocity.X + Transform.Forward * Velocity.Y;
            Transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, AngularVelocity * deltaTime);

            Velocity = Velocity * (1 - _linearDrag * deltaTime);
            AngularVelocity = AngularVelocity * (1 - _angularDrag * deltaTime);
        }
    }
}