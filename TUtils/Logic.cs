using System;
using Terraria;
using Terraria.Utilities;

namespace TUtils {
    public static class Logic {

        public static bool Invert(bool val) => !val;
        public static bool Invert(ref bool val) => val = !val;

        public static bool EvaluateChance(float chance) => chance > Main.rand.NextFloat(0, 1);
        public static bool EvaluateChanceWGen(float chance) => chance > WorldGen.genRand.NextFloat(0, 1);

        public static int RandomSign() => Main.rand.Next(new int[] { -1, 1 });

        public static bool IsNull(object o) => o == null;
        public static bool IsNotNull(object o) => o != null;

    }
}
