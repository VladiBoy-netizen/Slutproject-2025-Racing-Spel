using Raylib_cs;
using System.Numerics;

namespace UnderRun
{
    internal class MainMenu : GameBinary
    {
        public static Texture2D bgtile;
        public static Texture2D Arrow;
        public static Texture2D Enter;

        public static Vector2 offset = new(0, 0);

        public static int action = 0;
        public static List<float> actionsFontSize = [75, 40, 40, 40];

        public static int setting = 0;
        public static List<int> ints = [0, 1];

        static bool showHelp = false;
        static bool showOptions = false;

        public override void Start()
        {
            bgtile = Raylib.LoadTexture("Assets/bgtexture.png");
            Arrow = Raylib.LoadTexture("Assets/Arrow.png");
            Enter = Raylib.LoadTexture("Assets/Enter.png");
        }

        public override void Update()
        {
            offset.X += 20 * Raylib.GetFrameTime();
            offset.Y += 20 * Raylib.GetFrameTime();

            offset.X %= bgtile.Width;
            offset.Y %= bgtile.Height;

            if (!showHelp && !showOptions)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.W)) action--;
                if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.S)) action++;
            }

            if (action < 0) action = 0;
            if (action > 3) action = 3;

            if (!showHelp && showOptions)
            {
                //if (Raylib.IsKeyPressed())
            }

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
            if (!showHelp && !showOptions)
            {
                if (action == 0) Raylib.DrawText("[Start]", 175, (int)(250 - (actionsFontSize[0] / 2)), (int)actionsFontSize[0], Color.White);
                if (action != 0) Raylib.DrawText("Start", 175, (int)(250 - (actionsFontSize[0] / 2)), (int)actionsFontSize[0], Color.White);

                if (action == 1) Raylib.DrawText("[Options]", 175, (int)(350 - (actionsFontSize[1] / 2)), (int)actionsFontSize[1], Color.White);
                if (action != 1) Raylib.DrawText("Options", 175, (int)(350 - (actionsFontSize[1] / 2)), (int)actionsFontSize[1], Color.White);

                if (action == 2) Raylib.DrawText("[Help]", 175, (int)(450 - (actionsFontSize[2] / 2)), (int)actionsFontSize[2], Color.White);
                if (action != 2) Raylib.DrawText("Help", 175, (int)(450 - (actionsFontSize[2] / 2)), (int)actionsFontSize[2], Color.White);

                if (action == 3) Raylib.DrawText("[Exit]", 175, (int)(550 - (actionsFontSize[3] / 2)), (int)actionsFontSize[3], Color.White);
                if (action != 3) Raylib.DrawText("Exit", 175, (int)(550 - (actionsFontSize[3] / 2)), (int)actionsFontSize[3], Color.White);

                Raylib.DrawText("[Controlls]", 5, Raylib.GetScreenHeight() - 64, 24, Color.White);
                Raylib.DrawTextureEx(
                    Arrow, 
                    new Vector2(5, Raylib.GetScreenHeight() - 5 - 32), 
                    0, 
                    2, 
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5 + 64, Raylib.GetScreenHeight() - 5),
                    180, 
                    2,
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Enter,
                    new Vector2(5 + 64, Raylib.GetScreenHeight() - 5 - 32),
                    0,
                    2,
                    Color.White
                );

                Raylib.DrawText("[Under Run]", 150 - 2, 99, 76, Color.Blue);
                Raylib.DrawText("[Under Run]", 150 - 1, 98, 76, Color.Green);
                Raylib.DrawText("[Under Run]", 150 + 2, 101, 76, Color.Red);
                Raylib.DrawText("[Under Run]", 150, 100, 76, Color.White);
            }
            if (!showHelp && showOptions)
            {
                Raylib.DrawRectangleV(new Vector2(Raylib.GetScreenWidth() / 2, 0), new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight()), Raylib.Fade(Color.White, 0.15f));
                Raylib.DrawLineEx(new Vector2(Raylib.GetScreenWidth() / 2, 0), new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight()), 2, Raylib.Fade(Color.White, 0.85f));

                Raylib.DrawText("Sorry, nothing here", Raylib.GetScreenWidth() / 2 + 20, 20, 20, Color.White);

                Raylib.DrawText("[Controlls]", 5, Raylib.GetScreenHeight() - 64, 24, Color.White);
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5, Raylib.GetScreenHeight() - 5 - 32),
                    0,
                    2,
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5 + 64, Raylib.GetScreenHeight() - 5),
                    180,
                    2,
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5 + 96, Raylib.GetScreenHeight() - 5 - 32),
                    90,
                    2,
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5 + 96, Raylib.GetScreenHeight() - 5),
                    270,
                    2,
                    Color.White
                );
                Raylib.DrawText("ESC", 128 + 8, Raylib.GetScreenHeight() - 5 - 32, 20, Color.White);

                Raylib.DrawText("[Options]", 175 - 2, (int)(350 - (75 / 2)) - 1, 75, Color.Blue);
                Raylib.DrawText("[Options]", 175 - 1, (int)(350 - (75 / 2)) - 2, 75, Color.Red);
                Raylib.DrawText("[Options]", 175 + 2, (int)(350 - (75 / 2)) + 1, 75, Color.Green);
                Raylib.DrawText("[Options]", 175, (int)(350 - (75 / 2)), 75, Color.White);
            }
            if (showHelp && !showOptions)
            {
                Raylib.DrawText("[Controlls]", 5, Raylib.GetScreenHeight() - 64, 24, Color.White);
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5, Raylib.GetScreenHeight() - 5 - 32),
                    0,
                    2,
                    Color.White
                );
                Raylib.DrawTextureEx(
                    Arrow,
                    new Vector2(5 + 64, Raylib.GetScreenHeight() - 5),
                    180,
                    2,
                    Color.White
                );
                Raylib.DrawText("ESC", 64 + 8, Raylib.GetScreenHeight() - 5 - 32, 20, Color.White);

                Raylib.DrawRectangleV(new Vector2(Raylib.GetScreenWidth() / 3, 0), new Vector2(Raylib.GetScreenWidth() / 3 * 2, Raylib.GetScreenHeight()), Raylib.Fade(Color.White, 0.15f));
                Raylib.DrawLineEx(new Vector2(Raylib.GetScreenWidth() / 3, 0), new Vector2(Raylib.GetScreenWidth() / 3, Raylib.GetScreenHeight()), 2, Raylib.Fade(Color.White, 0.85f));

                Rectangle rect = new Rectangle(Raylib.GetScreenWidth() / 3 + 5, 5, Raylib.GetScreenWidth() / 3 * 2 - 10, Raylib.GetScreenHeight() - 10);

                RaylibTextHelper.DrawTextBoxed("[Input]\nArrows: Rotate camera\nWASD: Move car\nLeft Shift: Boost the car on gear 12\nSpace: Handbreak\nV: Change camera view\n\n[Gameplay]\nNow only drag race avaliable\nBut if you hit engine stress above 10, you DIE.", rect, 20, 2, Color.White);

                Raylib.DrawText("[Help]", 175 - 2, (int)(450 - (75 / 2)) - 1, 75, Color.Red);
                Raylib.DrawText("[Help]", 175 - 1, (int)(450 - (75 / 2)) - 2, 75, Color.Blue);
                Raylib.DrawText("[Help]", 175 + 2, (int)(450 - (75 / 2)) + 1, 75, Color.Green);
                Raylib.DrawText("[Help]", 175, (int)(450 - (75 / 2)), 75, Color.White);
            }
        }

        public override void LateUpdate()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Enter) && !showHelp && !showOptions)
            {
                if (action == 0)
                {
                    Program.sceneHandle.Binary = new GameScene();
                    Program.sceneHandle.changeScene = true;
                }
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Enter) && !showHelp && !showOptions) if (action == 1) showOptions = true;
            if (Raylib.IsKeyPressed(KeyboardKey.Enter) && !showHelp && !showOptions) if (action == 2) showHelp = true;
            if (Raylib.IsKeyPressed(KeyboardKey.Enter) && !showHelp && !showOptions) if (action == 3) Program.closeWindow = true;

            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                showOptions = false;
                showHelp = false;
            }
        }

        public override void Exit()
        {
            Raylib.UnloadTexture(bgtile);
            Raylib.UnloadTexture(Arrow);
        }
    }
}
