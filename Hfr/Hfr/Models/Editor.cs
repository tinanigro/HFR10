using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Dictionary<String, String> Data { get; set; }

        public int idxTopic; //Topic index in MainViewModel <Topics>
        

        public string Text
        {
            get
            {
                string value;
                if (Data.TryGetValue("content_form", out value))
                {
                    return Data["content_form"];
                }
                return "";
            }
            set
            {
                Data["content_form"] = value.ToString();
            }
        }

        
        public void PrepareForSubmit()
        {

            FixNewLines();
            SecureCleanUp();
        }

        public void FixNewLines()
        {
            Data["content_form"] = Data["content_form"].Replace("\r\n", "\r");
            Data["content_form"] = Data["content_form"].Replace("\r", Environment.NewLine);
        }

        public void SecureCleanUp()
        {
            //fix
            DeleteDataForKey("allowvisitor", "0");
            DeleteDataForKey("have_sondage", "0");
            DeleteDataForKey("sticky", "0");
            DeleteDataForKey("sticky_everywhere", "0");

            //delete super safety
            DeleteDataForKey("delete", "0");

        }

        private void DeleteDataForKey(string key, string condition)
        {
            string value;

            if (Data.TryGetValue(key, out value))
            {
                if (value == condition)
                {
                    Data.Remove(key);
                }
            }
        }
        
    }
}