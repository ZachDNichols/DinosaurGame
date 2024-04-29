using Godot;

namespace RecordBound.Scripts;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static float GetRotationFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Mathf.Pi * 3 / 2;
            case Direction.Down: 
                return Mathf.Pi / 2;
            case Direction.Left:
                return Mathf.Pi;
            case Direction.Right:
                return 0;
            default:
                return 0;
        }
    }
    
    public static Vector2 GetVectorFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2(0, -9);
            case Direction.Down:
                return new Vector2(0, 9);
            case Direction.Left:
                return new Vector2(-9, 0);
            case Direction.Right:
                return new Vector2(9, 0);
            default:
                return Vector2.Zero;
        }
    }
}