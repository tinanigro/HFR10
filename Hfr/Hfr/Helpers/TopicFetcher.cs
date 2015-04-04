using Hfr.ViewModel;
using Hfr.Views.MainPages;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hfr.Helpers
{
    public static class TopicFetcher
    {
        public static async Task GetPosts(int catId, string topicId, int topicNbPage)
        {
            Debug.WriteLine("Fetching Posts");
            await Fetch(catId, topicId, topicNbPage);
            Debug.WriteLine("Updating UI with new Posts list");
        }

        public static async Task Fetch(int catId, string topicId, int topicNbPage)
        {
            var html = await HttpClientHelper.Get("http://forum.hardware.fr/forum2.php?config=hfr.inc&cat=" + catId + "&post=" + topicId + "&page=" + topicNbPage + "&sondage=1&owntopic=1", Loc.Main.AccountManager.CurrentAccount.CookieContainer);
            if (string.IsNullOrEmpty(html)) return;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html); await ThreadUI.Invoke(() =>
            {
                Loc.Main.CurrentTopic.Html = html;

                var topicText = htmlDoc.DocumentNode.Descendants("div")
                        .Where(x => x.GetAttributeValue("id", "").Contains("para"))
                        .
                        Select(y => y.InnerHtml).ToArray();

                var messCase1 = htmlDoc.DocumentNode.Descendants("td").Where(x =>
                            x.GetAttributeValue("class", "").Contains("messCase1") &&
                            x.InnerHtml.Contains("<div><b class=\"s2\">Publicité</b></div>") == false &&
                            x.InnerHtml.Contains("Auteur") == false)
                            .Select(x => x.InnerHtml).ToArray();

                int i = 0;
                string posts = "";
                // This is absolutely quick and dirty code
                var body = "<html><head><link type=\"text/css\" rel=\"stylesheet\" href=\"http://files.thomasnigro.fr/hfr/Hfr/styletopic.css\"</link></head><body>";
                posts += body;
                foreach (var post in topicText)
                {
                    var div = "<div>";
                    // pseudo
                    int firstPseudo = messCase1[i].IndexOf("<b class=\"s2\">", StringComparison.Ordinal) +
                                      "<b class=\"s2\">".Length;
                    int lastPseudo = messCase1[i].LastIndexOf("</b>", StringComparison.Ordinal);
                    var posterPseudo = messCase1[i].Substring(firstPseudo, lastPseudo - firstPseudo);
                    posterPseudo = posterPseudo.Replace(((char)8203).ToString(), ""); // char seperator (jocebug)


                    // post text
                    int lastPostText = topicText[i].IndexOf("<div style=\"clear: both;\"> </div>", StringComparison.Ordinal);
                    if (lastPostText == -1)
                    {
                        lastPostText = topicText[i].Length;
                    }
                    var postText = topicText[i].Substring(0, lastPostText);
                    postText = Regex.Replace(WebUtility.HtmlDecode(postText), " target=\"_blank\"", "");
                    Debug.WriteLine(postText);
                    posts += div;
                    posts += "<h2>" + posterPseudo + "</h2>";
                    posts += postText;
                    posts += "</div>";
                    i++;
                }
                posts += "</body>";

                (Loc.NavigationService.CurrentPage as MainPage).TopicWebView.NavigateToString(posts);
            });
        }
    }
}
