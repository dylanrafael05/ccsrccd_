using System;
using Terraria;
using Terraria.ID;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class OvergrowthLauncherProjectile : SapinatorProjectile {

        public override int DustType => DustID.Grass;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            if (Main.rand.NextBool(4)) target.AddBuff( BuffID.Poisoned, 120 );

        }

    }
}
