using Godot;
using System;

public partial class GautcherRoot : CharacterBody3D
{
    [Export] public CharacterBody3D player;
    [Export] private GautcherMove move;
    [Export] private GautcherLook look;
    [Export] private float spotDistance;

    private enum GautcherState { Patrol, Chase }

    private GautcherState state = GautcherState.Patrol;

    public override void _PhysicsProcess(double delta)
    {
        ChangeState();
        UpdateState(delta);
    }

    private void ChangeState()
    {
        if (player.Position.DistanceTo(GlobalPosition) <= 1)
        {
            GetTree().ReloadCurrentScene();
        }
        else if (player.Position.DistanceTo(GlobalPosition) <= spotDistance)
        {
            state = GautcherState.Chase;
        }
        else
        {
            state = GautcherState.Patrol;
        }
    }

    private void UpdateState(double delta)
    {
        switch (state)
        {
            case GautcherState.Patrol:
                move.Patrol(delta);
                break;
            case GautcherState.Chase:
                move.ChasePlayer();
                look.LookPlayer();
                break;
        }
    }
}
