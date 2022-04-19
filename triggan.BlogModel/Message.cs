using System;
using System.ComponentModel.DataAnnotations;

namespace triggan.BlogModel
{
    public class Message : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string Content { get; set; }
    }
}