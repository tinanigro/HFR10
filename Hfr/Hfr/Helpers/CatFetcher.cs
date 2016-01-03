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
                var catName = catNode.InnerText?.Trim();
                var catUrl = catNode.GetAttributeValue("href", "");
                Debug.WriteLine(catName);
                var subcatNodes = cat.Descendants("a").Where(x => x.GetAttributeValue("class", "") == "Tableau").ToArray();

                var mainSubCategory = new SubCategory();
                mainSubCategory.CategoryName = catName;
                mainSubCategory.Name = $"Tout de {catName}";
                mainSubCategory.Url = catUrl;
                subcategories.Add(mainSubCategory);
                foreach (var subcatNode in subcatNodes)
                {
                    Debug.WriteLine($"--- {subcatNode.InnerText}");
                    var subCategory = new SubCategory();
                    subCategory.CategoryName = catName;
                    subCategory.Name = subcatNode.InnerText;
                    subCategory.Url = subcatNode.GetAttributeValue("href", "");
                    subcategories.Add(subCategory);
                }
                Debug.WriteLine("");
            }

            return subcategories;
        }
    }
}
