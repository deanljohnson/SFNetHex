using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class HexMap : Transformable, Drawable
    {
        private struct HexColorPair
        {
            public readonly Hex Hex;
            public Color Color;

            public HexColorPair(Hex h, Color c)
            {
                Hex = h;
                Color = c;
            }

            public void SetColor(Color c)
            {
                Color = c;
            }
        }

        private ConvexShape m_HexShape { get; }
        private Layout m_Layout { get; }
        private Dictionary<Vector2i, HexColorPair> m_HexTable { get; set; }

        public HexMap(int rad, Orientation o, Vector2f cellSize)
        {
            m_Layout = new Layout(o, cellSize, new Vector2f(0, 0));

            m_HexShape = BuildShape();
            BuildHexMap(rad);
        }

        public void SetColorOfCell(Vector2i i, Color c)
        {
            if (!m_HexTable.ContainsKey(i))
                throw new ArgumentException($"{i} is not a valid index in this HexMap");

            m_HexTable[i].SetColor(c);
        }

        public void SetColorOfCell(int x, int y, Color c)
        {
            SetColorOfCell(new Vector2i(x, y), c);
        }

        public Color GetColorOfCell(Vector2i i)
        {
            if (!m_HexTable.ContainsKey(i))
                throw new ArgumentException($"{i} is not a valid index in this HexMap");

            return m_HexTable[i].Color;
        }

        public Color GetColorOfCell(int x, int y)
        {
            return GetColorOfCell(new Vector2i(x, y));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            foreach (var hcp in m_HexTable)
            {
                m_HexShape.Position = HexUtils.HexToPixel(hcp.Value.Hex, m_Layout);
                m_HexShape.FillColor = hcp.Value.Color;
                target.Draw(m_HexShape, states);
            }
        }

        private ConvexShape BuildShape()
        {
            var hex = HexUtils.PixelToHex(new Vector2f(0, 0), m_Layout);
            var shape = new ConvexShape(6)
            {
                OutlineThickness = 1,
                FillColor = Color.Black
            };

            var corners = hex.Corners(m_Layout);
            for (var i = 0; i < corners.Count; i++)
            {
                shape.SetPoint((uint)i, corners[i]);
            }

            return shape;
        }

        private void BuildHexMap(int rad)
        {
            m_HexTable = new Dictionary<Vector2i, HexColorPair>();

            for (var q = -rad; q <= rad; q++)
            {
                var r1 = Math.Max(-rad, -q - rad);
                var r2 = Math.Min(rad, -q + rad);
                for (var r = r1; r <= r2; r++)
                {
                    var hcp = new HexColorPair(new Hex(q, r, -q - r), Color.Black);
                    m_HexTable.Add(new Vector2i(q, r), hcp);
                }
            }
        }
    }
}
