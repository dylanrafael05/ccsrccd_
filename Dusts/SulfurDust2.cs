using System;
using ChemistryClass.ModUtils;
using ChemistryClass.Tiles.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class SulfurDust2 : ModDust {

        public override bool Autoload(ref string name, ref string texture) {

            texture = "ChemistryClass/Dusts/SulfurDust";
            return true;

        }

        public override void OnSpawn(Dust dust) {

            dust.alpha = 0;
            dust.noGravity = true;

            dust.color.R = 200;
            dust.color.G = 250;
            dust.color.B = 50;

        }

        public override bool MidUpdate(Dust dust) {

            dust.alpha += 8;
            dust.scale -= 0.09f;

            if (dust.scale <= 0.1f) dust.active = false;

            Lighting.AddLight(dust.position, dust.color.R / 2000f, dust.color.G / 2000f, dust.color.B / 2000f);

            return false;

        }

    }
}
