using Godot;

namespace RecordBound.Scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	public const float Speed = 300.0f;
	[Export]
	private Direction Direction { get; set; } = Direction.Up;
	[Export]
	private PackedScene AttackNode { get; set; }

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
			Attack();
		}
	}

	private void Attack()
	{
		Attack attack = (Attack)AttackNode.Instantiate();
		AddChild(attack);
		attack.Initialize(DirectionExtensions.GetVectorFromDirection(Direction), DirectionExtensions.GetRotationFromDirection(Direction));
	}
}
