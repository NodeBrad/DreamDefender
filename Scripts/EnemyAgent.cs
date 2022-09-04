using Godot;
using System;
using Godot.Collections;
using System.Security.AccessControl;

public class EnemyAgent : KinematicBody
{
    private Vector3 velocity = Vector3.Zero;
    private float speed = 4f;
    private float health = 200f;
    private int goldOnDeath = 0;
    private NavigationAgent navAgent;
    private RayCast ray;

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent>("NavigationAgent");
        ray = GetNode<RayCast>("RayCast");
    }
    public void Initialise(Vector3 spawnPosition, float health, float speed, int goldOnDeath, Vector3 targetPos)
    {
        GameManager.instance.AddEnemy(this);
        GlobalTranslation = spawnPosition;
        this.health = health;
        this.speed = speed;
        this.goldOnDeath = goldOnDeath;
        navAgent.SetTargetLocation(targetPos);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 currentPos = GlobalTransform.origin;
        Vector3 target = navAgent.GetNextLocation();
        Vector3 normal = ray.GetCollisionNormal();
        Vector3 absNormal = new Vector3(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));
        Vector3 invFloorNormal = new Vector3(1, 1, 1) - absNormal;
        velocity = ((target - currentPos) * invFloorNormal).Normalized() * speed;
        navAgent.SetVelocity(velocity);

        if (navAgent.IsTargetReached())
        {
            GameManager.instance.RemoveLife();
            GameManager.instance.RemoveEnemy(this);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            GameManager.instance.AddGold(goldOnDeath);
            GameManager.instance.RemoveEnemy(this);
        }
    }

    private void onVelocityComputed(Vector3 safeVelocity)
    {
        MoveAndSlide(safeVelocity, Vector3.Up);
    }
}
