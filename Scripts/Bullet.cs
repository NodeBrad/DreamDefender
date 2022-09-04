using Godot;
using System;

public class Bullet : RigidBody
{
    private float speed = 50f;
    private float damage = 50f;
    private bool initialised = false;
    private Vector3 spawnPosition = Vector3.Zero;

    private void onBodyEntered(Node body)
    {
        if (GameManager.instance.IsEnemy(body as KinematicBody))
        {
            EnemyAgent enemy = body as EnemyAgent;
            enemy.TakeDamage(damage);
            QueueFree();
        }
        else
        {
            QueueFree();
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        ApplyImpulse(Transform.basis.z, -Transform.basis.z * speed);

        if (initialised)
            if (GlobalTranslation.DistanceTo(spawnPosition) > 5)
                QueueFree();


    }

    public void Initialise(Vector3 spawnPosition, Vector3 targetPosition, float damage)
    {
        GlobalTranslation = spawnPosition;
        this.spawnPosition = spawnPosition;
        LookAt(targetPosition, Vector3.Up);
        this.damage = damage;
        initialised = true;
    }
}
