/*  TreeDestructionController.cs
 *  Handles the destruction of the trees
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
    public class TreeDestructionController : StructureDestructionController
    {
        public float TotalFallTime { get; set; } = 0.3f;
        public SoundEffectInstancePool TreeDestructionSound { get; set; }

        public float fallTime = 0;

        public Quaternion originalRotation;
        public Quaternion newRotation;

        public bool fallingOver = false;

        private const float NINETY_DEGREES = 1.5708f;

        public override void DoDestruction()
        {
            // Change the collider's layer if entity is being destroyed
            Collider2D collider2D = GameObject.Transform.Root.GetComponent<Collider2D>();

            if (collider2D != null)
            {
                collider2D.GameObject.Layer = (int)LayerType.FallingDestruction;
            }

            SoundEffectInstance treeFallingSoundEffect = TreeDestructionSound.GetInstance();
            treeFallingSoundEffect.Volume = 0.5f;
            treeFallingSoundEffect.Play();
        }
    }
}