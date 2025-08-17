using Godot;

[GlobalClass]
public partial class IntroDialogueSequence : Resource
{
	[Export] public string SequenceId { get; set; }
	[Export(PropertyHint.MultilineText)] public string introLine { get; set; }
	[Export(PropertyHint.MultilineText)] public string[] Lines { get; set; }
	[Export] public string continueText { get; set; }
}
