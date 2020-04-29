using Autofac;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp
{
    public static class DIContainer
    {
        public static ISignalRClient SignalRClient
        {
            get
            {
                return DependencyService.Get<ISignalRClient>();
            }
        }
       
        public static IDeviceService Scan
        {
            get
            {
                return DependencyService.Get<IDeviceService>();
            }
        }
    }
}
