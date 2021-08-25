using System;

namespace triggan.BlogModel
{
    public class Update
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime PublicationDate { get; set; } = DateTime.Now;
    }
}