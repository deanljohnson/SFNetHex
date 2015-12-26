using SFML.Graphics;
using SFML.System;
using SFNetHex;

namespace SFNetHexDemo
{
    public class DrawableHex : ConvexShape
    {
        private Hex m_Hex;

        public DrawableHex(Vector2f pos, Layout l)
        {
            Position = pos;
            m_Hex = HexUtils.PixelToHex(new Vector2f(0, 0), l);

            SetPointCount(6);
            OutlineThickness = 1;
            FillColor = Color.Black;

            var corners = m_Hex.Corners(l);
            for (var i = 0; i < corners.Count; i++)
            {
                SetPoint((uint) i, corners[i]);
            }
        }
    }
}
