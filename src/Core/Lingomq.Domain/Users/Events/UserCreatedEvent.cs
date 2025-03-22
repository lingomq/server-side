using MediatR;

namespace LingoMQ.Core.Domain.Users.Events;

public class UserCreatedEvent : INotification
{
    public User User { get; set; }
    public AuthorizationType AuthorizationType { get; set; }
    public string ConfirmationToken { get; set; }

    public UserCreatedEvent(User user, AuthorizationType authorizationType, string confirmationToken = "")
    {
        User = user;
        AuthorizationType = authorizationType;
        ConfirmationToken = confirmationToken;
    }
}
