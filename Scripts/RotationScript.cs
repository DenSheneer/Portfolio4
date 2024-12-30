using Godot;
using System;

public partial class RotationScript : MeshInstance3D
{
	[Export] float rotationSpeed = 1.0f;
	public override void _Process(double delta)
	{
		RotateY(rotationSpeed * (float)delta);
	}
}
