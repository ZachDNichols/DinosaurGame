using Godot;

namespace RecordBound.Scripts;

public partial class AttackNode : Sprite2D
{
	[Export]
	private double _duration = .5;
	[Export]
	private Vector2 _size = new Vector2(1, 1);
	[Export]
	private int _distanceScale = 20;
	private static readonly Vector2 SpawnPosition = new Vector2(1, .1f);
	
	
	public override void _PhysicsProcess(double delta)
	{
		_duration -= delta;
		if (_duration <= 0)
		{
			QueueFree();
		}
	}
	
	public void SetSize(float multiplier)
	{
		_size *= multiplier;
	}
	
	public void SetDuration(double multiplier)
	{
		_duration *= multiplier;
	}

	public void Initialize()
	{
		Position = SpawnPosition * _distanceScale;
		Scale = _size;
	}
	
	
	
}
