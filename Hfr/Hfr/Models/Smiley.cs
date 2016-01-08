namespace Hfr.Models
{
    public class Smiley
    {
        public string Src { get; set; }
        public string Tag { get; set; }
        public Smiley(string src, string tag)
        {
            Src = src;
            Tag = tag;
        }
    }
}
