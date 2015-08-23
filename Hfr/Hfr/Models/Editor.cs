using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Hfr.Model;

namespace Hfr.Models
{
    public class Editor : ViewModelBase
    {
        public string FromUrl { get; set; }
        public string SubmitUrl { get; set; }

        private string _text;

        public string Title; //MP & New Topic.
        public string To; //MP only, recipient.
        public string Category; //NT only.

        public int idxTopic; //Topic index in MainViewModel <Topics>

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}