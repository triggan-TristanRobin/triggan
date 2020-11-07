using System;

namespace Model
{
    public abstract class Entity
    {
        public string Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Slug { get; set; }
        public bool Deleted { get; set; }
    }
}
