﻿using System;
using System.Collections.Generic;
using triggan.BlogModel.Enums;

namespace triggan.BlogModel
{
    public class Post : Entity
    {
        public string Title { get; set; }
        public int Views { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string CoverImagePath { get; set; }
        public bool Public { get; set; }
        public PostType Type { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}