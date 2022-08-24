using Godot;
using System;

public class DebugSphere : Spatial
{
    private RayCast ray;
    public override void _Ready()
    {
        ray = GetNode<RayCast>("MeshInstance/RayCast");
    }
    public bool isOnFloor()
    {
        if (ray.IsColliding())
            return true;
        else
            return false;
    }
}
