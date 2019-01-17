/*  BullerController.cs
 *  Handles the Bullet's in the game
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
    public class BulletController : ComponentController
    {
        public float Speed = 25.0f;
        public Vector3 StartPosition = Vector3.Zero;

        private ParticleSystem flashParticleSystem;

        public SoundEffectInstancePool ExplosionSoundEffect { get; set; }

        public override void Start()
        {
            StartPosition = Transform.Position;

            flashParticleSystem = (GameObject.Game as GameFireStorm).ParticleSystemExplosionFlash;
        }

        protected override void OnEnable()
        {
            StartPosition = Transform.Position;
        }

        public override void OnCollisionEnter2D(Collider2D sender)
        {
            // do the flash shockwave
            if (flashParticleSystem != null)
            {
                flashParticleSystem.Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, 0.3f);
                flashParticleSystem.StartSize = 2.8f;
                flashParticleSystem.Play();
            }

            // do the explosion particle effect
            ParticleSystem particleSystemExplosion = (GameObject.Game as GameFireStorm).ParticleSystemExplosion;
            particleSystemExplosion.GameObject.Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, 2.3f);
            particleSystemExplosion.Play();

            // Debris particles
            GameObject debrisParticles = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.BulletDebrisParticles);

            if (debrisParticles != null)
            {
                debrisParticles.Transform.Position = Transform.Position + Vector3.Backward;
                debrisParticles.SetActive(true);
            }

            //bullet dust - shooting a tree
            if (sender.GameObject.Tag == (int)GameObjectTag.Tree)
            {
                GameObject bulletDustParticles = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.BulletConcreteDustParticles);

                if (bulletDustParticles != null)
                {
                    //start dust at the base of the tree
                    bulletDustParticles.Transform.Position = sender.Transform.Position;
                    bulletDustParticles.SetActive(true);
                }
            }
            //bullet dust
            else 
            {
                GameObject bulletDustParticles = (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Take(PrefabType.BulletConcreteDustParticles);

                if (bulletDustParticles != null)
                {
                    bulletDustParticles.Transform.Position = Transform.Position;
                    bulletDustParticles.SetActive(true);
                }
            }

            SoundEffectInstance explosion = ExplosionSoundEffect.GetInstance();
            explosion.Volume = 1f;
            explosion.Play();

            RemoveBullet();
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GameObject.Transform.Position += GameObject.Transform.Forward * Speed * delta;

            if (GameObject.Transform.Position.Z < 0)
            {
                // do the explosion particle effect
                ParticleSystem particleSystemDirtExplosion = (GameObject.Game as GameFireStorm).ParticleSystemDirtExplosion;
                particleSystemDirtExplosion.GameObject.Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, 0.8f);
                particleSystemDirtExplosion.Play();

                RemoveBullet();
            }
            else if (Vector3.DistanceSquared(GameObject.Transform.Position, StartPosition) > 2000)
            {
                RemoveBullet();
            }
        }

        private void RemoveBullet()
        {
            (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Return(PrefabType.Bullet, GameObject);
        }
    }
}