using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

namespace UnderRun
{
    class Program
    {
        public static bool closeWindow = false;

        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        static bool consoleVisible = false;
        public static Scene sceneHandle = new();

        // Settings
        public static List<Vector2> Resolutions = [
            new(1152, 648), 
            new(1280, 720), 
            new(1366, 768), 
            new(1600, 900), 
            new(1920, 1080), 
            new(2560, 1440), 
            new(3840, 2160), 
            //new(7680, 4320), 
        ];

        public static List<ConfigFlags> configFlags = [];

        // Debugging
        public static bool isF3 = false;
        public static List<int> avgFPS = [];

        public static void ReloadScreen()
        {
            var settings = Saves.LoadSettings("settings.json");
            configFlags = [];

            ConfigFlags finalFlags = 0;
            foreach (var flag in settings.WindowFlags)
            {
                finalFlags |= flag;
                configFlags.Add(finalFlags);
            }
            Raylib.SetConfigFlags(finalFlags);
            Vector2 Resoulution = Resolutions[settings.ResolutionID];
            Raylib.InitWindow((int)Resoulution.X, (int)Resoulution.Y, "Under Run");
        }

        [STAThread]
        static void Main()
        {
            ReloadScreen();
            Raylib.SetTargetFPS(60);
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

            while (!closeWindow)
            {
                sceneHandle.Binary.Start();

                while (!sceneHandle.changeScene && !closeWindow)
                {
                    closeWindow = Raylib.WindowShouldClose();

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