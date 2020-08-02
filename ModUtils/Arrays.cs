using System;
using System.Collections;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;

namespace ChemistryClass.ModUtils {
    public static class Arrays {

        /*BOUNDED INDEXING
        public static char GetBounded(this string str, int i)
            => str[Mathematics.Clamp(ref i, 0, str.Length - 1)];
        public static T GetBounded<T>(this T[] ts, int i)
            => ts[Mathematics.Clamp(ref i, 0, ts.Length - 1)];
        public static T GetBounded<T>(this T[,] ts, int i, int j)
            => ts[Mathematics.Clamp(ref i, 0, ts.GetLength(0) - 1),
                Mathematics.Clamp(ref j, 0, ts.GetLength(1) - 1)];
        public static T GetBounded<T>(this T[,,] ts, int i, int j, int k)
            => ts[Mathematics.Clamp(ref i, 0, ts.GetLength(0) - 1),
                Mathematics.Clamp(ref j, 0, ts.GetLength(1) - 1),
                Mathematics.Clamp(ref k, 0, ts.GetLength(2) - 1)];
        public static T GetBounded<T>(this T[,,,] ts, int i, int j, int k, int l)
            => ts[Mathematics.Clamp(ref i, 0, ts.GetLength(0) - 1),
                Mathematics.Clamp(ref j, 0, ts.GetLength(1) - 1),
                Mathematics.Clamp(ref k, 0, ts.GetLength(2) - 1),
                Mathematics.Clamp(ref l, 0, ts.GetLength(3) - 1)];

        //VOIDED INDEXING
        public static char GetVoided(this string str, int i)
            => i < 0 || i >= str.Length ? '_' : str[i];
        public static T GetVoided<T>(this T[] ts, int i)
            => i < 0 || i >= ts.Length ? default : ts[i];
        public static T GetVoided<T>(this T[,] ts, int i, int j)
            => i < 0 || i >= ts.GetLength(0) ||
               j < 0 || j >= ts.GetLength(1) ? default : ts[i, j];
        public static T GetVoided<T>(this T[,,] ts, int i, int j, int k)
            => i < 0 || i >= ts.GetLength(0) ||
               j < 0 || j >= ts.GetLength(1) ||
               k < 0 || k >= ts.GetLength(2) ? default : ts[i, j, k];
        public static T GetVoided<T>(this T[,,,] ts, int i, int j, int k, int l)
            => i < 0 || i >= ts.GetLength(0) ||
               j < 0 || j >= ts.GetLength(1) ||
               k < 0 || k >= ts.GetLength(2) ||
               l < 0 || l >= ts.GetLength(3) ? default : ts[i, j, k, l];

        //ROUNDED INDEXING
        public static char GetRounded(this string str, double i)
            => str[Mathematics.RoundToInt(i)];
        public static T GetRounded<T>(this T[] ts, double i)
            => ts[Mathematics.RoundToInt(i)];
        public static T GetRounded<T>(this T[,] ts, double i, double j)
            => ts[Mathematics.RoundToInt(i), Mathematics.RoundToInt(j)];
        public static T GetRounded<T>(this T[,,] ts, double i, double j, double k)
            => ts[Mathematics.RoundToInt(i), Mathematics.RoundToInt(j),
                  Mathematics.RoundToInt(k)];
        public static T GetRounded<T>(this T[,,,] ts, double i, double j, double k, double l)
            => ts[Mathematics.RoundToInt(i), Mathematics.RoundToInt(j),
                  Mathematics.RoundToInt(k), Mathematics.RoundToInt(l)];*/

    }
}
