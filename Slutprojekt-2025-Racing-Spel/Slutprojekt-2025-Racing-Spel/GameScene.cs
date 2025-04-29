using Raylib_cs;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    internal class GameScene : GameBinary
    {
        public static Camera3D camera;
        private static float cameraAngle = (float)Math.PI; // in rads
        private static float oldCameraAngle = (float)Math.PI; // in rads
        private static float targetCameraAngle = (float)Math.PI;

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


            camera = new Camera3D
            {
                Projection = CameraProjection.Perspective,
                Position = new Vector3(-4.5f, 2.5f, 4.5f),
                Target = new Vector3(0, 1.75f, 0),
                FovY = 60,
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
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                if (oldCameraAngle > (float)Math.PI) targetCameraAngle = (float)Math.PI * 2;
                if (oldCameraAngle < (float)Math.PI) targetCameraAngle = 0;

            }
            else if (Raylib.IsKeyDown(KeyboardKey.Up) && !Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = (float)Math.PI;
            else if (Raylib.IsKeyDown(KeyboardKey.Up) && Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 5 * (float)Math.PI / 6;
            else if (Raylib.IsKeyDown(KeyboardKey.Up) && !Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 7 * (float)Math.PI / 6;
            else if (!Raylib.IsKeyDown(KeyboardKey.Right) && Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = 3 * (float)Math.PI / 2;
            else if (Raylib.IsKeyDown(KeyboardKey.Right) && !Raylib.IsKeyDown(KeyboardKey.Left)) targetCameraAngle = (float)Math.PI / 2;
            else { targetCameraAngle = (float)Math.PI; }

            float lerpSpeed = 5f * Raylib.GetFrameTime();
            cameraAngle = Single.Lerp(cameraAngle, targetCameraAngle, lerpSpeed);
        }

        public override void LateUpdate()
        {
            // Camera
            float distance = 4.5f;
            Vector3 offset = new(
                distance * (float)Math.Cos(cameraAngle),
                2.5f,
                distance * (float)Math.Sin(cameraAngle)
            );

            camera.Position = offset;
        }

        public override void Draw3D()
        {
            Raylib.BeginMode3D(camera);

            Raylib.DrawModel(PlayerBase, new Vector3(0, 0, 0), 1.0f, Color.White);

            Raylib.DrawGrid(10, 1);

            Raylib.EndMode3D();
        }

        public override void Exit()
        {
            Raylib.UnloadModel(PlayerBase);
            Raylib.UnloadTexture(PlayerBaseTexture);
            Raylib.UnloadTexture(PlayerGrassTexture);
        }
    }
}
