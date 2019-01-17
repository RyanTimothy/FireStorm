/*  BaseController.cs
 *  Handles the Spawning of the Player's tank
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class BaseController : ComponentController
    {
        private GameObject baseDoorLeft;
        private GameObject baseDoorRight;
        private GameObject elevatorLeft;
        private GameObject elevatorRight;

        private bool doorsOpened = false;
        private float doorPosition;

        private float elevatorHeight;

        private const float DOOR_SPEED = 1.5f;
        private const float ELEVATOR_SPEED = 3f;

        private Transform player;
        private TankController tankController;

        public Quaternion SpawnRotation { get; set; } = Quaternion.Identity * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180.0f));

        public override void Start()
        {
            GameObject playerGameObject = FindGameObjectByTag((int)GameObjectTag.Player);
            player = playerGameObject.Transform;
            tankController = playerGameObject.GetComponent<TankController>();

            baseDoorLeft = GameObject.Instantiate(Game);
            (baseDoorLeft.AddComponent<ModelRenderer>() as ModelRenderer).Model = Game.Content.Load<Model>("basedoor");
            baseDoorLeft.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

            baseDoorRight = GameObject.Instantiate(Game);
            (baseDoorRight.AddComponent<ModelRenderer>() as ModelRenderer).Model = Game.Content.Load<Model>("basedoor");
            baseDoorRight.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180.0f));

            elevatorLeft = GameObject.Instantiate(Game);
            (elevatorLeft.AddComponent<ModelRenderer>() as ModelRenderer).Model = Game.Content.Load<Model>("basedoor");
            elevatorLeft.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
            elevatorLeft.Transform.Position = Vector3.Forward * 6;

            elevatorRight = GameObject.Instantiate(Game);
            (elevatorRight.AddComponent<ModelRenderer>() as ModelRenderer).Model = Game.Content.Load<Model>("basedoor");
            elevatorRight.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180.0f));
            elevatorRight.Transform.Position = Vector3.Forward * 6;

            tankController.Restart();
            StartSpawn();
        }

        public void StartSpawn()
        {
            if (!player.GameObject.ActiveInGame)
            {
                player.GameObject.SetActive(true);
            }

            GameObject tankTurret = FindGameObjectByTag((int)GameObjectTag.TankTurret);

            if (tankTurret != null)
            {
                tankTurret.Transform.LocalRotation = Quaternion.Identity;
            }


            //Enable this ComponentController to have Update called
            Enabled = true;
            tankController.HandleUserInput = false;
            tankController.TankModelRenderer.Visible = true;
            tankController.TurretModelRenderer.Visible = true;

            doorPosition = 0;
            elevatorHeight = 6;

            player.Position = Transform.Position + Transform.Down * 6f;

            player.Rotation = SpawnRotation;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!doorsOpened)
            {
                doorPosition += DOOR_SPEED * deltaTime;

                baseDoorLeft.Transform.Position = Transform.Position + Transform.Left * doorPosition;
                baseDoorRight.Transform.Position = Transform.Position + Transform.Right * doorPosition;

                doorsOpened = (doorPosition > 4);
            }

            elevatorHeight -= ELEVATOR_SPEED * deltaTime;

            if (elevatorHeight <= 0)
            {
                elevatorHeight = 0;
                Enabled = false;

                tankController.SpawnComplete();
            }

            Vector3 elevatorPosition = Transform.Position + Transform.Down * elevatorHeight;

            elevatorLeft.Transform.Position = elevatorPosition;
            elevatorRight.Transform.Position = elevatorPosition;
            player.Position = elevatorPosition;
        }
    }
}
