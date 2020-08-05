using System;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Buffs.Debuffs {
    public class SulfurFire : ModBuff {

        public override void SetDefaults() {
            this.DisplayName.SetDefault("Sulfur Fire");
            this.Description.SetDefault("Rapidly losing life due to sulfuric gases");
            Main.debuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex) {

            npc.GetGlobalNPC<ChemistryClassGlobalNPC>().sulfurFire = true;

        }

        public override void Update(Player player, ref int buffIndex) {

            player.GetModPlayer<ChemistryClassPlayer>().sulfurFire = true;

        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {

            npc.buffTime[buffIndex] = time;
            return false;

        }

        public override bool ReApply(Player player, int time, int buffIndex) {

            player.buffTime[buffIndex] = time;
            return false;

        }

    }
}
