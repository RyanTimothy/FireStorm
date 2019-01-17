/*  TankTurretController.cs
 *  Handles the controls for the tank's turret
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class TankTurretController : ComponentController
    {
        private float fireRateInSeconds;
        ParticleSystem particleSystemMuzzleFire;
        TankController tankController;

        private const float TURRET_ROTATE_SPEED = 1.91986f; // 110 degrees per second in radians

        public SoundEffectInstance TurretRotateSoundEffect { get; set; }
        public SoundEffectInstancePool ShootingSoundEffect { get; set; }

        public override void Start()
        {
            GameObject muzzleFireGameObject = GameObject.Instantiate(GameObject.Game);
            muzzleFireGameObject.Transform.Parent = GameObject;
            muzzleFireGameObject.Transform.LocalPosition = Vector3.Forward * 3.1f + Vector3.Up * 1.4f;
            muzzleFireGameObject.Transform.LocalRotation = Quaternion.Identity;

            particleSystemMuzzleFire = muzzleFireGameObject.AddComponent<ParticleSystem>() as ParticleSystem;
            particleSystemMuzzleFire.SimulationSpace = ParticleSystemSimulationSpace.Local;
            particleSystemMuzzleFire.BillboardConstraint = BillboardConstraint.Vertical;
            particleSystemMuzzleFire.PlayOnAwake = false;
            particleSystemMuzzleFire.AnimateTilesX = 8;
            particleSystemMuzzleFire.AnimateTilesY = 1;
            particleSystemMuzzleFire.Texture = GameObject.Game.Content.Load<Texture2D>("muzzlefire");
            particleSystemMuzzleFire.DrawOrder = 101;
            particleSystemMuzzleFire.StartSize = 1.8f;
            particleSystemMuzzleFire.EmissionRate = 2f;
            particleSystemMuzzleFire.StartLifetime = 0.26f;
            particleSystemMuzzleFire.Duration = 0.01f;
            particleSystemMuzzleFire.Animate = true;
            particleSystemMuzzleFire.AnimateTilesPerSecond = 32;

            TurretRotateSoundEffect.Volume = 0.05f;
            TurretRotateSoundEffect.IsLooped = true;

            tankController = FindObjectByType<TankController>();
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool turretIsRotating = false;

            if (tankController.HandleUserInput)
            { 
                KeyboardState newKeyState = Keyboard.GetState();
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

                // Rotate tank turret - Left & Right
                if (newKeyState.IsKeyDown(Keys.Left) || (gamePadState.IsConnected && gamePadState.Triggers.Left >= 0.1f))
                {
                    if (newKeyState.IsKeyDown(Keys.Left))
                    {
                        GameObject.Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, TURRET_ROTATE_SPEED * deltaTime);
                    }
                    else
                    {
                        GameObject.Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, TURRET_ROTATE_SPEED * gamePadState.Triggers.Left * deltaTime);
                    }

                    if (TurretRotateSoundEffect.State != SoundState.Playing)
                    {
                        TurretRotateSoundEffect.Play();
                        TurretRotateSoundEffect.Volume = 0;
                    }

                    turretIsRotating = true;
                }
                if (newKeyState.IsKeyDown(Keys.Right) || (gamePadState.IsConnected && gamePadState.Triggers.Right >= 0.1f))
                {
                    if (newKeyState.IsKeyDown(Keys.Right))
                    {
                        GameObject.Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, -TURRET_ROTATE_SPEED * deltaTime);
                    }
                    else
                    {
                        GameObject.Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, -TURRET_ROTATE_SPEED * gamePadState.Triggers.Right * deltaTime);
                    }

                    if (TurretRotateSoundEffect.State != SoundState.Playing)
                    {
                        TurretRotateSoundEffect.Play();
                        TurretRotateSoundEffect.Volume = 0;
                    }

                    turretIsRotating = true;
                }

                // Fire Turret
                if (fireRateInSeconds > 0)
                {
                    fireRateInSeconds -= deltaTime;
                }
                else if (newKeyState.IsKeyDown(Keys.Space) || (gamePadState.IsConnected && gamePadState.Buttons.X == ButtonState.Pressed))
                {
                    Vector3 bulletStartPosition = Transform.Position + Transform.Forward * 2.9f + Transform.Up * 1.4f;

                    // if the camera is idle - stop it
                    tankController.StopCameraIdle();

                    // don't shoot if under water
                    if (bulletStartPosition.Z > 0)
                    {

                        SoundEffectInstance shootingSoundEffect = ShootingSoundEffect.GetInstance();
                        shootingSoundEffect.Volume = 0.5f;
                        shootingSoundEffect.Play();

                        fireRateInSeconds = 0.35f;

                        // pull bullet from Bullet Pool
                        GameObject bullet = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.Bullet);

                        if (bullet != null)
                        {
                            bullet.Transform.Position = bulletStartPosition;
                            bullet.Transform.Rotation = Transform.Rotation;
                            bullet.Layer = (int)LayerType.PlayerBullet;
                            bullet.SetActive(true);
                        }

                        particleSystemMuzzleFire.Play();
                    }
                }
            }

            if (TurretRotateSoundEffect.State == SoundState.Playing)
            {
                if (!turretIsRotating)
                {
                    TurretRotateSoundEffect.Stop();
                }
                else
                {
                    TurretRotateSoundEffect.Volume = Math.Min(0.05f, MathHelper.SmoothStep(TurretRotateSoundEffect.Volume, 0.05f, 15 * deltaTime));
                }
            }
        }
    }
}
