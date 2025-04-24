using Godot;
using System;

public partial class PlayerLook : Node
{
    [Export] private CharacterBody3D player;
    [Export] private float mouseSensitivity;
}
