using System;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Buffs.Debuffs {
    public class Rusted : ModBuff {

        public override void Update(NPC npc, ref int buffIndex) {

            npc.GetGlobalNPC<ChemistryClassGlobalNPC>().rusted = true;

        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {

            npc.buffTime[buffIndex] = time;
            return false;

        }

    }
}
