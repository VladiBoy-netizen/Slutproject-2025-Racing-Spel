using Raylib_cs;
using System.Numerics;

namespace Slutprojekt_2025_Racing_Spel
{
    class Player
    {

    }
}

public class Wheel
{
    public static Vector3 Position = new();
    public static Vector3 Rotation = new();
    public static Vector3 Force = new();
    public static Model Model = new();
    public static bool isGrounded = false;
    public static float Radius = 0.15f;
}
