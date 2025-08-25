using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SettingsTab : Control
{
    [Export] public PackedScene SettingsWindowScene { get; set; }
    [Export] public SettingsManager _settingsManager { get; set; }
    [Export] public Panel ClickBox { get; set; }
    private bool _isSelected = false;


    public override void _Ready()
    {
        GD.Print("Setting up settings tab: " + SettingsWindowScene.ResourceName);
        MouseFilter = Control.MouseFilterEnum.Pass;
        GuiInput += OnTabSelected;
    }

    public void OnTabSelected(InputEvent @event)
    {
        if (SettingsWindowScene != null && @event.IsActionPressed("select") && !_isSelected)
        {
            _settingsManager.ShowSettingsWindow(this);
            ToggleSelection(true);
        }
    }

    public void ToggleSelection(bool isSelected)
    {
        _isSelected = isSelected;
        ClickBox.Visible = _isSelected;
    }
}