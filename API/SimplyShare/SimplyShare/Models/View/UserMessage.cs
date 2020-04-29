namespace SimplyShare.Models.View
{
    public class UserMessage
    {
        public string UserId { get; set; }
        public string UserText { get; set; }

        public UserMessage(string userId, string userText)
        {
            UserId = userId;
            UserText = userText;
        }
    }
}
