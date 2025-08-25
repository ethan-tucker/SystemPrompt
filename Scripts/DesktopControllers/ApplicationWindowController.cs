using System.Collections.Generic;
using Godot;

public partial class ApplicationWindowController : Control
{
    public static ApplicationWindowController Instance { get; private set; }
    [Export] public Control ApplicationWindowsContainer { get; set; }
    [Export] public UserMessageController UserMessageController { get; set; }
    private Dictionary<Node, DesktopIcon> _applicationWindows = new Dictionary<Node, DesktopIcon>();

    [Export] MessageNode startMessageNode;

    public override void _Ready()
    {
        Instance = this;
        if (UserMessageController != null)
        {
            CallDeferred(nameof(LateReady));
        }
    }

    private void LateReady()
    {
        UserMessageController.AddMessageBubble(startMessageNode);
    }

    public void sendToFront(Node window)
    {
        if (window != null && ApplicationWindowsContainer != null)
        {
            ApplicationWindowsContainer.MoveChild(window, ApplicationWindowsContainer.GetChildCount() - 1);
            GD.Print("Moved window to front: " + window.Name);
        }
    }

    public void RegisterApplicationWindow(Node window, DesktopIcon icon)
    {
        if (window != null && icon != null)
        {
            _applicationWindows[window] = icon;
            ApplicationWindowsContainer.AddChild(window);
            GD.Print("Registered application window: " + window.Name);
        }
    }

    public void CloseApplicationWindow(Node window)
    {
        if (window != null && _applicationWindows.ContainsKey(window))
        {
            _applicationWindows[window].ApplicationWindowClosed();
            ApplicationWindowsContainer.RemoveChild(window);
            _applicationWindows.Remove(window);
        }
    }
}
