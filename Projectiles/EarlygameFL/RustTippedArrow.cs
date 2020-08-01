using System;
using Terraria;
using Terraria.ModLoader;
using TUtils.Constants;
using ChemistryClass.ModContents;

namespace ChemistryClass.Projectiles.EarlygameFL {
    public class RustTippedArrow : ModProjectileWithCrit {

        public override void SafeSetDefaults() {

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.damage = 6;
            projectile.width = 6;
            projectile.height = 6;

            projectile.aiStyle = ProjectileAIStyle.Arrow;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Rusted>(), 120);

        }

    }
}
