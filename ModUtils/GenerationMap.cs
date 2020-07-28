using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChemistryClass.ModUtils {

    public struct RandChance {

        static RandChance() => new RandChance(0);
        public RandChance(float v) => _value = v;

        private float _value;
        public float Value {
            get => _value;
            set => _value = value.Clamp(0, 1);
        }

        public bool Evaluate()
            => WorldGen.genRand.NextFloat() < Value;

        public static implicit operator float(RandChance _this) => _this.Value;
        public static explicit operator RandChance(float _this) => new RandChance(_this);

        public static RandChance operator +(RandChance a) => a;
        public static bool operator true(RandChance a) => a.Evaluate();
        public static bool operator false(RandChance a) => a.Evaluate();
        public static RandChance operator !(RandChance a) => (RandChance)(1 - a.Value);

        public static RandChance operator +(RandChance a, RandChance b) => (RandChance)(a.Value + b);
        public static RandChance operator +(RandChance a, float b) => (RandChance)(a.Value + b);
        public static RandChance operator -(RandChance a, RandChance b) => (RandChance)(a.Value - b);
        public static RandChance operator -(RandChance a, float b) => (RandChance)(a.Value - b);
        public static RandChance operator |(RandChance a, RandChance b) => a + b;
        public static bool operator |(RandChance a, bool b) => a.Evaluate() || b;
        public static RandChance operator &(RandChance a, RandChance b) => a * b;
        public static bool operator &(RandChance a, bool b) => a.Evaluate() && b;

        public static RandChance operator *(RandChance a, RandChance b) => (RandChance)(a.Value * b);
        public static RandChance operator *(RandChance a, float b) => (RandChance)(a.Value * b);
        public static RandChance operator /(RandChance a, RandChance b) => (RandChance)(a.Value / b);
        public static RandChance operator /(RandChance a, float b) => (RandChance)(a.Value / b);

    }

    public static class RandomMapping {

        public static void PlaceInWorld(this RandChance[,] map, ushort tileId, int i, int j, Predicate<Tile> allowPlace) {

            Tile curTile;

            for (int iOff = 0; iOff < map.GetLength(0); iOff++) {
                for (int jOff = 0; jOff < map.GetLength(1); jOff++) {

                    curTile = Framing.GetTileSafely(i + iOff, j + jOff);

                    if(allowPlace(curTile) && map[iOff, jOff].Evaluate()) {

                        WorldGen.PlaceTile(i + iOff, j + jOff, tileId, true, true);

                    }

                }
            }

        }

        public static RandChance[,] EllipseRandom(int w, int h, int steps = 20) {

            RandChance[,] ret = EllipseGradient(2*w/3, 2*h/3).Resize(w, h);

            for(int step = 0; step < steps; step++) {

                int randW = WorldGen.genRand.Next(w / 16, w / 6);
                int randH = WorldGen.genRand.Next(w / 16, w / 6);

                int randI = WorldGen.genRand.Next(0, w);
                int randJ = WorldGen.genRand.Next(0, h);

                if(ret[randI, randJ] < 0.1f || ret[randI, randJ] > 0.7f) {
                    step--;
                    continue;
                }

                RandChance[,] randEllipse = EllipseGradient(randW, randH).ResizeCanvas(w, h, randI, randJ).ScaleValues(0.8f);

                if(ret[randI, randJ].Evaluate()) ret.Add(randEllipse);
                else ret.Subtract(randEllipse);

            }

            ret = ret.Smooth(3);
            ret = ret.ScaleValues(1.2f);

            return ret;

        }

        public static RandChance[,] CircleGradient(int size) {

            RandChance[,] ret = new RandChance[size, size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            Vector2 curVec;

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {

                    curVec = new Vector2(i, j);
                    ret[i, j] = (RandChance)(1f - 2 * Vector2.Distance(curVec, center) / size);

                }
            }

            return ret;

        }

        public static RandChance[,] EllipseGradient(int w, int h)
            => CircleGradient(Math.Min(w, h)).Resize(w, h);

        public static RandChance GetBound(this RandChance[,] chances, int x, int y) {

            if (x < 0) x = 0;
            if (x >= chances.GetLength(0)) x = chances.GetLength(0) - 1;

            if (y < 0) y = 0;
            if (y >= chances.GetLength(1)) y = chances.GetLength(1) - 1;

            return chances[x, y];

        }

        public static RandChance GetVoided(this RandChance[,] chances, int x, int y) {

            if (x < 0) return default;
            if (x >= chances.GetLength(0)) return default;

            if (y < 0) return default;
            if (y >= chances.GetLength(1)) return default;

            return chances[x, y];

        }

        public static RandChance[,] Smooth(this RandChance[,] orig, int passes = 1) {

            RandChance[,] ret = orig;

            for (int i = 0; i < ret.GetLength(0); i++) {
                for (int j = 0; j < ret.GetLength(1); j++) {

                    ret[i, j] =
                        (orig.GetBound(i, j) + orig.GetBound(i + 1, j) +
                         orig.GetBound(i, j + 1) + orig.GetBound(i - 1, j) +
                         orig.GetBound(i, j - 1)) / 5f;

                }
            }

            if (--passes == 0) return ret;
            else return Smooth(ret, passes);

        }

        public static RandChance[,] Resize(this RandChance[,] orig, int w, int h) {

            RandChance[,] ret = new RandChance[w, h];

            int origW = orig.GetLength(0);
            int origH = orig.GetLength(1);

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    float iOrig = (float)i / w * origW;
                    float jOrig = (float)j / h * origH;
                    RandChance nearest = orig[(int)iOrig, (int)jOrig];
                    ret[i, j] = nearest;

                }
            }

            return ret.Smooth();

        }

        public static RandChance[,] ResizeCanvas(this RandChance[,] orig, int w, int h, int displacementX, int displacementY) {

            RandChance[,] ret = new RandChance[w, h];

            int realDX = displacementX + w / 2 - orig.GetLength(0) / 2;
            int realDY = displacementY + h / 2 - orig.GetLength(1) / 2;

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {

                    ret[i, j] = orig.GetVoided(i - realDX, j - realDY);

                }
            }
            return ret;

        }

        public static RandChance[,] ForceCanvasOn(this RandChance[,] orig, ref RandChance[,] other)
            => other = other.Resize(orig.GetLength(0), orig.GetLength(1));

        public static RandChance[,] Add(this RandChance[,] orig, RandChance[,] other) {

            for (int i = 0; i < orig.GetLength(0); i++) {
                for (int j = 0; j < orig.GetLength(1); j++) {

                    orig[i, j] += other.GetVoided(i, j);

                }
            }

            return orig;

        }

        public static RandChance[,] Subtract(this RandChance[,] orig, RandChance[,] other) {

            for (int i = 0; i < orig.GetLength(0); i++) {
                for (int j = 0; j < orig.GetLength(1); j++) {

                    orig[i, j] -= other.GetVoided(i, j);

                }
            }

            return orig;

        }

        public static RandChance[,] ScaleValues(this RandChance[,] orig, float factor) {

            for (int i = 0; i < orig.GetLength(0); i++) {
                for (int j = 0; j < orig.GetLength(1); j++) {

                    orig[i, j] *= factor;

                }
            }

            return orig;

        }

    }

}
