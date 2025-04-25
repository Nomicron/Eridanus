using Godot;
using System;

public partial class PlayerLook : Node
{
    [Export] private CharacterBody3D player;
    [Export] private Node3D head;
    [Export] private float mouseSensitivity = 0.1f;
    [Export] private float minXRotation = -45;
    [Export] private float maxXRotation = 50;

    private Vector2 lookInput;

    // Is called when node has entered scene
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Is called when an input is recognized inside the program
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            player.RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * mouseSensitivity));
            head.RotateX(Mathf.DegToRad(eventMouseMotion.Relative.Y * mouseSensitivity));
            head.Rotation = Mathf.Clamp(head.Rotation.X, Mathf.DegToRad(minXRotation), Mathf.DegToRad(maxXRotation)) * Vector3.Right;
        }
    }

    // Reset mouse mode when exiting
    public override void _ExitTree()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;  
    }
}
