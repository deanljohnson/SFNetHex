using System;
using System.Collections.Generic;
using SFML.System;

namespace SFNetHex
{
    public struct Hex
    {
        //NOTE: If the values of this array are changed/reordered
        //check on the HexMap.GetHexesInRing method - it will probably need to be changed
        public static readonly Hex[] HexDirections = {
            new Hex(1, 0),
            new Hex(1, -1),
            new Hex(0, -1),
            new Hex(-1, 0),
            new Hex(-1, 1),
            new Hex(0, 1),
        };

        public static readonly Hex Zero = new Hex(0, 0, 0);

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

        /// <summary>
        /// Returns a List of all Hexes in the a line from start to end
        /// </summary>
        public static HashSet<Hex> GetHexesInLine(Hex start, Hex end)
        {
            if (start == end)
            {
                return new HashSet<Hex> { start };
            }

            var ret = new HashSet<Hex>();
            var n = HexUtils.HexDistance(start, end);

            for (var i = 0; i <= n; i++)
            {
                var hex = HexUtils.HexLerp(start, end, (1f / n) * i).Round();
                ret.Add(hex);
            }

            return ret;
        }

        /// <summary>
        /// Returns a List of all Hexes within range of center
        /// </summary>
        public static HashSet<Hex> GetHexesInRange(Hex center, int range)
        {
            var ret = new HashSet<Hex>();

            for (var x = -range; x <= range; x++)
            {
                for (var y = Math.Max(-range, -x - range); y <= Math.Min(range, -x + range); y++)
                {
                    ret.Add(center + new Hex(x, y));
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns a List of all Hexes that are at the given radius from the given center
        /// </summary>
        public static HashSet<Hex> GetHexesInRing(Hex center, int radius)
        {
            var ret = new HashSet<Hex>();
            //This is the first Hex on the ring
            //We start with direction 4 because it works that way
            //based on the ordering of the HexDirections array
            var onRing = center + (HexDirections[4] * radius);

            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < radius; j++)
                {
                    ret.Add(onRing);
                    onRing = onRing + HexDirections[i];
                }
            }

            return ret;
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
