using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class SignalRMessage
    {
        public bool IsNegotiation { get; set; }
        public string Message { get; set; }

        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Flow { get; set; }
        public SignalRMessage()
        {

        }
        public SignalRMessage(bool isNegotiation, string message = null, string type = null)
        {
            IsNegotiation = isNegotiation;
            Message = message;
            Type = type;
            DateTime = DateTime.Now;
            Flow = "Incoming";
        }
    }
}
