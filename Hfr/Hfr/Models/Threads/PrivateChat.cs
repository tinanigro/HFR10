using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using Hfr.Helpers;
using Hfr.ViewModel;

namespace Hfr.Models.Threads
{
    public class PrivateChat : Thread
    {
        public override SolidColorBrush Foreground
        {
            get
            {
                if (ThreadHasNewPost)
                {
                    return (SolidColorBrush)App.Current.Resources["SystemControlHighlightAltListAccentMediumBrush"];
                }
                if (Loc.Settings.IsApplicationThemeDark)
                    return new SolidColorBrush(Colors.WhiteSmoke);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public override FontWeight FontWeight
        {
            get { return FontWeights.Normal; }
        }
    }
}