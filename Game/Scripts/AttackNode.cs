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
	private const string MonsterTag = "monster";
	[Export] private Area2D _area2D;
	
	public override void _PhysicsProcess(double delta)
	{
		_duration -= delta;
		if (_duration <= 0)
		{
			QueueFree();
		}
	}

	private void SetSize(float multiplier)
	{
		_size *= multiplier;
	}

	private void SetDuration(double multiplier)
	{
		_duration *= multiplier;
	}

	public void Initialize()
	{
		_area2D.Monitoring = true;
		_area2D.BodyEntered += BodyEntered;
		Position = SpawnPosition * _distanceScale;
	}	
	
	private void BodyEntered(Node2D body)
	{
		if (body.IsInGroup(MonsterTag))
		{
			Monster monster = body as Monster;
			monster?.TakeDamage();
		}
		
		_duration = .2f;
	}
}


