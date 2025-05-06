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

        static bool consoleVisible = false;

        static Scene sceneHandle = new();

        // Debugging
        public static bool isF3 = false;
        public static List<int> avgFPS = [];

        [STAThread]
        static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow | ConfigFlags.FullscreenMode);
            Raylib.InitWindow(1280, 720, "Under Run");
            Raylib.SetTargetFPS(0);
            Raylib.SetExitKey(KeyboardKey.Null);
            Raylib.HideCursor();

            sceneHandle.Binary = new MainMenu();

            if (consoleVisible)
            {
                AllocConsole();
                RebindConsoleOutput();
                Console.WriteLine("[CONSOLE] Console enabled.");
                Console.Title = "[CONSOLE] Racing Spel";
            }

            //GameBinary gameBinary = new();

            while (!Raylib.WindowShouldClose())
            {
                sceneHandle.Binary.Start();

                while (!sceneHandle.changeScene && !Raylib.WindowShouldClose())
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

                    sceneHandle.Binary.PreUpdate();
                    sceneHandle.Binary.Update();

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.Black);

                    sceneHandle.Binary.Draw3D();
                    sceneHandle.Binary.Draw2D();
                    sceneHandle.Binary.DrawGUI();

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

                    sceneHandle.Binary.LateUpdate();

                    avgFPS.Add(Raylib.GetFPS());
                    if (avgFPS.Count > 100) avgFPS.RemoveAt(0);
                }

                sceneHandle.Binary.Exit();
                sceneHandle.changeScene = false;
            }

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