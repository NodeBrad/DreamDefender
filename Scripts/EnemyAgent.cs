using Godot;
using System;
using Godot.Collections;

public class EnemyAgent : KinematicBody
{
    private Vector3[] path = {};
    private int currentPathIndex = 0;
    private KinematicBody target = null;
    private Vector3 velocity = Vector3.Zero;
    private float threshold = 0.1f;
    private Navigation nav;
    private float speed = 4f;
    private float health = 200f;
    private int goldOnDeath = 0;

    public override void _Ready()
    {
        nav = GetParent().GetParent<Navigation>();
        target = GetParent().GetParent().GetParent().GetNode<KinematicBody>("Player");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (path.Length > 0)
            moveTowardsTarget(delta);
    }

    private void moveTowardsTarget(float delta)
    {
        if (currentPathIndex >= path.Length)
            return;

        if (GlobalTransform.origin.DistanceTo(path[currentPathIndex]) < threshold)
            currentPathIndex++;
        else
        {
            Vector3 direction = path[currentPathIndex] - GlobalTransform.origin;
            velocity = direction.Normalized();
            velocity = MoveAndSlide(velocity * speed, Vector3.Up);
        }
    }

    private void getTargetPath(Vector3 targetPosition)
    {
        path = nav.GetSimplePath(GlobalTransform.origin, targetPosition);
        currentPathIndex = 0;
    }

    private void onTimerTimeout()
    {
        getTargetPath(target.GlobalTransform.origin);
    }

    public void Initialise(Vector3 spawnPosition, float health, float speed, int goldOnDeath)
    {
        GameManager.instance.AddEnemy(this);
        GlobalTranslation = spawnPosition;
        this.health = health;
        this.speed = speed;
        this.goldOnDeath = goldOnDeath;
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
}
