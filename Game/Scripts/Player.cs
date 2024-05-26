using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private const float Speed = 300.0f;
	[Export] private PackedScene AttackNode { get; set; }
	[Export] private Vector2 CameraZoom { get; set; } = new Vector2(3, 3);

	private double _attackCooldown = 0.5;
	private double _attackCooldownDuration = 0.5;
	private bool _isAttacking;
	public static Player Instance { get; private set; }
	private Knockback _knockback = new();
	[Export] private int _vitality = 25;

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer(delta);
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
	
	public override void _Ready()
	{
		_knockback.Direction = Vector2.Zero;
		Instance = this;
	}

	private void MovePlayer(double delta)
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

		Velocity = velocity + _knockback.Direction;
		
		_knockback.DecreaseKnockBack();
		KinematicCollision2D collision2D = MoveAndCollide(Velocity * (float)delta);
		if (collision2D != null)
			ReactToCollision(collision2D);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			Attack();
		}
	}
	
	private void ReactToCollision(KinematicCollision2D collision)
	{
		if (collision.GetCollider() != null && collision.GetCollider() is Monster monster)
		{
			TakeDamage(monster);
		}
	}

	private void TakeDamage(Monster monster)
	{
		_vitality -= 2;

		if (_vitality < 0)
		{
			GD.Print("Game over!");
		}
		
		Vector2 direction = monster.GlobalPosition.DirectionTo(GlobalPosition);
		_knockback.SetDirection(direction);
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
