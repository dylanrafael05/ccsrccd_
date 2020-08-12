using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class BloodyBeingStaff : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "Summons a bloodied ball of flesh to attack enemies\n" +
                "The being can be repositioned by using the weapon while it is active\n" +
                "Uses purity when repositioning, and slowly while " +
                "the being is alive."
                );

        }

        public override void SafeSetDefaults() {

            item.width = 32;
            item.height = 32;

            item.damage = 9;
            item.knockBack = 0;
            item.crit = 0;

            item.useTime = 80;
            item.useAnimation = 80;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item44;
            item.autoReuse = false;

            item.noMelee = true;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 20, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.BloodyBeing>();
            item.shootSpeed = 0;

            minutesToDecay = 4f;

            SetRefinementData(

                (ItemID.CrimstoneBlock, 1 / 20f),
                (ItemID.CrimsandBlock, 1 / 20f),
                (ItemID.Vertebrae, 1 / 3f)

                );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Vector2 mouseP = Main.screenPosition + Main.MouseScreen;

            if (player.ownedProjectileCounts[type] > 0) {

                foreach (var proj in Main.projectile) {
                    if (proj.type == type)
                        proj.Kill();
                }

            }

            Projectile.NewProjectile(
                mouseP, Vector2.Zero, type,
                damage, knockBack, player.whoAmI
                );

            //Main.NewText(Main.screenPosition + Main.MouseScreen);
            return false;

        }

        public override bool CanUseItem(Player player) {
            Vector2 mouseP = Main.screenPosition + Main.MouseScreen;
            for (int i = -1; i < 1; i++) {
                for (int j = -1; j < 1; j++) {
                    if (WorldGen.SolidTile(mouseP.X.RoundToInt() / 16 + i, mouseP.Y.RoundToInt() / 16 + j)) return false;
                }
            }
            return true;
        }

        public override void AddRecipes() {

            this.SetRecipe(

                ChemistryClass.BeakerTileID,
                1,
                (ItemID.TissueSample, 6),
                (ItemID.Vertebrae, 5),
                (ItemID.Shadewood, 10)

                );

        }
    }
}