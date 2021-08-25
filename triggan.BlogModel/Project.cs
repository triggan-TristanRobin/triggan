using System;
using System.Collections.Generic;
using System.Linq;
using triggan.BlogModel.Enums;

namespace triggan.BlogModel
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
        public List<Update> Updates { get; set; } = new List<Update>();

        public DateTime LastUpdate { get; set; } = DateTime.Now;

        public void SetUpdate(Update update)
        {
            Updates.Add(update);
            LastUpdate = Updates.Max(u => u.PublicationDate);
        }
    }
}