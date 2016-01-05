using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hfr.Models
{
    public class PrivateChat
    {
        public string Subject { get; set; }
        public string Poster { get; set; }
        public bool NewMsg { get; set; }
        public DateTime DateTime { get; set; }

        public string DateTimeString => DateTime.ToString("g", new CultureInfo("fr-FR"));
    }
}
