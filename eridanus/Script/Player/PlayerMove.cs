using Godot;
using System;

public partial class PlayerMove : Node
{
	[Export] private CharacterBody3D player;
	[Export] private Node3D head;
	[Export] private float speed = 5.0f;
	[Export] private float jumpVelocity = 4.5f;

	private Vector3 velocity;
	private Vector2 moveInput;
	private Vector3 direction;

    //Update synced with program framerate
    public override void _Process(double delta)
    {
        base._Process(delta);
    }

	//Update synced with physics
	public override void _PhysicsProcess(double delta)
	{
		velocity = player.Velocity;

		direction = Vector3.Zero;

		UpdateDirection();

		UpdateGravity(delta);

		UpdateJump();

		CalculateVelocity();

		player.MoveAndSlide();
	}

    //Get movement direction from input
    private void UpdateDirection()
    {
	    if (Input.IsActionPressed("MoveAhead"))
		{
			moveInput.Y = 1;

			GD.Print("MoveAhead");
		}

	    if (Input.IsActionPressed("MoveBack"))
		{
			moveInput.Y = -1;

			GD.Print("MoveBack");
		}

	    if (Input.IsActionPressed("MoveLeft"))
		{
			moveInput.X = 1;

			GD.Print("MoveLeft");
		}

	    if (Input.IsActionPressed("MoveRight"))
		{
			moveInput.X = -1;

			GD.Print("MoveRight");
		}
    }

	//Apply gravity to character
	private void UpdateGravity(double delta)
    {
		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)delta;
		}
    }

	//Apply jump force when action is pressed 
    private void UpdateJump()
    {
		if (Input.IsActionJustPressed("Jump") && player.IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}
    }

	//Calculate and apply velocity to character
    private void CalculateVelocity()
    {
		direction = Vector3.Zero;

		direction = (player.Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(player.Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, speed);
		}

		player.Velocity = velocity;
    }

}
