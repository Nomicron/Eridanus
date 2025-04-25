using Godot;

public partial class Movement : CharacterBody3D
{
	[Export] public int Speed { get; set; } = 14;

	private Vector3 _targetVelocity = Vector3.Zero;
	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;
		if (Input.IsActionPressed("MoveLeft"))
			direction.X -= 1.0f;
		if (Input.IsActionPressed("MoveRight"))
			direction.X += 1.0f;

		// Ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();
		if (MoveAndSlide() == true)
		{
			GD.Print("1");
		}
	}
}
