using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class UserMessageController : Control
{
    [Export] private Control messageContainerToggle;
    [Export] private Control messageContainerToggleIcon;
    [Export] private Control mainContainer;
    [Export] private bool isOpen = true;
    [Export] private float tweenDuration = 0.5f;
    [Export] private PackedScene messageBubbleScene;
    [Export] private PackedScene playerMessageBubbleScene;
    [Export] private PackedScene messageOptionScene;
    [Export] private bool enabled = true;
    [Export] private Control messageContainer;
    [Export] private Control messageOptionsContainer;

    private Tween tween = null;
    private Tween angleTween = null;
    private float originalYPosition;

    public override void _Ready()
    {
        messageContainerToggle.GuiInput += ToggleClicked;
        originalYPosition = this.Position.Y;

        if (!isOpen)
        {
            this.Position = new Vector2(this.Position.X, originalYPosition - mainContainer.Size.Y);
        }
    }

    public void AddMessageBubble(MessageNode messageNode)
    {
        MessageBubble bubble = messageNode.IsPlayerNode() ? playerMessageBubbleScene.Instantiate<MessageBubble>() : messageBubbleScene.Instantiate<MessageBubble>();
        messageContainer.AddChild(bubble);
        bubble.SetMessageNode(messageNode, () => { if (!bubble.currentMessageNode.IsPlayerNode()) bubbleClicked(bubble); });
        if (bubble != null && messageContainer != null)
        {
            ScrollContainer scrollContainer = messageContainer.GetParent() as ScrollContainer;
            if (scrollContainer != null)
            {
                scrollContainer.ScrollVertical = (int)scrollContainer.GetVScrollBar().MaxValue;
            }
        }
    }

    private void bubbleClicked(MessageBubble bubble)
    {
        // show message options
        if (bubble.currentMessageNode.messagePool[0] is not NonPlayerMessage) return;
        NonPlayerMessage nonPlayerMessage = bubble.currentMessageNode.messagePool[0] as NonPlayerMessage;
        List<MessageNode> messageNodes = nonPlayerMessage.respondingMessageNodes.ToList();
        for (int i = 0; i < nonPlayerMessage.respondingMessageNodes.Length; i++)
        {
            MessageNode messageNode = nonPlayerMessage.respondingMessageNodes[i];
            GD.Print(messageNode);
            ChatOption optionBubble = messageOptionScene.Instantiate<ChatOption>(); 
            messageOptionsContainer.AddChild(optionBubble);
            optionBubble.SetPotentialMessageNode(messageNode, () => MessageOptionClicked(messageNode));
        }
    }

    private void MessageOptionClicked(MessageNode message)
{
        AddMessageBubble(message);
        messageOptionsContainer.GetChildren().ToList().ForEach(item => item.QueueFree());
    }

    // Handles minimizing/maximizing the message container

    private void ToggleClicked(InputEvent @event)
    {
        // start tween that minimizes the main container
        if (@event.IsActionPressed("select") && enabled)
        {
            //GD.Print("Toggle Clicked");
            AnimateContainer();
        }
    }

    private void AnimateContainer()
    {
        // Kill existing tween if running
        if (tween != null)
        {
            tween.Kill();
        }
        if (angleTween != null)
        {
            angleTween.Kill();
        }

        // Create new tween
        tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Quart);
        tween.TweenProperty(this, "position", new Vector2(this.Position.X, isOpen ? originalYPosition - mainContainer.Size.Y : originalYPosition), tweenDuration);
        tween.TweenProperty(this, "position", new Vector2(this.Position.X, isOpen ? originalYPosition - mainContainer.Size.Y : originalYPosition), tweenDuration);

        angleTween = CreateTween();
        angleTween.SetEase(Tween.EaseType.Out);
        angleTween.SetTrans(Tween.TransitionType.Quart);
        angleTween.TweenProperty(messageContainerToggleIcon, "rotation", Mathf.DegToRad(isOpen ? 180 : 0), tweenDuration);
        isOpen = !isOpen;
        //tween.TweenCallback(Callable.From(() => tween = null));
    }

    public void SetEnabled(bool enable, bool instant = true)
    {
        GD.Print("HERE: " + enable);
        this.enabled = enable;
        if (instant)
        {
            this.Position = new Vector2(this.Position.X, this.enabled ? originalYPosition : originalYPosition - mainContainer.Size.Y);
            messageContainerToggleIcon.Rotation = Mathf.DegToRad(this.enabled ? 0 : 180);
        }
        else
        {
            // Eh handle this case when we get there
        }
    }
}
