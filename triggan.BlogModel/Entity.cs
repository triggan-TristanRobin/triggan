using System;

namespace triggan.BlogModel
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Slug { get; set; }
        public int Stars { get; set; }
        public bool Deleted { get; set; }
    }
}