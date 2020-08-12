using System;
using Terraria;
using Terraria.ModLoader;
using ChemistryClass.ModUtils;
using ChemistryClass.ModUtils.Constants;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class RustTippedArrow : ModProjectilePlus {

        public int DustType => DustID.Iron;

        public int DustTime {

            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;

        }

        public override void SafeSetDefaults() {

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.damage = 6;

            projectile.width = 6;
            projectile.height = 6;

            this.drawOffsetX = this.drawOriginOffsetY = -4;

            projectile.damage = 6;

            resetDustTime();

            projectile.ignoreWater = true;

        }

        public override void AI() {

            projectile.rotation = projectile.velocity.ToRotation() + CCUtils.HALF_PI_FLOAT;

            projectile.velocity.Y += 0.1f;
            projectile.velocity.Y.Clamp(-10, 10);

            if (FrameCounter >= DustTime) {
                newDust();
                resetDustTime();
                FrameCounter = -1;
            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Rusted>(), 120);
            for (int _ = 0; _ < 5; _++) { newDust(); }

        }

        public override void OnHitPvp(Player target, int damage, bool crit) {
            for (int _ = 0; _ < 5; _++) { newDust(DustID.Dirt); }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            for (int _ = 0; _ < 5; _++) { newDust(DustID.Dirt); }
            return true;
        }

        private void newDust(int type = -1) {

            if (type == -1) type = DustType;

            Dust.NewDust(projectile.Center,
                             0, 0, type,
                             Main.rand.NextFloat(-1, 1),
                             Main.rand.NextFloat(-1, 1));

        }

        private void resetDustTime()
            => DustTime = Main.rand.Next(3, 15);

    }
}
