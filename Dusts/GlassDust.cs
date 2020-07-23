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

        public override bool Update(Dust dust) {

            dust.scale -= 0.01f;
            dust.rotation += dust.velocity.X / 12800f;

            if (dust.scale <= 0.1f) dust.active = false;

            return true;

        }

    }
}
