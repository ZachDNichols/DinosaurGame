using Godot;
using Vector2 = Godot.Vector2;

namespace RecordBound.Scripts;

public partial class Monster : CharacterBody2D
{
	[Export]
	private const float Speed = 100.0f;
	[Export]
	private NavigationAgent2D _navigationAgent;
	[Export]
	private Timer _timer;
	private static readonly Player Player = Player.Instance;
	private Vector2 _randomVector;
	private bool _isCloseToPlayer;
	[Export]
	private float _distanceToPlayer = 50f;
	[Export]
	private float _vitality = 25f;
	[Export]
	private float _knockbackForce = 300f;
	[Export]
	private const float KnockbackCooldown = 0.03f;
	private Vector2 _knockback;
	
	
	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_timer = GetNode<Timer>("Timer");
		
		_timer.Timeout += OnTimerTimeout;
		
		_randomVector = new Vector2((float)GD.RandRange(-_distanceToPlayer, _distanceToPlayer), (float)GD.RandRange(-_distanceToPlayer, _distanceToPlayer));
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 dir = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		Velocity = dir * Speed + _knockback;
		
		MoveAndSlide();
		_knockback = _knockback.Lerp(Vector2.Zero, KnockbackCooldown);
		GD.Print(_knockback);
		
		if (Position.DistanceTo(Player.GlobalPosition) < _distanceToPlayer + 20 && !_isCloseToPlayer)
		{
			_isCloseToPlayer = true;
			MakePath();	
		}
		else if (Position.DistanceTo(Player.GlobalPosition) > _distanceToPlayer + 20 && _isCloseToPlayer)
		{
			_isCloseToPlayer = false;
			MakePath();
		}
	}

	private void MakePath()
	{
		if (Player != null)
		{
			if (!_isCloseToPlayer)
			{
				_navigationAgent.TargetPosition = Player.GlobalPosition + _randomVector;
			}
			else
			{
				_navigationAgent.TargetPosition = Player.GlobalPosition;
			}
		}
	}

	private void OnTimerTimeout()
	{
		MakePath();
	}

	public void TakeDamage()
	{
		_vitality -= 5;
		
		if (_vitality <= 0)
		{
			QueueFree();
			return;
		}
		
		Vector2 direction = Player.GlobalPosition.DirectionTo(GlobalPosition);
		_knockback = direction * _knockbackForce;

	}
}
