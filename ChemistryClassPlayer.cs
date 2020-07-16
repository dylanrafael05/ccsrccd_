using System;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass {
    public class ChemistryClassPlayer : ModPlayer {

        //Chemistry class values
        public float ChemicalDamageAdd = 0f;
        public float ChemicalDamageMult = 1f;

        public float ChemicalKnockbackAdd = 0f;
        public float ChemicalKnockbackMult = 1f;

        public float ChemicalCritAdd = 0f;
        public float ChemicalCritMult = 1f;

        public float DecayRateMult = 1f;
        public float DecayRateAdd = 0f;
        public float DecayChanceMult = 1f;
        public float DecayChanceAdd = 0f;

    }
}
