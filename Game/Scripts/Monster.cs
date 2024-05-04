using Godot;
using System.Linq;

namespace RecordBound.Scripts;

public partial class Monster : CharacterBody2D
{
	[Export]
	private const float Speed = 200.0f;
	[Export]
	private NavigationAgent2D _navigationAgent;
	[Export]
	private Timer _timer;
	private static Player _player = Player.Instance;

	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_timer = GetNode<Timer>("Timer");
		
		_timer.Timeout += OnTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 dir = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		Velocity = dir * Speed;

		MoveAndSlide();

		if (_navigationAgent.TargetPosition == _player.GlobalPosition)
		{
			GD.Print("Player Found!");
		}
	}

	private void MakePath()
	{
		if (_player != null)
		{
			_navigationAgent.TargetPosition = _player.GlobalPosition;
		}
	}

	private void OnTimerTimeout()
	{
		MakePath();
	}
}
