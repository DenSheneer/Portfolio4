using Godot;
using System;

public partial class Coin : Node3D
{
	public Action OnCollected;

	[Export] CollisionShape3D collider;
 
    public void body_entered(Node3D node)
    {
        if (node is Player)
        {
            OnCollected?.Invoke();
            QueueFree();
        }
    }
}
