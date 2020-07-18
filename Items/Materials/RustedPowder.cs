using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Materials {
    public class RustedPowder : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A fine powder made of iron oxide");

        }

        public override void SetDefaults() {

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 1, 0);
            item.maxStack = 999;

        }

        public override void AddRecipes() {

            this.SetRecipe(

                ModContent.TileType<Tiles.MetallurgyStandTile>(),
                2,
                (ItemID.IronOre, 1)

                );

            this.SetRecipe(

                ModContent.TileType<Tiles.MetallurgyStandTile>(),
                2,
                (ItemID.LeadOre, 1)

                );

        }

    }
}
