using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triggan.BlogModel.Enums;

namespace triggan.BlogModel
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ShortReview { get; set; }
        public string LongReview { get; set; }
        public DateTime DateRead { get; set; }
        public int Score { get; set; }
        public string BuyLink { get; set; }
        public BookCategory Category { get; set; }
    }
}
