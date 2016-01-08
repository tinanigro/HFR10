using System;
using Hfr.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HtmlAgilityPack;
using Hfr.Utilities;
using System.Diagnostics;
using System.Net;

namespace Hfr.Helpers
{
    public static class SmileyFetcher
    {
        public static async Task<List<Smiley>> Fetch(string keyword)
        {
            var html = await HttpClientHelper.Get(HFRUrl.WikiSmileySearch + keyword);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var smileyNodes = htmlDoc.DocumentNode.Descendants("th").Where(x => x.GetAttributeValue("class", "") == "cBackTab1" && x.GetAttributeValue("width","") == "16%");

            if (smileyNodes == null || !smileyNodes.Any()) return null;

            var smileys = new List<Smiley>();
            foreach (var smileyNode in smileyNodes)
            {
                var imgNode = smileyNode.FirstChild;
                var src = imgNode.GetAttributeValue("src", "");
                var tag = imgNode.GetAttributeValue("title", "").CleanFromWeb();
                var smiley = new Smiley(src, tag);
                Debug.WriteLine($"src:{src}");
                smileys.Add(smiley);
            }
            return smileys;
        }
    }
}
