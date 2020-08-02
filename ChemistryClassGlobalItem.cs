using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass {
    public class ChemistryClassGlobalItem : GlobalItem {

        public override void OpenVanillaBag(string context, Player player, int arg) {

            if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag) {

                player.QuickSpawnItem(

                    ModContent.ItemType<Items.Materials.Earlygame.PrismaticLens>(),
                    Main.rand.Next(9,14)

                    );

            }

        }

    }
}
