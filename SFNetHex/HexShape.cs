using SFML.Graphics;

namespace SFNetHex
{
    public class HexShape : ConvexShape
    {
        public HexShape(Layout layout)
            : base(6)
        {
            var corners = new Hex(0, 0).Corners(layout);

            for (var i = 0; i < corners.Count; i++)
            {
                var corner = corners[i];
                SetPoint((uint) i, corner);
            }
        }
    }
}
