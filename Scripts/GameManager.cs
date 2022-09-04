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
    private Position3D basePosition3D;
    private Vector3 spawnPoint = Vector3.Zero;
    private Vector3 basePoint = Vector3.Zero;
    private NavigationMeshInstance navMeshInstance;
    private Player player;
    private Camera camera;

    private enum GameState { Play, Watch};
    private GameState gameState = GameState.Play;

    // Wave Variables
    private int[] waves = { 0, 2, 5, 8, 10, 15, 20 };
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
        basePosition3D = GetNode<Position3D>("BasePoint");
        basePoint = basePosition3D.GlobalTransform.origin;
        navMeshInstance = GetNode<NavigationMeshInstance>("Navigation/NavigationMeshInstance");
        player = GetNode<Player>("Player");
        camera = GetNode<Camera>("Camera");
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

        if (enemyList.Count > 0)
            gameState = GameState.Watch;
        else
            gameState = GameState.Play;

        if (gameState == GameState.Play)
        {
            player.isEnabled = true;
            camera.Current = false;
        }
        if (gameState == GameState.Watch)
        {
            player.isEnabled = false;
            camera.Current = true;
        }

    }

    public void AddEnemy(KinematicBody enemy)
    {
        enemyList.Add(enemy);
    }

    public void RemoveEnemy(KinematicBody enemy)
    {
        enemyList.Remove(enemy);
        enemy.QueueFree();
    }

    private void spawnEnemy()
    {
        EnemyAgent enemy = (EnemyAgent)enemyInstance.Instance();
        navMeshInstance.AddChild(enemy);
        enemy.Initialise(spawnPoint, 100f, 5f, 25, basePoint);
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
        navMeshInstance.AddChild(turret);
        turret.GlobalTranslation = location;
        isPlacing = false;
        gold -= turretCost;

        navMeshInstance.BakeNavigationMesh();
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetLives()
    {
        return lives;
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

    public String GetWave()
    {
        return currentWave.ToString() + "/" + (waves.Length - 1).ToString();
    }
}
