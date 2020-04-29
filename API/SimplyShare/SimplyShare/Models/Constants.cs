namespace SimplyShare.Models
{
    public static class Constants
    {
        public const string AZUREKEYVAULT_SIGNALR_CONNECTIONSTRING_KEY = "SignalRConnectionString";
        public const string AZUREKEYVAULT_B2C_TENANT = "B2CTenant";
        public const string AZUREKEYVAULT_B2C_CLIENT_ID = "B2CClientId";

        public const string AUTHORIZATION_HEADER = "Authorization";

        public const string MOBILE_IDENTIFIER_HEADER = "x-origin";

        public const string SCHEMA_NAME = "ADB2C";

        public const string AUTH_CALLBACK_PATH = "/signin-oidc";
        public const string POLICY_SIGNIN_OR_SIGNUP = "B2C_1_SignInOrSignUp";
        public const string POLICY_PASSWORD_RESET = "B2C_1_PasswordReset";
    }
}
