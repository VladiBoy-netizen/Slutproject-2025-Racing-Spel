using System;
using System.Numerics;
using Raylib_cs;

namespace Slutprojekt_2025_Racing_Spel
{
    class Player
    {
        public Wheel FLWheel = new();
        public Wheel FRWheel = new();
        public Wheel BLWheel = new();
        public Wheel BRWheel = new();

        public Vector3 Position = new();
        public Vector3 Rotation = new();
        public Vector3 Force = new();
        public Model Model = new();
        public Vector3 oldPos;

        public bool Dead = false;

        public float stress = 0;
        public bool stressApplied = false;

        //public float weight = 800;
        public int speed = 0;
        public double shiftDelay = 0.3;
        private double lastShiftTime = 0;
        public bool isReversing = false;

        public List<float> gearSpeeds = new List<float> {
        8.9408f,      // Reverse speed (gearShift = 0)
        8.9408f,
        15.6464f,
        22.352f,
        29.0576f,
        35.7632f,
        42.4688f,
        49.1744f,
        55.88f,
        62.5856f,
        69.2912f,
        75.9968f,
        82.7024f
    };
        public List<float> minSpeeds = new List<float> {
        0f,
        0f,
        4f,
        8f,
        12f,
        16f,
        20f,
        24f,
        28f,
        32f,
        36f,
        40f,
        44f 
    };

        public int gearShift = 1;

        public void Init(Model model, Vector3 position, Vector3 rotation)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
        }

        public void Draw()
        {
            Raylib.DrawModelEx(Model, Position, Vector3.UnitY, Rotation.Y, Vector3.One, Color.White);
            DrawWheel(FLWheel);
            DrawWheel(FRWheel);
            DrawWheel(BLWheel);
            DrawWheel(BRWheel);
        }

        public void Update()
        {
            Dead = false || stress > 10;

            float frameTime = Raylib.GetFrameTime();

            HandleSteeringInput();
            HandleGearInput();
            ApplyDriveForce();

            Vector3 carForward = Vector3.Normalize(GetForwardDirection(Rotation));

            FLWheel.Update(carForward, Force, speed);
            FRWheel.Update(carForward, Force, speed);
            BLWheel.Update(carForward, Force, speed);
            BRWheel.Update(carForward, Force, speed);

            Force = (FLWheel.Force + FRWheel.Force + BLWheel.Force + BRWheel.Force) / 2f;
            if (Force.Length() < 0.001f) Force = new Vector3(0, 0, 0);
            Position += Force;

            float steeringEffect = ((FLWheel.Rotation.Y - FLWheel.SavedRotation.Y) + (FRWheel.Rotation.Y - FRWheel.SavedRotation.Y));
            float directionFactor = isReversing ? -1f : 1f;

            float forceMagnitude = Force.Length();

            float steeringScale = Single.Clamp(forceMagnitude * 20f, 0f, 1f);
            Rotation.Y -= steeringEffect * directionFactor * steeringScale * frameTime;

            AutoShift();

            if (stress > 0) stress -= 3 * frameTime;
            if (stress > 0 && !stressApplied) stress -= 3 * frameTime;
            if (stress < 0) stress = 0;
            stressApplied = false;

            float distance = (Position - oldPos).Length();
            speed = (int)((distance / frameTime) * 3600 / 1000);

            oldPos = Position;
        }

        private void HandleSteeringInput()
        {
            if (Raylib.IsKeyDown(KeyboardKey.A) && !Raylib.IsKeyDown(KeyboardKey.D))
            {
                FLWheel.Target(-33.3f);
                FRWheel.Target(-30f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D) && !Raylib.IsKeyDown(KeyboardKey.A))
            {
                FLWheel.Target(30f);
                FRWheel.Target(33.3f);
            }
            else
            {
                FLWheel.Target(0);
                FRWheel.Target(0);
            }
        }

        private void DrawWheel(Wheel wheel)
        {
            Matrix4x4 localRotation =
                Matrix4x4.CreateRotationX(MathF.PI * (wheel.Rotation.X - Rotation.X) / 180f) *
                Matrix4x4.CreateRotationY(MathF.PI * (wheel.Rotation.Y - Rotation.Y) / 180f) *
                Matrix4x4.CreateRotationZ(MathF.PI * (wheel.Rotation.Z - Rotation.Z) / 180f);

            wheel.Model.Transform = localRotation;

            Quaternion worldRotation = Quaternion.CreateFromYawPitchRoll(
                Rotation.Y * MathF.PI / 180f,
                Rotation.X * MathF.PI / 180f,
                Rotation.Z * MathF.PI / 180f
            );

            Vector3 offset = Vector3.Transform(wheel.Position, worldRotation);

            Raylib.DrawModel(wheel.Model, Position + offset, 1.0f, Color.White);
        }

        public void AutoShift()
        {
            float currentSpeed = (float)speed;
            double currentTime = Raylib.GetTime();

            if (currentTime - lastShiftTime < shiftDelay) return;

            if (gearShift < 1) gearShift = 1;
            if (gearShift >= gearSpeeds.Count) gearShift = gearSpeeds.Count - 1;

            if (gearShift < gearSpeeds.Count - 1 && currentSpeed > gearSpeeds[gearShift])
            {
                gearShift++;
                lastShiftTime = currentTime;
                stress += 1;
                stressApplied = true;
            }
            while (gearShift > 1 && currentSpeed < gearSpeeds[gearShift - 1] * 0.9f)
            {
                gearShift--;
                lastShiftTime = currentTime;
                stress += 1;
                stressApplied = true;
            }
        }

        public void HandleGearInput()
        {
            if (Raylib.IsKeyDown(KeyboardKey.W) && !Raylib.IsKeyDown(KeyboardKey.S))
            {
                isReversing = false;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S) && !Raylib.IsKeyDown(KeyboardKey.W))
            {
                isReversing = true;
            }

            if (isReversing)
                gearShift = 0;
            else if (gearShift == 0)
                gearShift = 1;
        }

        public void ApplyDriveForce()
        {
            float frameTime = Raylib.GetFrameTime();
            float baseSpeed = gearSpeeds[gearShift];
            bool isHandbrake = Raylib.IsKeyDown(KeyboardKey.Space);

            Vector3 forwardDirection = Vector3.Normalize(GetForwardDirection(Rotation));
            Vector3 force = Vector3.Zero;

            if (!isHandbrake)
            {
                if (Raylib.IsKeyDown(KeyboardKey.W) && !Raylib.IsKeyDown(KeyboardKey.S))
                {
                    if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && gearShift == 12)
                    {
                        force = forwardDirection * (baseSpeed * 2) * frameTime;
                        stress += 5f * frameTime;
                        stressApplied = true;
                    }
                    else force = forwardDirection * baseSpeed * frameTime;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.S) && !Raylib.IsKeyDown(KeyboardKey.W))
                {
                    force = -forwardDirection * baseSpeed * frameTime;
                }

                if (force != Vector3.Zero)
                {
                    BLWheel.AddForce(force);
                    BRWheel.AddForce(force);
                }
            }
            else
            {
                BLWheel.WantedForce *= new Vector3(1 - (1 * Raylib.GetFrameTime()), 1 - (1 * Raylib.GetFrameTime()), 1 - (1 * Raylib.GetFrameTime()));
                BRWheel.WantedForce *= new Vector3(1 - (1 * Raylib.GetFrameTime()), 1 - (1 * Raylib.GetFrameTime()), 1 - (1 * Raylib.GetFrameTime()));
            }
        }

        public Vector3 GetForwardDirection(Vector3 rotation)
        {
            float yaw = rotation.Y * MathF.PI / 180f;

            return new Vector3(MathF.Cos(yaw), 0, -MathF.Sin(yaw));
        }
    }
}

public class Wheel
{
    public Vector3 Position = new();
    public Vector3 Rotation = new();
    public Vector3 SavedRotation = new();
    public Vector3 Force = new();
    public Vector3 WantedForce = new();
    private Vector3 targetForce = new();
    public Model Model = new();
    public float maxSuspension = 0.3f;
    public float minSuspension = -0.025f;
    public float lift = 300;
    public bool isGrounded = true;
    public float Radius = 0.3f;
    public bool reversed = false;

    private bool forceAppliedThisFrame = false;
    public float accelerationRate = 2f;
    public float decelerationRate = 0.6125f;

    public void Init(Model model, Vector3 position, Vector3 rotation, float radius, bool y180turn)
    {
        Model = model;
        Position = position;
        SavedRotation = rotation;
        Rotation = rotation;
        Radius = radius;
        reversed = y180turn;
    }

    public void Target(float angle)
    {
        Rotation.Y = Single.Lerp(Rotation.Y, SavedRotation.Y + angle, 5f * Raylib.GetFrameTime());
    }

    public void AddForce(Vector3 force)
    {
        targetForce = force;
        forceAppliedThisFrame = true;
    }

    public void Update(Vector3 forward, Vector3 externalForce, int speed)
    {
        float deltaTime = Raylib.GetFrameTime();

        if (forceAppliedThisFrame)
        {
            WantedForce = Vector3.Lerp(WantedForce, targetForce, 1 - MathF.Exp(-accelerationRate * deltaTime));
        }
        else if (isGrounded && WantedForce.LengthSquared() > 0.0001f)
        {
            Vector3 decay = Vector3.Normalize(WantedForce) * decelerationRate * deltaTime;
            if (WantedForce.Length() <= decay.Length()) WantedForce = Vector3.Zero;
            else WantedForce -= decay;
        }

        Force = isGrounded ? Vector3.Dot(WantedForce, forward) * forward : Vector3.Zero;

        float forwardForce = Vector3.Dot(externalForce, forward) * 20;
        float rotationDirection = MathF.Sign(forwardForce);

        float rotationAngle = MathF.Abs(forwardForce) / Radius * deltaTime;

        if (isGrounded) Rotation.Z += rotationAngle * rotationDirection * (reversed ? 1 : -1) * speed;
        else Rotation.Z += rotationAngle * rotationDirection * (reversed ? 1 : -1) * 330;

        forceAppliedThisFrame = false;
    }
}
