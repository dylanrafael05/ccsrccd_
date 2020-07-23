using System;
using ChemistryClass.Dusts;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles {
    public class GlassShard : ModProjectilePlus {

        public override void SafeSetDefaults() {

            projectile.damage = 5;
            projectile.knockBack = 3;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.penetrate = 2;
            projectile.maxPenetrate = 2;

            projectile.width = 6;
            projectile.height = 6;

        }

        public override void AI() {

            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;

        }

        public override void Kill(int timeLeft) => SpawnDusts();
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => SpawnDusts();
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
