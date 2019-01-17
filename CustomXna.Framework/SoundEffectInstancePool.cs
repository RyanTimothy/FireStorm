/*  SoundEffectInstancePool.cs
 *  Handles the collection of the sound effect instances used for pooling
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public sealed class SoundEffectInstancePool
    {
        // dictionary of this class's instance
        private static readonly Dictionary<SoundEffect, SoundEffectInstancePool> soundEffectPool = new Dictionary<SoundEffect, SoundEffectInstancePool>();

        // pool of Sound Effect Instances
        private readonly HashSet<SoundEffectInstance> soundEffectInstances = new HashSet<SoundEffectInstance>();
        // the sound effect
        private readonly SoundEffect soundEffect;

        // private constructor for the SoundEffect type
        private SoundEffectInstancePool(SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
        }

        /// <summary>
        /// assigns the class instance of the specific SoundEffect
        /// </summary>
        public static SoundEffectInstancePool Assign(SoundEffect soundEffect)
        {
            if (!soundEffectPool.ContainsKey(soundEffect))
            {
                soundEffectPool.Add(soundEffect, new SoundEffectInstancePool(soundEffect));
            }

            return soundEffectPool[soundEffect];
        }

        /// <summary>
        /// Gets an available instance of the SoundEffect or creates an instance of non are available
        /// </summary>
        public SoundEffectInstance GetInstance()
        {
            // remove any potentially disposed sound effects before iterating through instances
            soundEffectInstances.RemoveWhere(s => s.IsDisposed);

            // return the first sound effect instance that is stopped
            foreach (SoundEffectInstance s in soundEffectInstances)
            {
                if (s.State == SoundState.Stopped)
                {
                    return s;
                }
            }

            // instance a new sound effect - there were not available ones
            SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
            soundEffectInstances.Add(soundEffectInstance);

            return soundEffectInstance;
        }
    }
}