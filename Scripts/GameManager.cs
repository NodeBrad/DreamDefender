using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Spatial
{
    public static GameManager instance;

    [Export]
    private PackedScene enemyInstance;
    [Export]
    private PackedScene turret1;
    [Export]
    private PackedScene turret2;

    private PackedScene turretInstance;
    private int turretCost = 0;
    private int turret1Cost = 100;

    private int gold = 200;
    private int lives = 20;

    public List<KinematicBody> enemyList;
    private Timer timer;
    private Position3D spawnPosition3D;
    private Vector3 spawnPoint = Vector3.Zero;

    // Wave Variables
    private int[] waves = { 0, 5, 10 };
    private int waveCounter = 0;
    private int currentWave = 0;
    private bool canSpawn = true;

    private bool isPlacing = false;

    public override void _Ready()
    {
        instance = this;
        enemyList = new List<KinematicBody>();
        timer = GetNode<Timer>("SpawnTimer");
        spawnPosition3D = GetNode<Position3D>("SpawnPoint");
        spawnPoint = spawnPosition3D.GlobalTransform.origin;
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

    public void AddEnemy(KinematicBody enemy)
    {
        enemyList.Add(enemy);
        GD.Print(enemy + " added to list, list size is " + enemyList.Count);
    }

    public void RemoveEnemy(KinematicBody enemy)
    {
        enemyList.Remove(enemy);
        GD.Print(enemy + " removed from list, list size is " + enemyList.Count);
        enemy.QueueFree();
    }

    private void spawnEnemy()
    {
        EnemyAgent enemy = (EnemyAgent)enemyInstance.Instance();
        NavigationMeshInstance nav = GetNode<NavigationMeshInstance>("Navigation/NavigationMeshInstance");
        nav.AddChild(enemy);
        enemy.Initialise(spawnPoint, 100f, 5f, 25);
    }

    public bool IsEnemy(KinematicBody node)
    {
        foreach (KinematicBody n in enemyList)
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
        if (gold < turretCost)
        {
            GD.Print("Not enough gold!");
            isPlacing = false;
            return;
        }
        Turret turret = (Turret)turretInstance.Instance();
        AddChild(turret);
        turret.GlobalTranslation = location;
        isPlacing = false;
        gold -= turretCost;
    }

    public int GetGold()
    {
        return gold;
    }

    public bool IsPlacing()
    {
        return isPlacing;
    }

    public void SetBuildable(int id)
    {
        switch(id)
        {
            case 0:
                break;
            case 1:
                turretInstance = turret1;
                turretCost = turret1Cost;
                break;
            default:
                break;
        }

        isPlacing = true;
    }

    public void AddGold(int goldToAdd)
    {
        gold += goldToAdd;
    }

    public void RemoveLife()
    {
        lives--;
    }
}
