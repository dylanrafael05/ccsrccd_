using System;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class BloodyBeing : RottingBeing {

        private protected override int ReqItem => ModContent.ItemType<Items.Weaponry.LatePHM.BloodyBeingStaff>();

        public override void PostAI() {

            if (TargetIndex == -1) return;

            if (Main.npc[TargetIndex].Distance(projectile.position) > 25 && FrameCounter % 240 == 0) {

                Vector2 speed = (Main.npc[TargetIndex].position - projectile.position).WithMagnitude(20f);

                Projectile.NewProjectile(
                    projectile.Center + speed * 2,
                    speed,
                    ModContent.ProjectileType<CrimsonLaser>(),
                    projectile.damage * 3,
                    projectile.knockBack,
                    projectile.owner
                    );

                Main.PlaySound(SoundID.Item12);

            }

        }

    }
}
