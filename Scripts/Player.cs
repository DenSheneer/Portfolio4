using Godot;
using System;

public partial class Player : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    [Export] public float _lookSenitivity = 2.0f;
    [Export] private Node3D _cameraMount = null;
    [Export] private AnimationPlayer _animationPlayer = null;


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _lookSenitivity *= 0.1f;
    }

    public override void _Input(InputEvent inputEvent)
    {

        if (inputEvent is InputEventMouseMotion)
        {
            InputEventMouseMotion inputEventMouseMotion = (InputEventMouseMotion)inputEvent;
            float radiansY = Mathf.Pi * 0.00555f * -inputEventMouseMotion.Relative.X * _lookSenitivity;
            RotateY(radiansY);

            float radiansX = Mathf.Pi * 0.00555f * -inputEventMouseMotion.Relative.Y * _lookSenitivity;
            _cameraMount.RotateX(radiansX);
            _cameraMount.RotationDegrees = new Vector3(Mathf.Clamp(_cameraMount.RotationDegrees.X, -90.0f, 90.0f), _cameraMount.RotationDegrees.Y, _cameraMount.RotationDegrees.Z);

        }

    }


    public override void _PhysicsProcess(double delta)
    {
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
            if (_animationPlayer.CurrentAnimation != "walking")
                _animationPlayer.Play("walking");


            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            if (_animationPlayer.CurrentAnimation != "idle")
                _animationPlayer.Play("idle");

            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}