using Godot;
using System;

public partial class LookAtPlayer : Node
{
    [Export] private Node3D gautcher;
    [Export] private GautcherRoot root;

    public void LookPlayer()
    {
        gautcher.LookAt(root.player.Position);
    }
}
