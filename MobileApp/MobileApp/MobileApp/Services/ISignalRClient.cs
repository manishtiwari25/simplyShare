using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public interface ISignalRClient
    {
        Task EstablishConnection();
        HubConnection GetHubConnection();
        Task SendMessage(string userId, string message,string hub);
        Task CloseConnection();
        Task ReconnectConnection();
        Task StopConnection();

        Task<string> GetConnectionId();
    }
}
