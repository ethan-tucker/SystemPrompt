using Godot;
using System;

public partial class SettingsTab : Control
{
    [Export] public PackedScene SettingsWindowScene { get; set; }
    [Export] public SettingsManager _settingsManager { get; set; }
    
    
    public override void _Ready() {
    }
    
    private void OnTabSelected()
    {
        if (SettingsWindowScene != null)
        {
            _settingsManager.ShowSettingsWindow(SettingsWindowScene);
        }
    }
}