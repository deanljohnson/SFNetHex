using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFNetHex;

namespace SFNetHexDemo
{
    public static class Game
    {
        private static readonly Vector2u DefaultWindowSize = new Vector2u(1600, 900);
        private const uint DISPLAY_RATE = 60;
        private const String GAME_NAME = "My Game";
        private static Stopwatch m_Timer { get; } = new Stopwatch();
        private static long m_LastTime { get; set; }

        public static RenderWindow Window { get; private set; }
        public static HexMap HexMap { get; set; }

        public static void Main()
        {
            InitializeWindow();
            Start();
        }

        private static void Start()
        {
            m_Timer.Start();

            HexMap = new HexMap(30, Orientation.LayoutFlat, new Vector2f(10, 7))
            {
                Position = new Vector2f(Window.Size.X / 2f, Window.Size.Y / 2f)
            };

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
            Window.Draw(HexMap);
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
