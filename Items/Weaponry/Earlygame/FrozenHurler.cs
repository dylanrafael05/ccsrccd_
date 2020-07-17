using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class FrozenHurler : Sapinator {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("Throws ice-cold balls of boreal wood\nInflicts \"Frostburn\"");

        }

        public override void SafeSetDefaults() {

            base.SafeSetDefaults();

            item.damage = 14;
            item.shoot = ModContent.ProjectileType<Projectiles.FrozenHurlerProjectile>();
            item.shootSpeed += 2;

            minutesToDecay *= 1.5f;

        }

        public override void AddRecipes() {

            this.SetRecipe(

                ChemistryClass.BeakerTileID,
                1,
                (ModContent.ItemType<Sapinator>(), 1),
                (ItemID.IceBlock, 20),
                (ItemID.SlushBlock, 2),
                (ItemID.BorealWood, 6)

                );

        }

    }
}
