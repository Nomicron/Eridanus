using Godot;
using System;

public partial class LookAtPlayer : Node
{
    [Export] private Node3D Gautcher;
    [Export] private GautcherRoot Root;
    public override void _PhysicsProcess(double delta)
    {
        Gautcher.LookAt(Root.Player.Position);
        Gautcher.Rotate(Vector3.Up, Mathf.Pi);
    }
}
