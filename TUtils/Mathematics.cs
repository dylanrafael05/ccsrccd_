using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TUtils {

    public static class Mathematics {

        //CLAMPING AND OTHER SHENANIGANS
        public static int Clamp(ref int value, int min, int max)
            => value = value < min ? min : (value > max? max : value);
        public static uint Clamp(ref uint value, uint min, uint max)
            => value = value < min ? min : (value > max ? max : value);
        public static long Clamp(ref long value, long min, long max)
            => value = value < min ? min : (value > max ? max : value);
        public static float Clamp(ref float value, float min, float max)
            => value = value < min ? min : (value > max ? max : value);
        public static double Clamp(ref double value, double min, double max)
            => value = value < min ? min : (value > max ? max : value);

        public static int LimitMin(ref int value, int min)
            => value = value < min ? min : value;
        public static uint LimitMin(ref uint value, uint min)
            => value = value < min ? min : value;
        public static long LimitMin(ref long value, long min)
            => value = value < min ? min : value;
        public static float LimitMin(ref float value, float min)
            => value = value < min ? min : value;
        public static double LimitMin(ref double value, double min)
            => value = value < min ? min : value;

        public static int LimitMax(ref int value, int max)
            => value = value > max ? max : value;
        public static uint LimitMax(ref uint value, uint max)
            => value = value > max ? max : value;
        public static long LimitMax(ref long value, long max)
            => value = value > max ? max : value;
        public static float LimitMax(ref float value, float max)
            => value = value > max ? max : value;
        public static double LimitMax(ref double value, double max)
            => value = value > max ? max : value;

        //ROUNDING TO INTEGRAL
        public static int RoundToInt(decimal d) {
            if (d < 0) {
                return (int)(d - 0.5m);
            }
            return (int)(d + 0.5m);
        }
        public static int RoundToInt(double d) {
            if (d < 0) {
                return (int)(d - 0.5);
            }
            return (int)(d + 0.5);
        }
        public static int RoundToInt(float d) {
            if (d < 0) {
                return (int)(d - 0.5f);
            }
            return (int)(d + 0.5f);
        }

        //MAPPING
        public static float Map(float val, float min, float max, float newMin, float newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static double Map(double val, double min, double max, double newMin, double newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static decimal Map(decimal val, decimal min, decimal max, decimal newMin, decimal newMax)
            => (val - min) * (newMax - newMin) / (max - min) + newMin;
        public static float Map(ref float val, float min, float max, float newMin, float newMax)
            => val = Map(val, min, max, newMin, newMax);
        public static double Map(ref double val, double min, double max, double newMin, double newMax)
            => val = Map(val, min, max, newMin, newMax);
        public static decimal Map(ref decimal val, decimal min, decimal max, decimal newMin, decimal newMax)
            => val = Map(val, min, max, newMin, newMax);

        //BOOLEAN CHECKING
        public static bool IsMultipleOf(double val, double n) => val % n == 0;
        public static bool IsMultipleOf(decimal val, decimal n) => val % n == 0;

        //AVERAGING
        public static double Average(params double[] nums) {

            double ret = 0;

            foreach (double num in nums) { ret += num; }

            return ret / nums.Length;

        }

        public static decimal Average(params decimal[] nums) {

            decimal ret = 0;

            foreach (decimal num in nums) { ret += num; }

            return ret / nums.Length;

        }

        public static double HarmonicMean(params double[] nums) {

            double ret = 0;

            foreach (double num in nums) { ret += 1 / num; }

            return nums.Length / ret;

        }

        public static decimal HarmonicMean(params decimal[] nums) {

            decimal ret = 0;

            foreach (decimal num in nums) { ret += 1 / num; }

            return nums.Length / ret;

        }

        public static double GeometricMean(params double[] nums) {

            double ret = 0;

            foreach (double num in nums) { ret *= num; }

            return Math.Pow(ret, 1d / nums.Length);

        }

        //RECTANGLE TOOLS
        public static Rectangle AddMargin(ref Rectangle rec, int margin)
            => new Rectangle(rec.X + margin / 2, rec.Y + margin / 2, rec.Width - margin, rec.Height - margin);

        public static Rectangle CenteredRescale(ref Rectangle rec, int w, int h)
            => new Rectangle(rec.X + (w - rec.Width) / 2, rec.Y + (h - rec.Height) / 2, w, h);

        //SINUSOID
        public static double Sinusoid(double amplitude = 1f, double cycleTime = 1f, double phaseShift = 0f)
            => amplitude * Math.Sin(Timers.TimerUtils.activeUpdateTimer / cycleTime * ONE_RPS - phaseShift * TWO_PI_FLOAT);

        //CONSTANTS
        public const float PI_FLOAT = (float)Math.PI;
        public const float TWO_PI_FLOAT = PI_FLOAT * 2;
        public const float HALF_PI_FLOAT = PI_FLOAT / 2;
        public const float QUARTER_PI_FLOAT = PI_FLOAT / 4;

        public const float ONE_RPS = PI_FLOAT / 30;

        public const float SQRT_2 = 1.41421353816986083984375f;

    }

}
