using System;

namespace SFNetHex
{
    public struct Orientation
    {
        public static readonly Orientation LayoutPointy = 
            new Orientation((float) Math.Sqrt(3), (float) (Math.Sqrt(3) / 2f), 0f, 3f / 2f, 
                            (float) (Math.Sqrt(3) / 3f), -1f/3f, 0f, 2f/3f, .5f);
        public static readonly Orientation LayoutFlat =
            new Orientation(3f/2f, 0f, (float)(Math.Sqrt(3) / 2f), (float)Math.Sqrt(3),
                            2f/3f, 0f, -1f/3f, (float)(Math.Sqrt(3) / 3f), 0f);

        public readonly float F0, F1, F2, F3;
        public readonly float B0, B1, B2, B3; //This is the inverse conversion matrix
        public readonly float StartAngle;

        public Orientation(float f0, float f1, float f2, float f3,
                            float b0, float b1, float b2, float b3,
                            float startAngle)
        {
            F0 = f0;
            F1 = f1;
            F2 = f2;
            F3 = f3;
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            StartAngle = startAngle;
        }
    }
}
