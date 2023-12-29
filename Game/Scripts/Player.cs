using Godot;
using System;
using RecordBound.Scripts;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public Direction Direction { get; private set; } = Direction.Up;

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer();
	}

	private void MovePlayer()
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector2 velocity = Velocity;
		
		if (direction != Vector2.Zero)
		{
			velocity = direction.Normalized() * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		if (direction == Vector2.Up)
		{
			Direction = Direction.Up;
		}
		else if (direction == Vector2.Down)
		{
			Direction = Direction.Down;
		}
		else if (direction == Vector2.Right)
		{
			Direction = Direction.Right;
		}
		else if (direction == Vector2.Left)
		{
			Direction = Direction.Left;
		}
			
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			GD.Print(Direction.ToString());
		}
	}
}
