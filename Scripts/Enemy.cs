using Godot;
using System;

public class Enemy : Spatial
{
    private int enemySpeed = 5;
    private PathFollow pf;
    private bool initialised = false;
    public override void _Ready()
    {
        pf = GetNode<PathFollow>("Path/PathFollow");
    }

    public override void _Process(float delta)
    {
        pf.Offset += enemySpeed * delta;

        if (!initialised)
        {
            init();
            initialised = true; 
        }
    }

    private void init()
    {
        GameManager.instance.AddEnemy(this);
    }
}
