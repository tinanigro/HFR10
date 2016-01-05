using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using Hfr.ViewModel;

namespace Hfr.Model
{

    public class Topic : ViewModelBase
    {
        private string _url;
        private string _html;
        private string _topicReponseId;
        private int _topicCurrentPage = 1;

        public string TopicName { get; set; }
        public string TopicAuthor { get; set; }
        public bool TopicIsSticky { get; set; }
        public bool TopicIsClosed { get; set; }

        public int TopicCatId { get; set; }
        public string TopicSubCatId { get; set; }
        public string TopicId { get; set; }
        public string TopicCatName { get; set; }
        public double TopicLastPostDate { get; set; }
        public string TopicLastPost { get; set; }
        public TimeSpan TopicLastPostTimeSpan { get; set; }

        public int TopicFirstPage => 1;

        public int TopicNbPage { get; set; }

        public int TopicCurrentPage
        {
            get { return _topicCurrentPage; }
            set
            {
                Set(ref _topicCurrentPage, value);
                RaisePropertyChanged(nameof(CanGoForward));
                RaisePropertyChanged(nameof(CanGoPrevious));
            }
        }

        public string TopicReponseId
        {
            get { return _topicReponseId?.Replace("rep", ""); }
            set { _topicReponseId = value; }
        }

        public string TopicNewPostUriForm { get; set; }

        public int TopicIndexCategory { get; set; }

        public string TopicDrapURI
        {
            get
            {
                WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(_url);
                // Manually set the desired page
                // For flags
                var pageKeyValue = decoder.FirstOrDefault(x => x.Name == "page");
                if (pageKeyValue != null)
                {
                    var page = pageKeyValue.Value;
                    _url = _url.Replace($"&page={page}", $"&page={TopicCurrentPage}");
                }
                else
                {
                    // For standard topics
                    // Standard case : url ends with "_1.htm"
                    int lastUnderlineIndex = _url.LastIndexOf('_');
                    _url = _url.Substring(0, lastUnderlineIndex);
                    _url += $"_{TopicCurrentPage}.htm";
                }
                return _url;
            }
            set { _url = value; }
        }
        
        public string Html
        {
            get { return _html; }
            set { Set(ref _html, value); }
        }

        public FontWeight FontWeight
        {
            get
            {
                return TopicIsSticky ? FontWeights.SemiBold : FontWeights.Normal;
            }
        }

        public SolidColorBrush Foreground
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
                return new SolidColorBrush(Colors.WhiteSmoke);
            }
        }

        public bool CanGoForward
        {
            get { return TopicCurrentPage < TopicNbPage; }
        }

        public bool CanGoPrevious
        {
            get { return TopicCurrentPage > 1; }
        }
    }
}
