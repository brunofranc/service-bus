using ServiceBus.Controllers.InputModels;

namespace SerivceBus.Services
{
    public interface INotificationService
    {
        Task Send(UserFollowingInputModel userFollowing);
    }
}