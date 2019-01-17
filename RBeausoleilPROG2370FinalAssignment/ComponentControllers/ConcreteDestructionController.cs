/*  ConcreteDestructionController.cs
 *  Handles the destruction of concrete structures
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class ConcreteDestructionController : StructureDestructionController
    {
        public SoundEffectInstancePool ConcreteDebrisSoundEffect { get; set; }
        public SoundEffectInstancePool ExplosionEchoSoundEffect { get; set; }

        public override void DoDestruction()
        {
            ConcreteDestructionController[] concrete = FindObjectsByType<ConcreteDestructionController>();

            for (int i = 0; i < concrete.Length; i++)
            {
                if (concrete[i].GameObject.Layer == (int)LayerType.Wall)
                {
                    if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallHorizontal || concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedEast || concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedWest)
                    {
                        // Is wall to the East
                        if ((Transform.Position + new Vector3(4, 0, 0) - concrete[i].Transform.Position).LengthSquared() < 0.1f)
                        {
                            if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallHorizontal)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedWest, Game, concrete[i].Transform.Position);
                            }
                            else if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedEast)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedFullHorizontal, Game, concrete[i].Transform.Position);
                            }
                            else
                            {
                                continue;
                            }
                            GameObject.Destroy(concrete[i].GameObject);
                        }
                        else if ((Transform.Position + new Vector3(-4, 0, 0) - concrete[i].Transform.Position).LengthSquared() < 0.1f)
                        {
                            if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallHorizontal)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedEast, Game, concrete[i].Transform.Position);
                            }
                            else if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedWest)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedFullHorizontal, Game, concrete[i].Transform.Position);
                            }
                            else
                            {
                                continue;
                            }
                            GameObject.Destroy(concrete[i].GameObject);
                        }
                    }
                    else if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallVertical || concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedNorth || concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedSouth)
                    {
                        if ((Transform.Position + new Vector3(0, 4, 0) - concrete[i].Transform.Position).LengthSquared() < 0.1f)
                        {
                            if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallVertical)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedSouth, Game, concrete[i].Transform.Position);
                            }
                            else if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedNorth)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedFullVertical, Game, concrete[i].Transform.Position);
                            }
                            else
                            {
                                continue;
                            }
                            GameObject.Destroy(concrete[i].GameObject);
                        }
                        else if ((Transform.Position + new Vector3(0, -4, 0) - concrete[i].Transform.Position).LengthSquared() < 0.1f)
                        {
                            if (concrete[i].GameObject.Tag == (int)GameObjectTag.WallVertical)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedNorth, Game, concrete[i].Transform.Position);
                            }
                            else if(concrete[i].GameObject.Tag == (int)GameObjectTag.WallDamagedSouth)
                            {
                                GameObjectPrefabManager.InstantiateType(PrefabType.WallDamagedFullVertical, Game, concrete[i].Transform.Position);
                            }
                            else
                            {
                                continue;
                            }
                            GameObject.Destroy(concrete[i].GameObject);
                        }
                    }
                }
            }


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

            // Debris explosion
            GameObject debrisExplosion = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.ConcreteExplodeDebrisParticles);

            if (debrisExplosion != null)
            {
                debrisExplosion.Transform.Position = Transform.Position + Transform.Up * 5.5f;
                debrisExplosion.SetActive(true);
            }

            // if it's a tower, destroy the turret controller so it's not targetting you while it's falling and its particle system
            TowerTurretController turretController = GameObject.GetComponentInChildren<TowerTurretController>();

            if (turretController != null)
            {
                Destroy(turretController);

                // get the particle system controller
                ParticleSystemPoolController particleSystem = GameObject.GetComponentInChildren<ParticleSystemPoolController>();

                // remove the particle system controller from parent so it doesn't get destroyed before it would get added to pool
                if (particleSystem != null)
                {
                    particleSystem.Transform.Parent = null;
                }
            }

            if (ConcreteDebrisSoundEffect != null)
            {
                SoundEffectInstance concreteDebrisSoundEffect = ConcreteDebrisSoundEffect.GetInstance();
                concreteDebrisSoundEffect.Volume = 0.2f;
                concreteDebrisSoundEffect.Play();
            }

            if (ExplosionEchoSoundEffect != null)
            {
                SoundEffectInstance explosionEchoSoundEffect = ExplosionEchoSoundEffect.GetInstance();
                explosionEchoSoundEffect.Volume = 0.1f;
                explosionEchoSoundEffect.Play();
            }
        }
    }
}
