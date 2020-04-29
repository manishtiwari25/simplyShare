namespace SimplyShare.Models
{
    public class SignalRMessage
    {
        public bool IsNegotiation { get; set; }
        public string Message { get; set; }

        public string Type { get; set; }

        public SignalRMessage()
        {

        }
        public SignalRMessage(bool isNegotiation, string message = null, string type = null)
        {
            IsNegotiation = isNegotiation;
            Message = message;
            Type = type;
        }
    }
}
