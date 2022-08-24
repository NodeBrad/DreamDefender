using Godot;
using System;

public class Bullet : RigidBody
{
    private float speed = 10f;
    private float damage = 50f;

    private void onBodyEntered(Node body)
    {
        Node bodyRoot = body.GetParent().GetParent().GetParent();
        if (GameManager.instance.isEnemy(bodyRoot as Spatial))
        {
            GD.Print("Enemy Hit!");
            Enemy e = bodyRoot as Enemy;
            e.TakeDamage(damage);
            QueueFree();
        }
        else
        {
            GD.Print("Something Else Hit! " + body);
            QueueFree();
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        ApplyImpulse(Transform.basis.z, -Transform.basis.z * speed);
    }

    public void Initialise(Vector3 spawnPosition, Vector3 targetPosition, float damage)
    {
        GlobalTranslation = spawnPosition;
        LookAt(targetPosition, Vector3.Up);
        this.damage = damage;
    }
}
