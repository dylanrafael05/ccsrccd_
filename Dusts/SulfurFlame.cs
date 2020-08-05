using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class SulfurFlame : ModDust {

        public override void OnSpawn(Dust dust) {

            dust.alpha = 175;
            dust.noGravity = true;

            dust.color.R = 200;
            dust.color.G = 250;
            dust.color.B = 50;

        }

        public override bool MidUpdate(Dust dust) {

            dust.scale -= 0.005f;
            dust.rotation += dust.velocity.X / 100;
            dust.velocity *= 0.99f;

            if (dust.scale <= 0.1f) dust.active = false;

            Lighting.AddLight(dust.position, dust.color.R / 2000f, dust.color.G / 2000f, dust.color.B / 2000f);

            return false;

        }

    }
}
