using System;
using System.Collections.Generic;
using SFML.System;

namespace SFNetHex
{
    public struct Hex
    {
        private static readonly Hex[] HexDirections = {
            new Hex(1, 0),
            new Hex(1, -1),
            new Hex(0, -1),
            new Hex(-1, 0),
            new Hex(-1, 1),
            new Hex(0, 1),
        };

        public readonly int X, Y, Z;

        public Hex(int x, int y)
        {
            X = x;
            Y = y;
            Z = -x - y;
        }

        public Hex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public List<Vector2f> Corners(Layout l)
        {
            var ret = new List<Vector2f>();
            var center = HexUtils.HexToPixel(this, l);

            for (var i = 0; i < 6; i++)
            {
                ret.Add(center + HexUtils.HexCornerOffset(i, l));
            }

            return ret;
        }

        public Hex GetNeighbor(int dir)
        {
            return this + HexDirections[dir % 6];
        }

        public int Length()
        {
            return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) / 2;
        }

        #region Operators/Equality Members
        public override string ToString()
        {
            return $"[Hex]: X({X}) Y({Y}) Z({Z})";
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return new Hex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Hex operator -(Hex a, Hex b)
        {
            return new Hex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Hex operator *(Hex a, int b)
        {
            return new Hex(a.X * b, a.Y * b, a.Z * b);
        }

        public static Hex operator /(Hex a, int b)
        {
            return new Hex(a.X / b, a.Y / b, a.Z / b);
        }

        public bool Equals(Hex other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Hex && Equals((Hex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Hex left, Hex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Hex left, Hex right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
