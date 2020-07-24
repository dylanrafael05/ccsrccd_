using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace ChemistryClass {

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

        //SINUSOID FUNCTIONS / CONSTANT FUNCTIONS
        public static double Sinusoid(double amplitude = 1f, double cycleTime = 1f, double phaseModulation = 0f)
            => amplitude * Math.Sin(ChemistryClass.UnpausedUpdateCount * PI_OVER_THIRTY / cycleTime + phaseModulation);
        public static double SinusoidFrame(double amplitude = 1f, double cycleTime = 1f, double phaseModulation = 0f)
            => Sinusoid(amplitude, cycleTime / 60, phaseModulation);
        public static double SinusoidMinute(double amplitude = 1f, double cycleTime = 1f, double phaseModulation = 0f)
            => Sinusoid(amplitude, cycleTime * 60, phaseModulation);

        public static double OneFrameSinusoid
            => SinusoidFrame();
        public static double TwoFrameSinusoid
            => SinusoidFrame(cycleTime: 2f);
        public static double FourFrameSinusoid
            => SinusoidFrame(cycleTime: 4f);
        public static double TenFrameSinusoid
            => SinusoidFrame(cycleTime: 10f);

        public static double OneSecondSinusoid
            => Sinusoid();
        public static double TwoSecondSinusoid
            => Sinusoid(cycleTime: 2f);
        public static double FourSecondSinusoid
            => Sinusoid(cycleTime: 4f);
        public static double TenSecondSinusoid
            => Sinusoid(cycleTime: 10f);

        public static double OneMinuteSinusoid
            => SinusoidMinute();
        public static double TwoMinuteSinusoid
            => SinusoidMinute(cycleTime: 2f);
        public static double FourMinuteSinusoid
            => SinusoidMinute(cycleTime: 4f);
        public static double TenMinuteSinusoid
            => SinusoidMinute(cycleTime: 10f);
        public static double OneHourSinusoid
            => SinusoidMinute(cycleTime: 60f);

        //CONVERSION EXTENSIONS
        public static ChemistryClassPlayer Chemistry(this Player player)
            => player.GetModPlayer<ChemistryClassPlayer>();

        public static ChemistryClassItem Chemistry(this Item item)
            => item.modItem as ChemistryClassItem;

        public static bool IsChemistry(this Item item)
            => item.modItem is ChemistryClassItem;

        public static StyleDimension ToStyleDimension(this float i)
            => new StyleDimension(i, 0);

        //RECTANGLE / UI EXTENSIONS
        public static float MinDimension(this Rectangle i)
            => Math.Min(i.Width, i.Height);
        public static float MinDimension(this Texture2D i)
            => Math.Min(i.Width, i.Height);

        public static float MaxDimension(this Rectangle i)
            => Math.Max(i.Width, i.Height);
        public static float MaxDimension(this Texture2D i)
            => Math.Max(i.Width, i.Height);

        public static void AddMargin(this Rectangle i, int margin) {

            i.X += margin;
            i.Y += margin;
            i.Width  -= margin * 2;
            i.Height -= margin * 2;

        }

        public static void AddMargin(this Rectangle i, float margin)
            => i.AddMargin((int)margin);

        public static void ShrinkBy(this Rectangle i, float margin)
            => new Rectangle(

                (int)(i.Center.X - (i.Width / 2)*(1 - margin)),
                (int)(i.Center.Y - (i.Height / 2)*(1 - margin)),
                (int)(i.Width * (1 - margin)),
                (int)(i.Height * (1 - margin))

                );

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

        //COLOR MIXING
        public static Color MixRGB(this Color one, Color two, float f1 = 1f, float f2 = 1f) {

            double r = Math.Sqrt((one.R * one.R * f1 + two.R * two.R * f2) / (f1 + f2));
            double g = Math.Sqrt((one.G * one.G * f1 + two.G * two.G * f2) / (f1 + f2));
            double b = Math.Sqrt((one.B * one.B * f1 + two.B * two.B * f2) / (f1 + f2));

            return new Color((int)r, (int)g, (int)b, 0);

        }

        public static Color MatchAlpha(this Color one, Color two)
            => new Color(one.R, one.G, one.B, two.A);

        public static Color WithAlpha(this Color one, float a)
            => new Color(one.R, one.G, one.B, a);

        //CONSTS (BASICALLY)
        public const float PI = (float)Math.PI;

        public const float HALF_PI = PI / 2;
        public const float THIRD_PI = PI / 3;
        public const float QUARTER_PI = PI / 4;
        public const float SIXTH_PI = PI / 6;
        public const float EIGHTH_PI = PI / 8;
        public const float TENTH_PI = PI / 10;
        public const float PI_OVER_THIRTY = PI / 30;

        public const float THREE_HALVES_PI = HALF_PI * 3;

        public const float TWO_THIRDS_PI = THIRD_PI * 2;
        public const float FOUR_THIRDS_PI = THIRD_PI * 4;

        public const float THREE_QUARTER_PI = QUARTER_PI * 3;
        public const float FIVE_QUARTER_PI = QUARTER_PI * 5;
        public const float SEVEN_QUARTER_PI = QUARTER_PI * 7;

        public const float FIVE_SIXTHS_PI = SIXTH_PI * 5;
        public const float SEVEN_SIXTHS_PI = SIXTH_PI * 7;
        public const float ELEVEN_SIXTHS_PI = SIXTH_PI * 11;

        public const float THREE_EIGHTHS_PI = EIGHTH_PI * 3;
        public const float FIVE_EIGHTHS_PI = EIGHTH_PI * 5;
        public const float SEVEN_EIGHTHS_PI = EIGHTH_PI * 7;
        public const float NINE_EIGHTHS_PI = EIGHTH_PI * 9;
        public const float ELEVEN_EIGHTHS_PI = EIGHTH_PI * 11;
        public const float THIRTEEN_EIGHTHS_PI = EIGHTH_PI * 13;
        public const float FIFTEEN_EIGHTHS_PI = EIGHTH_PI * 15;

        public const float TWO_PI = PI * 2;

        public static float PiOver(int denominator) => PI / denominator;

    }

}
