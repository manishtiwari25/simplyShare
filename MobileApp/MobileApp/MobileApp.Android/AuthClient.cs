using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;
using MobileApp.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(MobileApp.Droid.Services.AuthClient))]
namespace MobileApp.Droid.Services
{
    public class AuthClient:  IAuthClient
    {

        private Auth0Client _auth0Client;

        public AuthClient()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = Constants.DOMAIN,
                ClientId = Constants.ANDROID_CLIENT_ID,
                Scope = "openid offline_access"
            });
        }

        private async Task StoreTokenDataInSecureStorage(string idToken, string refereshToken, string exp)
        {

            await SecureStorage.SetAsync(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_TOKEN_KEY, idToken);
            await SecureStorage.SetAsync(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_TOKEN_EXPIRY_KEY, exp);
            await SecureStorage.SetAsync(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_REFERESH_TOKE_KEY, refereshToken);
        }

        public async Task<UserInfoResult> GetUserInfoAsync(string accessToken)
        {
            return await _auth0Client.GetUserInfoAsync(accessToken);
        }

        public async Task<LoginResult> LoginAsync(object extraParameters = null, CancellationToken cancellationToken = default)
        {
            var loginInfo = await _auth0Client.LoginAsync(extraParameters, cancellationToken);
            if (loginInfo != null && (!loginInfo.IsError))
                await StoreTokenDataInSecureStorage(loginInfo.IdentityToken, loginInfo.RefreshToken,loginInfo.AccessTokenExpiration.ToString());
            return loginInfo;
        }

        public async Task<BrowserResultType> LogoutAsync(bool federated = false, object extraParameters = null, CancellationToken cancellationToken = default)
        {
            SecureStorage.Remove(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_TOKEN_KEY);
            SecureStorage.Remove(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_REFERESH_TOKE_KEY);
            SecureStorage.Remove(Constants.SIMPLYSHARE_AUTH0_SECURESTORAGE_TOKEN_EXPIRY_KEY);
            return await _auth0Client.LogoutAsync(federated, extraParameters, cancellationToken);
        }

        public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken, object extraParameters = null, CancellationToken cancellationToken = default)
        {
            var refereshTokenInfo = await _auth0Client.RefreshTokenAsync(refreshToken, extraParameters, cancellationToken);
            if (refereshTokenInfo != null && (!refereshTokenInfo.IsError))
                await StoreTokenDataInSecureStorage(refereshTokenInfo.IdentityToken, refereshTokenInfo.RefreshToken, refereshTokenInfo.AccessTokenExpiration.ToString());
            return refereshTokenInfo;
        }
    }
}