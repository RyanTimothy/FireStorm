/*  BoxCollider2D.cs
 *  Collider for 2D collision as a box
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.10.01: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class BoxCollider2D : Collider2D
    {     
        private Vector2 _size;
        private Vector2 _extents;

        // always store the location of corners regardless if AABB or OBB collider
        protected Vector2[] cornersLocal = new Vector2[4];

        public Vector2 Extents { get => _extents; }
        
        public Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                _extents = _size * 0.5f;

                if (GameObject != null && GameObject.ActiveSelf)
                {
                    UpdateBounds();
                }
            }
        }
        

        public BoxCollider2D()
        {
        }

        public override void UpdateBounds()
        {
            // todo: Add scale support
                
            // get corners of OBB
            Vector3 cornerTL = (Transform.Right * (Offset.X - Extents.X) + Transform.Up * (Offset.Y + Extents.Y)) * Transform.Scale;
            Vector3 cornerTR = (Transform.Right * (Offset.X + Extents.X) + Transform.Up * (Offset.Y + Extents.Y)) * Transform.Scale;

            Vector3 cornerBL = (Transform.Right * (Offset.X - Extents.X) + Transform.Up * (Offset.Y - Extents.Y)) * Transform.Scale;
            Vector3 cornerBR = (Transform.Right * (Offset.X + Extents.X) + Transform.Up * (Offset.Y - Extents.Y)) * Transform.Scale;
                
            cornersLocal[0] = new Vector2(cornerTL.X, cornerTL.Y);
            cornersLocal[3] = new Vector2(cornerBL.X, cornerBL.Y);

            cornersLocal[1] = new Vector2(cornerTR.X, cornerTR.Y);
            cornersLocal[2] = new Vector2(cornerBR.X, cornerBR.Y);

            // get width and height of bounding box
            float width = Math.Max(Math.Abs(cornersLocal[1].X), Math.Abs(cornersLocal[2].X)) * 2;
            float height = Math.Max(Math.Abs(cornersLocal[1].Y), Math.Abs(cornersLocal[2].Y)) * 2;

            cornersWorld[0].X = cornersLocal[0].X + Transform.Position.X;
            cornersWorld[0].Y = cornersLocal[0].Y + Transform.Position.Y;
            cornersWorld[1].X = cornersLocal[1].X + Transform.Position.X;
            cornersWorld[1].Y = cornersLocal[1].Y + Transform.Position.Y;

            cornersWorld[2].X = cornersLocal[2].X + Transform.Position.X;
            cornersWorld[2].Y = cornersLocal[2].Y + Transform.Position.Y;
            cornersWorld[3].X = cornersLocal[3].X + Transform.Position.X;
            cornersWorld[3].Y = cornersLocal[3].Y + Transform.Position.Y;

            // update the bounds for the axis aligned bounding box
            _leftX = Math.Min(cornersWorld[0].X, Math.Min(cornersWorld[1].X, Math.Min(cornersWorld[2].X, cornersWorld[3].X)));
            _rightX = Math.Max(cornersWorld[0].X, Math.Max(cornersWorld[1].X, Math.Max(cornersWorld[2].X, cornersWorld[3].X)));

            _topY = Math.Max(cornersWorld[0].Y, Math.Max(cornersWorld[1].Y, Math.Max(cornersWorld[2].Y, cornersWorld[3].Y)));
            _bottomY = Math.Min(cornersWorld[0].Y, Math.Min(cornersWorld[1].Y, Math.Min(cornersWorld[2].Y, cornersWorld[3].Y)));
        }
    }
}