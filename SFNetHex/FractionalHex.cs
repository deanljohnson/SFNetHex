using System;

namespace SFNetHex
{
    public struct FractionalHex
    {
        public readonly float X, Y, Z;

        public FractionalHex(float x, float y)
        {
            X = x;
            Y = y;
            Z = -x - y;
        }

        public FractionalHex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Length()
        {
            return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) / 2;
        }

        public Hex Round()
        {
            //We have to perform this round/dif check to maintain the 
            //cubic coordinate trait that x + y + z = 0
            var rx = (int)Math.Round(X);
            var ry = (int)Math.Round(Y);
            var rz = (int)Math.Round(Z);
            var xdif = Math.Abs(rx - X);
            var ydif = Math.Abs(ry - Y);
            var zdif = Math.Abs(rz - Z);

            if (xdif > ydif && xdif > zdif)
            {
                rx = -ry - rz;
            }
            else if (ydif > zdif)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Hex(rx, ry, rz);
        }

        #region Operators/Equality Members
        public override string ToString()
        {
            return $"[FractionalHex]: X({X}) Y({Y}) Z({Z})";
        }

        public static FractionalHex operator +(FractionalHex a, FractionalHex b)
        {
            return new FractionalHex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static FractionalHex operator -(FractionalHex a, FractionalHex b)
        {
            return new FractionalHex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static FractionalHex operator *(FractionalHex a, float b)
        {
            return new FractionalHex(a.X * b, a.Y * b, a.Z * b);
        }

        public static FractionalHex operator /(FractionalHex a, float b)
        {
            return new FractionalHex(a.X / b, a.Y / b, a.Z / b);
        }

        public bool Equals(FractionalHex other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FractionalHex && Equals((FractionalHex)obj);
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

        public static bool operator ==(FractionalHex left, FractionalHex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FractionalHex left, FractionalHex right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
