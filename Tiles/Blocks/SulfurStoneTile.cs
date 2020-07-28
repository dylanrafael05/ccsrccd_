using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChemistryClass.Tiles.Blocks {
    public class SulfurStoneTile : ModTile {

        public override void SetDefaults() {

            this.dustType = DustID.Stone;
            this.minPick = 55;
            this.soundType = SoundID.Tink;
            this.drop = ModContent.ItemType<Items.Placeable.SulfurStone>();

            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 440;
            Main.tileShine[Type] = 975;

            //Main.tileBlendAll[Type] = true;

            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<SulfuricOreTile>()] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;
            //Main.tileBlockLight[Type] = true;

            ModTranslation text = CreateMapEntryName();
            text.AddTranslation(GameCulture.English, "Sulphorous Deposit");
            AddMapEntry(new Color(0.6f, 0.6f, 0.3f), text);

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {

            r = 0.03f;
            g = 0.05f;
            b = 0.01f;

        }

    }
}
