using System;
using ChemistryClass.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class SulfurFlame : ModProjectilePlus {

        public override string Texture => "Terraria/Item_0";

        public override void SafeSetDefaults() {

            projectile.damage = 5;
            projectile.knockBack = 1;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.width = 16;
            projectile.height = 16;

            projectile.tileCollide = false;
            projectile.timeLeft = 25;

            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;

            projectile.ignoreWater = true;

        }

        public override void AI() {

            int rand = Main.rand.Next(6, 11);

            for (int _ = 0; _ < rand; _++) {
                Dust.NewDustDirect(
                    projectile.position - projectile.Size / 2,
                    projectile.width,
                    projectile.height,
                    ModContent.DustType<Dusts.SulfurFlame>(),
                    Scale: Main.rand.NextFloat(1f, 2f)
                    );
            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfurFire>(), 240);

        }

        public override void OnHitPvp(Player player, int damage, bool crit) {

            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfurFire>(), 240);

        }

    }
}
