using SFML.System;

namespace SFNetHex
{
    public struct Layout
    {
        public readonly Orientation Orientation;
        public readonly Vector2f Size;
        public readonly Vector2f Origin;

        public Layout(Orientation orientation, Vector2f size, Vector2f origin)
        {
            Orientation = orientation;
            Size = size;
            Origin = origin;
        }
    }
}
