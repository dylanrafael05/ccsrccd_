using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class PristineDust : ModDust {

        public override void OnSpawn(Dust dust) {

            dust.scale = 2f;
            dust.noGravity = true;
            dust.fadeIn = 1f;
            dust.alpha = 100;

        }

        public override bool MidUpdate(Dust dust) {

            dust.frame = new Rectangle(0, 0, 6, 6);

            dust.scale -= 0.03f;
            dust.rotation += 0.2f;

            if (dust.scale <= 0.1f) dust.active = false;

            Lighting.AddLight(

                dust.position,
                dust.color.ToVector3() / 2f * dust.scale

            );

            return true;

        }

    }
}
