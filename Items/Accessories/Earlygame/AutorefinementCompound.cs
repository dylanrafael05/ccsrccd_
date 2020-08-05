using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Accessories.Earlygame {
    public class AutorefinementCompound : ModItem {
        public override void SetStaticDefaults() {

            Tooltip.SetDefault("An apparatus which enables chemical weapons to be\nautomatically refined once if below 50% purity");

        }

        public override void SetDefaults() {

            item.rare = 3;
            item.value = Item.buyPrice(0, 1, 50, 0);

            item.width = 18;
            item.height = 36;

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

            player.Chemistry().autoRefine = !hideVisual;

        }
    }
}
