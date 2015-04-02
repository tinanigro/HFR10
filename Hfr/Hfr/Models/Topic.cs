using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Hfr.Model
{
    public class Topic : ViewModelBase
    {
        private string _html;
        public string TopicName { get; set; }
        public int TopicCatId { get; set; }
        public string TopicSubCatId { get; set; }
        public string TopicId { get; set; }
        public string TopicCatName { get; set; }
        public double TopicLastPostDate { get; set; }
        public string TopicLastPost { get; set; }
        public TimeSpan TopicLastPostTimeSpan { get; set; }
        public int TopicNbPage { get; set; }
        public int TopicCurrentPage { get; set; }
        public string TopicReponseId { get; set; }
        public int TopicIndexCategory { get; set; }

        public ObservableCollection<Post> Posts { get; set; }

        public string Html
        {
            get { return _html; }
            set { Set(ref _html, value); }
        }
    }
}
