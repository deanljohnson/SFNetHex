using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFNetHex;

namespace SFNetHexDemo
{
    public static class Game
    {
        private static readonly Vector2u DefaultWindowSize = new Vector2u(1920, 1080);
        private const uint DISPLAY_RATE = 60;
        private const String GAME_NAME = "My Game";
        private static Stopwatch m_Timer { get; } = new Stopwatch();
        private static long m_LastTime { get; set; }

        public static RenderWindow Window { get; private set; }
        public static List<DrawableHex> Hexes { get; set; } = new List<DrawableHex>();

        public static void Main()
        {
            InitializeWindow();
            Start();
        }

        private static void Start()
        {
            m_Timer.Start();

            var l = new Layout(Orientation.LayoutFlat, new Vector2f(20, 20), new Vector2f(200, 200));
            BuildHexMap(5, l);

            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                Update(GetDeltaTime());

                Window.Clear(Color.Black);

                Render();

                Window.Display();
            }
        }

        private static void Update(float dt)
        {

        }

        private static void Render()
        {
            foreach (var hex in Hexes)
            {
                Window.Draw(hex);
            }
        }

        private static void BuildHexMap(int rad, Layout l)
        {
            for (var q = -rad; q <= rad; q++)
            {
                var r1 = Math.Max(-rad, -q - rad);
                var r2 = Math.Min(rad, -q + rad);
                for (var r = r1; r <= r2; r++)
                {
                    Hexes.Add(new DrawableHex(HexUtils.HexIndexToPixel(q, r, l), l));
                    if (q == 0 && r == 0)
                    {
                        Hexes.Last().FillColor = Color.Blue;
                    }
                }
            }
        }

        private static float GetDeltaTime()
        {
            var elapsedMs = m_Timer.ElapsedMilliseconds - m_LastTime;
            m_LastTime = m_Timer.ElapsedMilliseconds;
            return (elapsedMs / 1000f);
        }

        private static void InitializeWindow()
        {
            Window = new RenderWindow(new VideoMode(DefaultWindowSize.X, DefaultWindowSize.Y, 32), GAME_NAME, Styles.Default);
            Window.SetFramerateLimit(DISPLAY_RATE);

            Window.Closed += OnWindowClose;
        }

        private static void OnWindowClose(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
