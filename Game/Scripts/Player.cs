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

	private double _attackCooldown = 0.5;
	private double _attackCooldownDuration = 0.5;
	private bool _isAttacking;

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer();
		LookAt(GetGlobalMousePosition());
		if (_isAttacking)
		{
			_attackCooldown -= delta;
			if (_attackCooldown <= 0)
			{
				_attackCooldown = _attackCooldownDuration;
				_isAttacking = false;
			}
		}
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

	private void Attack()
	{
		if (_isAttacking)
		{
			return;
		}
		
		_isAttacking = true;
		AttackNode attack = (AttackNode)AttackNode.Instantiate();
		AddChild(attack);
		attack.Initialize();
	}
}
