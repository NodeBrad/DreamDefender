using Godot;
using System;

public class Turret : Spatial
{
    private MeshInstance turretHead;
    private Position3D barrelTip;
    private KinematicBody target;
    private Timer timer;

    private float range = 5f;
    private float damage = 10f;
    private float fireRate = 0.2f;
    private bool canShoot = true;

    [Export]
    public PackedScene bulletInstance;
    public override void _Ready()
    {
        turretHead = GetNode<MeshInstance>("TBody/THead");
        barrelTip = GetNode<Position3D>("TBody/THead/TBarrel/TBarrelTip");
        timer = GetNode<Timer>("Timer");
        timer.WaitTime = fireRate;
    }

    public override void _Process(float delta)
    {
        findTarget();
        lookAtTarget();
        if (canShoot && target != null)
        {
            shoot();
            timer.Start();
        }
    }

    private void onTimeout()
    {
        canShoot = true;
    }

    private void findTarget()
    {
        // Calculate the distance between each instanced enemy and set the closest one as target
        float nearestDistance = 100;
        target = null;

        foreach (KinematicBody node in GameManager.instance.enemyList)
        {
            float nodeDistance = node.GlobalTranslation.DistanceTo(GlobalTranslation);
            if (nodeDistance < range && nearestDistance > nodeDistance)
            {
                target = node;
                nearestDistance = nodeDistance;
            }
        }
    }

    private void lookAtTarget()
    {
        if (target != null)
        {
            Vector3 enemyHead = target.GetNode<Position3D>("EnemyHead").GlobalTranslation;
            turretHead.LookAt(enemyHead, Vector3.Up);
        }
    }

    private void shoot()
    {
        Bullet bullet = (Bullet)bulletInstance.Instance();
        AddChild(bullet);
        Vector3 enemyHead = target.GetNode<Position3D>("EnemyHead").GlobalTranslation;
        bullet.Initialise(barrelTip.GlobalTranslation, enemyHead, damage);
        canShoot = false;
    }
}
