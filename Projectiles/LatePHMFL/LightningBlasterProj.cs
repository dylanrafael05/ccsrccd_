using System;
namespace ChemistryClass.Projectiles.LatePHMFL {
    public class LightningBlasterProj : LightningBase {

        public override bool TileCollide => true;
        public override float TargetFLength => 300;
        public override int Penetrate => 2;
        public override int Width => 16;

        public override void SaferSetDefaults() {
            projectile.friendly = true;
            projectile.damage = 60;
            projectile.knockBack = 5;
        }

    }
}
