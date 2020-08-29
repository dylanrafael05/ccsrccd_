using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Placeable.Crafting {
    public class MetallurgyStand : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "\"With the power of hellstone as a heat source,\n" +
                "bunson burners always help in heating up metals\"\n" +
                "Used to make special metallic materials"

            );

        }

        public override void SetDefaults() {

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 5, 0);

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 20;
            item.useAnimation = 20;

            item.autoReuse = true;

            item.consumable = true;

            item.maxStack = 99;

            item.createTile = ModContent.TileType<Tiles.Multitiles.BunsonBurnerTile>();

        }

        public override void AddRecipes() {

            this.SetRecipe(

                TileID.Anvils,
                1,
                (ModContent.ItemType<Beaker>(), 1),
                (ItemID.Torch, 10),
                ("IronBar", 12),
                ("\0GoldBar", 12)

                );

        }

    }
}
