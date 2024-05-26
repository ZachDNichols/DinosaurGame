using Godot;

public class Knockback
{
    public Vector2 Direction = Vector2.Zero;
    [Export]
    private float _force = 300f;
    [Export]
    const float Cooldown = 0.03f;
    
    public void DecreaseKnockBack()
    {
        Direction = Direction.Lerp(Vector2.Zero, Cooldown);
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction * _force;
    }
}