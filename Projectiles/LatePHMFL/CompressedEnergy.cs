using System;
using ChemistryClass.Items.Weaponry.LatePHM;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class CompressedEnergy : ModProjectilePlus {

        public float FadeState {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float Shots {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void SafeSetDefaults() {

            projectile.tileCollide = true;

            projectile.width =
            projectile.height = 32;

        }

        public override void FirstFrame() {
            CritChance = 0;
            projectile.alpha = 255;
            Shots = 0;
        }

        public override void AI() {

            if (FadeState == 2) {
                projectile.alpha += 10;
                if (projectile.alpha > 250) projectile.Kill();
            }

            if (FadeState == 1) {
                projectile.alpha += (CCUtils.PingPong((int)ChemistryClass.UnpausedUpdateCount, 12) - 6) / 3;
                if(ChemistryClass.TimeIsMultOf(17)) {
                    int dmg = OwningPlayer.HeldItem.type == ModContent.ItemType<EnergyCompressorStaff>() ? OwningPlayer.HeldItem.Chemistry().currentDamage : 5;
                    Projectile.NewProjectile(
                        projectile.Center, Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(6, 8), Main.rand.NextFloat(6, 8)),
                        ModContent.ProjectileType<SmallLightning>(), dmg, 0.5f, projectile.owner
                        );
                    Shots++;
                }
                if (Shots > 6) FadeState = 2;
            }

            if (FadeState == 0) {
                projectile.alpha -= 10;
                if (projectile.alpha < 100) FadeState = 1;
            }

            if (projectile.alpha < 150)
            Lighting.AddLight(projectile.Center, LightningBase.LineColor.ToVector3());

            projectile.velocity *= 0.98f;

        }

        public override bool OnTileCollide(Vector2 oldVelocity) {

            projectile.velocity = oldVelocity;
            FadeState = 2;

            return false;

        }

    }
}
