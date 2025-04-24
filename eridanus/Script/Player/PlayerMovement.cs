using Godot;

public partial class PlayerMovement : CharacterBody3D
{
	[Export] private float Speed = 5.0f;
	[Export] private float JumpVelocity = 4.5f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector3 direction = Vector3.Zero;

	    if (Input.IsActionPressed("MoveAhead"))
		{
			direction.Y = 1;
		}

	    if (Input.IsActionPressed("MoveBack"))
		{
			direction.Y = -1;
		}

	    if (Input.IsActionPressed("MoveLeft"))
		{
			direction.X = -1;
		}

	    if (Input.IsActionPressed("MoveRight"))
		{
			direction.X = 1;

			GD.Print("Right");
		}

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
