/*  ParticleSystemPoolController.cs
 *  Handles the auto pooling feature of the particles
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
    public class ParticleSystemPoolController : ComponentController
    {
        private ParticleSystem particleSystem;

        public PrefabType PrefabType { get; set; }

        public override void Start()
        {
            particleSystem = GameObject.GetComponent<ParticleSystem>();
        }

        protected override void OnEnable()
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (particleSystem != null && !particleSystem.IsPlaying)
            {
                if (PrefabType != PrefabType.None && (GameObject.Game as GameFireStorm).GameObjectPrefabPool.ContainsPrefabType(PrefabType))
                {
                    (GameObject.Game as GameFireStorm).GameObjectPrefabPool.Return(PrefabType, GameObject);
                }
                else
                {
                    GameObject.Destroy(GameObject);
                }
            }
        }
    }
}