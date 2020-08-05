using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChemistryClass.Tiles.Blocks {
    public class SulfuricOreTile : ModTile {

        public override void SetDefaults() {

            this.dustType = ModContent.DustType<Dusts.SulfurDust>();
            this.minPick = 100;
            this.mineResist = 4f;
            this.soundType = SoundID.Tink;
            this.drop = ModContent.ItemType<Items.Placeable.Blocks.SulfurClump>();

            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 450;
            Main.tileShine2[Type] = true;
            //Main.tileShine[Type] = 975;

            //Main.tileBlendAll[Type] = true;

            Main.tileMergeDirt[Type] = true;
            //Main.tileMerge[Type][TileID.Stone] = true;
            //Main.tileMerge[TileID.Stone][Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;
            //Main.tileBlockLight[Type] = true;

            ModTranslation text = CreateMapEntryName();
            text.AddTranslation(GameCulture.English, "Sulfur");
            AddMapEntry(new Color(0.8f, 0.8f, 0.2f), text);

        }

        public static readonly Vector3 LightColor = new Vector3(0.1f, 0.15f, 0.02f);

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {

            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;

        }

        public override bool CanExplode(int i, int j)
            => false;

    }
}
