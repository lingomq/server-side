namespace LingoMQ.Infrastructure.EmailSender.Models;

public class EnvelopTemplate
{
    public string? Header { get; set; }
    public List<string> Welcomes { get; set; } = new();
    public List<string> Contents { get; set; } = new();
    public List<string> Hides { get; set; } = new();
    public List<MailButtonTemplate> Buttons { get; set; } = new();
}
