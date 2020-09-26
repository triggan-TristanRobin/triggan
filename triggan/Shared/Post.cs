using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triggan.Shared
{

    public class Post : Entity
    {
        public string Title { get; set; }
        public int Views { get; set; } = 0;
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string CoverImagePath { get; set; }
        public bool Public { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<string> Tags { get; set; }

    }
}
