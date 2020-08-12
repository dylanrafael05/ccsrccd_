using System;
using System.Collections.Generic;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class RocketBlastBeaker : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "A beaker which has been melted so that it has a fine tip\n" +
                "and has flaming alcohol inside.\n" +
                "It can be used to create small explosions\n" +
                "and to propell oneself into the air."
                );
            Item.staff[item.type] = true;

        }

        public override void SafeSetDefaults() {

            item.width = 20;
            item.height = 28;

            item.damage = 36;
            item.knockBack = 7;
            item.crit = 4;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;

            item.noMelee = true;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 55, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.ExplosiveSpew>();
            item.shootSpeed = 12;

            minutesToDecay = 2f;

            fanName = "KazyHachi <3";

            SetRefinementData(

                (ItemID.Ale, 1 / 2f)

                );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Vector2 vec = new Vector2(speedX, speedY);
            Projectile.NewProjectile(
                position + vec.WithMagnitude(item.width * 4f / 3),
                vec.RotatedBy(Main.rand.NextFloat(-CCUtils.PI_FLOAT / 60f, CCUtils.PI_FLOAT / 60f)),
                type, damage, knockBack, player.whoAmI
                );

            float maxLaunch = 12.5f;
            if(player.velocity.Length() < maxLaunch) {
                player.velocity /= 2f;
                player.velocity += vec.WithMagnitude(-maxLaunch);
                player.velocity.ClampMagnitude(-maxLaunch, maxLaunch);
            }

            return false;

        }

        public override void AddRecipes() {

            this.SetRecipe(
                ModContent.TileType<Tiles.Multitiles.MetallurgyStandTile>(),
                1,
                (ModContent.ItemType<Placeable.Crafting.Beaker>(), 1),
                (ItemID.Ale, 4),
                (ItemID.HellstoneBar, 1)
                );

        }

    }
}
