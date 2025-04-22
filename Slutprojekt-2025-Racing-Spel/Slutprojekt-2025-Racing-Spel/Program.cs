using System;
using System.IO;
using System.Runtime.InteropServices;
using Raylib_cs;

namespace Slutprojekt_2025_Racing_Spel
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        static bool consoleVisible = true;

        // Debugging
        public static bool isF3 = true;
        public static List<int> avgFPS = [];

        [STAThread]
        static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow);
            Raylib.InitWindow(1280, 720, "Racing Spel");
            Raylib.SetTargetFPS(0);
            Raylib.SetExitKey(KeyboardKey.Null);
            Raylib.DisableCursor();

            if (consoleVisible)
            {
                AllocConsole();
                RebindConsoleOutput();
                Console.WriteLine("[CONSOLE] Console enabled.");
                Console.Title = "[CONSOLE] Racing Spel";
            }

            GameBinary gameBinary = new();
            gameBinary.Start();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyReleased(KeyboardKey.F3))
                {
                    isF3 = !isF3;
                    try
                    {
                        if (isF3) Console.WriteLine("[DEBUG] F3 Menu Enabled"); else Console.WriteLine("[DEBUG] F3 Menu Disabled");
                    }
                    catch { }
                }
                if (Raylib.IsKeyReleased(KeyboardKey.F11)) Raylib.ToggleFullscreen();

                // Toggle console with ~ key
                if (Raylib.IsKeyPressed(KeyboardKey.Grave))
                {
                    consoleVisible = !consoleVisible;
                    if (consoleVisible)
                    {
                        AllocConsole();
                        RebindConsoleOutput();
                        Console.WriteLine("[CONSOLE] Console enabled.");
                        Console.Title = "[CONSOLE] Racing Spel";
                    }
                    else
                    {
                        Console.WriteLine("[CONSOLE] Disabling console...");
                        FreeConsole();
                    }
                }

                gameBinary.PreUpdate();
                gameBinary.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                gameBinary.Draw3D();
                gameBinary.Draw2D();
                gameBinary.DrawGUI();

                if (isF3)
                {
                    Raylib.DrawText("FPS : " + Raylib.GetFPS().ToString(), 5, 5, 10, Color.RayWhite);
                    float avgFPS100 = 0;
                    try
                    {
                        foreach (float f in avgFPS) avgFPS100 += f;
                        avgFPS100 /= avgFPS.Count;
                        avgFPS100 = (float)Math.Round(avgFPS100);
                    }
                    catch { avgFPS100 = Raylib.GetFPS(); }
                    Raylib.DrawText("Avg1% FPS : " + avgFPS100.ToString(), 5, 15, 10, Color.RayWhite);
                }
                Raylib.EndDrawing();

                gameBinary.LateUpdate();

                avgFPS.Add(Raylib.GetFPS());
                if (avgFPS.Count > 100) avgFPS.RemoveAt(0);
            }

            gameBinary.Exit();
            Raylib.CloseWindow();
        }

        static void RebindConsoleOutput()
        {
            var stdout = Console.OpenStandardOutput();
            var writer = new StreamWriter(stdout) { AutoFlush = true };
            Console.SetOut(writer);
            Console.SetError(writer);
        }
    }
}