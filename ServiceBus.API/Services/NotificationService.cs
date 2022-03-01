using SendGrid;
using SendGrid.Helpers.Mail;
using ServiceBus.Controllers.InputModels;

namespace SerivceBus.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;
        public NotificationService(ISendGridClient sendGridClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }
       
        public async Task Send(UserFollowingInputModel userFollowing)
        {
            var defaultSubject = _configuration.GetSection("Notification:DefaultSubject").Value;
            var from = new EmailAddress(_configuration.GetSection("Notification:DefaultFrom").Value, 
            _configuration.GetSection("Notification:DefaultFromName").Value);
            var to = new EmailAddress(userFollowing.Email, "Bruno");
            var body = $"User {userFollowing.IdUserFollower} followed you.";

            var message = new SendGridMessage
            {
                From = from,
                Subject = defaultSubject
            };

            message.AddContent(MimeType.Html, body);
            message.AddTo(to);

            await _sendGridClient.SendEmailAsync(message);
        }
    }
}