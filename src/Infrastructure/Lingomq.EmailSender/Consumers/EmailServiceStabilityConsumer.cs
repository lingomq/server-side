// using LingoMQ.EmailSender.Application.Services.EmailSender;
// using LingoMQ.EmailSender.Application.Services.EmailSender.MailBuilders;
// using LingoMQ.EventBusDotnetLib.Senders;
// using LingoMQ.EventBusDotnetLib.WatchDog;
// using MassTransit;
// using Microsoft.Extensions.Configuration;

// namespace LingoMQ.Infrastructure.EmailSender.Consumers;

// public class EmailServiceStabilityConsumer : IConsumer<SendersStatusCheckedEvent>
// {
//     private readonly IEmailSender _sender;
//     private readonly IConfiguration _configuration;

//     public EmailServiceStabilityConsumer(IEmailSender sender, IConfiguration configuration)
//     {
//         _sender = sender;
//         _configuration = configuration;
//     }

//     public Task Consume(ConsumeContext<SendersStatusCheckedEvent> context)
//     {
//         EmailBuilder builder = new MailServiceStability();
//         builder.PrepareEmailModel(
//             new StabilityCheckEvent()
//             {
//                 ServiceName = context.Message.ServiceName,
//                 Status = context.Message.Status,
//             }
//         );

//         _sender.Send(
//             builder,
//             _configuration["Mail:ServiceMail"]!,
//             $"Проблемы с сервисом {context.Message.ServiceName}"
//         );

//         return Task.CompletedTask;
//     }
// }
