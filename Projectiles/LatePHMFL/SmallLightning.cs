using System;
namespace ChemistryClass.Projectiles.LatePHMFL {
    public class SmallLightning : LightningBase {

        public override int Width => 8;
        public override int Penetrate => 100;
        public override int TimeLeft => 30;

        public override void SaferSetDefaults() {
            projectile.damage = 5;
            projectile.friendly = true;
        }

    }
}
