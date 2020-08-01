using System;
using Terraria.ID;
using Terraria.ModLoader;
using TUtils;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class OvergrowthLauncher : Sapinator {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Flings overgrown chunks of mahogany at enemies\nInflicts \"Poisoned\"");

        }

        public override void SafeSetDefaults() {

            base.SafeSetDefaults();

            item.damage = 16;
            item.shoot = ModContent.ProjectileType<Projectiles.EarlygameFL.OvergrowthLauncherProjectile>();
            item.shootSpeed += 2;

            minutesToDecay *= 1.1f;

        }

        public override void AddRecipes() {

            this.SetRecipe(

                ChemistryClass.BeakerTileID,
                1,
                (ModContent.ItemType<Sapinator>(), 1),
                (ItemID.JungleSpores, 3),
                (ItemID.Stinger, 1),
                (ItemID.RichMahogany, 6)

                );

        }

    }
}
