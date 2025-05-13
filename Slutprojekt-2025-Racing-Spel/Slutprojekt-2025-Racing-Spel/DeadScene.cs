using System;
using Raylib_cs;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    internal class DeadScene : GameBinary
    {
        private float time = 0;
        public static Texture2D Engine;
        private Texture2D fragLeft, fragRight, fragBottom;

        private bool breakEngine = false;
        private bool startFragments = false;
        private bool TextPopUp = false;

        private float shakeDuration = 2.0f;
        private float shakeTime = 0.0f;

        private float shakeInterval = 0.05f;
        private float shakeIntervalTime = 0.0f;
        private int xOffset = 0;

        private Vector2 leftPos, rightPos, bottomPos;
        private Vector2 leftVel = new Vector2(-50, -30);
        private Vector2 rightVel = new Vector2(50, -30);
        private Vector2 bottomVel = new Vector2(0, 60);
        private float fragmentGravity = 150.0f;

        private bool leftOff = false, rightOff = false, bottomOff = false;

        private Random rand = new Random();

        public override void Start()
        {
            Engine = Raylib.LoadTexture("Assets/Engine.png");
            fragLeft = Raylib.LoadTexture("Assets/EngineFRG1.png");
            fragRight = Raylib.LoadTexture("Assets/EngineFRG2.png");
            fragBottom = Raylib.LoadTexture("Assets/EngineFRG3.png");
        }

        public override void Update()
        {
            float delta = Raylib.GetFrameTime();
            time += delta;

            if (time > 1 && !breakEngine)
            {
                breakEngine = true;
                shakeTime = 0;
                shakeIntervalTime = 0;
            }

            if (breakEngine && !startFragments)
            {
                shakeTime += delta;
                shakeIntervalTime += delta;

                if (shakeIntervalTime >= shakeInterval)
                {
                    xOffset = rand.Next(-5, 6);
                    shakeIntervalTime = 0;
                }

                if (shakeTime >= shakeDuration)
                {
                    startFragments = true;

                    int centerX = Raylib.GetScreenWidth() / 2;
                    int centerY = Raylib.GetScreenHeight() / 2;

                    leftPos = new Vector2(centerX - 32, centerY - 32);
                    rightPos = new Vector2(centerX + 0, centerY - 32);
                    bottomPos = new Vector2(centerX - 32, centerY + 0);
                }
            }

            if (startFragments)
            {
                leftVel.Y += fragmentGravity * delta;
                rightVel.Y += fragmentGravity * delta;
                bottomVel.Y += fragmentGravity * delta;

                leftPos += leftVel * delta;
                rightPos += rightVel * delta;
                bottomPos += bottomVel * delta;

                int screenW = Raylib.GetScreenWidth();
                int screenH = Raylib.GetScreenHeight();

                if (leftPos.X + 64 < 0 || leftPos.Y > screenH) leftOff = true;
                if (rightPos.X > screenW || rightPos.Y > screenH) rightOff = true;
                if (bottomPos.Y > screenH) bottomOff = true;

                if (leftOff && rightOff && bottomOff)
                {
                    TextPopUp = true;
                }
            }

            if (TextPopUp)
            {
                if (Raylib.IsKeyDown(KeyboardKey.Space) || Raylib.IsKeyDown(KeyboardKey.Enter))
                {
                    Program.sceneHandle.Binary = new MainMenu();
                    Program.sceneHandle.changeScene = true;
                }
            }
        }

        public override void DrawGUI()
        {
            int centerX = Raylib.GetScreenWidth() / 2;
            int centerY = Raylib.GetScreenHeight() / 2;

            if (!breakEngine)
            {
                Raylib.DrawTexture(Engine, centerX - Engine.Width / 2, centerY - Engine.Height / 2, Color.White);
                return;
            }

            if (breakEngine && !startFragments)
            {
                Raylib.DrawTexture(Engine, centerX - Engine.Width / 2 + xOffset, centerY - Engine.Height / 2, Color.White);
            }

            if (startFragments)
            {
                if (!leftOff) Raylib.DrawTexture(fragLeft, (int)leftPos.X, (int)leftPos.Y, Color.White);
                if (!rightOff) Raylib.DrawTexture(fragRight, (int)rightPos.X, (int)rightPos.Y, Color.White);
                if (!bottomOff) Raylib.DrawTexture(fragBottom, (int)bottomPos.X, (int)bottomPos.Y, Color.White);
            }

            if (TextPopUp)
            {
                string message = "You Died!";
                int fontSize = 40;
                int textWidth = Raylib.MeasureText(message, fontSize);
                int screenWidth = Raylib.GetScreenWidth();
                int screenHeight = Raylib.GetScreenHeight();

                int x = (screenWidth - textWidth) / 2;
                int y = screenHeight / 2 - fontSize / 2;

                Raylib.DrawText(message, x, y, fontSize, Color.Red);
            }

        }

        public override void Exit()
        {
            Raylib.UnloadTexture(Engine);
            Raylib.UnloadTexture(fragLeft);
            Raylib.UnloadTexture(fragRight);
            Raylib.UnloadTexture(fragBottom);
        }
    }
}
