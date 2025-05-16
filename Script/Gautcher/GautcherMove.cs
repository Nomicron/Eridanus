using System;
using Godot;

public partial class GautcherMove : Node
{
    [Export] private CharacterBody3D gautcher;
    [Export] private GautcherRoot root;

    private Random rnd = new Random();
    private float timer;
    private float direction;

    public void ChasePlayer()
    {
        gautcher.Velocity = -gautcher.Transform.Basis.Z * 2.5f;
        gautcher.MoveAndSlide();
    }

    public void Patrol(double delta)
    {
        if (timer <= 0)
        {
            timer = rnd.Next(1, 5);
            direction = rnd.Next(0, 360);
        }
        else
        {
            root.RotationDegrees = direction * Vector3.Up;
            gautcher.Velocity = -gautcher.Transform.Basis.Z * 1.25f;
            gautcher.MoveAndSlide();
        }

        timer -= (float)delta;
    }
}
