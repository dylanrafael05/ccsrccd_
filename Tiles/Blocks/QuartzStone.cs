using System;
using System.Linq;
using ChemistryClass.ModUtils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChemistryClass.Tiles.Blocks {
    public class QuartzStone : ModTile {

        public override void SetDefaults() {

            this.dustType = DustID.Stone;
            this.minPick = 60;
            this.soundType = SoundID.Tink;
            this.drop = ModContent.ItemType<Items.Materials.Earlygame.Quartz>();

            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 200;
            Main.tileShine[Type] = 400;

            //Main.tileBlendAll[Type] = true;

            Main.tileMergeDirt[Type] = true;

            this.CreateMerge(TileID.Stone);

            Main.tileSolid[Type] = true;
            //Main.tileBlockLight[Type] = true;

            ModTranslation text = CreateMapEntryName();
            text.AddTranslation(GameCulture.English, "Quartz");
            AddMapEntry(new Color(0.6f, 0.6f, 0.85f), text);

        }

        public override bool CanExplode(int i, int j)
            => false;

        public float ProjChance => 0.29f;

        public override void NearbyEffects(int i, int j, bool closer) {

            if (!closer || Main.gameInactive || Main.gamePaused) return;
            if (!ChemistryClassWorld.QuartzLightningValid || Main.rand.NextFloat() > ProjChance) return;

            //Main.NewText("lightning uwu");

            Vector2 realPos = new Point(i, j).ToWorldCoordinates();
            Player clPl = CCUtils.Min(Main.player, p => p.Distance(realPos));
            if (clPl.Distance(realPos) > 300) return;
            if (clPl.Distance(realPos) < 32) return;

            Projectile.NewProjectileDirect(

                realPos, 
                (clPl.Center - realPos).WithMagnitude(Main.rand.NextFloat(7, 10)),
                ModContent.ProjectileType<Projectiles.PreHMHS.LightningHostile>(),
                10,
                3f

                );

            ChemistryClassWorld.QuartzLightningCount++;
            Main.PlaySound(SoundID.Item12.WithVolume(0.4f));

            //Main.NewText(proj.Center);

        }

    }
}
