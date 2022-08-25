using Godot;
using System;

public class Enemy : Spatial
{
    // Enemy Settings
    private float speed = 20;
    private float health = 200f;
    private int goldOnDeath = 0;

    private PathFollow path;

    public override void _Ready()
    {
        path = GetNode<PathFollow>("Path/PathFollow");
    }

    public override void _Process(float delta)
    {
        // Move along path
        path.Offset += speed * delta;

        if (path.UnitOffset == 100)
            goalReached();
    }

    public void Initialise(float health, float speed, int goldOnDeath)
    {
        //GameManager.instance.AddEnemy(this);
        this.health = health;
        this.speed = speed;
        this.goldOnDeath = goldOnDeath;
    }

    public void TakeDamage(float damage)
    {
        health-=damage;
        if (health <= 0f)
        {
            GameManager.instance.AddGold(goldOnDeath);
            //GameManager.instance.RemoveEnemy(this);
        }
    }

    private void goalReached()
    {
        GameManager.instance.RemoveLife();
        //GameManager.instance.RemoveEnemy(this);
    }
}
