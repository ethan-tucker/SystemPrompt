using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class terminalTextController : VBoxContainer
{
	[Export] public IntroDialogueSequence dialogueSequence { get; set; }
	[Export] public float characterDelay { get; set; } = .05f;
	[Export] public float lineDelay { get; set; } = .4f;
	[Export] public float startingDelay { get; set; } = .5f;

	// Allowing for two different labels as they may need to be styled differently.
	private Label introLabel;
	private Label mainTextBodyLabel;
	private Label continueLabel;
	private int currentLineIndex = 0;
	private bool isTyping = false;
	private bool introComplete = false;
	private Tween typingTween;
	private ScrollContainer scrollContainer;
	private bool showingContinuePrompt = false;

	public override void _Ready()
	{
		introLabel = GetNode<Label>("systemPromptHeadingLabel");
		mainTextBodyLabel = GetNode<Label>("mainTerminalLabel");
		continueLabel = GetNode<Label>("continueLabel");
		introLabel.Text = "";
		mainTextBodyLabel.Text = "";

		scrollContainer = GetParent<ScrollContainer>();

		GD.Print(dialogueSequence.Lines.Length);

		if (dialogueSequence != null && dialogueSequence.Lines.Length > 0)
		{
			GetTree().CreateTimer(startingDelay).Timeout += () =>
			{
				StartDialogue();
			};
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("enter"))
		{
			if (!isTyping)
			{
				GD.Print("Enter pressed, showing next line");
				HideContinuePrompt();
				ShowNextLine();
			}
		}
	}

		public void StartDialogue()
	{
		currentLineIndex = 0;
		introComplete = false;
		
		// Start with intro line if it exists
		if (!string.IsNullOrEmpty(dialogueSequence.introLine))
		{
			TypeText(dialogueSequence.introLine, introLabel);
		}
		else
		{
			introComplete = true;
			ShowCurrentLine();
		}
	}


	private void ShowCurrentLine()
	{
		if (currentLineIndex >= dialogueSequence.Lines.Length)
		{
			// Dialogue finished
			OnDialogueComplete();
			return;
		}

		string currentText = dialogueSequence.Lines[currentLineIndex];
		TypeText(currentText, mainTextBodyLabel);
	}

	private async void TypeText(string text, Label targetLabel)
	{
		isTyping = true;

		// Type each character with a delay
		if (targetLabel == introLabel)
		{
			// Header: Type line by line
			await TypeLineByLine(text, targetLabel);
			introComplete = true;
			ShowCurrentLine();
		}
		else if (targetLabel == mainTextBodyLabel)
		{
			// Main dialogue: Type character by character
			await TypeCharacterByCharacter(text, targetLabel);
			//Start Enter to Continue Prompt
			ShowEnterToContinuePrompt();
		}
	}

	private void HideContinuePrompt()
	{
		mainTextBodyLabel.Text += "\n\n";
		continueLabel.Visible = false;
		showingContinuePrompt = false;
	}

	private async void ShowEnterToContinuePrompt()
	{
		continueLabel.Text = dialogueSequence.continueText;
		continueLabel.Visible = true;
		showingContinuePrompt = true;

		// start loop of 3 ellipses
		int ellipsisCount = 0;

		continueLabel.Text = dialogueSequence.continueText;
		isTyping = false;
		await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
		ScrollToBottom();
		while (showingContinuePrompt)
		{
			continueLabel.Text = dialogueSequence.continueText + new string('.', (ellipsisCount + 1) % 4);
			await ToSignal(GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
			ellipsisCount++;
		}
	}

	private async Task TypeLineByLine(string text, Label targetLabel)
	{
		string[] lines = text.Split('\n');
		string currentText = "";

		foreach (string line in lines)
		{
			currentText += (currentText.Length > 0 ? "\n" : "") + line;
			targetLabel.Text = currentText;

			// Brief pause between lines
			await ToSignal(GetTree().CreateTimer(lineDelay), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private async Task TypeCharacterByCharacter(string text, Label targetLabel)
	{
		for (int i = 0; i < text.Length; i++)
		{
			targetLabel.Text += text[i];
			ScrollToBottom();
			if (i < text.Length)
			{
				await ToSignal(GetTree().CreateTimer(characterDelay), SceneTreeTimer.SignalName.Timeout);
			}
		}
	}

	private void ShowNextLine()
	{
		if (!introComplete)
		{
			// Still on intro, complete it and move to main dialogue
			introComplete = true;
			ShowCurrentLine();
		}
		else
		{
			// Move to next line in main dialogue
			currentLineIndex++;
			ShowCurrentLine();
		}
	}
	

	private void ScrollToBottom()
	{
		// Wait one frame for layout to update, then scroll
		scrollContainer.ScrollVertical = (int)scrollContainer.GetVScrollBar().MaxValue;
	}
	
	private void OnDialogueComplete()
	{
		GD.Print("Dialogue sequence complete!");
		// Add your completion logic here
	}
}
