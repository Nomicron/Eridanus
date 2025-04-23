using Godot;
using System;
using System.Diagnostics;

public partial class CharacterBody3d : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export]
    public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    //public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;
        if (Input.IsActionPressed("move_left"))
        direction.X -= 1.0f;
        if (Input.IsActionPressed("move_right"))
        direction.X += 1.0f;

                // Ground velocity
        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        // Moving the character
        Velocity = _targetVelocity;
        MoveAndSlide();
        if (MoveAndSlide() == true)
        {
            GD.Print("1");
        }
    }
}
