using System.Numerics;
using Raylib_cs;

namespace Slutprojekt_2025_Racing_Spel
{
    class GameBinary
    {
        public static Camera3D camera;

        public void Start()
        {
            camera = new Camera3D
            {
                Projection = CameraProjection.Perspective,
                Position = new Vector3(-3, 2.8f, 6),
                Target = new Vector3(0, 1.75f, 0),
                FovY = 65,
                Up = new Vector3(0, 1, 0)
            };
        }

        public void PreUpdate() { }

        public void Update()
        {
            
        }

        public void LateUpdate() { }

        public void Draw3D()
        {
            Raylib.BeginMode3D(camera);

            Raylib.DrawGrid(10, 1);

            Raylib.EndMode3D();
        }

        public void Draw2D() { }

        public void DrawGUI() { }

        public void Exit()
        {
            
        }
    }
}
