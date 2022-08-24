using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Spatial
{
    public static GameManager instance;

    [Export]
    public PackedScene enemyInstance;

    public List<Spatial> enemyList;

    public override void _Ready()
    {
        instance = this;
        enemyList = new List<Spatial>();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("debug_spawn"))
            spawnEnemy();  
    }

    public void AddEnemy(Spatial enemy)
    {
        enemyList.Add(enemy);
        GD.Print("Enemy added to list, list size is " + enemyList.Count);
    }

    private void spawnEnemy()
    {
        Node enemy = enemyInstance.Instance();
        AddChild(enemy);
    }
}
