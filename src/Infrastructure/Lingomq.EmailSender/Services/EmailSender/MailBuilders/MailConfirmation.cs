using LingoMQ.Infrastructure.EmailSender.Models;

namespace LingoMQ.Infrastructure.EmailSender.Services.EmailSender.MailBuilders;

public class MailConfirmation : EmailBuilder
{
    public override void PrepareEmailModel(object value)
    {
        Template.Header = "Подтверждение почты!";

        Template.Contents.Add("Добрый день! Вы успешно зарегистрировались в lingoMq!");

        Template.Contents.Add("Пожайлуста, подтвердите Ваш адрес электронной почты");

        Template.Buttons.Add(
            new MailButtonTemplate()
            {
                Title = "Подтвердить",
                Link = "http://localhost:9998/api.lingomq/auth/confirm/email/" + (string)value,
            }
        );
    }
}
