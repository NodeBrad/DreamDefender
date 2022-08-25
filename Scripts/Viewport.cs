using Godot;
using System;

[Tool]
public class Viewport : Godot.Viewport
{
    private Label label;
    public override void _Ready()
    {
        label = GetNode<Label>("Label");
    }
    public override void _Process(float delta)
    {
        Size = label.RectSize;
    }

}
