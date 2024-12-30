using Godot;
using System;

public partial class Coin : Node3D
{
	public Action OnCollected;

	[Export] CollisionShape3D collider;
    public override void _Ready()
    {

    }
    public override void _Process(double delta)
	{
	}

    public void body_entered(Node3D node)
    {
        if (node is Player)
        {
            OnCollected?.Invoke();
            QueueFree();
        }
    }
}
