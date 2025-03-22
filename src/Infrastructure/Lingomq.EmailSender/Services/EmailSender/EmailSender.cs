using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LingoMQ.Infrastructure.EmailSender.Services.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration) => _configuration = configuration;

    public void Send(EmailBuilder builder, string to, string subject)
    {
        string mail = builder.Build();

        MailMessage message = CreateMailMessage(to, subject, mail);
        SmtpClient smtpClient = CreateSmtpClient();

        smtpClient.Send(message);
    }

    private MailMessage CreateMailMessage(string to, string subject, string body)
    {
        MailMessage message = new MailMessage()
        {
            From = new MailAddress(_configuration["Mail:From"]!),
            Subject = subject,
            BodyEncoding = Encoding.UTF8,
            Body = body,
            IsBodyHtml = true,
        };

        message.To.Add(to);
        message.Priority = MailPriority.Normal;

        return message;
    }

    private SmtpClient CreateSmtpClient()
    {
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = _configuration["Mail:Smtp:Client"]!;
        smtpClient.Port = int.Parse(_configuration["Mail:Smtp:Port"]!);
        smtpClient.EnableSsl = true;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(
            _configuration["Mail:Credentials:Client"],
            _configuration["Mail:Credentials:Password"]
        );

        return smtpClient;
    }
}
