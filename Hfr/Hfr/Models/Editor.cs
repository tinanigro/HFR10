using System;
using System.Collections.Generic;
using Windows.System.Profile;
using GalaSoft.MvvmLight;
using Hfr.Models;

namespace Hfr.Model
{
    public class Editor : ViewModelBase
    {
        private EditorIntent _intent;

        public EditorIntent Intent
        {
            get { return _intent; }
            set { Set(ref _intent, value); }
        }

        public string FromUrl { get; set; }
        public string SubmitUrl { get; set; }

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
                    var text = Data["content_form"].Trim(); // We trim to prevent empty string with spare odd \r or \n
                    // However if the string is not empty after a trim, it then contains something, and we want to add an extra NewLine for the user
                    if (!string.IsNullOrEmpty(text))
                    {
                        text += Environment.NewLine;
                    }
                    return text;
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
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile") // If Mobile, add the Phone icon to the Msg
            {
                Data["MsgIcon"] = "20";
            }
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