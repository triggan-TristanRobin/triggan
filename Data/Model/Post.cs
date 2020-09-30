using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class Post : Entity
    {
        public string Title { get; set; }
        public int Views { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string CoverImagePath { get; set; }
        public bool Public { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<string> Tags { get; set; }

    }
}
