using Godot;
using Godot.Collections;

[GlobalClass]
public partial class NonPlayerMessage : Message
{
    [Export] public MessageNode[] respondingMessageNodes { get; set; }
}