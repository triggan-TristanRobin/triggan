using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Project : Entity
    {
        public string Title { get; set; }
        public int Views { get; set; }
        public string Excerpt { get; set; }
        public string CoverImagePath { get; set; }
        public bool Public { get; set; }
        public ProjectState State { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<Post> Updates { get; set; } = new List<Post>();
    }
}
