using System;
using Microsoft.Xna.Framework;

namespace ChemistryClass.ModUtils {
    public interface IMathematicalFunction {

        float Evaluate(float x);

    }

    public struct LinearFunction : IMathematicalFunction {

        float M, B;

        public LinearFunction(float m, float b) {
            M = m;
            B = b;
        }

        public float Evaluate(float x)
            => M * x + B;

        public static LinearFunction FromPoints(Vector2 a, Vector2 b)
            => new LinearFunction(

                (b.Y - a.Y) / (b.X - a.X),
                a.Y - (b.Y - a.Y) / (b.X - a.X) * a.X

                );

        public LinearFunction DropPerpendicular(Vector2 a) {
            LinearFunction function = new LinearFunction(-1f / M, 0);
            function.B = a.Y - a.X * function.M;
            return function;
        }

        public LinearFunction Inverse
            => new LinearFunction(1 / M, -B);

        public Vector2 PointOnLine(float x)
            => new Vector2(x, Evaluate(x));
        public Vector2 PointOnLineY(float y)
           => new Vector2(Inverse.Evaluate(y), y);

        public Vector2 UnitVector(float dx)
            => Vector2.Normalize(PointOnLine(Math.Sign(dx)));

        public Vector2? Intersection(LinearFunction b) {

            if ( M == b.M ) return null;

            return PointOnLine((b.B - B) / (M + b.M));

        }

        public bool IsAbove(Vector2 a)
            => Evaluate(a.X) > a.Y;
        public bool IsBelow(Vector2 a)
            => Evaluate(a.X) < a.Y;
        public bool IsOn(Vector2 a)
            => Evaluate(a.X) == a.Y;
        public bool IsOnFloor(Vector2 a)
            => Math.Floor(Evaluate(a.X)) == Math.Floor(a.Y);

        public static bool operator ==(LinearFunction a, LinearFunction b) => a.M == b.M && a.B == b.B;
        public static bool operator !=(LinearFunction a, LinearFunction b) => a.M != b.M || a.B != b.B;

    }
}


