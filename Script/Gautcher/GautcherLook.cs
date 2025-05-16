using Godot;

public partial class GautcherLook : Node
{
    [Export] private Node3D gautcher;
    [Export] private GautcherRoot root;

    public void LookPlayer()
    {
        gautcher.LookAt(root.player.Position);
    }
}
