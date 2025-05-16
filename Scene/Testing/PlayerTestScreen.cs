using Godot;
using System;

public partial class PlayerTestScreen : Node
{
    private bool windowed;

    public override void _EnterTree()
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

        windowed = false;
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
