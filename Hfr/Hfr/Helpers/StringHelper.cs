using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hfr.Helpers
{
    public static class StringHelper
    {
        public static string CleanFromWeb(this string text)
        {
            return WebUtility.HtmlDecode(text.Trim());
        }
    }
}
