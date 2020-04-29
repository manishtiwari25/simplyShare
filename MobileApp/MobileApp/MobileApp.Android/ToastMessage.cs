using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MobileApp.Droid.Services.ToastMessage))]
namespace MobileApp.Droid.Services
{
    public class ToastMessage :  IToastMessage
    {
        public void ShowLong(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShowShort(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}