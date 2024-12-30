using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody3D
{
    public const float JumpVelocity = 4.5f;

    [Export] private float _runSpeed = 5.0f;
    [Export] private float _walkSpeed = 3.0f;
    [Export] private Node3D _vfx = null;
    [Export] private AnimationPlayer _animationPlayer = null;

    private float _currentSpeed = 0.0f;
    private bool _isRunning = false;
    private bool _isLocked = false;

    public override void _Ready()
    {
        //Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_animationPlayer.IsPlaying())
        {
            _isLocked = false;
        }

        _isRunning = Input.IsActionPressed("sprint");
        _currentSpeed = _isRunning ? _runSpeed : _walkSpeed;

        if (Input.IsActionJustPressed("kick"))
        {
            if (_animationPlayer.CurrentAnimation != "kick")
                _animationPlayer.Play("kick");

            _isLocked = true;
        }

        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            if (!_isLocked)
            {
                if (_isRunning)
                {
                    if (_animationPlayer.CurrentAnimation != "running")
                        _animationPlayer.Play("running");
                }
                else
                {
                    if (_animationPlayer.CurrentAnimation != "walking")
                        _animationPlayer.Play("walking");
                }

                _vfx.LookAt(Position - direction);

                velocity.X = direction.X * _currentSpeed;
                velocity.Z = direction.Z * _currentSpeed;
            }
        }
        else
        {
            if (_animationPlayer.CurrentAnimation != "idle")
                _animationPlayer.Play("idle");

            velocity.X = Mathf.MoveToward(Velocity.X, 0, _currentSpeed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _currentSpeed);
        }

        Velocity = velocity;

        if (!_isLocked)
        {
            MoveAndSlide();
        }
    }
}
