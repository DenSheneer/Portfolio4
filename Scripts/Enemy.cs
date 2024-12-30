using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class Enemy : CharacterBody3D
{
    [Export] private float _speed = 2.5f;
    [Export] private float _gravity = 9.8f;
    [Export] private float _timeToEscape = 1.0f;
    [Export] private NavigationAgent3D agent = null;
    [Export] private Area3D _visionArea = null;
    [Export] private RayCast3D _raycast = null;
    [Export] private AudioStreamPlayer3D _audioPlayer = null;

    private List<Point_Of_Interest> _pointsOfInterest = new List<Point_Of_Interest>();
    public List<Point_Of_Interest> PointsOfInterest { set { _pointsOfInterest = value; } }

    private Node3D _chaseTarget = null;
    private bool _isNavigating = false;
    private bool _isChasing;
    private Task _escapeTask = null;

    public Action OnPlayerHit;

    public override void _Ready()
    {
        agent.TargetReached += FindNewTarget;
    }

    public void OnVisionTimerTimeOut()
    {
        var player = checkForPlayerLOS();
        if (player != null)
        {
            if (!_isChasing)
            {
                chasePlayer(player);
            }
        }
        else if (_isChasing && _escapeTask == null)
        {
            _escapeTask = cancelChase();
        }
    }
    public void BodyEntered(Node3D node)
    {
        if (node is Player)
        {
            _escapeTask?.Dispose();
            OnPlayerHit?.Invoke();
        }
    }

    private Player checkForPlayerLOS()
    {
        var overlaps = _visionArea.GetOverlappingBodies();
        if (overlaps.Count > 0)
        {
            foreach (var element in overlaps)
            {
                if (element is Player)
                {
                    var playerPos = element.GlobalTransform.Origin;
                    _raycast.LookAt(playerPos, Vector3.Up);
                    _raycast.ForceRaycastUpdate();

                    if (_raycast.IsColliding())
                    {
                        var collider = _raycast.GetCollider();
                        if (collider is Player)
                        {
                            return collider as Player;
                        }
                    }
                }
            }
        }
        return null;
    }

    private void chasePlayer(Player player)
    {
        _audioPlayer.Play();
        _isChasing = true;
        _chaseTarget = player;
        agent.TargetReached -= FindNewTarget;
    }

    private async Task cancelChase()
    {
        await Task.Delay((int)_timeToEscape * 1000);

        var player = checkForPlayerLOS();
        if (player == null)
        {
            _isChasing = false;
            agent.TargetReached += FindNewTarget;
            FindNewTarget();
        }
        _escapeTask = null;
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
        if (_isChasing) { agent.TargetPosition = _chaseTarget.GlobalPosition; }

        Vector3 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;
        else
            velocity.Y = 2.0f;

        var nextLocation = agent.GetNextPathPosition();
        var currentLocation = GlobalTransform.Origin;
        var newVelocity = (nextLocation - currentLocation).Normalized() * _speed;

        velocity = velocity.MoveToward(newVelocity, 0.25f);

        LookAt(nextLocation, Vector3.Up);
        Velocity = velocity;
        MoveAndSlide();
    }

    public void TargetPosition(Vector3 target)
    {
        agent.TargetPosition = target;
    }
}