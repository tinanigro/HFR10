using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hfr.Models;
using System.Diagnostics;
using Hfr.ViewModel;

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

            /* Parsing */

            return new Editor()
            {
                FromUrl = FormUrl,
                Text = "dummy text",
                idxTopic = 0,
            };

        }
    }
}
