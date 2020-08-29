using System;
using Terraria;
using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace ChemistryClass.Sounds.Item {
    public class HeavyEnergyBurst : ModSound {

        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type) {

            SoundEffectInstance soundEff = sound.CreateInstance();
            soundEff.Volume = volume * 1.3f;
            soundEff.Pan = pan;
            soundEff.Pitch = 0;
            return soundEff;

        }

    }
}
