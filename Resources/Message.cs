using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Message : Resource
{
    [Export] public Array<MessageCondition> messageConditions { get; set; } = new Array<MessageCondition>();
    [Export] public string chatOptionDisplayText { get; set; } = "";
    [Export(PropertyHint.MultilineText)] public string messageText { get; set; } = "";

    public enum MessageCondition
    {
        None,
        HasFoundFile
    }

    public Message(string text = "")
    {
        chatOptionDisplayText = text;
        messageText = text;
    }
}