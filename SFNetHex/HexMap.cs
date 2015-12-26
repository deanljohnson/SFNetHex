using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class HexMap : Transformable, Drawable
    {
        protected struct HexColorPair
        {
            public readonly Hex Hex;
            public readonly Color Color;

            public HexColorPair(Hex h, Color c)
            {
                Hex = h;
                Color = c;
            }
        }

        protected ConvexShape HexShape { get; }
        protected Layout Layout { get; }
        protected Dictionary<Vector2i, HexColorPair> HexTable { get; set; }

        private HexMap(Orientation o, Vector2f cellSize)
        {
            Layout = new Layout(o, cellSize, new Vector2f(0, 0));
            HexShape = BuildShape();
        }

        /// <summary>
        /// Creates a hexagon shaped HexMap of the given radius
        /// </summary>
        public HexMap(int rad, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            BuildHexMap(rad);
        }

        /// <summary>
        /// Creates a parallelogram shaped HexMap with the given ranges of indices
        /// </summary>
        public HexMap(int x1, int x2, int y1, int y2, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            BuildParallelogramMap(x1, x2, y1, y2);
        }

        public void SetColorOfCell(Vector2i i, Color c)
        {
            if (!HexTable.ContainsKey(i))
                throw new ArgumentException($"{i} is not a valid index in this HexMap");

            HexTable[i] = new HexColorPair(HexTable[i].Hex, c);
        }

        public void SetColorOfCell(int x, int y, Color c)
        {
            SetColorOfCell(new Vector2i(x, y), c);
        }

        public Color GetColorOfCell(Vector2i i)
        {
            if (!HexTable.ContainsKey(i))
                throw new ArgumentException($"{i} is not a valid index in this HexMap");

            return HexTable[i].Color;
        }

        public Color GetColorOfCell(int x, int y)
        {
            return GetColorOfCell(new Vector2i(x, y));
        }

        public void ClearCellColors(Color c)
        {
            var keys = HexTable.Keys.ToList();
            foreach (var key in keys)
            {
                HexTable[key] = new HexColorPair(HexTable[key].Hex, c);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            foreach (var hcp in HexTable)
            {
                HexShape.Position = HexUtils.HexToPixel(hcp.Value.Hex, Layout);
                HexShape.FillColor = hcp.Value.Color;
                target.Draw(HexShape, states);
            }
        }

        private ConvexShape BuildShape()
        {
            var hex = HexUtils.PixelToHex(new Vector2f(0, 0), Layout);
            var shape = new ConvexShape(6)
            {
                OutlineThickness = 1,
                FillColor = Color.Black
            };

            var corners = hex.Corners(Layout);
            for (var i = 0; i < corners.Count; i++)
            {
                shape.SetPoint((uint)i, corners[i]);
            }

            return shape;
        }

        private void BuildHexMap(int rad)
        {
            HexTable = new Dictionary<Vector2i, HexColorPair>();

            for (var q = -rad; q <= rad; q++)
            {
                var r1 = Math.Max(-rad, -q - rad);
                var r2 = Math.Min(rad, -q + rad);
                for (var r = r1; r <= r2; r++)
                {
                    var hcp = new HexColorPair(new Hex(q, r, -q - r), Color.Black);
                    HexTable.Add(new Vector2i(q, r), hcp);
                }
            }
        }

        private void BuildParallelogramMap(int q1, int q2, int r1, int r2)
        {
            HexTable = new Dictionary<Vector2i, HexColorPair>();

            for (var q = q1; q <= q2; q++)
            {
                for (var r = r1; r <= r2; r++)
                {
                    var hcp = new HexColorPair(new Hex(q, r, -q - r), Color.Black);
                    HexTable.Add(new Vector2i(q, r), hcp);
                }
            }
        }
    }
}
