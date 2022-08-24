using Godot;
using System;

public class Turret : Spatial
{
    private MeshInstance turretHead;
    private KinematicBody target;
    public override void _Ready()
    {
        turretHead = GetNode<MeshInstance>("TBody/THead");
    }

    public override void _Process(float delta)
    {
        findTarget();
        lookAtTarget();
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
            if (nodeDistance < 10 && nearestDistance > nodeDistance)
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
}
