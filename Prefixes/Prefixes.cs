﻿using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChemistryClass.Prefixes {

    public class StablePrefs : ChemistryClassPrefix {

        private sbyte stab = 0;

        public StablePrefs(sbyte s) : base() => stab = s;
        public StablePrefs() : base() { }

        public override bool Autoload(ref string name) {

            mod.AddPrefix("Stable", new StablePrefs(1));
            mod.AddPrefix("Inert", new StablePrefs(3));
            mod.AddPrefix("Heterogeneous", new StablePrefs(-1));
            mod.AddPrefix("Unstable", new StablePrefs(-3));
            mod.AddPrefix("Decaying", new StablePrefs(-5));

            return false;

        }

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1f - stab * 0.06f;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 1 + stab * 0.035f;
            
        }

    }

    public class VolatilePrefs : ChemistryClassPrefix {

        private float power;

        public VolatilePrefs(float p) : base() => power = p;
        public VolatilePrefs() : base() {}

        public override bool Autoload(ref string name) {

            mod.AddPrefix("Reactive", new VolatilePrefs(1.4f));
            mod.AddPrefix("Volatile", new VolatilePrefs(2f));
            mod.AddPrefix("Softened", new VolatilePrefs(-0.7f));

            return false;

        }

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1f + 0.18f * power;

        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {

            damageMult *= 1f + 0.095f * power;
            critBonus += 3 * (int)power;
            knockbackMult *= 1f + 0.02f * power;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 1f + 0.14f * power;

        }

    }

    public class Contaminated : ChemistryClassPrefix {

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1.1f;

        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {

            damageMult *= 0.89f;
            critBonus -= 2;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 0.94f;

        }

    }

    public class Ruined : ChemistryClassPrefix {

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1.26f;

        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {

            damageMult *= 0.75f;
            useTimeMult *= 1.07f;
            critBonus -= 5;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 0.83f;

        }

    }

    public class Uncatalyzed : ChemistryClassPrefix {

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1.01f;

        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {

            useTimeMult = 1.13f;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 0.95f;

        }

    }

    public class DriedPrefs : ChemistryClassPrefix {

        private sbyte power;

        public DriedPrefs(sbyte p) : base() => power = p;
        public DriedPrefs() : base() { }

        public override bool Autoload(ref string name) {

            mod.AddPrefix("Dry", new DriedPrefs(1));
            mod.AddPrefix("Brittle", new DriedPrefs(2));

            return false;

        }

        public override void SetDecayStats(ref float lossMult) {

            lossMult = 1f + 0.05f * power;

        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {

            damageMult = 1f - 0.12f * power;
            knockbackMult = 1f - 0.17f * power;

        }

        public override void ModifyValue(ref float valueMult) {

            valueMult *= 1f - 0.05f * power;

        }

    }

}
