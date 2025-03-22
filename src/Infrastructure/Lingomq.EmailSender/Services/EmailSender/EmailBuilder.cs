using System.Text;
using LingoMQ.Infrastructure.EmailSender.Models;

namespace LingoMQ.Infrastructure.EmailSender.Services.EmailSender;

public abstract class EmailBuilder
{
    public EnvelopTemplate Template { get; set; } = new();

    public string Build()
    {
        string template = File.ReadAllText("EmailTemplates/MailTemplate.html");

        template = template.Replace("{Header}", Template.Header);

        string welcome = BuildWelcome();
        string content = BuildContent();
        string hides = BuildHides();
        string buttons = BuildButtons();

        template = template.Replace("{Welcomes}", welcome);
        template = template.Replace("{Contents}", content);
        template = template.Replace("{Hides}", hides);
        template = template.Replace("{Buttons}", buttons);

        return template;
    }

    protected virtual string BuildContent()
    {
        StringBuilder result = new StringBuilder();
        string contentTemplate = File.ReadAllText("EmailTemplates/ContentTemplate.html");

        foreach (string value in Template.Contents)
        {
            string content = contentTemplate;
            content = content.Replace("{ContentText}", value);
            result.Append(content);
        }

        return result.ToString();
    }

    protected virtual string BuildWelcome()
    {
        StringBuilder result = new StringBuilder();
        string welcomeTemplate = File.ReadAllText("EmailTemplates/WelcomeTemplate.html");

        foreach (string value in Template.Welcomes)
        {
            string welcome = welcomeTemplate;
            welcome = welcome.Replace("{WelcomeText}", value);
            result.Append(welcome);
        }

        return result.ToString();
    }

    protected virtual string BuildButtons()
    {
        StringBuilder result = new StringBuilder();
        string buttonTemplate = File.ReadAllText("EmailTemplates/ButtonTemplate.html");

        foreach (MailButtonTemplate value in Template.Buttons)
        {
            string button = buttonTemplate;
            button = button.Replace("{ButtonName}", value.Title);
            button = button.Replace("{ButtonAddress}", value.Link);
            result.Append(button);
        }

        return result.ToString();
    }

    protected virtual string BuildHides()
    {
        StringBuilder result = new StringBuilder();
        string hideTemplate = File.ReadAllText("EmailTemplates/HideTemplate.html");

        foreach (string value in Template.Hides)
        {
            string hide = hideTemplate;
            hide = hide.Replace("{HideText}", value);
            result.Append(hide);
        }

        return result.ToString();
    }

    /// <summary>
    /// Метод для автоматической подставки контента. <br/>
    /// <b>Должен быть реализован если нужно его использование</b>
    /// </summary>
    /// <exception cref="NotImplementedException">Если метод не будет реализован</exception>
    public virtual void PrepareEmailModel() =>
        throw new NotImplementedException("Метод не был переопределен");

    /// <summary>
    /// Метод для автоматической подставки контента. <br/>
    /// <b>Должен быть реализован если нужно его использование</b>
    /// </summary>
    /// <exception cref="NotImplementedException">Если метод не будет реализован</exception>
    /// <param name="value">Передаваемое значение, может быть любым типом</param>
    public virtual void PrepareEmailModel(object value) =>
        throw new NotImplementedException("Метод не был переопределен");
}
