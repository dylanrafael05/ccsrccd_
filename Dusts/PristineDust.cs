using System;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class PristineDust : ModDust {

        public override bool Update(Dust dust) {

            dust.alpha += 5;
            dust.rotation += (float)Math.PI / 16;
            dust.scale += 0.1f;

            if (dust.alpha > 255) dust.active = false;

            dust.noGravity = true;
            dust.frame = Texture.Bounds;

            Lighting.AddLight(

                dust.position,
                dust.color.ToVector3()

                );

            return true;

        }

    }
}
