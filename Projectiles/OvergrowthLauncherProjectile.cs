using System;
using Terraria;
using Terraria.ID;

namespace ChemistryClass.Projectiles {
    public class OvergrowthLauncherProjectile : SapinatorProjectile {

        public override int DustType => DustID.Grass;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff( BuffID.Poisoned, 120 );

        }

    }
}
