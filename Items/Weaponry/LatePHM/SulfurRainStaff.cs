﻿using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class SulfurRainStaff : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(

                "Lets the corrosive rains fall"

                );

            Item.staff[item.type] = true;

        }

        public override void SafeSetDefaults() {

            item.damage = 20;
            item.crit = 4;
            item.knockBack = 1;

            item.width = 32;
            item.height = 32;

            item.useTime = 50;
            item.useAnimation = 50;
            item.UseSound = SoundID.Item21;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;

            item.noMelee = true;

            item.rare = 3;
            item.value = Item.buyPrice(0, 0, 50, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.SulfurRain>();
            item.shootSpeed = 1;

            minutesToDecay = 3f;

            minPurityMult = 0.8f;
            maxPurityMult = 1.05f;

            SetRefinementData(

                (ModContent.ItemType<Placeable.Blocks.SulfurClump>(), 1 / 5f)

                );

        }

        public override void AddRecipes() {

            this.SetRecipe(

                            ModContent.TileType<Tiles.Multitiles.BunsonBurnerTile>(),
                            1,
                            (ModContent.ItemType<Placeable.Blocks.SulfurClump>(), 15),
                            ("IronBar", 4)

            );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            for(int _ = 0; _ < Main.rand.Next(4, 7); _++) {

                Vector2 tpe = Main.screenPosition + Main.MouseScreen;
                Vector2 tp = tpe + new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, 10));

                Vector2 av = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-CCUtils.PI_FLOAT / 6, CCUtils.PI_FLOAT / 6));

                Vector2 sp = tp - av * MathHelper.Clamp(Main.MouseScreen.Y, 200, float.MaxValue) * 1.1f;
                Vector2 sv = av * Main.rand.NextFloat(35f, 45f);

                int p = Projectile.NewProjectile(sp, sv, type, damage, knockBack, player.whoAmI);

            }

            return false;

        }

    }
}
