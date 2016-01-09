using System;
using System.Globalization;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Hfr.Helpers
{
    public static class ThreadHelper
    {
        public static HtmlNode[] GetPostNodesFromHtmlDoc(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.Descendants("tr")
                    .Where(x => x.GetAttributeValue("class", "").Contains("sujet ligne_booleen cBackCouleurTab"))
                    .ToArray();
        }

        public static HtmlNode GetSujetCase10(HtmlNode node)
        {
            return node.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase10"));
        }

        public static HtmlNode GetSujetCase9(HtmlNode node)
        {
            return node.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase9"));
        }

        public static int GetIdFromSujetCase10Node(HtmlNode node)
        {
            var id = node?.FirstChild.GetAttributeValue("value", "");
            var idNb = 0;
            int.TryParse(id, out idNb);
            return idNb;
        }

        public static int GetCatIdFromSujetCase10Node(HtmlNode node)
        {
            var catIdString = node?.Descendants("input").FirstOrDefault(x => x.GetAttributeValue("name", "").Contains("valuecat")).GetAttributeValue("value", "");
            var catId = 0;
            int.TryParse(catIdString, out catId);
            return catId;
        }

        public static int GetSubCatId(string threadUrl)
        {
            var firstTopicSubCatId = threadUrl.IndexOf("&subcat=", StringComparison.Ordinal) + "&subcat=".Length;
            var lastTopicSubCatId = threadUrl.IndexOf("&", firstTopicSubCatId, StringComparison.Ordinal);
            var topicSubCatId = threadUrl.Substring(firstTopicSubCatId, lastTopicSubCatId - firstTopicSubCatId);
            var subCatId = 0;
            int.TryParse(topicSubCatId, out subCatId);
            return subCatId;
        }

        public static int GetBookmarkId(string threadUrl)
        {
            var firstReponseId = threadUrl.IndexOf("#t", StringComparison.Ordinal) + "#t".Length;
            var lastReponseId = threadUrl.Length;
            var reponseId = threadUrl.Substring(firstReponseId, lastReponseId - firstReponseId);
            var id = 0;
            int.TryParse(reponseId, out id);
            return id;
        }

        public static int GetNbPageFromNode(HtmlNode node)
        {
            var nbPageString = node.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase4"))?.FirstChild?.InnerText?.CleanFromWeb();
            if (string.IsNullOrEmpty(nbPageString))
            {
                nbPageString = "1";
            }
            var nbPage = 0;
            if (!int.TryParse(nbPageString, out nbPage))
            {
                nbPage = 1;
            }
            return nbPage;
        }

        public static int GetCurrentPage(string threadUrl)
        {
            var firstPageNumber = threadUrl.IndexOf("&page=", StringComparison.Ordinal) + "&page=".Length;
            var lastPageNumber = threadUrl.LastIndexOf("&p=", StringComparison.Ordinal);
            var pageNumber = int.Parse(threadUrl.Substring(firstPageNumber, lastPageNumber - firstPageNumber));
            return pageNumber;
        }

        public static DateTime GetDateTimeLastPostFromNode(HtmlNode node)
        {
            var dateInnerText = node.FirstChild.FirstChild.InnerText;
            var dateString = WebUtility.HtmlDecode(dateInnerText).Replace("à", "");
            var dateTime = DateTime.Parse(dateString, new CultureInfo("fr-FR"));
            return dateTime;
        }

        public static string ThreadLastPostMemberPseudo(HtmlNode node)
        {
            var pseudoInnerText = node.FirstChild.Descendants("b").FirstOrDefault().InnerText;
            return pseudoInnerText;
        }

        public static bool NewPost(HtmlNode node)
        {
            var isNewNode = node.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase1"));
            bool isMsgNew = isNewNode.FirstChild.Name == "img" && isNewNode.FirstChild.GetAttributeValue("alt", "") == "On";
            return isMsgNew;
        }

        public static string ThreadName(HtmlNode node)
        {
            return node.Descendants("a").FirstOrDefault(x => x.GetAttributeValue("class", "") == "cCatTopic").InnerText.CleanFromWeb();
        }

        public static string ThreadAuthor(HtmlNode node)
        {
            return node.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase6")).FirstChild.InnerText.CleanFromWeb();
        }
    }
}
