using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;

namespace Hfr.Models.Threads
{
    interface IThread
    {
        int ThreadId { get; set; }
        string ThreadName { get; set; }
        string ThreadAuthor { get; set; }
        string ThreadUri { get; set; }

        DateTime ThreadLastPostDate { get; set; }
        string ThreadLastPostMemberPseudo { get; set; }
        string ThreadLastPostText { get; }
        TimeSpan ThreadLastPostTimeSpan { get; }

        bool ThreadHasNewPost { get; set; }
        int ThreadFirstPage { get; }
        int ThreadNbPage { get; set; }
        int ThreadCurrentPage { get; set; }
        int ThreadBookmarkId { get; set; }
        string ThreadNewPostUriForm { get; set; }

        FontWeight FontWeight { get; }
        SolidColorBrush Foreground { get; }

        bool CanGoForward { get; }
        bool CanGoPrevious { get; }
    }
}
