/*  Camera.cs
 *  A Camera is the device through which the player views the world
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.9.24: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class Camera : ComponentController
    {
        public Matrix Projection { get; set; }
        public Matrix View { get; private set; }

        public BoundingFrustum BoundingFrustum { get; private set; }

        public bool FogEnabled { get; set; } = true;
        public Vector3 FogColor { get; set; } = new Color(4, 110, 140).ToVector3();
        public float FogStart { get; set; } = 80f;
        public float FogEnd { get; set; } = 140f;

        public Camera()
        {
        }

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
        }

        public override void Start()
        {
            Transform.TransformChanged += Transform_TransformChanged;

            CalculateView();

            BoundingFrustum = new BoundingFrustum(View * Projection);
        }

        private void Transform_TransformChanged(object sender, EventArgs e)
        {
            CalculateView();
            AssignFrustumMatrix();
        }

        private void AssignFrustumMatrix()
        {
            BoundingFrustum.Matrix = View * Projection;
        }
    }
}
