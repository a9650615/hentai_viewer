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

    public class htDetailObject {
        public string name { get; set; }
        public string img { get; set; }
        public string type { get; set; }
        public Information information { get; set; }
        public string rank_times { get; set; }
        public string rank_avg { get; set; }
        public string start { get; set; }

        public class Information {
            public string posted { get; set; }
            public string parent { get; set; }
            public string visible { get; set; }
            public string language { get; set; }
            public string file_size { get; set; }
            public string length { get; set; }
            public string favorited { get; set; }
        }
    }

    public class htImgObject {
        public string image { get; set; }
        public string next { get; set; }
        public string prev { get; set; }
        public List<string> inf { get; set; }
        public string now { get; set; }
        public string total { get; set; }
    }
    
}
