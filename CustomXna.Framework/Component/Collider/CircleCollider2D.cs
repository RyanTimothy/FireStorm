/*  CircleCollider2D.cs
 *  Collider for 2D collision as a circle
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.10.05: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class CircleCollider2D : Collider2D
    {
        private float _radius = 1;
        private float _halfRadius = 0.5f;

        /// <summary>
        /// The collision radius of the Circle2D - Must be higher than zero
        /// </summary>
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = Math.Max(0, value);
                _halfRadius = _radius * 0.5f;
            }
        }

        public override void UpdateBounds()
        {
            // update the bounds for the axis aligned bounding box
            _leftX = Transform.Position.X - _halfRadius;
            _rightX = Transform.Position.X + _halfRadius;

            _topY = Transform.Position.Y + _halfRadius;
            _bottomY = Transform.Position.Y - _halfRadius;
        }
    }
}
