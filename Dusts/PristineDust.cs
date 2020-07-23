using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChemistryClass.Dusts {
    public class PristineDust : ModDust {

        public override void OnSpawn(Dust dust) {

            dust.scale = 3f;
            dust.noLight = true;

            dust.noGravity = true;

            dust.frame = new Rectangle(0, 0, 6, 6);

        }

        public override bool Update(Dust dust) {

            dust.alpha += 5;
            dust.scale -= 0.003f;
            dust.rotation += 0.01f;

            if (dust.scale <= 0.1f) dust.active = false;

            if (dust.scale >= 0.5f)
                Lighting.AddLight(

                    dust.position,
                    dust.color.ToVector3()

                );

            return true;

        }

    }
}
