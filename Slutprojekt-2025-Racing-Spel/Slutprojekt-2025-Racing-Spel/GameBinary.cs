using System.Numerics;
using Raylib_cs;

namespace Slutprojekt_2025_Racing_Spel
{
    class GameBinary
    {
        virtual public void Start() { }

        virtual public void PreUpdate() { }

        virtual public void Update() { }

        virtual public void LateUpdate() { }

        virtual public void Draw3D() { }

        virtual public void Draw2D() { }

        virtual public void DrawGUI() { }

        virtual public void Exit() { }
    }
}
