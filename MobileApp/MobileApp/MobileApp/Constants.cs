using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    public static class Constants
    {
        #region SignalR
        public const string HOST_NAME = "https://simplyshare.azurewebsites.net/usermessagehub";
        
        public const string SIGNALR_USER_KEY  = "SignalRUser";
        public const string CONNECT_PAGE_DISABALED = "IsConnectPageDisabled";

        public const string SIGNALR_HUB_NEGOTIATE = "Negotiate";
        public const string SIGNALR_HUB_SENDMESSAGE = "Send"; 
        public const string SIGNALR_HUB_RECEIVEMESSAGE = "Receive";

        public const string GET_CONNECTION_ID = "getConnectionId";
        #endregion
        public static string TITLE => tenant_name;
        #region Azure Ad B2c
        const string tenant_name = "simplyShare";
        const string tenant_id = "simplyShare.onmicrosoft.com";
        const string clientid = "3df797e5-0f28-440c-8303-26d74d7603f8";
        const string policy_signin_or_signup = "B2C_1_SignInOrSignUp";
        const string policy_password_reset = "B2C_1_PasswordReset";
        static readonly string[] scopes = { "" };
        static readonly string authorityBase = $"https://{tenant_name}.b2clogin.com/tfp/{tenant_id}/";
        public static string ClientId
        {
            get
            {
                return clientid;
            }
        }
        public static string AuthoritySignin
        {
            get
            {
                return $"{authorityBase}{policy_signin_or_signup}";
            }
        }
        public static string[] Scopes
        {
            get
            {
                return scopes;
            }
        }
        public static string AuthorityPasswordReset
        {
            get
            {
                return $"{authorityBase}{policy_password_reset}";
            }
        }

        public const string SIMPLYSHARE_SECURESTORAGE_TOKEN_KEY = "simplyShare_authtoken_key";
        #endregion

        #region AdMob
        public const string AD_BANNER_ID = "ca-app-pub-2889277787752693/2619325994";
        public const string AD_DEBUG_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        public const string AD_APPLICATION_ID = "ca-app-pub-2889277787752693~6458498651";
        #endregion
    }
}
