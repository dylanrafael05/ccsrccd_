using System;
using System.Collections.Generic;
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

        public override void SetDefaults(Item item) {

            if (!ChemistryClass.Configuration.ChangePick) return;

            switch(item.type) {
                case ItemID.LeadPickaxe:
                    item.pick = 40;
                    break;
                case ItemID.SilverPickaxe:
                    item.pick = 50;
                    break;
                case ItemID.PlatinumPickaxe:
                    item.pick = 55;
                    break;
                case ItemID.NightmarePickaxe:
                    item.pick = 60;
                    break;
                case ItemID.DeathbringerPickaxe:
                    item.pick = 60;
                    break;
                case ItemID.CobaltPickaxe:
                    item.pick = 130;
                    break;
                case ItemID.MythrilPickaxe:
                    item.pick = 165;
                    break;
                case ItemID.TitaniumPickaxe:
                    item.pick = 180;
                    break;
            }
            
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {

            if (!ChemistryClass.Configuration.ChangePick) return;

            if(item.type == ItemID.NightmarePickaxe || item.type == ItemID.DeathbringerPickaxe) {

                int ind = tooltips.FindIndex(t => t.text.Contains("Hellstone"));
                tooltips[ind].text = "Able to mine Quartz";

            }

        }

    }
}
