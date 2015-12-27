using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class HexMap : Transformable
    {
        protected Layout Layout { get; }
        protected Dictionary<Vector2i, Hex> HexTable { get; set; }

        public Hex this[Vector2i i] => HexTable[i];
        public Hex this[int x, int y] => HexTable[new Vector2i(x, y)];

        private HexMap(Orientation o, Vector2f cellSize)
        {
            HexTable = new Dictionary<Vector2i, Hex>();
            Layout = new Layout(o, cellSize, new Vector2f(0, 0));
        }

        public HexMap(int rad, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            BuildHexMap(rad);
        }

        public HexMap(int x1, int x2, int y1, int y2, Orientation o, Vector2f cellSize) 
            : this(o, cellSize)
        {
            BuildParallelogramMap(x1, x2, y1, y2);
        }

        /// <summary>
        /// Returns the Position of the Hex at the given index in this HexMap
        /// </summary>
        public Vector2f GetHexPosition(Vector2i i)
        {
            var pos = HexUtils.HexIndexToPixel(i, Layout);
            return Transform.TransformPoint(pos);
        }

        /// <summary>
        /// Returns the Hex index that corresponds to the given Position.
        /// Will not necessarily be on the HexMap
        /// </summary>
        public Vector2i GetNearestHexIndex(Vector2f p)
        {
            p = InverseTransform.TransformPoint(p);
            return HexUtils.PixelToHexIndex(p, Layout);
        }

        /// <summary>
        /// Returns the nearest whole Hex coordinate that corresponds to the given Position.
        /// Will not necessarily be on the HexMap
        /// </summary>
        public Hex GetNearestWholeHex(Vector2f p)
        {
            p = InverseTransform.TransformPoint(p);
            return HexUtils.PixelToWholeHex(p, Layout);
        }

        // Exposed as a virtual method so that inheritors can know when a new Hex is added
        protected virtual void Add(Vector2i i, Hex h)
        {
            HexTable.Add(i, h);
        }

        private void BuildHexMap(int rad)
        {
            for (var q = -rad; q <= rad; q++)
            {
                var r1 = Math.Max(-rad, -q - rad);
                var r2 = Math.Min(rad, -q + rad);
                for (var r = r1; r <= r2; r++)
                {
                    Add(new Vector2i(q, r), new Hex(q, r, -q - r));
                }
            }
        }

        private void BuildParallelogramMap(int q1, int q2, int r1, int r2)
        {
            for (var q = q1; q <= q2; q++)
            {
                for (var r = r1; r <= r2; r++)
                {
                    Add(new Vector2i(q, r), new Hex(q, r, -q - r));
                }
            }
        }
    }
}
