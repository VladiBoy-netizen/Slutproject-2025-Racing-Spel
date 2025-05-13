using System.Numerics;
using Raylib_cs;

namespace UnderRun
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
