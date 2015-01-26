using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace HFR4WinRT.Model
{
    public class Topic : ViewModelBase
    {
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
    }
}
