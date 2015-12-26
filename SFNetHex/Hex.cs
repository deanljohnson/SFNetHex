namespace SFNetHex
{
    public struct Hex
    {
        public float X, Y, Z;

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
    }
}
