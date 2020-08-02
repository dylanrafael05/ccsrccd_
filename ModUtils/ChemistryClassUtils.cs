using System;
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
        public static long Clamp(this ref long val, long min, long max)
            => val = val < min ? min : (val > max ? max : val);
        public static float Clamp(this ref float val, float min, float max)
            => val = val < min ? min : (val > max ? max : val);
        public static double Clamp(this ref double val, double min, double max)
            => val = val < min ? min : (val > max ? max : val);
        public static decimal Clamp(this ref decimal val, decimal min, decimal max)
            => val = val < min ? min : (val > max ? max : val);

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

        //RECIPE HELPER
        public static void SetRecipe( this ModItem item, int requireTile = TileID.WorkBenches, int amount = 1, params (int, int)[] items ) {

            ModRecipe recipe = new ModRecipe(item.mod);

            recipe.AddTile(requireTile);
            foreach( var ing in items ) {

                recipe.AddIngredient(ing.Item1, ing.Item2);

            }

            recipe.SetResult(item.item.type, amount);

            recipe.AddRecipe();

        }

        //CONSTS (BASICALLY)
        public const float PI_FLOAT = (float)Math.PI;

        public const float HALF_PI_FLOAT = PI_FLOAT / 2;
        public const float THIRD_PI_FLOAT = PI_FLOAT / 3;
        public const float QUARTER_PI_FLOAT = PI_FLOAT / 4;
        public const float TWO_PI_FLOAT = PI_FLOAT * 2;

        public const float ONE_RPS = PI_FLOAT / 60;

    }

}
