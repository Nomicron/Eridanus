using Godot;

public partial class FollowPlayer : Node
{
    [Export] private CharacterBody3D gautcher;
    [Export] private GautcherRoot root;

    public void ChasePlayer()
    {
        gautcher.Velocity = -gautcher.Transform.Basis.Z * 2.5f;
        gautcher.MoveAndSlide();
    }
}
