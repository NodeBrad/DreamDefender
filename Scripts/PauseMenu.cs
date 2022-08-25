using Godot;
using System;

public class PauseMenu : Control
{
    public static PauseMenu instance;
    private bool showMenu = false;

    public override void _Ready()
    {
        instance = this;
    }

    public override void _Process(float delta)
    {
        if (showMenu)
            Visible = true;
        else
            Visible = false;

        if (Input.IsActionJustPressed("pause_game"))
        {
            if (ShopMenu.instance.Visible)
                return;

            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        showMenu = !showMenu;
        if (showMenu)
            Input.MouseMode = Input.MouseModeEnum.Visible;
        else
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void onBtnResumePressed()
    {
        ToggleMenu();
    }

    private void onBtnQuitPressed()
    {
        GetTree().Quit();
    }
}
