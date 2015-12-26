using System;
using System.Collections.Generic;
using SFML.System;

namespace SFNetHex
{
    public static class HexUtils
    {
        private static readonly Hex[] HexDirections = {
            new Hex(1, 0, -1),
            new Hex(1, -1, 0),
            new Hex(0, -1, 1),
            new Hex(-1, 0, 1),
            new Hex(-1, 1, 0),
            new Hex(0, 1, -1),
        };

        public static Hex HexDirection(int i)
        {
            if (i < 0 || i > 5)
            {
                throw new ArgumentException("HexDirection can only accept a number from 0-5", nameof(i));
            }

            return HexDirections[i];
        }

        public static Hex HexNeighbor(this Hex h, int dir)
        {
            return h + HexDirection(dir);
        }

        public static float HexLength(this Hex i)
        {
            return (Math.Abs(i.X) + Math.Abs(i.Y) + Math.Abs(i.Z))/2;
        }

        public static float HexDistance(Hex a, Hex b)
        {
            return (a - b).HexLength();
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

        public static Hex PixelToHex(Vector2f p, Layout l)
        {
            var o = l.Orientation;
            var pt = new Vector2f((p.X - l.Origin.X) / l.Size.X, 
                                    (p.Y - l.Origin.Y) / l.Size.Y);
            var x = o.B0*pt.X + o.B1*pt.Y;
            var y = o.B2*pt.X + o.B3*pt.Y;
            return new Hex(x, y, -x - y);
        }

        public static Hex PixelToHexIndex(Vector2f p, Layout l)
        {
            return PixelToHex(p, l).RoundHex();
        }

        public static Hex RoundHex(this Hex h)
        {
            //We have to perform this round/dif check to maintain the 
            //cubic coordinate trait that x + y + z = 0
            var rx = (float)Math.Round(h.X);
            var ry = (float)Math.Round(h.Y);
            var rz = (float)Math.Round(h.Z);
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

        public static List<Vector2f> Corners(this Hex h, Layout l)
        {
            var ret = new List<Vector2f>();
            var center = HexToPixel(h, l);

            for (var i = 0; i < 6; i++)
            {
                ret.Add(center + HexCornerOffset(i, l));
            }

            return ret;
        }
    }
}
