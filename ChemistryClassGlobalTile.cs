using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChemistryClass {
    public class ChemistryClassGlobalTile : GlobalTile {

        public override void RandomUpdate(int i, int j, int type) {

            if (type != TileID.MushroomGrass) return;

            Tile tile = Framing.GetTileSafely(i, j - 1);

            if (tile.active()) return;
            if (Main.rand.Next(400) != 0) return;

            WorldGen.Place1x1(i, j - 1, ModContent.TileType<Tiles.StabilityMushroomTile>());

        }

    }
}
