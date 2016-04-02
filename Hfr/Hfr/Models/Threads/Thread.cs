using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using Hfr.Helpers;

namespace Hfr.Models.Threads
{
    public abstract class Thread : ViewModelBase, IThread
    {
        private int _threadNbPage;
        private int _threadCurrentPage;
        private bool _threadHasNewPost;
        private string _url;

        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        public string ThreadAuthor { get; set; }
        public string ThreadUri
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
                    _url = _url.Replace($"&page={page}", $"&page={ThreadCurrentPage}");
                }
                else
                {
                    // For standard topics
                    // Standard case : url ends with "_1.htm"
                    int lastUnderlineIndex = _url.LastIndexOf('_');
                    _url = _url.Substring(0, lastUnderlineIndex);
                    _url += $"_{ThreadCurrentPage}.htm";
                }
                return _url;
            }
            set { _url = value; }
        }
        public DateTime ThreadLastPostDate { get; set; }
        public string ThreadLastPostMemberPseudo { get; set; }

        public TimeSpan ThreadLastPostTimeSpan
        {
            get
            {
                var currentTimeInFrance = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
                return currentTimeInFrance.Subtract(ThreadLastPostDate);
            }
        }

        public string ThreadLastPostText
        {
            get { return ThreadNameHelper.TimeSinceLastReadMsg(ThreadLastPostTimeSpan, ThreadLastPostMemberPseudo); }
        }

        public bool ThreadHasNewPost
        {
            get { return _threadHasNewPost; }
            set { Set(ref _threadHasNewPost, value); }
        }

        public int ThreadFirstPage => 1;

        public int ThreadNbPage
        {
            get { return _threadNbPage; }
            set
            {
                Set(ref _threadNbPage, value);
            }
        }

        public int ThreadCurrentPage
        {
            get { return _threadCurrentPage; }
            set
            {
                Set(ref _threadCurrentPage, value);
                RaisePropertyChanged(nameof(CanGoForward));
                RaisePropertyChanged(nameof(CanGoPrevious));
            }
        }

        public int ThreadBookmarkId { get; set; }
        public string ThreadNewPostUriForm { get; set; }

        public bool CanGoForward
        {
            get { return ThreadCurrentPage < ThreadNbPage; }
        }

        public bool CanGoPrevious
        {
            get { return ThreadCurrentPage > 1; }
        }

        public abstract FontWeight FontWeight { get; }
        public abstract SolidColorBrush Foreground { get; }
    }
}
