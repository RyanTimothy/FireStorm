using CustomXna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.Prefabs
{
    public class DustExplosion : GameObject
    {
        private ParticleSystem dust;

        public override void OnCreated()
        {
            dust = AddComponent<ParticleSystem>() as ParticleSystem;
            dust.SimulationSpace = ParticleSystemSimulationSpace.Local;
            dust.BillboardConstraint = BillboardConstraint.None;
            dust.PlayOnAwake = true;
            dust.SpawnAreaRadius = 3;
            dust.AnimateTilesX = 6;
            dust.AnimateTilesY = 5;
            dust.Texture = Game.Content.Load<Texture2D>("smoke");
            dust.DrawOrder = 102;
            dust.StartSize = 7f;
            dust.EmissionRate = 100f;
            dust.StartLifetime = 0.4f;
            dust.StartDelay = 0.1f;
            dust.Duration = 0.2f;
            dust.MaxParticles = 7;
            dust.Animate = true;
            dust.AnimateTilesPerSecond = 20;

            AddComponent<ParticleSystemController>();
        }

        public void Play()
        {
            dust.Play();
        }
    }
}