using Godot;

public static class WindowUtils {
    public static Vector2 ProcessDrag(Control window, bool isDragging, Vector2 dragOffset)
    {
        if (isDragging)
        {
            GD.Print("Dragging window: " + window.Name);
            Vector2 mousePos = window.GetGlobalMousePosition();
            Vector2 desiredPosition = mousePos - dragOffset;
            Vector2 windowSize = new Vector2(window.Size.X * window.Scale.X, window.Size.Y * window.Scale.Y);
            Vector2 screenSize = window.GetViewport().GetVisibleRect().Size;

            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(desiredPosition.X, 0, screenSize.X - windowSize.X),
                Mathf.Clamp(desiredPosition.Y, 0, screenSize.Y - windowSize.Y)
            );

            // Adjust offset if we hit a boundary
            if (clampedPosition != desiredPosition)
            {
                dragOffset = mousePos - clampedPosition;
            }

            window.GlobalPosition = clampedPosition;
        }
        
        return dragOffset; // Return the (potentially updated) offset
    }
}