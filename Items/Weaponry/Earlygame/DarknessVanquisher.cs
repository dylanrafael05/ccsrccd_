using System;
using ChemistryClass.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class DarknessVanquisher : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "A sword made of the clearest of glasses\n" +
                "Shoots peircing shards of glass"

                );

        }

        public override void SafeSetDefaults() {

            item.damage = 16;
            item.crit = 6;
            item.knockBack = 10;

            item.width = 48;
            item.height = 48;

            item.useTime = 30;
            item.useAnimation = 30;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);

            item.shoot = ModContent.ProjectileType<GlassShard>();
            item.shootSpeed = 20f;

            minutesToDecay = 4f;

            SetRefinementData(

                (ModContent.ItemType<Materials.PrismaticLens>(), 1f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            ChemistryClass.BeakerTileID,
                            1,
                            (ModContent.ItemType<Materials.PrismaticLens>(), 4)

            );

        }

    }
}
