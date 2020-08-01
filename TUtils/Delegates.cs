using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace TUtils {
    public static class Delegates {

        public delegate void CoordinateAction(int i, int j);
        public delegate void AreaAction(Rectangle rec);

        public delegate bool CoordinatePredicate(int i, int j);
        public delegate bool AreaPredicate(Rectangle rec);
        public delegate bool TilePredicate(Tile t);

    }
}
