using System;
using Terraria;
using Terraria.ID;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class FrozenHurlerProjectile : SapinatorProjectile {

        public override int DustType => DustID.Ice;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(BuffID.Frostburn, 120);

        }

    }
}
