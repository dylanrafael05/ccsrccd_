using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Materials.Earlygame {
    public class Quartz : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "An exceedingly rare gemstone\n" +
                "Generates energy through piezo and pyroelectricity"
                );

        }

        public override void SetDefaults() {

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 2, 0);
            item.maxStack = 999;

            item.width = 16;
            item.height = 16;

            //DEBUG
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Blocks.QuartzPlaced>();

        }

    }
}
