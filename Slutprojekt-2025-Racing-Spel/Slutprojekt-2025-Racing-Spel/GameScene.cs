using Raylib_cs;
using System.ComponentModel;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    internal class GameScene : GameBinary
    {
        public static Camera3D camera;
        private static float cameraAngle = (float)Math.PI; // in rads
        private static float oldCameraAngle = (float)Math.PI; // in rads
        private static float targetCameraAngle = (float)Math.PI;
        private static bool isFPS = true;

        //private static Texture2D mirrorOverlay;
        private static RenderTexture2D mirrorView;
        public static Camera3D mirrorFPS;

        //private static Shader mirrorShader;

        public static Model PlayerBase;
        public static Texture2D PlayerBaseTexture;
        public static Texture2D PlayerGrassTexture;
        
        override public void Start()
        {
            PlayerBase = Raylib.LoadModel("Assets/ACar.obj");
            PlayerBaseTexture = Raylib.LoadTexture("Assets/CarBase.png");
            PlayerGrassTexture = Raylib.LoadTexture("Assets/CarGlass.png");
            unsafe
            {
                PlayerBase.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.RayWhite;
                PlayerBase.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.RayWhite;
                PlayerBase.Materials[1].Maps[(int)MaterialMapIndex.Albedo].Texture = PlayerGrassTexture;
                PlayerBase.Materials[2].Maps[(int)MaterialMapIndex.Albedo].Color = Color.Black;
                //Shader shader = Raylib.LoadShader("simple_shader.vs", "simple_shader.fs");
                //PlayerBase.Materials[0].Shader = shader;
            }

            //mirrorOverlay = Raylib.LoadTexture("Assets/backmirror.png");
            mirrorView = Raylib.LoadRenderTexture(64*3, 32*3);

            if (!isFPS)
            {
                camera = new Camera3D
                {
                    Projection = CameraProjection.Perspective,
                    Position = new Vector3(-4.5f, 2.5f, 4.5f),
                    Target = new Vector3(0, 1.75f, 0),
                    FovY = 60,
                    Up = new Vector3(0, 1, 0)
                };
            }
            else
            {
                camera = new Camera3D
                {
                    Projection = CameraProjection.Perspective,
                    Position = new Vector3(0, 1.2f, -0.3f),
                    Target = new Vector3(2, 1.2f, -0.3f),
                    FovY = 60,
                    Up = new Vector3(0, 1, 0)
                };
            }

            mirrorFPS = new Camera3D
            {
                Projection = CameraProjection.Perspective,
                Position = new Vector3(0, 1.3f, 0),
                Target = new Vector3(-2, 1.3f, 0),
                FovY = 40, 
                Up = new Vector3(0, 1, 0)
            };
        }

        public override void PreUpdate()
        {
            // Camera
            oldCameraAngle = cameraAngle;
        }

        public override void Update()
        {
            // Camera
            if (!isFPS)
            {
                if (Raylib.IsKeyDown(KeyboardKey.Down))
                {
                    if (oldCameraAngle > (float)Math.PI) targetCameraAngle = (float)Math.PI * 2;
                    if (oldCameraAngle < (float)Math.PI) targetCameraAngle = 0;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.Up) && !Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = (float)Math.PI;
                else if (Raylib.IsKeyDown(KeyboardKey.Up) && !Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 5 * (float)Math.PI / 6;
                else if (Raylib.IsKeyDown(KeyboardKey.Up) && Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 7 * (float)Math.PI / 6;
                else if (Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 3 * (float)Math.PI / 2;
                else if (!Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = (float)Math.PI / 2;
                else { targetCameraAngle = (float)Math.PI; }
            }
            else
            {
                if (Raylib.IsKeyDown(KeyboardKey.Left) && Raylib.IsKeyDown(KeyboardKey.Right)) targetCameraAngle = (float)Math.PI;
                else if (!Raylib.IsKeyDown(KeyboardKey.Left) && Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Down)) targetCameraAngle = 3 * (float)Math.PI / 2;
                else if (Raylib.IsKeyDown(KeyboardKey.Left) && !Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Down)) targetCameraAngle = (float)Math.PI / 2;
                else if (Raylib.IsKeyDown(KeyboardKey.Left) && !Raylib.IsKeyDown(KeyboardKey.Right)) targetCameraAngle = 5 * (float)Math.PI / 6;
                else if (!Raylib.IsKeyDown(KeyboardKey.Left) && Raylib.IsKeyDown(KeyboardKey.Right)) targetCameraAngle = 7 * (float)Math.PI / 6;
                else { targetCameraAngle = (float)Math.PI; }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.V))
            {
                isFPS = !isFPS;
            }

            float lerpSpeed = 5f * Raylib.GetFrameTime();
            cameraAngle = Single.Lerp(cameraAngle, targetCameraAngle, lerpSpeed);
        }

        public override void LateUpdate()
        {
            // Camera
            if (!isFPS)
            {
                float distance = 4.5f;
                Vector3 offset = new(
                    distance * (float)Math.Cos(cameraAngle),
                    2.5f,
                    distance * (float)Math.Sin(cameraAngle)
                );

                camera.Position = offset;
                camera.Target = new Vector3(2, 1.75f, 0);
            }
            else
            {
                float distance = 4.5f;
                Vector3 offset = new(
                    (-distance * (float)Math.Cos(cameraAngle)) - 2,
                    1.2f,
                    (-distance * (float)Math.Sin(cameraAngle)) - 0.3f
                );

                camera.Position = new Vector3(0, 1.2f, -0.3f);
                camera.Target = offset;
            }
        }

        public override void Draw3D()
        {
            Raylib.BeginMode3D(camera);

            Scene3D();

            Raylib.EndMode3D();
        }

        public override void Draw2D()
        {
            if (isFPS)
            {
                Raylib.BeginTextureMode(mirrorView);
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginMode3D(mirrorFPS);
                Scene3D();
                Raylib.EndMode3D();
                Raylib.EndTextureMode();

                Raylib.DrawTextureRec(
                    mirrorView.Texture,
                    new Rectangle(0, 0, mirrorView.Texture.Width, -mirrorView.Texture.Height),
                    new Vector2(1280 / 2 - (mirrorView.Texture.Width / 2), 10),
                    Color.White
                );

                Raylib.BeginBlendMode(BlendMode.Additive);
                Raylib.DrawRectangleRec(
                    new Rectangle(
                        1280 / 2 - (mirrorView.Texture.Width / 2),
                        10,
                        mirrorView.Texture.Width,
                        mirrorView.Texture.Height
                    ),
                    new Color(255, 255, 255, 25)
                );
                Raylib.EndBlendMode();
            }
        }

        public override void Exit()
        {
            Raylib.UnloadModel(PlayerBase);
            Raylib.UnloadTexture(PlayerBaseTexture);
            Raylib.UnloadTexture(PlayerGrassTexture);
            //Raylib.UnloadTexture(mirrorOverlay);
            Raylib.UnloadRenderTexture(mirrorView);
            //Raylib.UnloadShader(mirrorShader);
        }


        void Scene3D()
        {
            if (!isFPS)
            {
                Raylib.DrawModel(PlayerBase, new Vector3(0, 0, 0), 1.0f, Color.White);
            }

            Raylib.DrawCubeV(new Vector3(-3, 0.5f, 0), new Vector3(1, 1, 1), Color.Red);
            Raylib.DrawCubeV(new Vector3(3, 0.5f, 0), new Vector3(1, 1, 1), Color.Blue);
            Raylib.DrawCubeWiresV(new Vector3(-3, 0.5f, 0), new Vector3(1, 1, 1), Color.Blue);
            Raylib.DrawCubeWiresV(new Vector3(3, 0.5f, 0), new Vector3(1, 1, 1), Color.Red);

            Raylib.DrawGrid(10, 1);
        }
    }
}
