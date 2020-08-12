using System;
using ChemistryClass.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class SulfurRain : ModProjectilePlus {

        public override string Texture => "Terraria/Item_0";

        public override void SafeSetDefaults() {

            projectile.damage = 5;
            projectile.knockBack = 1;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.width = 12;
            projectile.height = 12;

            projectile.ignoreWater = true;

        }

        public override void AI() {

            Dust.NewDustPerfect(

                    projectile.Center,
                    ModContent.DustType<SulfurDust2>(),
                    Vector2.UnitX.RotatedByRandom(CCUtils.TWO_PI_FLOAT) / 4f,
                    Scale: 1.25f + Main.rand.NextFloat(-0.1f, 0.1f)

                    );

            Dust.NewDustPerfect(

                    projectile.Center + projectile.velocity / 4,
                    ModContent.DustType<SulfurDust2>(),
                    Vector2.UnitX.RotatedByRandom(CCUtils.TWO_PI_FLOAT) / 4f,
                    Scale: 0.75f + Main.rand.NextFloat(-0.1f, 0.1f)

                    );

            Dust.NewDustPerfect(

                    projectile.Center + projectile.velocity / 2,
                    ModContent.DustType<SulfurDust2>(),
                    Vector2.UnitX.RotatedByRandom(CCUtils.TWO_PI_FLOAT) / 4f,
                    Scale: 0.5f + Main.rand.NextFloat(-0.1f, 0.1f)

                    );

            Dust.NewDustPerfect(

                    projectile.Center + 3 * projectile.velocity / 4,
                    ModContent.DustType<SulfurDust2>(),
                    Vector2.UnitX.RotatedByRandom(CCUtils.TWO_PI_FLOAT) / 4f,
                    Scale: 0.75f + Main.rand.NextFloat(-0.1f, 0.1f)

                    );

            projectile.tileCollide = projectile.position.Y >= Main.screenPosition.Y + Main.MouseScreen.Y;

        }

        public override void Kill(int timeLeft) => SpawnDusts();

        public void SpawnDusts() {

            for (int _ = 0; _ < 5; _++) {

                Dust.NewDustDirect(

                    projectile.Center,
                    3, 3,
                    ModContent.DustType<SulfurDust>(),
                    Main.rand.NextFloat(-2, 2),
                    Main.rand.NextFloat(-2, 2)

                    );

            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfurFire>(), 60);

        }

        public override void OnHitPvp(Player player, int damage, bool crit) {

            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfurFire>(), 60);

        }

    }
}
