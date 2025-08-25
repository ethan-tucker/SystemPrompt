using Godot;

public partial class ApplicationWindow : Control
{
    [Export] public Control DragContainer { get; set; }
    private bool _isDragging = false;
    private Vector2 _dragOffset;

    public override void _Ready()
    {
        if (DragContainer != null)
        {
            DragContainer.GuiInput += DragManager;
        }
        
        AddFocusBehaviorToChildren(this);
    }

    public void AddFocusBehaviorToChildren(Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is Control control)
            {
                control.GuiInput += (InputEvent @event) =>
                {
                    if (@event.IsActionPressed("select"))
                    {
                        FocusWindow();
                    }
                };
            }
            
            // Recursively add to children
            AddFocusBehaviorToChildren(child);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("select"))
        {
            _isDragging = false;
        }
    }

    public override void _Process(double delta)
    {
        _dragOffset = WindowUtils.ProcessDrag(this, _isDragging, _dragOffset);
    }

    protected virtual void DragManager(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            FocusWindow();
            _isDragging = true;
            _dragOffset = GetGlobalMousePosition() - GlobalPosition;
        }
    }

    protected virtual void FocusWindow()
    {
        ApplicationWindowController.Instance.sendToFront(this);
    }
}