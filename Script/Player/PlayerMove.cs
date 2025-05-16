using Godot;
using System;

public partial class PlayerMove : Node
{
	[Export] private CharacterBody3D player;
	[Export] private CollisionShape3D collider;
	[Export] private Node3D head;
	[Export] private float aheadSpeed = 3.25f;
	[Export] private float backSpeed = 2.75f;
	[Export] private float strafeSpeed = 2.5f;
	[Export] private float sprintSpeedModifier = 1.5f;
	[Export] private float crouchSpeedModifier = 0.5f;
	[Export] private float jumpSpeed = 4.0f;

	[Export] private float crouchHeight = 1f;
	[Export] private float standHeight = 1.85f;
	[Export] private float crouchColliderCenterY = -0.425f;
	[Export] private float standColliderCenterY = 0;
	[Export] private float standHeadHeight = 0.768f;
	[Export] private float crouchHeadHeight = -0.082f;

	private Vector3 velocity;
	private Vector2 moveInput;

	CapsuleShape3D capsule;

	private float currentSpeedModifier;

	// Called when node has entered the scene first time
	public override void _Ready()
	{
		capsule = collider.Shape as CapsuleShape3D;
	}

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

		currentSpeedModifier = 1.0f;
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
		if (Input.IsActionJustPressed("Jump") && player.IsOnFloor() && !Input.IsActionPressed("Crouch"))
		{
			velocity.Y = jumpSpeed;

			GD.Print("Jump");
		}
	}

	// Apply sprint speed when action is activated
	private void ApplySprint()
	{
		if (Input.IsActionPressed("Sprint") && player.IsOnFloor() && moveInput.Y < 0 && !Input.IsActionPressed("Crouch"))
		{
			currentSpeedModifier *= sprintSpeedModifier;

			GD.Print("Sprint");
		}
	}

	// Apply crouch speed and hitbox when action is activated
	private void ApplyCrouch()
	{
		CapsuleShape3D capsule = collider.Shape as CapsuleShape3D;

		if (Input.IsActionPressed("Crouch") && player.IsOnFloor())
		{
			currentSpeedModifier *= crouchSpeedModifier;

			if (capsule.Height != crouchHeight)
			{
				head.Position = new Vector3(head.Position.X, crouchHeadHeight, head.Position.Z);

				capsule.Height = crouchHeight;
				collider.Position = new Vector3(0, crouchColliderCenterY, 0);

				GD.Print("Crouch");
			}
		}
		else if (capsule.Height == crouchHeight)
		{
			head.Position = new Vector3(head.Position.X, standHeadHeight, head.Position.Z);

			capsule.Height = standHeight;
			collider.Position = new Vector3(0, standColliderCenterY, 0);

			GD.Print("Stand");
		}
	}

	// Calculate and apply velocity to character
	private void CalculateVelocity()
	{
		Vector3 aheadMove = Vector3.Forward * moveInput.Y * (moveInput.Y <= 0 ? aheadSpeed : backSpeed) * currentSpeedModifier;
		Vector3 strafeMove = Vector3.Right * moveInput.X * strafeSpeed * currentSpeedModifier;

		Vector3 localMove = aheadMove + strafeMove;

		Vector3 globalMove = player.Transform.Basis * localMove;

		velocity = new Vector3(globalMove.X, velocity.Y, globalMove.Z);

		player.Velocity = velocity;
	}
}
