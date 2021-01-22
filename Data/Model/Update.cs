using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Update
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime PublicationDate { get; set; } = DateTime.Now;
    }
}
