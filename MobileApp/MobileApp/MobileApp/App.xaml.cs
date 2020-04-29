using Microsoft.Identity.Client;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    public partial class App : Application
    {
        public static IPublicClientApplication AuthenticationClient { get; private set; }
        public static object UIParent { get; set; } = null;
        public App()
        {
            InitializeComponent();
            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
           .WithB2CAuthority(Constants.AuthoritySignin)
           .WithRedirectUri($"msal{Constants.ClientId}://auth")
           .Build();

            MainPage = new NavigationPage(new LoginPage());
        }
        protected override void CleanUp()
        {
            DIContainer.SignalRClient.CloseConnection();
        }
    }
}
