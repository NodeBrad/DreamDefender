using Godot;
using System;

public class Turret : Spatial
{
    private MeshInstance turretHead;
    private Position3D barrelTip;
    private KinematicBody target;
    private Timer timer;

    private float range = 10f;
    private float damage = 50f;

    [Export]
    public PackedScene bulletInstance;
    public override void _Ready()
    {
        turretHead = GetNode<MeshInstance>("TBody/THead");
        barrelTip = GetNode<Position3D>("TBody/THead/TBarrel/TBarrelTip");
        timer = GetNode<Timer>("Timer");
    }

    public override void _Process(float delta)
    {
        findTarget();
        lookAtTarget();
        if (Input.IsActionJustPressed("debug_shoot_turret") && target!=null)
            shoot();
    }

    private void onTimeout()
    {
        if (target!=null)
            shoot();
    }

    private void findTarget()
    {
        // Calculate the distance between each instanced enemy and set the closest one as target
        float nearestDistance = 100;
        target = null;

        foreach (Spatial node in GameManager.instance.enemyList)
        {
            KinematicBody nodeMesh = node.GetNode<KinematicBody>("Path/PathFollow/KinematicBody");
            float nodeDistance = nodeMesh.GlobalTranslation.DistanceTo(GlobalTranslation);
            if (nodeDistance < range && nearestDistance > nodeDistance)
            {
                target = nodeMesh;
                nearestDistance = nodeDistance;
            }
        }
    }

    private void lookAtTarget()
    {
        if (target != null)
            turretHead.LookAt(target.GlobalTranslation, Vector3.Up);
    }

    private void shoot()
    {
        Bullet bullet = (Bullet)bulletInstance.Instance();
        AddChild(bullet);
        
        bullet.Initialise(barrelTip.GlobalTranslation, target.GlobalTranslation, damage);
    }
}
