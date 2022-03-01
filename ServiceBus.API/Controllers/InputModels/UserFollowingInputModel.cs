namespace ServiceBus.Controllers.InputModels
{
    public class UserFollowingInputModel
    {
        public int IdUserFollower {get;set;}
        public int IdUserFolowee {get;set;}
        public DateTime FolloedAt {get;set;} = DateTime.Now;
        public string Email {get;set;} = "destinatary...@gmail.com";
    }
}