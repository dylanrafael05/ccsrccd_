using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class SulfurDust : ModDust {

        public override void OnSpawn(Dust dust) {

            dust.alpha = 0;
            dust.noLight = true;
            dust.noGravity = true;

        }

        public override bool MidUpdate(Dust dust) {

            dust.rotation += dust.velocity.X / 180f;
            dust.alpha += 5;
            dust.velocity *= 0.975f;

            if (dust.alpha >= 250) dust.active = false;

            return false;

        }

    }
}
