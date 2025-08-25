using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerMessage : Message
{
    [Export] public int messageCost { get; set; } = 0;
    [Export] public MessageNode overrideMessageRespondingMessageNode { get; set; }
    [Export(PropertyHint.MultilineText)] public string overrideMessageText { get; set; } = "";
    [Export] public MessageNode respondingMessageNode { get; set; }
}