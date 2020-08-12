using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class ToothballStaff : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "Summons a slow mass of flesh to attack foes\n" +
                "The mass can be repositioned by using the weapon while it is active\n" +
                "Uses purity when repositioning, and slowly while " +
                "the mass is alive."
                );

        }

        public override void SafeSetDefaults() {

            item.width = 32;
            item.height = 32;

            item.damage = 15;
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

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.RottingBeing>();
            item.shootSpeed = 0;

            minutesToDecay = 4f;

            SetRefinementData(

                (ItemID.EbonstoneBlock, 1 / 20f),
                (ItemID.EbonsandBlock, 1 / 20f),
                (ItemID.RottenChunk, 1 / 3f)

                );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Vector2 mouseP = Main.screenPosition + Main.MouseScreen;

            if(player.ownedProjectileCounts[type] > 0) {

                foreach(var proj in Main.projectile) {
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
            for(int i = -1; i < 1; i++) {
                for(int j = -1; j < 1; j++) {
                    if (WorldGen.SolidTile(mouseP.X.RoundToInt() / 16 + i, mouseP.Y.RoundToInt() / 16 + j)) return false;
                }
            }
            return true;
        }

        public override void AddRecipes() {

            this.SetRecipe(

                ChemistryClass.BeakerTileID,
                1,
                (ItemID.ShadowScale, 6),
                (ItemID.RottenChunk, 5),
                (ItemID.Ebonwood, 10)

                );

        }

    }
}
