using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

            item.shoot = ModContent.ProjectileType<Projectiles.SapinatorProjectile>();
            item.shootSpeed = 9;

            item.rare = 0;
            item.value = Item.buyPrice(0, 0, 0, 99);

            minutesToDecay = 2;

            SetRefinementData(

                (ItemID.Acorn, 0.15f)

                );

        }

        public override void AddRecipes() {

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup(RecipeGroupID.Wood, 12);
            recipe.AddIngredient(ItemID.Acorn, 2);

            recipe.AddTile(ChemistryClass.BeakerTileID);

            recipe.SetResult(this);

            recipe.AddRecipe();

        }

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
