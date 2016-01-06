using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using Hfr.Views.MainPages;
using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace Hfr.Helpers
{
    public static class TopicFetcher
    {
        public static async Task GetPosts(Topic currentTopic)
        {
            Debug.WriteLine("Fetching Posts");
            await Fetch(currentTopic);
            Debug.WriteLine("Updating UI with new Posts list");
        }

        public static async Task Fetch(Topic currentTopic)
        {
            await ThreadUI.Invoke(() =>
            {
                Loc.Topic.IsTopicLoading = true;
            });

            var html = await HttpClientHelper.Get(currentTopic.TopicDrapURI);
            if (string.IsNullOrEmpty(html)) return;

            await ThreadUI.Invoke(() =>
            {
                Loc.Topic.CurrentTopic.Html = html;
            });

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var postNodes =
                htmlDoc.DocumentNode.Descendants("table")
                    .Where(x => x.GetAttributeValue("class", "") == "messagetable")
                    .ToArray();

            if (postNodes == null || !postNodes.Any()) return;

            string TempHTMLMessagesList = "";
            string TempHTMLMessage = "";
            string TempHTMLTopic = "";

            string BodyTemplate = "";
            string MessageTemplate = "";

            // This is absolutely quick and dirty code :o
            Assembly asm = typeof(App).GetTypeInfo().Assembly;

            using (Stream stream = asm.GetManifestResourceStream(HFRRessources.Tpl_Topic))
            using (StreamReader reader = new StreamReader(stream))
            {
                BodyTemplate = reader.ReadToEnd();
            }

            using (Stream stream = asm.GetManifestResourceStream(HFRRessources.Tpl_Message))
            using (StreamReader reader = new StreamReader(stream))
            {
                MessageTemplate = reader.ReadToEnd();
            }

            var i = 0;
            foreach (var postNode in postNodes)
            {
                TempHTMLMessage = MessageTemplate;

                // Id de la réponse
                var reponseId =
                    postNode.Descendants("a")
                        .FirstOrDefault(x => x.GetAttributeValue("href", "").StartsWith("#t"))
                        .GetAttributeValue("href", "")
                        .Replace("#t", "");
                
                // Pseudo
                var pseudo = postNode.Descendants("b").FirstOrDefault(x => x.GetAttributeValue("class", "") == "s2").InnerText.CleanFromWeb();

                // Mood
                var mood = postNode.Descendants("span").FirstOrDefault(x=>x.GetAttributeValue("class","") == "MoodStatus")?.InnerText.CleanFromWeb();

                // Img
                var avatarUri = "ms-appx-web:///Assets/HTML/UI/rsz_no_avatar.png";
                var avatarClass = "no_avatar";
                var divAvatarNode = postNode.Descendants("div").FirstOrDefault(x=>x.GetAttributeValue("class", "") == "avatar_center");
                if (divAvatarNode != null && divAvatarNode.ChildNodes.Any())
                {
                    var imgAvatarNode = divAvatarNode.FirstChild;
                    var uri = imgAvatarNode.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(uri))
                    {
                        avatarUri = uri;
                        avatarClass = "";
                    }
                }

                // Date
                var date = postNode.Descendants("div").FirstOrDefault(x=>x.GetAttributeValue("class","") == "toolbar").InnerText.CleanFromWeb();
                date = date.Replace("Posté le ", "");

                // Content
                var content = postNode.Descendants("div").FirstOrDefault(x => x.GetAttributeValue("id", "").Contains("para")).InnerHtml;
                int lastPostText = content.IndexOf("<div style=\"clear: both;\"> </div>", StringComparison.Ordinal);
                if (lastPostText == -1)
                {
                    lastPostText = content.Length;
                }

                content = content.Substring(0, lastPostText);
                content = content.CleanFromWeb();

                TempHTMLMessage = TempHTMLMessage.Replace("%%ID%%", i.ToString());
                TempHTMLMessage = TempHTMLMessage.Replace("%%POSTID%%", reponseId);

                if (pseudo == "Modération")
                {
                    TempHTMLMessage = TempHTMLMessage.Replace("%%moderation%%", "moderation");
                }
                else
                {
                    TempHTMLMessage = TempHTMLMessage.Replace("%%moderation%%", "");
                }

                TempHTMLMessage = TempHTMLMessage.Replace("%%no_avatar_class%%", avatarClass);

                if (Loc.Settings.SquareAvatarStylePreferred)
                {
                    TempHTMLMessage = TempHTMLMessage.Replace("%%round_avatar_class%%", "");
                }
                else
                {
                    TempHTMLMessage = TempHTMLMessage.Replace("%%round_avatar_class%%", "round");
                }

                TempHTMLMessage = TempHTMLMessage.Replace("%%AUTEUR_AVATAR%%", avatarUri);
                TempHTMLMessage = TempHTMLMessage.Replace("%%AUTEUR_PSEUDO%%", pseudo);
                TempHTMLMessage = TempHTMLMessage.Replace("%%MESSAGE_DATE%%", date);

                TempHTMLMessage = TempHTMLMessage.Replace("%%MESSAGE_CONTENT%%", content);

                TempHTMLMessagesList += TempHTMLMessage;
                i++;
            }

            // Get URL of the new post form
            var url = htmlDoc.DocumentNode.Descendants("form").FirstOrDefault(x => x.GetAttributeValue("id", "") == "repondre_form").GetAttributeValue("action", "");
            currentTopic.TopicNewPostUriForm = WebUtility.HtmlDecode(url);

            TempHTMLTopic = BodyTemplate.Replace("%%MESSAGES%%", TempHTMLMessagesList);

            // Get Accent Color
            await ThreadUI.Invoke(() =>
            {
                SolidColorBrush color = (SolidColorBrush)App.Current.Resources["SystemControlHighlightAltListAccentLowBrush"];
                var colorString = $"{color.Color.R.ToString()}, {color.Color.G.ToString()}, {color.Color.B.ToString()}";

                TempHTMLTopic = TempHTMLTopic.Replace("%%ACCENTCOLOR%%", colorString);
            });

            // Get user selected font-size
            var fontSize = Loc.Settings.FontSizePreferred;
            TempHTMLTopic = TempHTMLTopic.Replace("%%FONTSIZE%%", fontSize.ToString());

            // Create/Open WebSite-Cache folder
            var subfolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(Strings.WebSiteCacheFolderName, CreationCollisionOption.OpenIfExists);
            var file = await subfolder.CreateFileAsync($"{Strings.WebSiteCacheFileName}", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, TempHTMLTopic);

            await ThreadUI.Invoke(() =>
            {
                Loc.Topic.UpdateTopicWebView(currentTopic);

                Loc.Topic.IsTopicLoading = false;
            });
        }
    }
}
