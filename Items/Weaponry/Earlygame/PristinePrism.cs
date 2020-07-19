using System;
using System.Linq;
using ChemistryClass.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class PristinePrism : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A prism which bends light into blinding beams of peircing light.");

        }

        public override void SafeSetDefaults() {

            //item.CloneDefaults(ItemID.LastPrism);
            item.channel = true;

            item.damage = 2;
            item.crit = 50;
            item.knockBack = 0;

            item.noMelee = true;

            item.width = 32;
            item.height = 32;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noUseGraphic = true;

            item.reuseDelay = 5;
            item.autoReuse = true;

            item.UseSound = SoundID.Item15;

            //item.UseSound = SoundID.Item15;

            item.shoot = ModContent.ProjectileType<PristinePrismHoldout>();
            item.shootSpeed = 32f;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);

            minutesToDecay = 2.5f;

            SetRefinementData(

                (ModContent.ItemType<Materials.PrismaticLens>(), 1f)

                );

            this.impureMult = 0.2f;

        }

        public override float PurityDamageMult => 1;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            //DEBUGGING
            //Main.NewText("AAAA");

            if (player.ownedProjectileCounts[ModContent.ProjectileType<PristinePrismHoldout>()] == 0) {

                Projectile.NewProjectile(

                    position,
                    new Vector2(speedX, speedY),
                    type,
                    damage,
                    knockBack,
                    player.whoAmI,
                    0

                    );

            } else {

                PristinePrismHoldout holdout = Main.projectile.
                                                ToList().
                                                Find(proj => proj.modProjectile is PristinePrismHoldout h
                                                && proj.owner == player.whoAmI)
                                                .modProjectile as PristinePrismHoldout;

                if (holdout == null) return true;

                Projectile.NewProjectile(

                    holdout.projectile.position,
                    holdout.projectile.velocity,
                    holdout.projectile.type,
                    damage,
                    knockBack,
                    player.whoAmI,
                    holdout.FrameCounter

                    );

                holdout.projectile.Kill();

                //DEBUGGING
                //Main.NewText("TEST");

            }

            return false;

        }

        public override void AddRecipes() {

            this.SetRecipe(

                TileID.GlassKiln,
                1,
                (ModContent.ItemType<Materials.PrismaticLens>(), 5)

                );

        }

    }
}