using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    [Export] private float _speed = 2.5f;
    [Export] private float _gravity = 9.8f;
    [Export] private NavigationAgent3D agent = null;

    public override void _Ready()
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;
        else
            velocity.Y = 2.0f;

        var nextLocation = agent.GetNextPathPosition();
        var currentLocation = GlobalTransform.Origin;
        var newVelocity = (nextLocation - currentLocation).Normalized() * _speed;
        //GD.Print($"next location: {nextLocation} - currentLocation {currentLocation} * speed  = {newVelocity}");

        velocity = velocity.MoveToward(newVelocity, 0.25f);

        Velocity = velocity;
        MoveAndSlide();

        //GD.Print($"Velocity: {Velocity}, moving towards: {agent.TargetPosition}");
    }

    public void TargetPosition(Vector3 target)
    {
        agent.TargetPosition = target;
    }
}