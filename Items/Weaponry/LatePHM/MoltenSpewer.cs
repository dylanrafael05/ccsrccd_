using System;
using System.Collections.Generic;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class MoltenSpewer : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "A water gun turned hot"
                );

        }

        public override void SafeSetDefaults() {

            item.width = 50;
            item.height = 28;

            item.damage = 21;
            item.knockBack = 3;
            item.crit = 4;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;

            item.noMelee = true;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 60, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.MoltenSpew>();
            item.shootSpeed = 10;

            minutesToDecay = 4f;

            fanName = "Ender";

            SetRefinementData(

                (ItemID.LavaBucket, 1)

                );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Vector2 vec = new Vector2(speedX, speedY);

            for (int _ = 0; _ < 3; _++) {
                Projectile.NewProjectile(
                    position + vec.WithMagnitude(item.width * 4f / 3),
                    vec.RotatedBy(Main.rand.NextFloat(-CCUtils.PI_FLOAT / 40f, CCUtils.PI_FLOAT / 40f)),
                    type, damage, knockBack, player.whoAmI
                    );
            }

            return false;

        }

        public override void AddRecipes() {

            this.SetRecipe(
                ChemistryClass.BeakerTileID,
                1,
                (ItemID.HellstoneBar, 2),
                (ItemID.Obsidian, 12),
                (ItemID.LavaBucket, 1)
                );

        }

    }
}
