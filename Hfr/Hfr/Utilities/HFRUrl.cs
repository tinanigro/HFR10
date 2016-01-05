namespace Hfr.Utilities
{
    public static class HFRUrl
    {
        public static readonly string ForumUrl = "http://forum.hardware.fr";
        public static readonly string ConnectUrl = "/login_validation.php?config=hfr.inc";
        
        public static readonly string DrapsUrl = "/forum1f.php?owntopic=1";
        public static readonly string ReadsUrl = "/forum1f.php?owntopic=2";
        public static readonly string FavsUrl = "/forum1f.php?owntopic=3";

        public static readonly string MessagesUrl = "/forum1.php?config=hfr.inc&cat=prive&subcat=&sondage=0&owntopic=0&trash=0&trash_post=0&moderation=0&new=0&nojs=0&subcatgroup=0"; // Need to add "&page=1"
        public static readonly string ProfilePageUrl = "/user/editprofil.php?config=hfr.inc&page=5";

        /* Debug URL */
        public static readonly string Dbg_Form_QuoteSingleURL =
            "/message.php?config=hfr.inc&cat=23&post=26848&numrep=1545246&ref=10&page=2&p=1&subcat=540&sondage=0&owntopic=1&new=0#formulaire";

        public static readonly string Dbg_Topic_FirstPageFlagThirdPostURL = 
            "/forum2.php?config=hfr.inc&cat=23&subcat=540&post=26848&page=1&p=1&sondage=0&owntopic=1&trash=0&trash_post=0&print=0&numreponse=0&quote_only=0&new=0&nojs=0#t1545199";

    }
}
