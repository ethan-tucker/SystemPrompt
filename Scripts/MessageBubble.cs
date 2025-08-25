using Godot;
using System;
using System.Diagnostics;

public partial class MessageBubble : Control
{
    private const float MAX_BUBBLE_WIDTH = 1400f;

    [Export] private RichTextLabel messageLabel;
    [Export] private Control hoverBox;
    [Export] private MarginContainer contentContainer;
    [Export] private Control replyButton;
    [Export] private Control messageContainer;
    [Export] public Control outerHBoxContainer;
    public MessageNode currentMessageNode { get; private set; }

    public override void _Ready()
    {
        SetupNodes();
        replyButton.Visible = false;
        ToggleAlpha(false);
    }

    public void ToggleAlpha(bool visible)
    {
        Modulate = visible ? Colors.White : Colors.Transparent;
    }

    public void SetMessageNode(MessageNode messageNode, Action onClick = null)
    {
        if (messageNode != null)
        {
            currentMessageNode = messageNode;
            SetMessage(messageNode.GetNextMessage().messageText, () => ToggleAlpha(true));
            if (!messageNode.IsPlayerNode())
            {
                hoverBox.GuiInput += (InputEvent ev) => ReplyClicked(ev, onClick);
                hoverBox.MouseEntered += BubbleHovered;
                hoverBox.MouseExited += BubbleUnhovered;
            }
        }
    }
    
    private void BubbleHovered()
    {
        GD.Print("Bubble hovered");
        replyButton.Visible = true;
    }

    private void BubbleUnhovered()
    {
        GD.Print("Bubble unhovered");
        replyButton.Visible = false;
    }

    public void ReplyClicked(InputEvent @event, Action onClick = null)
    {
        if (@event.IsActionPressed("select"))
        {
            onClick?.Invoke();
        }
    }

    private void SetupNodes()
    {
        // MessageBubble (this) setup
        /*SetAnchorsPreset(Control.LayoutPreset.TopLeft);
        SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;*/

        // MessageLabel initial setup
        messageLabel.FitContent = true;

        //messageLabel.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        //messageLabel.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
        messageLabel.ClipContents = false; // Don't truncate
        SetMessage(messageLabel.Text); // Initial sizing
    }

    public async void SetMessage(string text, Action _callback = null)
    {
        messageLabel.Text = text;

        // PASS 1: Get natural text width (no wrapping)
        messageLabel.AutowrapMode = TextServer.AutowrapMode.Off;
        messageLabel.CustomMinimumSize = Vector2.Zero;

        // Wait for Godot to calculate the layout
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        // Get the width the text naturally wants
        float naturalWidth = messageLabel.GetContentWidth();

        // Calculate available width (account for margins)
        float marginWidth = contentContainer.GetThemeConstant("margin_left") +
                           contentContainer.GetThemeConstant("margin_right");
        float maxContentWidth = MAX_BUBBLE_WIDTH - marginWidth;

        // PASS 2: Apply wrapping if needed
        if (naturalWidth > maxContentWidth)
        {
            // Text is too wide - constrain and wrap
            messageContainer.CustomMinimumSize = new Vector2(maxContentWidth, 0);
            messageLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        }
        else
        {
            messageContainer.CustomMinimumSize = new Vector2(naturalWidth + marginWidth, 0);
            // Text fits naturally - no wrapping needed
            messageLabel.AutowrapMode = TextServer.AutowrapMode.Off;
            // Don't set minimum size - let it be natural width
        }

        // Wait for wrapping to be applied
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        // Update bubble size to match content
        UpdateBubbleSize(_callback);
    }

    private async void UpdateBubbleSize(Action _callback = null)
    {
        // Get the final content size after wrapping
        Vector2 contentSize = messageLabel.Size;

        // Add margins to get total bubble size
        float marginWidth = contentContainer.GetThemeConstant("margin_left") +
                        contentContainer.GetThemeConstant("margin_right");
        float marginHeight = 20;

        Vector2 bubbleSize = new Vector2(
            contentSize.X + marginWidth,
            contentSize.Y + marginHeight
        );

        // Set both size AND minimum size to force layout respect
        Size = new Vector2(Size.X, bubbleSize.Y);
        this.CustomMinimumSize = new Vector2(Size.X, bubbleSize.Y);
        messageContainer.CustomMinimumSize = bubbleSize;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        //if (currentMessageNode != null) SetDisplaySide(currentMessageNode.IsPlayerNode());
        if (_callback != null)
        {
            _callback.Invoke();
        }
    }

    public void SetDisplaySide(bool sentByPlayer)
    {
        outerHBoxContainer.SetAnchorsPreset(sentByPlayer ? Control.LayoutPreset.TopRight : Control.LayoutPreset.TopLeft);
    }
}