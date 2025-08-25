using Godot;

public partial class SettingsManager : ApplicationWindow
{
    private SettingsWindow _currentWindow;
    private SettingsTab _currentTab;
    [Export] public TextureRect ExitButton { get; set; }
    [Export] public VBoxContainer SettingsTabContainer { get; set; }
    [Export] public Panel SettingsWindowContainer { get; set; }

    public override void _Ready()
    {
        base._Ready(); // This sets up the dragging and focus functionality
        
        SettingsWindowContainer.GetChild(0)?.QueueFree();
        SettingsTab settingsTab = SettingsTabContainer.GetChild<SettingsTab>(0);
        ShowSettingsWindow(settingsTab);
        settingsTab.ToggleSelection(true);

        ExitButton.GuiInput += CloseSettingsMenu;
    }

    public void ShowSettingsWindow(SettingsTab settingsTab)
    {
        PackedScene windowScene = settingsTab.SettingsWindowScene;
        _currentTab?.ToggleSelection(false);
        _currentTab = settingsTab;

        _currentWindow?.QueueFree();

        _currentWindow = windowScene.Instantiate<SettingsWindow>();
        SettingsWindowContainer.AddChild(_currentWindow);

        _currentWindow.SetupWindow();
        _currentWindow.LoadSettings();
        AddFocusBehaviorToChildren(_currentWindow);
    }

    public void CloseSettingsMenu(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            FocusWindow();
            ApplicationWindowController.Instance.CloseApplicationWindow(this);
            _currentWindow?.Cleanup();
            _currentWindow?.QueueFree();
            _currentWindow = null;

            _currentTab?.ToggleSelection(false);
            _currentTab = null;

            QueueFree(); // Close the settings manager
        }
    }
}