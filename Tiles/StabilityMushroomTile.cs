using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChemistryClass.Tiles {
    public class StabilityMushroomTile : ModTile {

        public override void SetDefaults() {

            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileCut[Type] = true;
            Main.tileLighted[Type] = true;

            this.minPick = 0;
            this.dustType = DustID.Dirt;
            this.soundType = SoundID.Grass;

            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.MushroomGrass };

            TileObjectData.addTile(Type);

        }

        public override bool Drop(int i, int j) {

            Item.NewItem(i * 16, j * 16, 0, 0, ModContent.ItemType<Items.Accessories.StabilityMushroom>());
            return false;

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = 0f;
            g = 0f;
            b = 1f;
        }

    }
}
