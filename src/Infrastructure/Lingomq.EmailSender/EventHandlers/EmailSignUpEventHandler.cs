using LingoMQ.Core.Domain.Users;
using LingoMQ.Core.Domain.Users.Events;
using LingoMQ.Infrastructure.EmailSender.Services.EmailSender;
using LingoMQ.Infrastructure.EmailSender.Services.EmailSender.MailBuilders;
using MediatR;

namespace LingoMQ.Infrastructure.EmailSender.EventHandlers;

public class EmainSignUpEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailSender _sender;

    public EmainSignUpEventHandler(IEmailSender sender)
    {
        _sender = sender;
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.AuthorizationType.Type != AuthorizationTypeEnum.Email)
            return Task.CompletedTask;

        EmailBuilder builder = new MailConfirmation();
        builder.PrepareEmailModel(notification.ConfirmationToken);

        _sender.Send(
            builder,
            notification
                .User.Credentials.AuthorizationTypes.First(x =>
                    x.Type == AuthorizationTypeEnum.Email
                )
                .Value,
            "Регистрация"
        );

        return Task.CompletedTask;
    }
}
