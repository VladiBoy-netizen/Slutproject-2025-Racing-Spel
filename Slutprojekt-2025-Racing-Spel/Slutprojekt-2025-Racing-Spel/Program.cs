using System;
using Raylib_cs;

class Program
{
    static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow | ConfigFlags.FullscreenMode);
        Raylib.InitWindow(1280, 720, "Racing Spel");
        Raylib.SetTargetFPS(60);
        Raylib.SetExitKey(KeyboardKey.Null);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.EndDrawing();
        }
    }
}