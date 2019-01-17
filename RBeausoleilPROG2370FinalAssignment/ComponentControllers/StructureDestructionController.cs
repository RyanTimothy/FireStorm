/*  StructureDestructionController.cs
 *  Base class which handles the Destruction of structures
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

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class StructureDestructionController : ComponentController
    {
        protected ParticleSystem flashParticleSystem;

        private bool prefabReplacementInstantiated = false;
        public static Random random = new Random();
        private Vector3 destructionPosition;
        public PrefabType PrefabReplacement { get; set; }
        public float PrefabReplacementDelay { get; set; }
        public int HitHealth { get; set; } = 1;
        public bool Alive { get; protected set; } = true;

        public float DustCloudHeight { get; set; } = 3;

        public float FallingSpeed { get; set; } = 70f;

        private float fallVelocity;

        public float DestroyFallHeight = -5;

        public override void Start()
        {
            flashParticleSystem = (GameObject.Game as GameFireStorm).ParticleSystemExplosionFlash;
        }

        public override void OnCollisionEnter2D(Collider2D sender)
        {
            if (!Enabled)
            {
                Enabled = true;
            }

            if (Alive)
            {

                if (sender is Collider2D c)
                {
                    // only register collision damage if player bullet
                    if (c.GameObject.Layer == (int)LayerType.PlayerBullet || c.GameObject.Layer == (int)LayerType.EnemyBullet)
                    {
                        HitHealth--;

                        if (HitHealth <= 0)
                        {
                            Alive = false;
                            destructionPosition = Transform.Position;

                            // Remove the collider if entity is being destroyed
                            Collider2D collider2D = GameObject.Transform.Root.GetComponentInChildren<Collider2D>();

                            if (collider2D != null)
                            {
                                collider2D.GameObject.Layer = (int)LayerType.FallingDestruction;
                            }

                            DoDestruction();

                            if (PrefabReplacementDelay <= float.Epsilon)
                            {
                                InitializePrefabReplacement();
                            }
                        }
                    }
                }
            }
        }

        private void InitializePrefabReplacement()
        {
            if (PrefabReplacement != PrefabType.None)
            {
                prefabReplacementInstantiated = true;
                GameObject prefab = GameObjectPrefabManager.InstantiateType(PrefabReplacement, Game);

                if (prefab != null)
                {
                    prefab.Transform.Position = destructionPosition;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Alive)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!prefabReplacementInstantiated)
                {
                    if (PrefabReplacementDelay <= float.Epsilon)
                    {
                        InitializePrefabReplacement();
                    }
                    else
                    {
                        PrefabReplacementDelay -= deltaTime;
                    }
                }

                fallVelocity += FallingSpeed * deltaTime;

                Transform.Position += Vector3.Forward * fallVelocity * deltaTime;

                if (Transform.Position.Z < DestroyFallHeight)
                {
                    GameObject.Destroy(GameObject);

                    if (GameObject.Tag == (int)GameObjectTag.TargetBuilding)
                    {
                        FindObjectByType<TankController>()?.UpdateTargetBuildingCount();
                    }
                }
            }
        }

        public virtual void DoDestruction()
        {
            // do the flash shockwave
            if (flashParticleSystem != null)
            {
                flashParticleSystem.Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, 0.3f);
                flashParticleSystem.StartSize = 7f;
                flashParticleSystem.Play();
            }

            // Dust explosion
            GameObject dustExplosion = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.ExplodingConcreteDustCloudParticles);

            if (dustExplosion != null)
            {
                dustExplosion.Transform.Position = Transform.Position + Transform.Up * DustCloudHeight;
                dustExplosion.SetActive(true);
            }
        }
    }
}