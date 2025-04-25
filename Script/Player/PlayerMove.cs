using Godot;
using System;

public partial class PlayerMove : Node
{
	[Export] private CharacterBody3D player;
	[Export] private float aheadSpeed = 3.25f;
	[Export] private float backSpeed = 2.75f;
	[Export] private float strafeSpeed = 2.5f;
	[Export] private float sprintModifier = 1.5f;
	[Export] private float crouchModifier = 0.5f;
	[Export] private float jumpVelocity = 4.0f;

	private Vector3 velocity;
	private Vector2 moveInput;

	private float currentSpeedModifiers;

	// Update synced with physics
	public override void _PhysicsProcess(double delta)
	{
		ResetVariables();

		UpdateMoveInput();

		ApplyGravity(delta);

		ApplyJump();

		ApplySprint();

		ApplyCrouch();

		CalculateVelocity();

		player.MoveAndSlide();
	}

	// Resets variables to default values each frame synced with physics
	private void ResetVariables()
	{
		velocity = player.Velocity;

		moveInput = Vector2.Zero;

		currentSpeedModifiers = 1.0f;
	}

	// Get movement direction from input
	private void UpdateMoveInput()
	{
		if (Input.IsActionPressed("MoveAhead"))
		{
			moveInput.Y = -1;

			GD.Print("MoveAhead");
		}

		if (Input.IsActionPressed("MoveBack"))
		{
			moveInput.Y = 1;

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

		if (moveInput.Length() > 1)
			moveInput = moveInput.Normalized();
	}

	// Apply gravity to character
	private void ApplyGravity(double delta)
	{
		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)delta;
		}
	}

	// Apply jump force when action is activated 
	private void ApplyJump()
	{
		if (Input.IsActionJustPressed("Jump") && player.IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}
	}

	// Apply sprint speed when action is activated
	private void ApplySprint()
	{
		if (Input.IsActionPressed("Sprint") && player.IsOnFloor() && moveInput.Y < 0 && !Input.IsActionPressed("Crouch"))
		{
			currentSpeedModifiers *= sprintModifier;

			GD.Print("Sprint");
		}
	}

	// Apply crouch speed and hitbox when action is activated
	private void ApplyCrouch()
	{
		if (Input.IsActionPressed("Crouch") && player.IsOnFloor() && !Input.IsActionPressed("Sprint"))
		{
			currentSpeedModifiers *= crouchModifier;

			GD.Print("Crouch");
		}
	}

	// Calculate and apply velocity to character
	private void CalculateVelocity()
	{
		Vector3 forwardMove = Vector3.Forward * moveInput.Y * (moveInput.Y >= 0 ? aheadSpeed : backSpeed) * currentSpeedModifiers;
		Vector3 strafeMove = Vector3.Right * moveInput.X * strafeSpeed * currentSpeedModifiers;

		Vector3 localMove = forwardMove + strafeMove;

		Vector3 globalMove = player.Transform.Basis * localMove;

		velocity = new Vector3(globalMove.X, velocity.Y, globalMove.Z);

		player.Velocity = velocity;
	}
}
