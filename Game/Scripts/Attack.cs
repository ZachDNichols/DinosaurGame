using Godot;
using System;
using RecordBound.Scripts;

public partial class Attack : Sprite2D
{
	public void Initialize(Vector2 position, float rotation)
	{
		Position += position;
		Rotation = rotation;
		GD.Print(Rotation);
	}
}
