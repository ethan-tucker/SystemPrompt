using Godot;

public partial class SettingsManager : Control
{
    private SettingsWindow _currentWindow;
    
    public void ShowSettingsWindow(PackedScene windowScene)
    {
        _currentWindow?.QueueFree();
        
        _currentWindow = windowScene.Instantiate<SettingsWindow>();
        AddChild(_currentWindow);
        
        _currentWindow.SetupWindow();
        _currentWindow.LoadSettings();
    }
}