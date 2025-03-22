namespace LingoMQ.Infrastructure.EmailSender.Services.EmailSender;

public interface IEmailSender 
{
    void Send(EmailBuilder emailBuilder, string to, string subject);
}