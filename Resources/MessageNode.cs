using Godot;

[GlobalClass]
public partial class MessageNode : Resource
{
    [Export] public int messageIndex { get; set; } = 0;
    [Export] public MessageType messageType { get; set; }
    [Export(PropertyHint.MultilineText)] public Message[] messagePool { get; set; }

    // Using enum instead of separate classes so that they can be switched in the editor
    public enum MessageType
    {
        Static, // always choose the first message in the pool
        Pool, // choose a random message from the pool
        Incremental, // choose the next message in the pool each time
    }

    public bool IsPlayerNode() {
        return messagePool[0] is PlayerMessage;
    }

    public MessageNode(string text = "")
    {
        messagePool = new Message[] { new Message(text) };
        messageType = MessageType.Static;
    }

    public MessageNode()
    {
        messagePool = new Message[0]; // or Array.Empty<Message>()
        messageType = MessageType.Static;
    }

    public Message GetNextMessage()
    {
        if (messagePool.Length == 0) return null;

        Message selectedMessage = null;
        switch (messageType)
        {
            case MessageType.Static:
                selectedMessage = messagePool[0];
                break;
            case MessageType.Pool:
                var rand = new RandomNumberGenerator();
                rand.Randomize();
                selectedMessage = messagePool[rand.RandiRange(0, messagePool.Length - 1)];
                break;
            case MessageType.Incremental:
                selectedMessage = messagePool[messageIndex];
                messageIndex = (messageIndex + 1) % messagePool.Length;
                break;
        }
        return selectedMessage;
    }

    public string GetMessageDisplayText()
    {
        Message selectedMessage = null;
        switch (messageType)
        {
            case MessageType.Static:
            case MessageType.Pool:
                selectedMessage = messagePool[0];
                break;
            case MessageType.Incremental:
                selectedMessage = messagePool[messageIndex];
                break;
        }
        return selectedMessage.chatOptionDisplayText;
    }
}