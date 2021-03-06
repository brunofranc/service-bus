using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using SendGrid;
using SerivceBus.Services;
using ServiceBus.Controllers.InputModels;

namespace SerivceBus.Consumers
{
    public class EmailNotificationMessageConsumer
    {
     private readonly IConfiguration _configuration;
        private readonly QueueClient _queueClient;
        private readonly INotificationService _notificationService;
        private const string QUEUE_NAME = "email-queue";
        public EmailNotificationMessageConsumer(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _notificationService = new NotificationService(sendGridClient, _configuration);
            var connectionString = _configuration.GetSection("ServiceBus:ConnectionString").Value;
            _queueClient = new QueueClient(connectionString, QUEUE_NAME);
        }

        public void RegisterHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessageHandler, messageHandlerOptions);
        }

        private async Task ProcessMessageHandler(Message message, CancellationToken cancellationToken)
        {
            var messageString = Encoding.UTF8.GetString(message.Body);
            var userFollowingInputModel = JsonConvert.DeserializeObject<UserFollowingInputModel>(messageString);
            await _notificationService.Send(userFollowingInputModel);

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}