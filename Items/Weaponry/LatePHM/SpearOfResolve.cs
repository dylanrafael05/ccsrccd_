using System;
using System.Collections.Generic;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.LatePHM {
    public class SpearOfResolve : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault(
                "A spear for those worthy"
                );

        }

        public override void SafeSetDefaults() {

            item.width = 64;
            item.height = 64;

            item.damage = 17;
            item.knockBack = 2;
            item.crit = 8;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

            item.noMelee = true;

            item.rare = 2;
            item.value = Item.buyPrice(0, 0, 35, 0);

            item.shoot = ModContent.ProjectileType<Projectiles.LatePHMFL.SpearOfResolveProjectile>();
            item.shootSpeed = 1;

            minutesToDecay = 4f;

            fanName = "EnderCreep72";

            SetRefinementData(

                (ModContent.ItemType<Materials.Earlygame.RustedPowder>(), 1 / 15f),
                (ItemID.Bone, 1 / 3f)

                );

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            Projectile.NewProjectile(
                position,
                new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-CCUtils.PI_FLOAT / 20f, CCUtils.PI_FLOAT / 20f)),
                type, damage, knockBack, player.whoAmI
                );

            return false;

        }

        public override bool CanUseItem(Player player)
            => player.ownedProjectileCounts[item.shoot] < 1;

        public override void AddRecipes() {

            this.SetRecipe(
                ChemistryClass.BeakerTileID,
                1,
                (ItemID.Wood, 4),
                ("IronBar", 10),
                (ItemID.Bone, 4),
                (ModContent.ItemType<Materials.Earlygame.RustedPowder>(), 18)
                );

        }

    }
}
