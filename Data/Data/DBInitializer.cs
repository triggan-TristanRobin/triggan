using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize(TrigganDBContext context)
        {
            Trace.TraceInformation("Migrating DB");
#if DEBUG
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
#endif

            if (context.Posts.Count() == 0)
            {
                Trace.TraceInformation("Add default posts");
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

                context.AddRange(posts);
                context.SaveChanges();
            }

            if (context.Projects.Count() == 0)
            {
                Trace.TraceInformation("Add default projects");
                var projects = new Project[]
                {
                    new Project
                    {
                        Title = "Awesome project",
                        Slug = "pr1",
                        Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                        CoverImagePath = "https://photographycourse.net/wp-content/uploads/2014/11/Landscape-Photography-steps.jpg",
                        Public = true,
                        PublicationDate = DateTime.Now.AddDays(-5),
                        Updates = new List<Update>
                        {
                            new Update
                            {
                                Content = "Update number 1, hell yeah!",
                                PublicationDate = DateTime.Now.AddDays(-3),
                            },
                            new Update
                            {
                                Content = "Update number 2, hell yeah!",
                                PublicationDate = DateTime.Now.AddDays(-1),
                            },
                            new Update
                            {
                                Content = "# Hello world\nThis is update number 3!\n"+
                                            "**Lorem** ipsum dolor sit"+
                                            "amet, *consectetur* adipiscing elit. Sed"+
                                            "eu est nec metus luctus tempus. Pellentesque"+
                                            "at elementum sapien, ac faucibus sem"+
                                            "![surprise](https://media.giphy.com/media/fdyZ3qI0GVZC0/giphy.gif)",
                                PublicationDate = DateTime.Now,
                            },
                        }
                    },
                    new Project
                    {
                        Title = "Awesome project",
                        Slug = "pr2",
                        Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                        CoverImagePath = "https://photographycourse.net/wp-content/uploads/2014/11/Landscape-Photography-steps.jpg",
                        Public = true,
                        PublicationDate = DateTime.Now.AddDays(-10),
                        Updates = new List<Update>
                        {
                            new Update
                            {
                                Title = "This is an important update so there is a title",
                                Content = "Update number 1, hell yeah!",
                                PublicationDate = DateTime.Now.AddDays(-5),
                            },
                            new Update
                            {
                                Content = "# Hello world\nThis is update number 3!\n"+
                                            "**Lorem** ipsum dolor sit"+
                                            "amet, *consectetur* adipiscing elit. Sed"+
                                            "eu est nec metus luctus tempus. Pellentesque"+
                                            "at elementum sapien, ac faucibus sem"+
                                            "![surprise](https://media.giphy.com/media/fdyZ3qI0GVZC0/giphy.gif)",
                                PublicationDate = DateTime.Now.AddDays(-6),
                            },
                            new Update
                            {
                                Content = "Update number 2, hell yeah!",
                                PublicationDate = DateTime.Now.AddDays(-3),
                            },
                        }
                    },
                    new Project
                    {
                        Title = "Awesome project",
                        Slug = "pr3",
                        Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                        CoverImagePath = "https://photographycourse.net/wp-content/uploads/2014/11/Landscape-Photography-steps.jpg",
                        Public = true,
                        PublicationDate = DateTime.Now
                    },
                    new Project
                    {
                        Title = "Awesome project",
                        Slug = "pr4",
                        Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere dolor ut ligula laoreet cursus sed quis nulla. Aenean suscipit placerat ex, ut laoreet odio maximus id.",
                        CoverImagePath = "https://photographycourse.net/wp-content/uploads/2014/11/Landscape-Photography-steps.jpg",
                        Public = true,
                        PublicationDate = DateTime.Now
                    }
                };

                context.AddRange(projects);
                context.SaveChanges();
            }
        }
    }
}
