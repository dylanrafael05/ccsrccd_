using System;
using System.Collections.Generic;
using System.Linq;
using IL.Terraria.Achievements;
using IL.Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace ChemistryClass.ModUtils {

    public static class CCUtils {

        //MATHEMATICAL EXTENSIONS
        public static int Clamp(this ref int val, int min, int max)
            => val = val < min ? min : (val > max ? max : val);
        public static int GetClamp(this int val, int min, int max)
           => val < min ? min : (val > max ? max : val);
        public static long Clamp(this ref long val, long min, long max)
            => val = val < min ? min : (val > max ? max : val);
        public static long GetClamp(this long val, long min, long max)
            => val < min ? min : (val > max ? max : val);
        public static float Clamp(this ref float val, float min, float max)
            => val = val < min ? min : (val > max ? max : val);
        public static float GetClamp(this float val, float min, float max)
            => val < min ? min : (val > max ? max : val);
        public static double Clamp(this ref double val, double min, double max)
            => val = val < min ? min : (val > max ? max : val);
        public static double GetClamp(this double val, double min, double max)
            => val < min ? min : (val > max ? max : val);
        public static decimal Clamp(this ref decimal val, decimal min, decimal max)
            => val = val < min ? min : (val > max ? max : val);
        public static decimal GetClamp(this decimal val, decimal min, decimal max)
            => val < min ? min : (val > max ? max : val);
        public static Vector2 ClampMagnitude(this ref Vector2 val, float min, float max)
            => val = val.Length() < min ? val.WithMagnitude(min) : (val.Length() > max ? val.WithMagnitude(max) : val);
        public static Vector2 GetClampMagnitude(this Vector2 val, float min, float max)
            => val.Length() < min ? val.WithMagnitude(min) : (val.Length() > max ? val.WithMagnitude(max) : val);

        public static int EnforceMin(this ref int val, int min)
            => val = val < min ? min : val;
        public static long EnforceMin(this ref long val, long min)
            => val = val < min ? min : val;
        public static float EnforceMin(this ref float val, float min)
            => val = val < min ? min : val;
        public static double EnforceMin(this ref double val, double min)
            => val = val < min ? min : val;
        public static decimal EnforceMin(this ref decimal val, decimal min)
            => val = val < min ? min : val;
        public static Vector2 EnforceMinMagnitude(this ref Vector2 val, float min)
            => val = val.Length() < min ? val.WithMagnitude(min) : val;

        public static int EnforceMax(this ref int val, int max)
            => val = val > max ? max : val;
        public static long EnforceMax(this ref long val, long max)
            => val = val > max ? max : val;
        public static float EnforceMax(this ref float val, float max)
            => val = val > max ? max : val;
        public static double EnforceMax(this ref double val, double max)
            => val = val > max ? max : val;
        public static decimal EnforceMax(this ref decimal val, decimal max)
            => val = val > max ? max : val;
        public static Vector2 EnforceMaxMagnitude(this ref Vector2 val, float max)
            => val = val.Length() > max ? val.WithMagnitude(max) : val;

        public static void SetMap(this ref int val, int min, int max, int newMin, int newMax)
            => val = val.Map(min, max, newMin, newMax);
        public static void SetMap(this ref long val, long min, long max, long newMin, long newMax)
            => val = val.Map(min, max, newMin, newMax);
        public static void SetMap(this ref float val, float min, float max, float newMin, float newMax)
            => val = val.Map(min, max, newMin, newMax);
        public static void SetMap(this ref double val, double min, double max, double newMin, double newMax)
            => val = val.Map(min, max, newMin, newMax);
        public static void SetMap(this ref decimal val, decimal min, decimal max, decimal newMin, decimal newMax)
            => val = val.Map(min, max, newMin, newMax);

        public static int Map(this ref int val, int min, int max, int newMin, int newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static long Map(this ref long val, long min, long max, long newMin, long newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static float Map(this ref float val, float min, float max, float newMin, float newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static double Map(this ref double val, double min, double max, double newMin, double newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static decimal Map(this ref decimal val, decimal min, decimal max, decimal newMin, decimal newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;

        public static bool IsMultipleOf(this int val, int m) => val % m == 0;
        public static bool IsMultipleOf(this long val, long m) => val % m == 0;
        public static bool IsMultipleOf(this float val, float m) => val % m == 0;
        public static bool IsMultipleOf(this double val, double m) => val % m == 0;
        public static bool IsMultipleOf(this decimal val, decimal m) => val % m == 0;

        public static bool Invert(this ref bool val) => val = !val;

        public static T Min<T>(T a, T b, Evaluator<T> eval)
            => eval(a) < eval(b) ? a : b;
        public static T Max<T>(T a, T b, Evaluator<T> eval)
            => eval(a) < eval(b) ? b : a;
        public static T Min<T>(IEnumerable<T> ts, Evaluator<T> eval) where T : class {

            T min = null;
            T[] arr = ts.ToArray();

            if (arr.Length <= 0) return default;
            if (arr.Length == 1) return arr[0];

            min = arr[0];

            foreach(var t in arr) {
                if (t == null) continue;
                min = eval(min) < eval(t) ? min : t;
            }

            return min;

        }
        public static T Max<T>(IEnumerable<T> ts, Evaluator<T> eval) where T : class {

            T max = null;
            T[] arr = ts.ToArray();

            if (arr.Length <= 0) return default;
            if (arr.Length == 1) return arr[0];

            max = arr[0];

            foreach (var t in arr) {
                if (t == null) continue;
                max = eval(max) < eval(t) ? t : max;
            }

            return max;

        }

        public static float AvoidZero(this float val, float cuddle = 1)
            => val + Math.Sign(val) * cuddle * (float)Math.Exp(-Math.Abs(val) / cuddle);
        public static float AvoidZeroAndSet(this ref float val, float cuddle = 1)
            => val = val.AvoidZero(cuddle);

        public static int RoundToInt(this float val)
            => (int)(val > 0 ? val - 0.5f : val + 0.5f);

        //ANIMATION MATHS
        public static int PingPong(int val, int max)
            => val % (max * 2) >= max ? max - (val % max) : val % max;
        public static int FlatPong(int val, int max, int flatTime)
            => GetClamp(PingPong(val, max + flatTime*2) - flatTime, 0, max);
        public static int BlinkPong(int val, int frameCount, int restTime, int flatTime)
            => GetClamp(FlatPong(val, frameCount - 1 + restTime, flatTime) + frameCount - 1 - restTime, 0, frameCount - 1);

        //Vector math
        public static Vector2 ChargeTarget(this Vector2 velocity, Vector2 targetVelocity, float speed, float inertia)
            => (velocity * (inertia - 1) + targetVelocity.WithMagnitude(speed)) / inertia;
        public static Vector2 ChargeTarget(this Vector2 velocity, Vector2 currentPos, Vector2 targetPos, float speed, float inertia)
            => velocity.ChargeTarget(targetPos - currentPos, speed, inertia);
        public static Vector2 ChargeTargetAndSet(this ref Vector2 velocity, Vector2 targetVelocity, float speed, float inertia)
            => velocity = velocity.ChargeTarget(targetVelocity, speed, inertia);
        public static Vector2 ChargeTargetAndSet(this ref Vector2 velocity, Vector2 currentPos, Vector2 targetPos, float speed, float inertia)
            => velocity = velocity.ChargeTarget(currentPos, targetPos, speed, inertia);

        public static Vector2 WithMagnitude(this Vector2 vec, float magnitude)
            => Vector2.Normalize(vec) * magnitude;
        public static Vector2 SetMagnitude(this ref Vector2 vec, float magnitude)
            => vec = vec.WithMagnitude(magnitude);

        public static Vector2 AvoidZero(this Vector2 vec, float cuddle = 1)
            => vec.WithMagnitude(vec.Length().AvoidZero(cuddle));
        public static Vector2 AvoidZeroAndSet(this ref Vector2 vec, float cuddle = 1)
            => vec = vec.AvoidZero(cuddle);

        //RANDOM FUNCTIONS
        public static int RandomSign => Main.rand.Next(new int[] {-1, 1});

        //SINUSOID FUNCTIONS / CONSTANT FUNCTIONS
        public static double Sinusoid(double amplitude = 1f, double cycleTime = 1f, double phaseShift = 0f)
            => amplitude * Math.Sin(ChemistryClass.UnpausedUpdateCount * ONE_RPS / cycleTime - TWO_PI_FLOAT * phaseShift);

        //CONVERSION EXTENSIONS
        public static ChemistryClassPlayer Chemistry(this Player player)
            => player.GetModPlayer<ChemistryClassPlayer>();

        public static ChemistryClassItem Chemistry(this Item item)
            => item.modItem as ChemistryClassItem;

        public static bool IsChemistry(this Item item)
            => item.modItem is ChemistryClassItem;

        public static StyleDimension ToStyleDimension(this float i)
            => new StyleDimension(i, 0);

        public static Color ToColor(this Vector3 vector) =>
            new Color(vector.X * 255, vector.Y * 255, vector.Z * 255);

        //RECTANGLE / UI EXTENSIONS
        public static bool ContainsMouse<T>(this T uiEl) where T : UIElement
            => uiEl.ContainsPoint(Main.MouseScreen);

        //TILE SETUPS
        public static void CreateMerge(int typeA, int typeB) {
            Main.tileMerge[typeA][typeB] = true;
            Main.tileMerge[typeB][typeA] = true;
        }
        public static void CreateMerge(this ModTile t, int typeB) {
            Main.tileMerge[t.Type][typeB] = true;
            Main.tileMerge[typeB][t.Type] = true;
        }

        public static bool HasUndergroundWall(this Tile curTile)
            => curTile.wall == WallID.None || curTile.wall == WallID.Dirt
                    || curTile.wall == WallID.DirtUnsafe
                    || (curTile.wall >= WallID.DirtUnsafe1 && curTile.wall <= WallID.DirtUnsafe4)
                    || curTile.wall == WallID.Stone
                    || (curTile.wall >= WallID.RocksUnsafe1 && curTile.wall <= WallID.RocksUnsafe4)
                    || (curTile.wall >= WallID.LavaUnsafe1 && curTile.wall <= WallID.LavaUnsafe4)
                    || (curTile.wall >= WallID.CaveUnsafe && curTile.wall <= WallID.Cave6Unsafe)
                    || curTile.wall == WallID.Cave7Unsafe || curTile.wall == WallID.Cave8Unsafe
                    || curTile.wall == WallID.CaveWall || curTile.wall == WallID.CaveWall2;

        public static bool solid(this Tile tile)
            => Main.tileSolid[tile.type];

        public static bool solidTop(this Tile tile)
           => Main.tileSolidTop[tile.type];

        public static bool axe(this Tile tile)
           => Main.tileAxe[tile.type];

        public static bool hammer(this Tile tile)
           => Main.tileHammer[tile.type];

        //ROTATIONS
        public static float ShortestRotTo(float from, float to) {

            from %= TWO_PI_FLOAT;
            to %= TWO_PI_FLOAT;

            if (Math.Abs(from - to) > PI_FLOAT) {
                return from - to + Math.Sign(to - from) * TWO_PI_FLOAT;
            } else return from - to;

        }

        //SHIT STUFFS
        public static Rectangle ScreenRectangle
            => new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

        //CONSTS (BASICALLY)
        public const float PI_FLOAT = (float)Math.PI;

        public const float HALF_PI_FLOAT = PI_FLOAT / 2;
        public const float THIRD_PI_FLOAT = PI_FLOAT / 3;
        public const float QUARTER_PI_FLOAT = PI_FLOAT / 4;
        public const float TWO_PI_FLOAT = PI_FLOAT * 2;

        public const float ONE_RPS = PI_FLOAT / 60;

        public static float SQRT_2 { get; } = (float)Math.Sqrt(2);

    }

}
