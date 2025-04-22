using System.Numerics;
using Raylib_cs;

namespace Slutprojekt_2025_Racing_Spel
{
    class GameBinary
    {
        public static Camera3D camera;

        public static Model PlayerBase;
        public static Texture2D PlayerBaseTexture;

        public void Start()
        {
            PlayerBase = Raylib.LoadModel("Assets/castle.obj");
            PlayerBaseTexture = Raylib.LoadTexture("Assets/castle_diffuse.png");
            unsafe
            {
                PlayerBase.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = PlayerBaseTexture;
            }


            camera = new Camera3D
            {
                Projection = CameraProjection.Perspective,
                Position = new Vector3(-3, 2.8f, 0),
                Target = new Vector3(0, 1.75f, 0),
                FovY = 30,
                Up = new Vector3(0, 1, 0)
            };


        }

        public void PreUpdate() { }

        public void Update()
        {
            Raylib.UpdateCamera(ref camera, CameraMode.Free);
        }

        public void LateUpdate() { }

        public void Draw3D()
        {
            Raylib.BeginMode3D(camera);

            Raylib.DrawModel(PlayerBase, new Vector3(0, 0, 0), 1.0f, Color.White);

            Raylib.DrawGrid(10, 1);

            Raylib.EndMode3D();
        }

        public void Draw2D() { }

        public void DrawGUI() { }

        public void Exit()
        {
            Raylib.UnloadModel(PlayerBase);
        }
    }
}
