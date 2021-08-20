using System;

namespace Model
{
	public class Message
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Name { get; set; }
		public string EMail { get; set; }
		public string Content { get; set; }
	}
}
