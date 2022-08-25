using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Spatial
{
    public static GameManager instance;

    [Export]
    public PackedScene enemyInstance;
    [Export]
    public PackedScene turretInstance;

    public List<Spatial> enemyList;
    private Timer timer;

    private int[] waves = { 0, 5, 10 };
    private int waveCounter = 0;
    private int currentWave = 0;
    private bool canSpawn = true;

    public override void _Ready()
    {
        instance = this;
        enemyList = new List<Spatial>();
        timer = GetNode<Timer>("SpawnTimer");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("debug_spawn"))
        {
            nextWave();
            timer.Start(-1);
        }

        if (waveCounter < waves[currentWave] && canSpawn)
        {
            spawnEnemy();
            waveCounter++;
            canSpawn = false;
        }
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
        Enemy enemy = (Enemy)enemyInstance.Instance();
        AddChild(enemy);
        enemy.Initialise(100f, 5);
    }

    public bool IsEnemy(Spatial node)
    {
        foreach (Spatial n in enemyList)
        {
            if (n == node)
                return true;
        }
        return false;
    }

    private void onSpawnTimeout()
    {
        canSpawn = true;
    }

    private void nextWave()
    {
        waveCounter = 0;
        currentWave++;
    }

    public void PlaceTurret(Vector3 location)
    {
        Turret turret = (Turret)turretInstance.Instance();
        AddChild(turret);
        turret.GlobalTranslation = location;
    }
}
