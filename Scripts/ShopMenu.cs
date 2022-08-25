using Godot;
using System;

public class ShopMenu : Control
{
    public static ShopMenu instance;
    private bool showMenu = false;
    private Label goldLabel;

    public override void _Ready()
    {
        instance = this;
        goldLabel = GetNode<Label>("CenterContainer/VBoxContainer/GoldLabel");
    }

    public override void _Process(float delta)
    {
        if (showMenu)
            Visible = true;
        else
            Visible = false;

        goldLabel.Text = "Current Gold: " + GameManager.instance.GetGold();

        if (Input.IsActionJustPressed("open_shop"))
        {
            if (PauseMenu.instance.Visible)
                return;

            OpenMenu();
        }

        if (Input.IsActionJustPressed("close_menu"))
        {
            if (PauseMenu.instance.Visible)
                return;

            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        showMenu = true;
    }

    public void CloseMenu()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        showMenu = false;
    }

    private void onTButton1Pressed()
    {
        GameManager.instance.SetBuildable(1);
        CloseMenu();
    }
}
