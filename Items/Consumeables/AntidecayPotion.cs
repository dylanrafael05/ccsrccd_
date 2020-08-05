using System;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Consumeables {
    public class AntidecayPotion : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Reduces all decay by 10%");

        }

        public override void SetDefaults() {

            item.value = Item.buyPrice(0, 0, 10, 0);
            item.rare = 1;

            item.width = 16;
            item.height = 32;

            item.consumable = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.UseSound = SoundID.Item3;

            item.maxStack = 30;

            item.buffType = ModContent.BuffType<Buffs.Normal.Antidecay>();
            item.buffTime = 14400;

        }

        public override void AddRecipes() {

            this.SetRecipe(

                TileID.Bottles,
                1,
                (ItemID.BottledWater, 1),
                (ItemID.Daybloom, 1),
                (ItemID.Mushroom, 1),
                (ModContent.ItemType<Placeable.Blocks.SulfurClump>(), 1)

                );

        }

    }
}
