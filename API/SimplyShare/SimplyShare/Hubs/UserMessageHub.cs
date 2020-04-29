using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace SimplyShare.Hubs
{
    [Authorize(AuthenticationSchemes = Models.Constants.SCHEMA_NAME)]
    public class UserMessageHub : Hub
    {
        private ILogger<UserMessageHub> _logger;

        public UserMessageHub(ILogger<UserMessageHub> logger)
        {
            _logger = logger;
        }
        public async Task Negotiate(string userId, string message)
        {
            _logger.LogTrace(default(EventId), message);
            await Clients.Client(userId).SendAsync("Negotiate", message);
        }
        public async Task Send(string userId, string message)
        {
            _logger.LogTrace(default(EventId), message);
            await Clients.Client(userId).SendAsync("Send", message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}