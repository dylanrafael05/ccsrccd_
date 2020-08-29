using System;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Tools {
    public class AncientPickaxe : ModItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "A pickaxe crafted using the most ancient techniques\n" +
                "Able to mine Hellstone"

                );

        }

        public override void SetDefaults() {

            item.damage = 15;
            item.crit = 4;
            item.knockBack = 3f;

            item.width = 48;
            item.height = 48;

            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.useTurn = true;

            item.pick = 70;

            item.rare = 2;
            item.value = Item.buyPrice(0, 0, 20, 0);

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            TileID.Anvils,
                            1,
                            (ItemID.Bone, 8),
                            ("Wood", 6)

            );

        }

    }
}
