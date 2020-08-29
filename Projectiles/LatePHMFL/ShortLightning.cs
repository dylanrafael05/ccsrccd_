using System;
namespace ChemistryClass.Projectiles.LatePHMFL {
    public class ShortLightning : LightningBase {

        public override int Width => 6;
        public override int Penetrate => 100;
        public override int TimeLeft => 5;

        public override void SaferSetDefaults() {
            projectile.damage = 5;
            projectile.friendly = true;
        }

    }
}
