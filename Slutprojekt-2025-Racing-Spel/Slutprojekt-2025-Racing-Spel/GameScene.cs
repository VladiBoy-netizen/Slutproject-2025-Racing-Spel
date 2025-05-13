using Raylib_cs;
using System.ComponentModel;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    internal class GameScene : GameBinary
    {
        public static Camera3D camera;
        private static Camera3D freeCam;
        private static float cameraAngle = (float)Math.PI;
        private static float oldCameraAngle = (float)Math.PI;
        private static float targetCameraAngle = (float)Math.PI;
        private static bool isFPS = false;

        private Vector3 oldPos= new();

        public Player player = new();

        //private static Texture2D mirrorOverlay;
        private static RenderTexture2D mirrorView;
        public static Camera3D mirrorFPS;

        //private static Shader mirrorShader;

        public static Model PlayerBase;
        public static Model WheelBase;
        public static Texture2D PlayerBaseTexture;
        public static Texture2D PlayerGrassTexture;
        public static Texture2D WheelTexture;
        public static Texture2D DiskTexture;

        public static Model sceneModel;
        
        override public void Start()
        {
            //Raylib.DisableCursor();
            Console.WriteLine("Loaded DragRace scene");
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

            WheelBase = Raylib.LoadModel("Assets/Wheel.obj");
            WheelTexture = Raylib.LoadTexture("Assets/Wheel.png");
            DiskTexture = Raylib.LoadTexture("Assets/WheelDisk.png");
            unsafe
            {
                WheelBase.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.RayWhite;
                WheelBase.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = WheelTexture;
                WheelBase.Materials[1].Maps[(int)MaterialMapIndex.Albedo].Color = Color.RayWhite;
                WheelBase.Materials[1].Maps[(int)MaterialMapIndex.Albedo].Texture = DiskTexture;
            }

            sceneModel = Raylib.LoadModel("Assets/dragscene.obj");
            unsafe
            {
                sceneModel.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.RayWhite;
            }

            //mirrorOverlay = Raylib.LoadTexture("Assets/backmirror.png");
            mirrorView = Raylib.LoadRenderTexture(64*3, 32*3);

            freeCam = new Camera3D
            {
                Projection = CameraProjection.Perspective,
                Position = new Vector3(-4.5f, 2.5f, 4.5f),
                Target = new Vector3(0, 1.75f, 0),
                FovY = 60,
                Up = new Vector3(0, 1, 0)
            };

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

            player.Init(PlayerBase, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            player.FLWheel.Init(WheelBase, new Vector3(1.3f, 0.3f, -0.75f), new Vector3(0, 0, 0), 0.3f, true);
            player.FRWheel.Init(WheelBase, new Vector3(1.3f, 0.3f, 0.75f), new Vector3(0, 180, 0), 0.3f, false);
            player.BLWheel.Init(WheelBase, new Vector3(-1.175f, 0.3f, -0.75f), new Vector3(0, 0, 0), 0.3f, true);
            player.BRWheel.Init(WheelBase, new Vector3(-1.175f, 0.3f, 0.75f), new Vector3(0, 180, 0), 0.3f, false);
        }

        public override void PreUpdate()
        {
            // Camera
            oldCameraAngle = cameraAngle;

            //Raylib.UpdateCamera(ref freeCam, CameraMode.Free);
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

            player.Update();

            float lerpSpeed = 5f * Raylib.GetFrameTime();
            cameraAngle = Single.Lerp(cameraAngle, targetCameraAngle, lerpSpeed);
        }

        public override void LateUpdate()
        {
            if (player.Dead)
            {
                Program.sceneHandle.Binary = new DeadScene();
                Program.sceneHandle.changeScene = true;
            }

            oldPos = player.Position;

            // Camera
            if (!isFPS)
            {
                float distance = 4.5f;

                Vector3 localOffset = new(
                    distance * MathF.Cos(cameraAngle),
                    2.5f,
                    distance * MathF.Sin(cameraAngle)
                );

                Matrix4x4 rotationY = Matrix4x4.CreateRotationY(MathF.PI * player.Rotation.Y / 180f);
                Vector3 rotatedOffset = Vector3.Transform(localOffset, rotationY);

                camera.Position = player.Position + rotatedOffset;
                camera.Target = player.Position + Vector3.Transform(new Vector3(2, 1.75f, 0), rotationY);

            }
            else
            {
                float distance = 4.5f;

                Vector3 localOffset = new(
                    (-distance * (float)Math.Cos(cameraAngle)) - 2,
                    1.2f,
                    (-distance * (float)Math.Sin(cameraAngle)) - 0.3f
                );

                Matrix4x4 rotationY = Matrix4x4.CreateRotationY(MathF.PI * player.Rotation.Y / 180f);
                Vector3 rotatedOffset = Vector3.Transform(localOffset, rotationY);

                camera.Position = player.Position + Vector3.Transform(new Vector3(0, 1.2f, -0.3f), rotationY);
                camera.Target = player.Position + rotatedOffset;

                Vector3 localOffsetMirror = new(
                    (distance * (float)Math.Cos(MathF.PI)),
                    1.3f,
                    (-distance * (float)Math.Sin(MathF.PI))
                );

                Vector3 rotatedOffsetMirror = Vector3.Transform(localOffsetMirror, rotationY);
                mirrorFPS.Position = player.Position + Vector3.Transform(new Vector3(0, 1.3f, 0), rotationY);
                mirrorFPS.Target = player.Position + rotatedOffsetMirror;
            }

            camera.FovY = 60 + (player.Force.Length() * 30);
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
            Raylib.UnloadModel(WheelBase);
            Raylib.UnloadTexture(PlayerBaseTexture);
            Raylib.UnloadTexture(PlayerGrassTexture);
            //Raylib.UnloadTexture(mirrorOverlay);
            Raylib.UnloadRenderTexture(mirrorView);
            //Raylib.UnloadShader(mirrorShader);
        }

        public override void DrawGUI()
        {
            Raylib.DrawText(player.speed.ToString(), 50, 50, 20, Color.White);
            if (player.gearShift > 0) Raylib.DrawText(player.gearShift.ToString(), 50, 70, 20, Color.White);
            else if (player.gearShift == 0) Raylib.DrawText("R", 50, 70, 20, Color.White);
            //Raylib.DrawText(player.Dead.ToString(), 50, 150, 20, Color.White);
            Raylib.DrawText(player.stress.ToString(), 50, 90, 20, Color.White);
        }

        void Scene3D()
        {
            if (!isFPS)
            {
                player.Draw();
            }

            float gridWidth = 2.0f;
            float gridLength = 402.0f;
            float spacing = 1.0f;

            int numLines = (int)(gridLength / spacing);

            for (int i = 0; i <= numLines; i++)
            {
                float x = i * spacing;

                Raylib.DrawLine3D(new Vector3(x, 0, -gridWidth / 2), new Vector3(x, 0, gridWidth / 2), Color.Gray);
            }

            Raylib.DrawModel(sceneModel, new Vector3(0, 0, 0), 1, Color.White);

            Raylib.DrawLine3D(new Vector3(0, 0, -gridWidth / 2), new Vector3(gridLength, 0, -gridWidth / 2), Color.DarkBlue);
            Raylib.DrawLine3D(new Vector3(0, 0, gridWidth / 2), new Vector3(gridLength, 0, gridWidth / 2), Color.DarkBlue);
            Raylib.DrawLine3D(new Vector3(0, 0, 0), new Vector3(gridLength, 0, 0), Color.Red);
        }
    }
}
