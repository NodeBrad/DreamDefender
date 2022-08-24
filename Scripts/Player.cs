using Godot;
using System;

public class Player : KinematicBody
{
    private int maxSpeed = 12;
    private int acceleration = 60;
    private int friction = 50;
    private int airFriction = 10;
    private int jumpImpulse = 15;
    private int gravity = -40;

    private float mouseSensitivity = 0.1f;
    private Vector3 velocity = Vector3.Zero;
    private Vector3 snapVector = Vector3.Zero;
    private Spatial head;
    private RayCast ray;
    private DebugSphere ds;
    private bool canPlace = false;

    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");
        ray = GetNode<RayCast>("Head/RayCast");
        ds = GetNode<DebugSphere>("DebugSphere");

        Input.MouseMode = Input.MouseModeEnum.Captured; // Capture the mouse
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Handle mouse window capture
        if (@event.IsActionPressed("left_click")) // If player clicks in the window
        {
            Input.MouseMode = Input.MouseModeEnum.Captured; // Capture the mouse
        }

        // Toggle window capture
        if (@event.IsActionPressed("toggle_mouse_mode"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                Input.MouseMode = Input.MouseModeEnum.Visible;
            else
                Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if ((@event is InputEventMouseMotion mouseMotion) && (Input.MouseMode == Input.MouseModeEnum.Captured))
        {
            RotateY(Mathf.Deg2Rad(-mouseMotion.Relative.x * mouseSensitivity));
            head.RotateX(Mathf.Deg2Rad(-mouseMotion.Relative.y * mouseSensitivity));
            //head.Rotation.x = Mathf.Clamp(head.Rotation.x, Mathf.Deg2Rad(-89), Mathf.Deg2Rad(89));
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 inputVector = getInputVector();
        Vector3 direction = getDirection(inputVector);
        applyMovement(direction, delta);
        applyFriction(direction, delta);
        applyGravity(delta);
        jump();
        velocity = MoveAndSlide(velocity, Vector3.Up);

        checkPlacement();
        manageBuilding();
    }

    private Vector3 getInputVector()
    {
        Vector3 inputVector = Vector3.Zero;
        inputVector.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        inputVector.z = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");

        if (inputVector.Length() > 1)
            return inputVector.Normalized();
        else
            return inputVector;
    }

    private Vector3 getDirection(Vector3 inputVector)
    {
        Vector3 direction = Vector3.Zero;
        direction = (inputVector.x * Transform.basis.x) + (inputVector.z * Transform.basis.z);
        return direction;
    }

    private void applyMovement(Vector3 direction, float delta)
    {
        if (direction != Vector3.Zero)
        {
            velocity.x = velocity.MoveToward(direction * maxSpeed, acceleration * delta).x;
            velocity.z = velocity.MoveToward(direction * maxSpeed, acceleration * delta).z;
        }
    }

    private void applyFriction(Vector3 direction, float delta)
    {
        if (direction == Vector3.Zero)
        {
            if (IsOnFloor())
                velocity = velocity.MoveToward(Vector3.Zero, friction * delta);
            else
            {
                velocity.x = velocity.MoveToward(direction * maxSpeed, airFriction * delta).x;
                velocity.z = velocity.MoveToward(direction * maxSpeed, airFriction * delta).z;
            }
        }
    }

    private void applyGravity(float delta)
    {
        velocity.y += gravity * delta;
        velocity.y = Mathf.Clamp(velocity.y, gravity, jumpImpulse);
    }

    private void jump()
    {
        if ((Input.IsActionJustPressed("jump")) && (IsOnFloor()))
        {
            velocity.y = jumpImpulse;
        }
        if ((Input.IsActionJustReleased("jump")) && (velocity.y > jumpImpulse / 2))
        {
            velocity.y = jumpImpulse / 2;
        }
    }

    private void checkPlacement()
    {
        if (ray.GetCollider() != null)
        {
            ds.GlobalTranslation = ray.GetCollisionPoint();
            if (ds.isOnFloor())
            {
                ds.Visible = true;
                canPlace = true;
            }
        }
        else
        {
            ds.Visible = false;
            canPlace = false;
        }
    }

    private void manageBuilding()
    {
        if ((Input.IsActionJustPressed("interact")) && canPlace)
        {
            GameManager.instance.PlaceTurret(ds.GlobalTranslation);
        }
    }

}
