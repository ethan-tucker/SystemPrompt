using Godot;
using System;

public partial class DesktopIcon : Control
{
    [Export] public PackedScene RelatedScene { get; set; }
    private Node _applicationWindow = null;
    private Panel ClickBox;

    public override void _Ready()
    {
        ClickBox = GetNode<Panel>("backgroundColor");
        ClickBox.GuiInput += OnIconClicked;
    }

    private void OnIconClicked(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            GD.Print("Icon clicked: " + Name);
            if (_applicationWindow == null)
            {
                _applicationWindow = RelatedScene.Instantiate();
                ApplicationWindowController.Instance.RegisterApplicationWindow(_applicationWindow, this);
            }
            ApplicationWindowController.Instance.sendToFront(_applicationWindow);
        }
    }

    public void ApplicationWindowClosed()
    {
        _applicationWindow = null;
    }
}
