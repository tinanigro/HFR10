using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hfr.Model;
using Hfr.Models;
using Hfr.Utilities;
using Hfr.ViewModel;
using HtmlAgilityPack;

namespace Hfr.Helpers
{
    public static class CatFetcher
    {
        public static async Task GetCats()
        {
            Debug.WriteLine("Fetching Categories");
            var cats = await Fetch();
            Debug.WriteLine("Updating UI with new Drapeaux list");
            await ThreadUI.Invoke(() =>
            {
                Loc.SubCategory.Categories = cats;
                if (cats != null)
                {
                    Loc.SubCategory.CategoriesGrouped = cats.GroupBy(x => x.CategoryName);
                }
            });
        }

        static async Task<List<SubCategory>> Fetch()
        {
            var html = await HttpClientHelper.Get(HFRUrl.ForumUrl);
            if (string.IsNullOrEmpty(html)) return null;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var catsArray =
                htmlDoc.DocumentNode.Descendants("td")
                    .Where(x => x.GetAttributeValue("class", "") == "catCase1")
                    .ToArray();
            if (catsArray == null) return null;

            var subcategories = new List<SubCategory>();
            foreach (var cat in catsArray)
            {
                var catNode = cat.Descendants("a").FirstOrDefault(x => x.GetAttributeValue("class", "") == "cCatTopic");
                var catName = catNode.InnerText?.CleanFromWeb();
                var catUrl = catNode.GetAttributeValue("href", "");
                Debug.WriteLine(catName);
                var subcatNodes = cat.Descendants("a").Where(x => x.GetAttributeValue("class", "") == "Tableau").ToArray();

                var mainSubCategory = new SubCategory();
                mainSubCategory.CategoryName = catName;
                mainSubCategory.Name = $"{catName}";
                mainSubCategory.Url = catUrl;
                subcategories.Add(mainSubCategory);
                foreach (var subcatNode in subcatNodes)
                {
                    Debug.WriteLine($"--- {subcatNode.InnerText}");
                    var subCategory = new SubCategory();
                    subCategory.CategoryName = catName;
                    subCategory.Name = subcatNode.InnerText.CleanFromWeb();
                    subCategory.Url = subcatNode.GetAttributeValue("href", "");
                    subcategories.Add(subCategory);
                }
                Debug.WriteLine("");
            }
            return subcategories;
        }

        public static async Task GetTopics(SubCategory subcat)
        {
            Debug.WriteLine($"Fetching topics from {subcat.Name}");
            var topics = await FetchTopics(subcat);
            Debug.WriteLine("Updating UI with topics from cat");
            await ThreadUI.Invoke(() =>
            {
                Loc.SubCategory.Topics = topics;
            });
        }

        static async Task<List<Topic>> FetchTopics(SubCategory subCategory)
        {
            var url = subCategory.Url.Replace("liste_sujet-1", $"liste_sujet-{Loc.SubCategory.TopicsPage}");
            
            var html = await HttpClientHelper.Get(url);

            if (string.IsNullOrEmpty(html)) return null;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var topicNodes = htmlDoc.DocumentNode.Descendants("tr").Where(x => x.GetAttributeValue("class", "").Contains("sujet ligne_booleen")).ToArray();

            if (topicNodes == null) return null;
            var topics = new List<Topic>();
            foreach (var topicNode in topicNodes)
            {
                var topicName = topicNode.Descendants("a").FirstOrDefault(x => x.GetAttributeValue("class", "") == "cCatTopic").InnerText;
                var topicAuthor = topicNode.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "").Contains("sujetCase6")).InnerText.Trim();

                var topicPageNode = topicNode.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "") == "sujetCase4").InnerText;
                var topicPage = 1;
                if (!string.IsNullOrEmpty(topicPageNode))
                {
                    int.TryParse(topicPageNode, out topicPage);
                    if (topicPage == 0)
                        topicPage = 1;
                }
                
                var sujetCase3Node = topicNode.Descendants("td").FirstOrDefault(x => x.GetAttributeValue("class", "") == "sujetCase3");

                var topicUrl = sujetCase3Node.Descendants("a").FirstOrDefault(x => x.GetAttributeValue("href", "").StartsWith("/hfr/")).GetAttributeValue("href","");

                var topicIsStickyNodes = sujetCase3Node.Descendants("img");
                var topicIsSticky = topicIsStickyNodes.FirstOrDefault(x => x.GetAttributeValue("src", "").Contains("flechesticky"));
                var topicIsClosed = topicIsStickyNodes.FirstOrDefault(x=>x.GetAttributeValue("alt", "") == "closed");
                

                var topic = new Topic();
                topic.TopicName = topicName;
                topic.TopicAuthor = topicAuthor;
                topic.TopicIsSticky = topicIsSticky != null;
                topic.TopicIsClosed = topicIsClosed != null;
                topic.TopicNbPage = topicPage;
                topic.TopicDrapURI = topicUrl;

                topics.Add(topic);
            }

            return topics;
        }
    }
}
