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
    public class TowerMuzzleFire : GameObject
    {
        private ParticleSystem particleSystemTowerMuzzleFire;

        public override void OnCreated()
        {
            particleSystemTowerMuzzleFire = AddComponent<ParticleSystem>() as ParticleSystem;
            particleSystemTowerMuzzleFire.SimulationSpace = ParticleSystemSimulationSpace.Local;
            particleSystemTowerMuzzleFire.BillboardConstraint = BillboardConstraint.AxisZ;
            particleSystemTowerMuzzleFire.PlayOnAwake = true;
            particleSystemTowerMuzzleFire.AnimateTilesX = 8;
            particleSystemTowerMuzzleFire.AnimateTilesY = 1;
            particleSystemTowerMuzzleFire.Texture = Game.Content.Load<Texture2D>("muzzlefire");
            particleSystemTowerMuzzleFire.DrawOrder = 102;
            particleSystemTowerMuzzleFire.StartSize = 3.5f;
            particleSystemTowerMuzzleFire.EmissionRate = 10f;
            particleSystemTowerMuzzleFire.StartLifetime = 0.32f;
            particleSystemTowerMuzzleFire.Duration = 0.05f;
            particleSystemTowerMuzzleFire.Animate = true;
            particleSystemTowerMuzzleFire.AnimateTilesPerSecond = 26;

            AddComponent<ParticleSystemController>();
        }

        public void Play()
        {
            particleSystemTowerMuzzleFire.Play();
        }
    }
}
