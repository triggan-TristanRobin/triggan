using System;

namespace triggan.BlogModel
{
    public class Message : Entity
    {
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Content { get; set; }
    }
}