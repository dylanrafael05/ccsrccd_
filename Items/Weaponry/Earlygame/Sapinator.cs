using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class Sapinator : ChemistryClassItem {

        public override void SetStaticDefaults() {
            
            Tooltip.SetDefault("Tosses chunks of wood at foes");

        }

        public override void SafeSetDefaults() {

            item.damage = 11;
            item.crit = 4;
            item.knockBack = 1;
            item.useTime = 30;
            item.useAnimation = 30;

            item.noMelee = true;

            item.width = 32;
            item.height = 32;

            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;

            item.shoot = ModContent.ProjectileType<Projectiles.EarlygameFL.SapinatorProjectile>();
            item.shootSpeed = 9;

            item.rare = 0;
            item.value = Item.buyPrice(0, 0, 0, 99);

            minutesToDecay = 1f;

            SetRefinementData(

                (ItemID.Acorn, 1 / 9f)

                );

        }

        public override void AddRecipes() => this.SetRecipe(18, 1, ("Wood", 12), (ItemID.Acorn, 2));

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Vector2 vector = new Vector2(speedX, speedY);
            vector = vector.RotateRandom(MathHelper.TwoPi / 60);

            Projectile.NewProjectile(

                position, vector, type, damage, knockBack, player.whoAmI

                );

            return false;

        }

    }
}
