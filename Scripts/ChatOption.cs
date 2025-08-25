using Godot;
using System;

public partial class ChatOption : Control
{

    [Export] private Control hoverBox;
    [Export] private RichTextLabel label;
    public void SetPotentialMessageNode(MessageNode messageNode, Action onClick = null)
    {
        GD.Print("SETTING MESSAGE NODE:" + messageNode);
        if (messageNode != null)
        {
            GD.Print(messageNode.GetMessageDisplayText());
            label.Text = messageNode.GetMessageDisplayText();
            hoverBox.GuiInput += (InputEvent ev) => MessageOptionClicked(ev, onClick);
        }
    }
    
    private void MessageOptionClicked(InputEvent @event, Action onClick = null)
    {
        if (@event.IsActionPressed("select"))
        {
            onClick?.Invoke();
        }
    }
}
