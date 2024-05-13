using System;
using Godot;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
	private static Player _player = Player.Instance;
	private Vector2 _randomVector;
	private bool _isCloseToPlayer = false;
	[Export]
	private float _distanceToPlayer = 50f;
	[Export]
	private float _vitality = 25f;
	private const string PlayerAttackTag = "player_attack";
	[Export]
	private float _knockbackForce = 300f;

	private float _knockbackDuration;
	private const float KnockbackCooldown = 1f;
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
		_knockback = _knockback.Lerp(Vector2.Zero, 0.03f);
		GD.Print(_knockback);
		
		if (Position.DistanceTo(_player.GlobalPosition) < _distanceToPlayer + 20 && !_isCloseToPlayer)
		{
			_isCloseToPlayer = true;
			MakePath();	
		}
		else if (Position.DistanceTo(_player.GlobalPosition) > _distanceToPlayer + 20 && _isCloseToPlayer)
		{
			_isCloseToPlayer = false;
			MakePath();
		}
	}

	private void MakePath()
	{
		if (_player != null)
		{
			if (!_isCloseToPlayer)
			{
				_navigationAgent.TargetPosition = _player.GlobalPosition + _randomVector;
			}
			else
			{
				_navigationAgent.TargetPosition = _player.GlobalPosition;
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
		
		Vector2 direction = _player.GlobalPosition.DirectionTo(GlobalPosition);
		_knockback = direction * _knockbackForce;

	}
}
