using System;
using System.Text.RegularExpressions;

namespace Hfr.Helpers
{
    public class ThreadNameHelper
    {
        public static string Shorten(string input)
        {
            string output = Regex.Replace(input, "topic unique", "T.U.", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, "topique unique", "T.U.", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, "topic unik", "T.U.", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, "topik unik", "T.U.", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, "topic officiel", "T.U.", RegexOptions.IgnoreCase);

            return output;
        }

        public static string TimeSinceLastReadMsg(TimeSpan timeSpent, string favorisLastPostUser)
        {
            if (timeSpent.Ticks <= 0) return ("dernier message par " + favorisLastPostUser);
            if (timeSpent.Days > 365)
            {
                return ("il y a + d'un an par " + favorisLastPostUser);
            }
            if (timeSpent.Days > 31)
            {
                return ("il y a " + Math.Round(Convert.ToDecimal(timeSpent.Days / 31), 0).ToString() + " mois par " + favorisLastPostUser);
            }
            if (timeSpent.Days > 1)
            {
                return ("il y a " + timeSpent.Days + " jours par " + favorisLastPostUser);
            }
            if (timeSpent.Days == 1)
            {
                return ("hier par " + favorisLastPostUser);
            }
            if (timeSpent.Hours > 1)
            {
                return ("il y a " + timeSpent.Hours + "h par " + favorisLastPostUser);
            }
            if (timeSpent.Hours == 1)
            {
                return ("il y a 1h par " + favorisLastPostUser);
            }
            if (timeSpent.Minutes > 1)
            {
                return ("il y a " + timeSpent.Minutes + " min par " + favorisLastPostUser);
            }
            if (timeSpent.Minutes == 1)
            {
                return ("il y a 1 min par " + favorisLastPostUser);
            }
            return ("il y a " + timeSpent.Seconds + "s par " + favorisLastPostUser);
        }
    }
}
