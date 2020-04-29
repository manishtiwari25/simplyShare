using Acr.UserDialogs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Identity.Client;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendOrReceivePage : ContentPage
    {
        private bool copyFromClipboard;
        private string editorText;
        private bool editorDisabled;

        ObservableCollection<SignalRMessage> signalRMessages = new ObservableCollection<SignalRMessage>();
        public ObservableCollection<SignalRMessage> SignalRMessages { get { return signalRMessages; } }

        private async void LogoutClicked(object sender, EventArgs e) 
        {
            IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();
            while (accounts.Any())
            {
                await App.AuthenticationClient.RemoveAsync(accounts.First());
                accounts = await App.AuthenticationClient.GetAccountsAsync();
            }
            await Navigation.PushAsync(new LoginPage());
        }
       
        private void UpsertProperties(string key, string value)
        {
            if (!Application.Current.Properties.ContainsKey(key))
                Application.Current.Properties.Add(key, value);
            else
            {
                Application.Current.Properties.Remove(key);
                Application.Current.Properties.Add(key, value);
            }
        }
        protected async override void OnAppearing()
        {
            if (DIContainer.SignalRClient.GetHubConnection().State == HubConnectionState.Connected && signalRMessages.Count == 0)
            {
                DIContainer.SignalRClient.GetHubConnection().On<string>(Constants.SIGNALR_HUB_SENDMESSAGE, (message) =>
                {
                    var msg = JsonSerializer.Deserialize<SignalRMessage>(message);
                    signalRMessages.Add(msg);
                });
            }
            var result = await DIContainer.Scan.ScanAsync();
            if (!string.IsNullOrEmpty(result))
            {
                UpsertProperties(Constants.SIGNALR_USER_KEY, result);
                var id = await DIContainer.SignalRClient.GetConnectionId();
                var message = new SignalRMessage(true, id);
                var textForSend = System.Text.Json.JsonSerializer.Serialize(message);

                await DIContainer.SignalRClient.SendMessage(result, textForSend, Constants.SIGNALR_HUB_NEGOTIATE);
                Vibration.Vibrate();
            }
        }

        public SendOrReceivePage()
        {
            this.Title = Constants.TITLE;
            InitializeComponent();
            MyListView.ItemsSource = SignalRMessages;
            MyListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var item = (SignalRMessage)e.SelectedItem;
                UserDialogs.Instance.Toast("Copied to clipboard");
                await Clipboard.SetTextAsync(item.Message);
            };
        }
        async void SendData(object sender, EventArgs args)
        {
            if (Application.Current.Properties.TryGetValue(Constants.SIGNALR_USER_KEY, out var signalRUser))
            {
                if (copyFromClipboard && Clipboard.HasText)
                {
                    var clipboardText = await Clipboard.GetTextAsync();
                    var message = new SignalRMessage(false, clipboardText, nameof(String));
                    var textForSend = System.Text.Json.JsonSerializer.Serialize(message);
                    await DIContainer.SignalRClient.SendMessage(signalRUser.ToString(), textForSend, Constants.SIGNALR_HUB_SENDMESSAGE);
                }
                else if (editorDisabled)
                {
                    var message = new SignalRMessage(false, editorText, nameof(String));
                    var textForSend = System.Text.Json.JsonSerializer.Serialize(message);
                    await DIContainer.SignalRClient.SendMessage(signalRUser.ToString(), textForSend, Constants.SIGNALR_HUB_SENDMESSAGE);
                }
            }
        }
        void SwitchToggle(object sender, ToggledEventArgs e)
        {
            copyFromClipboard = e.Value;
            var editor = (Editor)FindByName("editor");
            editor.IsVisible = !copyFromClipboard;
            editor.IsEnabled = !copyFromClipboard;
            editorDisabled = editor.IsVisible;
        }
        void HandleEditorText(object sender, EventArgs editorEventArgs)
        {
            editorText = ((Editor)sender).Text;
        }
    }
}