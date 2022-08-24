using Godot;
using System;

public class Enemy : Spatial
{
    private int speed = 5;
    private float health = 100f;
    private PathFollow pf;
    private bool initialised = false;
    private KinematicBody body;
    private Label label;
    public override void _Ready()
    {
        pf = GetNode<PathFollow>("Path/PathFollow");
        body = GetNode<KinematicBody>("Path/PathFollow/KinematicBody");
        label = GetNode<Label>("Path/PathFollow/KinematicBody/Sprite3D/Viewport/Label");
    }

    public override void _Process(float delta)
    {
        pf.Offset += speed * delta;

        if (!initialised)
        {
            init();
            initialised = true; 
        }
        label.Text = health.ToString();
    }

    private void init()
    {
        GameManager.instance.AddEnemy(this);
        label.Text = this.ToString();
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
