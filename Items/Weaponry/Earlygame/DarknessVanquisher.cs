using System;
using ChemistryClass.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class DarknessVanquisher : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "A sword made of the clearest of lenses\n" +
                "Shoots pressure-cracked shards of glass"

                );

        }

        public override void SafeSetDefaults() {

            item.damage = 19;
            item.crit = 6;
            item.knockBack = 7f;

            item.width = 48;
            item.height = 48;

            item.useTime = 30;
            item.useAnimation = 30;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.EarlygameFL.GlassShard>();
            item.shootSpeed = 15f;

            minutesToDecay = 3f;

            SetRefinementData(

                (ModContent.ItemType<Materials.Earlygame.PrismaticLens>(), 1f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            TileID.GlassKiln,
                            1,
                            (ModContent.ItemType<Materials.Earlygame.PrismaticLens>(), 4)

            );

        }

    }
}
