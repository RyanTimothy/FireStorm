/*  CameraViewController.cs
 *  Handles the auto rotation of the Camera to view the Target + Offset (offset used for when camera enters idle mode so that camera is aiming higher up)
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public class CameraViewController : ComponentController
    {
        private Transform cameraController;
        private Transform player;

        public float HeightOffset { get; set; }

        public override void Start()
        {
            cameraController = FindGameObjectByTag((int)GameObjectTag.CameraController)?.Transform;
            player = FindGameObjectByTag((int)GameObjectTag.Player)?.Transform;
        }

        public override void Update(GameTime gameTime)
        {
            Transform.Rotation = QuaternionHelper.LookRotation(Transform.Position - (cameraController.Position + Transform.Up * HeightOffset), Vector3.Up);
        }
    }
}