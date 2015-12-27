using System;
using SFML.System;

namespace SFNetHex
{
    public static class HexUtils
    {
        public static int HexDistance(Hex a, Hex b)
        {
            return (a - b).Length();
        }

        public static Vector2f HexToPixel(Hex h, Layout l)
        {
            var o = l.Orientation;
            var x = (o.F0 * h.X + o.F1 * h.Y) * l.Size.X;
            var y = (o.F2 * h.X + o.F3 * h.Y) * l.Size.Y;

            return new Vector2f(x + l.Origin.X, y + l.Origin.Y);
        }

        public static Vector2f HexIndexToPixel(int ix, int iy, Layout l)
        {
            var o = l.Orientation;
            var x = (o.F0 * ix + o.F1 * iy) * l.Size.X;
            var y = (o.F2 * ix + o.F3 * iy) * l.Size.Y;

            return new Vector2f(x + l.Origin.X, y + l.Origin.Y);
        }

        public static Vector2f HexIndexToPixel(Vector2i i, Layout l)
        {
            return HexIndexToPixel(i.X, i.Y, l);
        }

        public static FractionalHex PixelToHex(Vector2f p, Layout l)
        {
            var o = l.Orientation;
            var pt = new Vector2f((p.X - l.Origin.X) / l.Size.X, 
                                    (p.Y - l.Origin.Y) / l.Size.Y);
            var x = o.B0*pt.X + o.B1*pt.Y;
            var y = o.B2*pt.X + o.B3*pt.Y;
            return new FractionalHex(x, y);
        }

        public static Hex PixelToWholeHex(Vector2f p, Layout l)
        {
            return PixelToHex(p, l).RoundHex();
        }

        public static Vector2i PixelToHexIndex(Vector2f p, Layout l)
        {
            var h = PixelToHex(p, l).RoundHex();
            return new Vector2i(h.X, h.Y);
        }

        public static Hex RoundHex(this FractionalHex h)
        {
            //We have to perform this round/dif check to maintain the 
            //cubic coordinate trait that x + y + z = 0
            var rx = (int)Math.Round(h.X);
            var ry = (int)Math.Round(h.Y);
            var rz = (int)Math.Round(h.Z);
            var xdif = Math.Abs(rx - h.X);
            var ydif = Math.Abs(ry - h.Y);
            var zdif = Math.Abs(rz - h.Z);

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

        public static Vector2f HexCornerOffset(int corner, Layout l)
        {
            var angle = (float) (2f*Math.PI*(corner + l.Orientation.StartAngle)/6);

            return new Vector2f((float) (l.Size.X * Math.Cos(angle)), (float) (l.Size.Y * Math.Sin(angle)));
        }
    }
}
