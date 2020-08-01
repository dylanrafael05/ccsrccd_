using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChemistryClass.ModContents;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class SapinatorProjectile : ModProjectileWithCrit {

        public int DustTimer {

            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;

        }

        public int DustWaitTime {

            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;

        }

        public virtual int DustType => DustID.Dirt;

        public override void SafeSetDefaults() {

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.damage = 6;
            projectile.width = 12;
            projectile.height = 12;

            drawOffsetX = -3;
            drawOriginOffsetY = -3;

        }

        public override void AI() {

            //Ensure valid dust timer & increment
            if (DustWaitTime == 0) DustWaitTime = Main.rand.Next(15, 31);
            DustTimer++;

            //Rotate over time
            projectile.rotation += (float)Math.PI / 100f * projectile.velocity.X;

            //Gravity
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 10) projectile.velocity.Y = 10f;

            //Occasional dust
            if (DustTimer >= DustWaitTime) {

                newDust();
                DustWaitTime = Main.rand.Next(20, 51);
                DustTimer = 0;

            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft) {

            for (int _ = 0; _ < 5; _++) { newDust(); }

        }

        private void newDust() {

            Dust.NewDust(projectile.Center,
                             0, 0, DustType,
                             Main.rand.NextFloat(-1, 1),
                             Main.rand.NextFloat(-1, 1));

        }

    }
}
