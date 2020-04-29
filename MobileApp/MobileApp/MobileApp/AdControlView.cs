using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MobileApp
{
    public class AdControlView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                      nameof(AdUnitId),
                      typeof(string),
                      typeof(AdControlView),
                      string.Empty);

        public string AdUnitId
        {
            get => Constants.AD_BANNER_ID;
        }
    }
}