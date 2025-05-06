using Raylib_cs;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    internal class MainMenu : GameBinary
    {
        public static Texture2D bgtile;

        public static Vector2 offset = new(0, 0);

        public static int action = 0;
        public static List<float> actionsFontSize = [75, 40, 40, 40];

        public override void Start()
        {
            bgtile = Raylib.LoadTexture("Assets/bgtexture.png");
        }

        public override void Update()
        {
            offset.X += 20 * Raylib.GetFrameTime();
            offset.Y += 20 * Raylib.GetFrameTime();

            offset.X %= bgtile.Width;
            offset.Y %= bgtile.Height;

            if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.W)) action--;
            if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.S)) action++;

            if (action < 0) action = 0;
            if (action > 3) action = 3;

            for (int i = 0; i < actionsFontSize.Count; i++)
            {
                if (action == i) actionsFontSize[i] = Single.Lerp(actionsFontSize[i], 75, 5 * Raylib.GetFrameTime());
                else actionsFontSize[i] = Single.Lerp(actionsFontSize[i], 40, 5 * Raylib.GetFrameTime());

                if (action == i && actionsFontSize[i] > (75 * 0.975)) actionsFontSize[i] = 75;
                if (action != i && actionsFontSize[i] < (45 * 1.025)) actionsFontSize[i] = 45;
            }
        }

        public override void Draw2D()
        {
            for (int y = -bgtile.Height; y < Raylib.GetScreenHeight(); y += bgtile.Height)
            {
                for (int x = -bgtile.Width; x < Raylib.GetScreenWidth(); x += bgtile.Width)
                {
                    Raylib.DrawTexture(bgtile, x + (int)offset.X, y + (int)offset.Y, Raylib.Fade(Color.DarkGray, 0.4f));
                }
            }
        }

        public override void DrawGUI()
        {
            if (action == 0) Raylib.DrawText("[Start]", 175, (int)(250 - (actionsFontSize[0] / 2)), (int)actionsFontSize[0], Color.White);
            if (action != 0) Raylib.DrawText("Start", 175, (int)(250 - (actionsFontSize[0] / 2)), (int)actionsFontSize[0], Color.White);

            if (action == 1) Raylib.DrawText("[Options]", 175, (int)(350 - (actionsFontSize[1] / 2)), (int)actionsFontSize[1], Color.White);
            if (action != 1) Raylib.DrawText("Options", 175, (int)(350 - (actionsFontSize[1] / 2)), (int)actionsFontSize[1], Color.White);

            if (action == 2) Raylib.DrawText("[Help]", 175, (int)(450 - (actionsFontSize[2] / 2)), (int)actionsFontSize[2], Color.White);
            if (action != 2) Raylib.DrawText("Help", 175, (int)(450 - (actionsFontSize[2] / 2)), (int)actionsFontSize[2], Color.White);

            if (action == 3) Raylib.DrawText("[Exit]", 175, (int)(550 - (actionsFontSize[3] / 2)), (int)actionsFontSize[3], Color.White);
            if (action != 3) Raylib.DrawText("Exit", 175, (int)(550 - (actionsFontSize[3] / 2)), (int)actionsFontSize[3], Color.White);

            Raylib.DrawText("[Under Run]", 150 - 2, 99, 76, Color.Blue);
            Raylib.DrawText("[Under Run]", 150 - 1, 98, 76, Color.Green);
            Raylib.DrawText("[Under Run]", 150 + 2, 101, 76, Color.Red);
            Raylib.DrawText("[Under Run]", 150, 100, 76, Color.White);
        }

        public override void Exit()
        {
            Raylib.UnloadTexture(bgtile);
        }
    }
}
