/*  TowerTurretController.cs
 *  Handles the targetting and shooting of the tower's turrets
 *  Also uses Raycast to determine if a clear line of sight is present before firing
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
    class TowerTurretController : ComponentController
    {
        private Quaternion targetRotation;
        private static Random random = new Random();

        private float rotationalVelocity;

        public float FireRateInSeconds { get; set; } = 1.0f;
        public SoundEffectInstancePool ShootingSoundEffect { get; internal set; }

        public float TargetingDistance;
        public float TargetingDistanceLineOfSight = 1600;
        public float ShootDistance = 900;

        private TankController target;

        public override void Start()
        {
            // get the Player GameObject
            target = FindGameObjectByTag((int)GameObjectTag.Player)?.GetComponent<TankController>();

            TargetingDistance = random.Next(200, (int)TargetingDistanceLineOfSight);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (target != null && !target.IsDead && !target.GameOver && !target.LevelComplete)
            {
                float distanceFromTarget = Vector3.DistanceSquared(target.Transform.Position, Transform.Position);

                if (distanceFromTarget < TargetingDistance)
                {
                    TargetingDistance = TargetingDistanceLineOfSight;

                    targetRotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateWorld(Transform.Position, Vector3.Normalize(target.Transform.Position - Transform.Position), Vector3.Backward));

                    Quaternion newRotation = QuaternionHelper.RotateTowards(Transform.Rotation, targetRotation, rotationalVelocity * deltaTime);

                    if (Math.Abs(QuaternionHelper.Angle(Transform.Rotation, targetRotation)) > float.Epsilon)
                    {
                        rotationalVelocity = Math.Min(90, rotationalVelocity + 80 * deltaTime);
                        Transform.Rotation = newRotation;
                    }
                    else
                    {
                        rotationalVelocity = Math.Max(0, rotationalVelocity - 60 * deltaTime);
                    }
                    
                    // bullet start position
                    Vector3 bulletStartPosition = Transform.Position + Transform.Forward * 5f + Transform.Up * 1.2f;

                    Vector3 lineOfSightEndPoint = bulletStartPosition + Transform.Forward * 34;
                    
                    float distance = 34;

                    if (bulletStartPosition.Z > 0 && lineOfSightEndPoint.Z < 0)
                    {
                        float x = 0;
                        if (Math.Abs(lineOfSightEndPoint.X - bulletStartPosition.X) > float.Epsilon)
                        {
                            float xzM = (lineOfSightEndPoint.Z - bulletStartPosition.Z) / (lineOfSightEndPoint.X - bulletStartPosition.X);
                            float xzB = bulletStartPosition.Z - (xzM * bulletStartPosition.X);
                            x = (-xzB) / xzM;
                        }

                        float y = 0;
                        if (Math.Abs(lineOfSightEndPoint.Z - bulletStartPosition.Z) > float.Epsilon)
                        {
                            float yzM = (lineOfSightEndPoint.Z - bulletStartPosition.Z) / (lineOfSightEndPoint.Y - bulletStartPosition.Y);
                            float yzB = bulletStartPosition.Z - (yzM * bulletStartPosition.Y);
                            y = (-yzB) / yzM;
                        }

                        distance = Vector3.Distance(bulletStartPosition, new Vector3(x, y, 0));
                    }

                    // Only if fire rate is zero or below
                    if (FireRateInSeconds <= 0 && distanceFromTarget < ShootDistance)
                    {
                        Collider2D collider2D = Raycast2D(new Vector2(bulletStartPosition.X, bulletStartPosition.Y), new Vector2(Transform.Forward.X, Transform.Forward.Y), distance);

                        if (collider2D != null && collider2D.GameObject.Tag == (int)GameObjectTag.Player)
                        {
                            FireRateInSeconds = 1.0f;

                            // Muzzle fire effect
                            GameObject muzzleFire = (Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.TowerMuzzleFlash);

                            if (muzzleFire != null)
                            {
                                muzzleFire.Transform.Position = Transform.Position + Transform.Forward * 3.15f + Transform.Up * 1.5f;
                                muzzleFire.Transform.Parent = GameObject;
                                muzzleFire.Transform.LocalRotation = Quaternion.Identity;
                                muzzleFire.SetActive(true);
                            }

                            // pull bullet from Bullet Pool
                            GameObject bullet = (Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.Bullet);

                            if (bullet != null)
                            {
                                bullet.Transform.Position = bulletStartPosition;
                                bullet.Transform.Rotation = Transform.Rotation;
                                bullet.Layer = (int)LayerType.EnemyBullet;
                                bullet.SetActive(true);
                            }

                            SoundEffectInstance shootingBulletSoundEffect = ShootingSoundEffect.GetInstance();
                            shootingBulletSoundEffect.Volume = 0.5f;
                            shootingBulletSoundEffect.Play();
                        }
                    }
                }
            }

            // Fire Turret
            if (FireRateInSeconds > 0)
            {
                FireRateInSeconds -= deltaTime;
            }
        }
    }
}