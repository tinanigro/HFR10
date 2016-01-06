using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfr.Model;
using System.Diagnostics;
using Hfr.ViewModel;
using HtmlAgilityPack;
using System.Net;
using Hfr.Models;

namespace Hfr.Helpers
{
    public static class FormFetcher
    {
        public static async Task GetEditor(EditorPackage package)
        {
            if (package.Intent == EditorIntent.MultiQuote)
            {
                if (string.IsNullOrEmpty(Loc.Editor.MultiQuoteTemporaryContent))
                {
                    Loc.Editor.MultiQuoteTemporaryContent = "";
                }
                Loc.Editor.MultiQuoteTemporaryContent += await FetchMessageContent(package);
                Loc.Editor.MultiQuoteTemporaryContent += Environment.NewLine;
            }
            else
            {
                Debug.WriteLine("Fetching Form");
                var editor = await Fetch(package);
                Debug.WriteLine("Updating UI with new Editor");

                await ThreadUI.Invoke(() =>
                {
                    Loc.Editor.CurrentEditor = editor;
                });
            }
        }

        private static async Task<Editor> Fetch(EditorPackage package)
        {
            if (string.IsNullOrEmpty(package.PostUriForm)) return null;
            var html = await HttpClientHelper.Get(package.PostUriForm);

            HtmlNode.ElementsFlags.Remove("form");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            HtmlNode formNode = htmlDoc.DocumentNode.Descendants("form").First(x => (string)x.GetAttributeValue("id", "") == "hop");

            //Submit URL
            string submitUrl = formNode.GetAttributeValue("action", "");

            //All inputs/select data
            Dictionary<string, string> formInputs = new Dictionary<string, string>();

            foreach (HtmlNode node in formNode.Descendants("input"))
            {
                if (node.GetAttributeValue("value", "") != "" && node.GetAttributeValue("name", "") != "")
                {
                    if (node.GetAttributeValue("name", "") == "MsgIcon")
                    {
                        // Message' icon - Only if checked
                        if (node.GetAttributeValue("checked", "") == "checked")
                        {
                            formInputs.Add(node.GetAttributeValue("name", ""), node.GetAttributeValue("value", ""));
                        }
                    }
                    else if (node.GetAttributeValue("type", "") == "checkbox")
                    {
                        // Checkboxes
                        formInputs.Add(node.GetAttributeValue("name", ""), node.GetAttributeValue("checked", "") == "checked" ? "1" : "0");
                    }
                    else
                    {
                        try
                        {
                            formInputs.Add(node.GetAttributeValue("name", ""), node.GetAttributeValue("value", ""));
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("ext0 =" + node.GetAttributeValue("name", "") + "," + node.GetAttributeValue("value", ""));
                            throw;
                        }
                    }
                }

            }

            foreach (HtmlNode node in formNode.Descendants("select"))
            {
                if (node.GetAttributeValue("name", "") == "subcat")
                {
#warning "Categories list used to select/change FP category not implemented"
                }

                // add Selected category (on edit), or first one if none selected.
                HtmlNode selectedNode = node
                        .Descendants("option")
                        .FirstOrDefault(x => (string)x.GetAttributeValue("selected", "") == "selected");

                if (selectedNode != null)
                {
                    formInputs.Add(node.GetAttributeValue("name", ""), selectedNode.GetAttributeValue("value", ""));
                }
                else
                {
                    formInputs.Add(node.GetAttributeValue("name", ""), node.Descendants("option").First().GetAttributeValue("value", " "));
                }
            }

            foreach (HtmlNode node in formNode.Descendants("textarea"))
            {
                formInputs.Add(node.GetAttributeValue("name", ""), WebUtility.HtmlDecode(node.InnerText) + Environment.NewLine);
            }

            //Debug.WriteLine("Parsing OK");

            //foreach (KeyValuePair<string, string> entry in formInputs)
            //{
            //    Debug.WriteLine("inputs = " + entry.Key + " " + entry.Value);
            //}

            // Populate with multi quote temporary content if found and that content_form is empty
            var content = "";
            if (formInputs.TryGetValue("content_form", out content))
            {
                if (string.IsNullOrEmpty(content.CleanFromWeb()))
                {
                    if (!string.IsNullOrEmpty(Loc.Editor.MultiQuoteTemporaryContent))
                    {
                        formInputs["content_form"] = Loc.Editor.MultiQuoteTemporaryContent;
                        Loc.Editor.MultiQuoteTemporaryContent = null;
                    }
                }
            }

            return new Editor
            {
                FromUrl = package.PostUriForm,
                SubmitUrl = submitUrl,
                Data = formInputs,
                idxTopic = 0,
                Intent = package.Intent
            };
        }

        static async Task<string> FetchMessageContent(EditorPackage package)
        {
            if (string.IsNullOrEmpty(package.PostUriForm)) return null;
            var html = await HttpClientHelper.Get(package.PostUriForm);

            HtmlNode.ElementsFlags.Remove("form");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            HtmlNode formNode = htmlDoc.DocumentNode.Descendants("form").First(x => (string)x.GetAttributeValue("id", "") == "hop");

            //All inputs/select data
            Dictionary<string, string> formInputs = new Dictionary<string, string>();

            foreach (HtmlNode node in formNode.Descendants("textarea"))
            {
                formInputs.Add(node.GetAttributeValue("name", ""), WebUtility.HtmlDecode(node.InnerText) + Environment.NewLine);
            }
            return formInputs["content_form"];
        }
    }
}
