using Godot;
using System;

public class Bullet : RigidBody
{
    private float speed = 50f;
    private float damage = 50f;

    private void onBodyEntered(Node body)
    {
        if (GameManager.instance.IsEnemy(body as KinematicBody))
        {
            GD.Print("Enemy Hit!");
            EnemyAgent e = body as EnemyAgent;
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
