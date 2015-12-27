using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class DrawableHexMap : HexMap, Drawable
    {
        protected ConvexShape HexShape { get; }
        protected Dictionary<Hex, Color> ColorTable { get; set; } = new Dictionary<Hex, Color>();

        public Color OutlineColor {
            get { return HexShape.OutlineColor; }
            set { HexShape.OutlineColor = value; }
        }

        public float OutlineThickness {
            get { return HexShape.OutlineThickness; }
            set { HexShape.OutlineThickness = value; }
        }

        /// <summary>
        /// Creates a hexagon shaped DrawableHexMap of the given radius
        /// </summary>
        public DrawableHexMap(int rad, Orientation o, Vector2f cellSize)
            : base(rad, o, cellSize)
        {
            HexShape = BuildShape();
        }

        /// <summary>
        /// Creates a DrawableHexMap of the given hexes
        /// </summary>
        public DrawableHexMap(HashSet<Hex> hexes, Orientation o, Vector2f cellSize)
            : base(hexes, o, cellSize)
        {
            HexShape = BuildShape();

            foreach (var hex in hexes)
            {
                ColorTable.Add(hex, Color.Black);
            }
        }

        public bool Contains(Hex h)
        {
            return HexSet.Contains(h);
        }

        public void SetColorOfCell(Hex h, Color c)
        {
            if (ColorTable.ContainsKey(h))
            {
                ColorTable[h] = c;
                return;
            }
            
            throw new KeyNotFoundException($"{h} is not a valid Hex in this HexMap");
        }

        public void SetColorOfCell(int x, int y, Color c)
        {
            SetColorOfCell(new Hex(x, y), c);
        }

        public Color GetColorOfCell(Hex h)
        {
            return ColorTable[h];
        }

        public Color GetColorOfCell(int x, int y)
        {
            return GetColorOfCell(new Hex(x, y));
        }

        public void ClearCellColors(Color c)
        {
            var keys = ColorTable.Keys.ToList();
            foreach (var key in keys)
            {
                ColorTable[key] = c;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            foreach (var ct in ColorTable)
            {
                HexShape.Position = HexUtils.HexToPixel(ct.Key, Layout);
                HexShape.FillColor = ct.Value;
                target.Draw(HexShape, states);
            }
        }

        protected override void OnAdd(Hex h)
        {
            ColorTable.Add(h, Color.Black);
        }

        private ConvexShape BuildShape()
        {
            var hex = HexUtils.PixelToHex(new Vector2f(0, 0), Layout).Round();
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
    }
}
