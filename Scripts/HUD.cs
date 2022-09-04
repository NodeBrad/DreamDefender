using Godot;
using System;

public class HUD : Control
{
    public static HUD instance;
    private Label label;

    public override void _Ready()
    {
        instance = this;
        label = GetNode<Label>("CanvasLayer/Label");
    }

    public override void _Process(float delta)
    {
        label.Text = "Gold: " + GameManager.instance.GetGold() + "\nLives: " + GameManager.instance.GetLives() + "\nWave: " + GameManager.instance.GetWave();
    }
}
