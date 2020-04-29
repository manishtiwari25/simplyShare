using Microsoft.Identity.Client;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.Title = Constants.TITLE;
            InitializeComponent();
        }
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
            await DIContainer.SignalRClient.EstablishConnection();
            if (Application.Current.Properties.ContainsKey(Constants.CONNECT_PAGE_DISABALED))
            {
                Application.Current.Properties.TryGetValue(Constants.CONNECT_PAGE_DISABALED, out object isMainPageDisabled);
                if (bool.TryParse((string)isMainPageDisabled, out var res) && res)
                {
                    var sendOrReceive = new SendOrReceivePage();
                    await Navigation.PushAsync(sendOrReceive);
                }
            }
        }
        protected void DontShowPage(object sender, ToggledEventArgs e)
        {
            UpsertProperties(Constants.CONNECT_PAGE_DISABALED, e.Value.ToString());
        }

        protected async void OnConnectClicked(object sender, EventArgs args)
        {
            var sendOrReceive = new SendOrReceivePage();
            await Navigation.PushAsync(sendOrReceive);
        }
    }
}
