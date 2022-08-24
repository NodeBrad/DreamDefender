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
        GD.Print(enemy + " added to list, list size is " + enemyList.Count);
    }

    public void RemoveEnemy(Spatial enemy)
    {
        enemyList.Remove(enemy);
        GD.Print(enemy + " removed from list, list size is " + enemyList.Count);
        enemy.QueueFree();
    }

    private void spawnEnemy()
    {
        Node enemy = enemyInstance.Instance();
        AddChild(enemy);
    }

    public bool isEnemy(Spatial node)
    {
        foreach (Spatial n in enemyList)
        {
            GD.Print("Comparing " + node + " from bullet to " + n + " from Manager List.");
            if (n == node)
                return true;
        }
        return false;
    }
}
