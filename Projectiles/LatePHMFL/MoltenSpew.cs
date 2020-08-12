using System;
using ChemistryClass.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class MoltenSpew : ModProjectilePlus {

        public override string Texture => "Terraria/Item_0";

        public override void SafeSetDefaults() {

            projectile.damage = 5;
            projectile.knockBack = 1;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.width = 8;
            projectile.height = 8;

            projectile.ignoreWater = true;

        }

        public override void AI() {

            int rand = Main.rand.Next(6, 11);

            for (int _ = 0; _ < rand; _++) {
                Dust d = Dust.NewDustDirect(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.Fire,
                    Scale: Main.rand.NextFloat(1f, 2f)
                    );
                d.noGravity = true;
            }

            if (projectile.ai[0] == 0f)
                projectile.ai[0] = Main.rand.NextFloat(0.15f, 0.3f);

            projectile.velocity.Y += projectile.ai[0];
            projectile.velocity.Y.Clamp(-15, 15);

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            if(Main.rand.NextBool(4))
                target.AddBuff(BuffID.OnFire, 180);

        }

        public override void OnHitPvp(Player player, int damage, bool crit) {

            if(Main.rand.NextBool(4))
                player.AddBuff(BuffID.OnFire, 180);

        }

        public override void Kill(int timeLeft) {
            int rand = Main.rand.Next(5, 10);

            for (int _ = 0; _ < rand; _++) {
                Dust d = Dust.NewDustDirect(
                    projectile.position - projectile.Size / 2,
                    projectile.width,
                    projectile.height,
                    DustID.Fire,
                    Scale: Main.rand.NextFloat(2f, 4f)
                    );
                d.noGravity = true;
            }
        }

    }
}
