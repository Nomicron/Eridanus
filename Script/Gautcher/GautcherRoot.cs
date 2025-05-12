using Godot;
using System;

public partial class GautcherRoot : CharacterBody3D
{
    [Export] internal CharacterBody3D player;
    [Export] private FollowPlayer followPlayer;
    [Export] private LookAtPlayer lookPlayer;

    private enum GautcherState { Idle, Chase }

    private GautcherState state = GautcherState.Idle;

    public override void _PhysicsProcess(double delta)
    {
        if (player.Position.DistanceTo(GlobalPosition) < 5)
        {
            followPlayer.ChasePlayer();
            lookPlayer.LookPlayer();
        }
    }
}
