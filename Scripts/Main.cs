using Godot;
using System;

public partial class Main : Node3D
{
	[Export] Node3D target;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetTree().CallGroup("Enemy", "TargetPosition", target.GlobalTransform.Origin);
	}
}
