using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class GlassDust : ModDust {

        public override void OnSpawn(Dust dust) {

            dust.scale = 1.25f;
            dust.noLight = true;

            dust.frame = new Rectangle(0, 0, 8, 8);

        }

        public override bool MidUpdate(Dust dust) {

            dust.scale -= 0.05f;
            dust.rotation += dust.velocity.X / 120f;

            if (dust.scale <= 0.1f) dust.active = false;

            return false;

        }

    }
}
