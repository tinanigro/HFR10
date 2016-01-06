namespace Hfr.Models
{
    public class EditorPackage
    {
        public EditorIntent Intent { get; set; }
        public string PostUriForm { get; set; }

        public EditorPackage(EditorIntent intent, string postUriForm)
        {
            this.Intent = intent;
            this.PostUriForm = postUriForm;
        }
    }
}
