using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Accessories.Earlygame {
    public class StabilityMushroom : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A strange-looking, yet chemically stable mushroom\nReduces all decay by 6%");

        }

        public override void SetDefaults() {

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 3, 0);

            item.width = 28;
            item.height = 28;

            //DEBUGGING
            //item.useStyle = ItemUseStyleID.SwingThrow;
            //item.useTime = 20;
            //item.useAnimation = 20;
            //item.autoReuse = true;
            //item.consumable = true;
            //item.createTile = ModContent.TileType<Tiles.StabilityMushroomTile>();

            item.maxStack = 1;

            item.accessory = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual) {

            if (hideVisual) return;

            player.Chemistry().decayRateMult -= 0.06f;

        }

    }
}
