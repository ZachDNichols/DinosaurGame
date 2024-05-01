using System;
using System.Threading.Tasks;
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

	private bool _isAttacking = false;

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer();
		LookAt(GetGlobalMousePosition());
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
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			Attack();
		}
	}

	private async void Attack()
	{
		if (_isAttacking)
		{
			return;
		}
		_isAttacking = true;
		Sprite2D attack = (Sprite2D)AttackNode.Instantiate();
		AddChild(attack);
		attack.Position = new Vector2 (1, .1f) * 20;
		await Task.Delay(TimeSpan.FromSeconds(1));
		RemoveChild(attack);
		attack.QueueFree();
		_isAttacking = false;
	}
}
