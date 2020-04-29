using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.AspNetCore.SignalR.Client;
using Android.Content;
using Auth0.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MobileApp.Models;
using Xamarin.Essentials;
using Acr.UserDialogs;
using Android.Gms.Ads;
using Xamarin.Forms.Platform.Android;
using Microsoft.Identity.Client;

namespace MobileApp.Droid
{
    [Activity(Label = "simplyShare", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsApplicationActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);

            UserDialogs.Init(this);
            MobileAds.Initialize(ApplicationContext, Constants.AD_APPLICATION_ID);

            LoadApplication(new App());
            App.UIParent = this;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}