using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Models;
using System.Diagnostics;
using Hfr.ViewModel;
using HtmlAgilityPack;
using System.Net;

namespace Hfr.Helpers
{
    public static class FormFetcher
    {
        public static async Task GetEditor(string url)
        {
            Debug.WriteLine("Fetching Form");
            var editor = await Fetch(url);
            Debug.WriteLine("Updating UI with new Editor");

            await ThreadUI.Invoke(() =>
            {
                Loc.Editor.CurrentEditor = editor;
            });
        }

        public static async Task<Editor> Fetch(string FormUrl)
        {
            var html = await HttpClientHelper.Get(FormUrl);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            
            string[] editText = htmlDoc.DocumentNode.Descendants("textarea").Where(x => (string)x.GetAttributeValue("name", "") == "content_form").
            Select(y => y.InnerText).ToArray();

            var content = WebUtility.HtmlDecode(editText[0]) + Environment.NewLine;
         
            return new Editor()
            {
                FromUrl = FormUrl,
                Text = content,
                idxTopic = 0,
            };

        }
    }
}
