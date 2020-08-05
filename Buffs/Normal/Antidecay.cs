using System;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Buffs.Normal {
    public class Antidecay : ModBuff {

        public override void SetDefaults() {
            this.DisplayName.SetDefault("Antidecay");
            this.Description.SetDefault("Reduces all decay by 10%");
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {

            player.Chemistry().decayChanceMult *= 0.9f;

        }

        public override bool ReApply(Player player, int time, int buffIndex) {

            player.buffTime[buffIndex] = time;
            return false;

        }

    }
}
