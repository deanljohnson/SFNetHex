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

        public static Vector2f HexToPixel(int ix, int iy, Layout l)
        {
            var o = l.Orientation;
            var x = (o.F0 * ix + o.F1 * iy) * l.Size.X;
            var y = (o.F2 * ix + o.F3 * iy) * l.Size.Y;

            return new Vector2f(x + l.Origin.X, y + l.Origin.Y);
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
            return PixelToHex(p, l).Round();
        }

        public static Vector2i PixelToHexIndex(Vector2f p, Layout l)
        {
            var h = PixelToHex(p, l).Round();
            return new Vector2i(h.X, h.Y);
        }

        public static FractionalHex HexLerp(Hex a, Hex b, float t)
        {
            return new FractionalHex(a.X + ((b.X - a.X) * t),
                                    a.Y + ((b.Y - a.Y) * t),
                                    a.Z + ((b.Z - a.Z) * t));
        }

        public static Vector2f HexCornerOffset(int corner, Layout l)
        {
            var angle = (float) (2f*Math.PI*(corner + l.Orientation.StartAngle)/6);
            
            return new Vector2f((float) (l.Size.X * Math.Cos(angle)), (float) (l.Size.Y * Math.Sin(angle)));
        }
    }
}
