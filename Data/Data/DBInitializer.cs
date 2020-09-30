using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize(TrigganDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Posts.Any())
            {
                return;   // DB has been seeded
            }

            var posts = new Post[]
            {
                new Post
                {
                    Title = "Awesome blog post",
                    Slug = "p1",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id. Nam porttitor vel felis eget mollis. Donec suscipit, augue in feugiat tempus, urna nibh tincidunt eros, id gravida erat libero ut nunc. Donec molestie justo vehicula dui dapibus, ut lobortis arcu interdum. Aliquam at massa vel tortor bibendum volutpat ac id metus. In quis vulputate orci. Nulla vel orci quis purus eleifend finibus ac at ipsum. Aliquam lobortis lacinia metus id auctor. Donec volutpat nisl id libero venenatis efficitur. Sed non sem cursus, posuere risus eu, efficitur lectus. Ut ullamcorper consequat quam, ut facilisis orci scelerisque a. ",
                    Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                    Public = true, PublicationDate = DateTime.Now, Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
                },
                new Post
                {
                    Title = "Another blog post",
                    Slug = "p2",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id. Nam porttitor vel felis eget mollis. Donec suscipit, augue in feugiat tempus, urna nibh tincidunt eros, id gravida erat libero ut nunc. Donec molestie justo vehicula dui dapibus, ut lobortis arcu interdum. Aliquam at massa vel tortor bibendum volutpat ac id metus. In quis vulputate orci. Nulla vel orci quis purus eleifend finibus ac at ipsum. Aliquam lobortis lacinia metus id auctor. Donec volutpat nisl id libero venenatis efficitur. Sed non sem cursus, posuere risus eu, efficitur lectus. Ut ullamcorper consequat quam, ut facilisis orci scelerisque a. ",
                    Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                    Public = true, PublicationDate = DateTime.Now, Tags = new List<string> { "Tag1", "Tag3" }
                },
                new Post
                {
                    Title = "What the hell?",
                    Slug = "pos",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id. Nam porttitor vel felis eget mollis. Donec suscipit, augue in feugiat tempus, urna nibh tincidunt eros, id gravida erat libero ut nunc. Donec molestie justo vehicula dui dapibus, ut lobortis arcu interdum. Aliquam at massa vel tortor bibendum volutpat ac id metus. In quis vulputate orci. Nulla vel orci quis purus eleifend finibus ac at ipsum. Aliquam lobortis lacinia metus id auctor. Donec volutpat nisl id libero venenatis efficitur. Sed non sem cursus, posuere risus eu, efficitur lectus. Ut ullamcorper consequat quam, ut facilisis orci scelerisque a. ",
                    Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                    Public = true, PublicationDate = DateTime.Now, Tags = new List<string> { "Tag2" }
                },
                new Post(){
                    Title = "Markdown Test",
                    Slug = "md",
                    Excerpt = "Let's see what markdown can do",
                    Content =   "# Hello world\n"+
                                "**Lorem** ipsum dolor sit"+
                                "amet, *consectetur* adipiscing elit. Sed"+
                                "eu est nec metus luctus tempus. Pellentesque"+
                                "at elementum sapien, ac faucibus sem"+
                                "![surprise](https://media.giphy.com/media/fdyZ3qI0GVZC0/giphy.gif)",
                    Public = true, PublicationDate = DateTime.Now, Tags = new List<string> { "Tag2" }
                }
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();
        }
    }
}
