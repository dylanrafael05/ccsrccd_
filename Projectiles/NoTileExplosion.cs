using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ChemistryClass.ModUtils;

namespace ChemistryClass.Projectiles {
    public class NoTileExplosion : ModProjectile {

        public override string Texture => "Terraria/Item_0";

        public override void SetDefaults() {

            projectile.width =
            projectile.height = 100;

            projectile.damage = 10;
            projectile.knockBack = 7;

            projectile.friendly = true;
            projectile.hostile = true;

            projectile.penetrate = -1;
            projectile.maxPenetrate = -1;

            projectile.timeLeft = 3;

            projectile.tileCollide = false;

        }

        public override bool ShouldUpdatePosition() => false;

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
            damage = (int)(damage * 0.8f);
        }

        public override void Kill(int timeLeft) {

            //PLAY EXPLOSION SOUND
            Main.PlaySound(SoundID.Item14);

            //SPAWN DUSTS
            for(int _ = 0; _ < 40 * Math.Pow(projectile.scale, 2); _++) {

                Dust.NewDust(
                    projectile.Hitbox.Location.ToVector2(),
                    projectile.Hitbox.Width,
                    projectile.Hitbox.Height,
                    DustID.Fire,
                    Main.rand.NextFloat(-3, 3),
                    Main.rand.NextFloat(-3, 3),
                    Scale: Main.rand.NextFloat(1.5f, 2f)
                    );

                if (_ % 2 == 0) {
                    Dust.NewDust(
                        projectile.Hitbox.Location.ToVector2(),
                        projectile.Hitbox.Width,
                        projectile.Hitbox.Height,
                        DustID.Smoke,
                        Scale: Main.rand.NextFloat(0.9f, 2f)
                        );
                }

            }

            //SPAWN SMOKE
            for(int i = 0; i < 8 * Math.Pow(projectile.scale, 2); i++) {

                double dir = 0;

                if( i % 4 > 0 ) {
                    dir += CCUtils.HALF_PI_FLOAT;
                }
                if (i % 4 > 1) {
                    dir += CCUtils.HALF_PI_FLOAT;
                }
                if (i % 4 > 2) {
                    dir += CCUtils.HALF_PI_FLOAT;
                }

                Gore.NewGore(
                    projectile.Hitbox.Location.ToVector2() +
                    new Vector2(Main.rand.NextFloat(0, projectile.Hitbox.Width),
                                Main.rand.NextFloat(0, projectile.Hitbox.Height)),
                    Main.rand.NextVector2Unit() * 0.5f +
                    Vector2.UnitY.RotatedBy(dir),
                    Main.rand.Next(61, 64),
                    Main.rand.NextFloat(1, 1.7f)
                    );

            }

        }

    }
}
