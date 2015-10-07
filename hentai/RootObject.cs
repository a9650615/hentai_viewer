using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hentai
{
    /* old object
    public class RootObject
    {
        public string publish { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public string href { get; set; }
        public string torrent { get; set; }
        public Uploader uploader { get; set; }
        public string type { get; set; }
        public double rank { get; set; }

        public class Uploader {
            public string href { get; set; }
            public string name { get; set; }
        }
    }*/

    public class htObject {
        public string img { get; set; }
        public string name { get; set; }
        public string href { get; set; }
        public string files { get; set; }
        public string type { get; set; }
        public double rank { get; set; }
        public string torrent { get; set; }
        public bool delete { get; set; }
    }
    
}
