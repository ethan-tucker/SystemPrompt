using Godot;

public partial class SettingsWindow : Control
{
    // Virtual methods that all windows should implement
    public virtual void SetupWindow() { }
    public virtual void LoadSettings() { }
    public virtual void SaveSettings() { }
    public virtual void Cleanup() { }
}