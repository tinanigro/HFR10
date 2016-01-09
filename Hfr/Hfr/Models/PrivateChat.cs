using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Hfr.ViewModel;

namespace Hfr.Models
{
    public class PrivateChat
    {
        public string Subject { get; set; }
        public string Poster { get; set; }
        public bool NewMsg { get; set; }
        public DateTime DateTime { get; set; }

        public string DateTimeString => DateTime.ToString("g", new CultureInfo("fr-FR"));
        public SolidColorBrush Foreground
        {
            get
            {
                if (NewMsg)
                {
                    return (SolidColorBrush)App.Current.Resources["SystemControlHighlightAltListAccentMediumBrush"];
                }
                if (Loc.Settings.IsApplicationThemeDark)
                    return new SolidColorBrush(Colors.WhiteSmoke);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }
    }
}
