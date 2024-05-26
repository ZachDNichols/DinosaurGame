using Godot;
using Vector2 = Godot.Vector2;

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

	private Knockback _knockback = new();
	
	
	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_timer = GetNode<Timer>("Timer");
		
		_timer.Timeout += OnTimerTimeout;
		
		_randomVector = new Vector2((float)GD.RandRange(-_distanceToPlayer, _distanceToPlayer), (float)GD.RandRange(-_distanceToPlayer, _distanceToPlayer));

		_navigationAgent.PathPostprocessing = NavigationPathQueryParameters2D.PathPostProcessing.Corridorfunnel;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 dir = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		Velocity = dir * Speed + _knockback.Direction;
		
		MoveAndSlide();
		_knockback.DecreaseKnockBack();
		
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
		if (Player != null && Player.Velocity != Vector2.Zero)
		{
			if (!_isCloseToPlayer)
			{
				_navigationAgent.TargetPosition = Player.GlobalPosition + _randomVector;

				if (_navigationAgent.GetNextPathPosition() > Player.GlobalPosition)
				{
					_navigationAgent.TargetPosition = Player.GlobalPosition;
				}
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
		_knockback.SetDirection(direction);

	}
}
