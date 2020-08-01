using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Accessories.Earlygame {
    public class SulfurHeart : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("The heart of a sulfuric deposit\nIncreases max life by 50 when holding a chemical weapon");

        }

        public override void SetDefaults() {

            item.rare = 4;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.width = 26;
            item.height = 30;

            //DEBUGGING
            //item.useStyle = ItemUseStyleID.SwingThrow;
            //item.useTime = 20;
            //item.useAnimation = 20;
            //item.autoReuse = true;
            //item.consumable = true;
            //item.createTile = ModContent.TileType<Tiles.SulfurHeartTile>();

            item.maxStack = 1;

            item.accessory = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual) {

            player.Chemistry().SulfurHeart = true;

        }

    }
}