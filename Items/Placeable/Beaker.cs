using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Placeable {
    public class Beaker : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Very science. Much chemistry.");

        }

        public override void SetDefaults() {

            item.rare = 0;
            item.value = Item.buyPrice(0, 0, 0, 50);

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 35;
            item.useAnimation = 35;
            item.UseSound = SoundID.Item1;

            item.autoReuse = true;

            item.consumable = true;

            item.maxStack = 999;

            item.createTile = ModContent.TileType<Tiles.BeakerTile>();

        }

        public override void AddRecipes() {

            this.SetRecipe(TileID.Furnaces, 2, (ItemID.Glass, 1));

        }

    }
}
