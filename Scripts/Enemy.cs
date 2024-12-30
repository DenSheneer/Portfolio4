using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody3D
{
    [Export] private float _speed = 2.5f;
    [Export] private float _gravity = 9.8f;
    [Export] private NavigationAgent3D agent = null;

    private List<Point_Of_Interest> _pointsOfInterest = new List<Point_Of_Interest>();
    public List<Point_Of_Interest> PointsOfInterest { set { _pointsOfInterest = value; } }
    private bool _isNavigating = false;

    public override void _Ready()
    {
        agent.TargetReached += FindNewTarget;

    }

    public async void FindNewTarget()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);   // wait 1 frame

        if (_pointsOfInterest.Count > 0)
        {
            Random rnd = new Random();
            agent.TargetPosition = _pointsOfInterest[rnd.Next(_pointsOfInterest.Count)].GlobalPosition;
        }

        _isNavigating = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_isNavigating) { return; }

        Vector3 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;
        else
            velocity.Y = 2.0f;

        var nextLocation = agent.GetNextPathPosition();
        var currentLocation = GlobalTransform.Origin;
        var newVelocity = (nextLocation - currentLocation).Normalized() * _speed;

        velocity = velocity.MoveToward(newVelocity, 0.25f);

        Velocity = velocity;
        MoveAndSlide();
    }

    public void TargetPosition(Vector3 target)
    {
        agent.TargetPosition = target;
    }
}