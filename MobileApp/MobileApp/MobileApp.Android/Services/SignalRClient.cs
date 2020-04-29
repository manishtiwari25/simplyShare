using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.AspNetCore.SignalR.Client;
using MobileApp.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(MobileApp.Droid.Services.SignalRClient))]
namespace MobileApp.Droid.Services
{
    public class SignalRClient : ISignalRClient
    {
        private HubConnection hubConnection;

        public async Task CloseConnection()
        {
            await hubConnection.DisposeAsync();
        }

        public async Task ReconnectConnection()
        {
            using (UserDialogs.Instance.Loading("Re-Establishing Secure Connection"))
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                    await hubConnection.StartAsync();
                while (hubConnection.State != HubConnectionState.Connected);
            }
        }

        public HubConnection GetHubConnection()
        {
            return hubConnection;
        }
        public async Task StopConnection()
        {
            await hubConnection.StopAsync();
        }
        public async Task SendMessage(string userId, string message, string hub)
        {
            using (UserDialogs.Instance.Loading("Sending Data"))
            {
                await hubConnection.InvokeAsync(hub, userId, message);
            }
        }

        public async Task<string> GetConnectionId()
        {
            using (UserDialogs.Instance.Loading("Fetching Connection Details"))
            {
                string connectionId = await hubConnection.InvokeAsync<string>(Constants.GET_CONNECTION_ID);
                return connectionId;
            }
        }

        public async Task EstablishConnection()
        {
            if (hubConnection == null)
            {
                using (UserDialogs.Instance.Loading("Establishing Secure Connection"))
                {
                    hubConnection = new HubConnectionBuilder().WithUrl(Constants.HOST_NAME, async options =>
                    {
                        var token = await SecureStorage.GetAsync(Constants.SIMPLYSHARE_SECURESTORAGE_TOKEN_KEY);
                        options.Headers.Add("Authorization", $"Bearer {token}");
                        options.Headers.Add("x-origin", "Mobile");
                    }).WithAutomaticReconnect().Build();
                    await hubConnection.StartAsync();
                    while (hubConnection.State != HubConnectionState.Connected) ;
                }
            }
        }
    }
}