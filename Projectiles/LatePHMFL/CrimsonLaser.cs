using System;
using ChemistryClass.ModUtils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Projectiles.LatePHMFL {
    public class CrimsonLaser : ModProjectile {

        public override void SetDefaults() {

            projectile.width = 16;
            projectile.height = 2;

            projectile.damage = 5;
            projectile.knockBack = 0;

            projectile.friendly = true;
            projectile.hostile = false;

            projectile.ignoreWater = true;

        }

        public override void AI() {

            projectile.rotation = projectile.velocity.ToRotation() + CCUtils.PI_FLOAT;

        }

    }
}
