using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Models.Threads
{
    public class Topic : Thread
    {

        public override FontWeight FontWeight
        {
            get
            {
                return TopicIsSticky ? FontWeights.SemiBold : FontWeights.Normal;
            }
        }

        public override SolidColorBrush Foreground
        {
            get
            {
                if (TopicIsSticky)
                {
                    return (SolidColorBrush)App.Current.Resources["SystemControlHighlightAltListAccentMediumBrush"];
                }
                else if (TopicIsClosed)
                {
                    return (SolidColorBrush)App.Current.Resources["SystemControlForegroundBaseLowBrush"];
                }
                if (Loc.Settings.IsApplicationThemeDark)
                    return new SolidColorBrush(Colors.WhiteSmoke);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public int TopicCatId { get; set; }
        public int TopicSubCatId { get; set; }

        public bool TopicIsSticky { get; set; }
        public bool TopicIsClosed { get; set; }


        public string TopicCatName
        {
            get { return HFRCats.PlainNameFromId(TopicCatId); }
        }
    }
}
