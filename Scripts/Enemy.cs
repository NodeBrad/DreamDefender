using Godot;
using System;

public class Enemy : Spatial
{
    // Enemy Settings
    private float speed = 10;
    private float health = 200f;

    private PathFollow path;

    public override void _Ready()
    {
        path = GetNode<PathFollow>("Path/PathFollow");
    }

    public override void _Process(float delta)
    {
        // Move along path
        path.Offset += speed * delta;
    }

    public void Initialise(float health, float speed)
    {
        GameManager.instance.AddEnemy(this);
        this.health = health;
        this.speed = speed;
    }

    public void TakeDamage(float damage)
    {
        health-=damage;
        if (health <= 0f)
        {
            GameManager.instance.RemoveEnemy(this);
        }
    }
}
