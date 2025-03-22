// namespace LingoMQ.Infrastructure.EmailSender.Services.EmailSender.MailBuilders;

// public class MailServiceStability : EmailBuilder
// {
//     public override void PrepareEmailModel(object value)
//     {
//         StabilityCheckEvent model = (StabilityCheckEvent)value;
//         Template.Header = $"Проблемы с сервисом {model.ServiceName}!";

//         Template.Contents.Add(
//             $"С сервисом {model.ServiceName} сейчас не все в порядке. Его статус: {model.Status}"
//         );
//     }
// }
