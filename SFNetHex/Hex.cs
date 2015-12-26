namespace SFNetHex
{
    public struct Hex
    {
        public readonly float X, Y, Z;

        public Hex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return new Hex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Hex operator -(Hex a, Hex b)
        {
            return new Hex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Hex operator *(Hex a, float b)
        {
            return new Hex(a.X * b, a.Y * b, a.Z * b);
        }

        public static Hex operator /(Hex a, float b)
        {
            return new Hex(a.X / b, a.Y / b, a.Z / b);
        }

        public bool Equals(Hex other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
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
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
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
    }
}
