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
            context.Database.Migrate();

            if (!context.Posts.Any())
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

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }

            if (!context.Projects.Any())
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
                        Updates = new List<Post>
                        {
                            new Post
                            {
                                Content = "Update number 1, hell yeah!",
                                Slug = "pr1u1" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now.AddDays(-3),
                                Type = Model.Enums.PostType.Update,
                            },
                            new Post
                            {
                                Content = "Update number 2, hell yeah!",
                                Slug = "pr1u2" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now.AddDays(-1),
                                Type = Model.Enums.PostType.Update,
                            },
                            new Post
                            {
                                Content = "# Hello world\nThis is update number 3!\n"+
                                            "**Lorem** ipsum dolor sit"+
                                            "amet, *consectetur* adipiscing elit. Sed"+
                                            "eu est nec metus luctus tempus. Pellentesque"+
                                            "at elementum sapien, ac faucibus sem"+
                                            "![surprise](https://media.giphy.com/media/fdyZ3qI0GVZC0/giphy.gif)",
                                Slug = "pr1u3" + DateTime.Now.ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now,
                                Type = Model.Enums.PostType.Update,
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
                        Updates = new List<Post>
                        {
                            new Post
                            {
                                Content = "Update number 1, hell yeah!",
                                Slug = "pr2u1" + DateTime.Now.AddDays(-5).ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now.AddDays(-5),
                                Type = Model.Enums.PostType.Update,
                            },
                            new Post
                            {
                                Content = "# Hello world\nThis is update number 3!\n"+
                                            "**Lorem** ipsum dolor sit"+
                                            "amet, *consectetur* adipiscing elit. Sed"+
                                            "eu est nec metus luctus tempus. Pellentesque"+
                                            "at elementum sapien, ac faucibus sem"+
                                            "![surprise](https://media.giphy.com/media/fdyZ3qI0GVZC0/giphy.gif)",
                                Slug = "pr2u2" + DateTime.Now.AddDays(-6).ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now.AddDays(-6),
                                Type = Model.Enums.PostType.Update,
                            },
                            new Post
                            {
                                Content = "Update number 2, hell yeah!",
                                Slug = "pr2u3" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd"),
                                Public = true,
                                PublicationDate = DateTime.Now.AddDays(-3),
                                Type = Model.Enums.PostType.Update,
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

                context.Projects.AddRange(projects);
                context.SaveChanges();
            }
        }
    }
}
