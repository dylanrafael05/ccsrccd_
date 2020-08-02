using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Materials.Earlygame {
    public class PrismaticLens : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A lens which is so clear, it acts as a prism");

        }

        public override void SetDefaults() {

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 2, 0);
            item.maxStack = 999;

        }

    }
}
