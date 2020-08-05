using System;
using ChemistryClass.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class GlassShard : ModProjectilePlus {

        public override void SafeSetDefaults() {

            projectile.damage = 5;
            projectile.knockBack = 2;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.width = 6;
            projectile.height = 6;

        }

        public override void AI() {

            projectile.rotation = projectile.velocity.ToRotation() + CCUtils.HALF_PI_FLOAT;

        }

        public override void Kill(int timeLeft) => SpawnDusts();
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.immune[projectile.owner] = 7;

        }

        public override void OnHitPvp(Player target, int damage, bool crit) => SpawnDusts();

        public void SpawnDusts() {

            for(int _ = 0; _ < 5; _++) {

                Dust.NewDust(

                    projectile.Center,
                    1, 1,
                    ModContent.DustType<GlassDust>(),
                    Main.rand.NextFloat(-1,1),
                    Main.rand.NextFloat(-1, 1)

                    );

            }

        }

    }
}
