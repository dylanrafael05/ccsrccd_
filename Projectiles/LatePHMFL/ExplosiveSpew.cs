using System;
using ChemistryClass.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class ExplosiveSpew : ModProjectilePlus {

        public override string Texture => "Terraria/Item_0";

        public override void SafeSetDefaults() {

            projectile.damage = 40;
            projectile.knockBack = 1;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.width = 6;
            projectile.height = 6;

            projectile.ignoreWater = true;

        }

        public override void AI() {

            int rand = Main.rand.Next(10, 16);

            for (int _ = 0; _ < rand; _++) {
                Dust d = Dust.NewDustPerfect(
                    projectile.position +
                    new Vector2(
                        Main.rand.NextFloat(-projectile.velocity.X, 0),
                        Main.rand.NextFloat(-projectile.velocity.Y, 0)
                        ),
                    DustID.Fire,
                    Scale: Main.rand.NextFloat(0.5f, 1f)
                    );
            }

            projectile.velocity.Y += 0.1f;
            projectile.velocity.Y.Clamp(-15, 15);

        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
            damage = 1;
            target.immune = true;
            target.immuneTime = 1;
        }

        public override void SafeModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            damage = 1;
            target.immune[projectile.owner] = 1;
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(
                projectile.position,
                Vector2.Zero,
                ModContent.ProjectileType<NoTileExplosion>(),
                projectile.damage,
                projectile.knockBack,
                projectile.owner
                );
        }

    }
}
