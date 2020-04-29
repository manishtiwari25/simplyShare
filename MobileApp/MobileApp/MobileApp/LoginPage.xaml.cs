using Acr.UserDialogs;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.Title = Constants.TITLE;
            InitializeComponent();
        }


        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            AuthenticationResult result;
            try
            {
                result = await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .ExecuteAsync();
                await SecureStorage.SetAsync(Constants.SIMPLYSHARE_SECURESTORAGE_TOKEN_KEY, result.IdToken);
                await Navigation.PushAsync(new MainPage());
            }
            catch (MsalException ex)
            {
                if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                {
                    result = await OnForgotPassword();
                    await SecureStorage.SetAsync(Constants.SIMPLYSHARE_SECURESTORAGE_TOKEN_KEY, result.IdToken);
                    await Navigation.PushAsync(new MainPage());
                }
                else if (ex.ErrorCode != "authentication_canceled")
                {
                    await UserDialogs.Instance.AlertAsync("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }
        async Task<AuthenticationResult> OnForgotPassword()
        {
            try
            {
                return await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .WithB2CAuthority(Constants.AuthorityPasswordReset)
                    .ExecuteAsync();
            }
            catch (MsalException)
            {
                // Do nothing - ErrorCode will be displayed in OnLoginButtonClicked
                return null;
            }
        }
        protected override async void OnAppearing()
        {
            using (UserDialogs.Instance.Loading("Checking For Existing Account"))
            {
                try
                {
                    // Look for existing account
                    IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();

                    AuthenticationResult result = await App.AuthenticationClient
                        .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                    await SecureStorage.SetAsync(Constants.SIMPLYSHARE_SECURESTORAGE_TOKEN_KEY, result.IdToken);
                    await Navigation.PushAsync(new MainPage());
                }
                catch
                {
                    // Do nothing - the user isn't logged in
                }
                base.OnAppearing();
            }
        }
    }
}