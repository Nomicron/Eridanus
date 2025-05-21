using Godot;
using System;

public partial class PlayerTestScreen : Node
{
    private bool windowed;

    [Export] private CharacterBody3D gautcher;
    [Export] private Node3D scene;

    private float spawnTimer;
    private float spawnDelay = 10;

    public override void _EnterTree()
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

        windowed = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (spawnTimer >= spawnDelay)
        {
            spawnTimer = 0;
            CharacterBody3D gautcherDup = gautcher.Duplicate() as CharacterBody3D;
            gautcherDup.Position += Vector3.Back * 2.5f;
            scene.AddChild(gautcherDup);
        }
        else
        {
            spawnTimer += (float)delta;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ToggleFullscreen"))
        {
            if (windowed)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                windowed = false;
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                windowed = true;
            }
        }
    }
}
