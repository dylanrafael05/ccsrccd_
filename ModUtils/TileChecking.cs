using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChemistryClass.ModUtils {
    public static class TileChecking {

        public static bool solid(this Tile tile)
            => Main.tileSolid[tile.type];

        public static bool solidTop(this Tile tile)
           => Main.tileSolidTop[tile.type];

        public static bool axe(this Tile tile)
           => Main.tileAxe[tile.type];

        public static bool hammer(this Tile tile)
           => Main.tileHammer[tile.type];

        public static bool solidTrue(this Tile tile)
            => tile.solid() && tile.active();

        public static bool inpassable(this Tile tile)
            => tile.solidTrue() && !tile.solidTop();

        public static IEnumerable<Tile> GetContainedTiles(this Rectangle area) {
            for(int i = 0; i < area.Width; i++) {
                for (int j = 0; j < area.Height; j++) {
                    yield return Framing.GetTileSafely(i + area.Left, j + area.Top);
                }
            }
        }

        public static IEnumerable<Tile> GetContainedTiles(this Entity entity)
            => entity.GetTileHitbox().GetContainedTiles();

    }
}
