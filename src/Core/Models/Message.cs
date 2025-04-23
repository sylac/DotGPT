namespace DotGPT.Core.Models;

public class Message
{
    public string Content { get; set; } = string.Empty;
    public bool IsUserMessage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}