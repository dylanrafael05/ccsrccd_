using System;
using System.Linq;
using ChemistryClass.Projectiles.EarlygameFL;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUtils;
using TUtils.Timers;

namespace ChemistryClass.Items.Weaponry.Earlygame {
    public class PristinePrism : ChemistryClassItem {

        public override void SetStaticDefaults() {

            Tooltip.SetDefault("A prism which bends light into blinding beams of color");

        }

        public override void SafeSetDefaults() {

            //item.CloneDefaults(ItemID.LastPrism);
            item.channel = true;

            item.damage = 8;
            item.crit = 10;
            item.knockBack = 0;

            item.noMelee = true;

            item.width = 32;
            item.height = 32;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noUseGraphic = true;

            item.autoReuse = true;

            item.UseSound = SoundID.Item15;

            //item.UseSound = SoundID.Item15;

            item.shoot = ModContent.ProjectileType<PristinePrismHoldout>();
            item.shootSpeed = 32f;

            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);

            minutesToDecay = 3f;

            SetRefinementData(

                (ModContent.ItemType<Materials.Earlygame.PrismaticLens>(), 1f)

                );

            this.impureMult = 0.2f;

        }

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
                                                Find(proj => proj.modProjectile is PristinePrismHoldout
                                                && proj.owner == player.whoAmI)
                                                .modProjectile as PristinePrismHoldout;

                if (holdout == null) return true;

                int iProj = Projectile.NewProjectile(

                    holdout.projectile.position,
                    holdout.projectile.velocity,
                    holdout.projectile.type,
                    damage,
                    knockBack,
                    player.whoAmI

                    );

                PristinePrismHoldout pProj = Main.projectile[iProj].modProjectile as PristinePrismHoldout;

                pProj.SetDefaults();

                pProj.timer = new Timer();
                pProj.timer.Set(holdout.timer);

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
                (ModContent.ItemType<Materials.Earlygame.PrismaticLens>(), 5)

                );

        }

    }
}