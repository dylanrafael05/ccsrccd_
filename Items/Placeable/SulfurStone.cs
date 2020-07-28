﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Placeable {
    public class SulfurStone : ModItem {

        //public override void SetStaticDefaults() {
        //
        //    Tooltip.SetDefault("Very science. Much chemistry.");
        //
        //}

        public override void SetDefaults() {

            item.rare = 0;
            item.value = Item.buyPrice(0, 0, 0, 70);

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 20;
            item.useAnimation = 20;

            item.autoReuse = true;
            item.consumable = true;

            item.maxStack = 999;

            item.createTile = ModContent.TileType<Tiles.Blocks.SulfurStoneTile>();

        }

    }
}
