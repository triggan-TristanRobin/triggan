using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class PostRepository : ISlugRepository<Post>
    {
        private readonly TrigganContext context;

        public PostRepository(TrigganContext context)
        {
            this.context = context;
        }

        public IEnumerable<Post> GetAll()
        {
            var posts = context.Posts.ToList();
            posts.ForEach(p => p.Tags = p.Tags ?? new List<string>());
            return posts;
        }

        public Post Get(int PostId)
        {
            var p = context.Posts.Find(PostId);
            p.Tags = p.Tags ?? new List<string>();
            return p;
        }

        public Post Get(string slug)
        {
            var p = context.Posts.FirstOrDefault(e => e.Slug == slug);
            p.Tags = p.Tags ?? new List<string>();
            return p;
        }

        public IEnumerable<Post> Get(Expression<Func<Post, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IQueryable<Post> query = context.Posts;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            query = query.OrderByDescending(post => post.PublicationDate);
            query = count == 0 ? query : query.Take(count);

            foreach (var p in query)
            {
                p.Tags ??= new List<string>();
                yield return p;
            }
        }

        public void Insert(Post Post)
        {
            context.Posts.Add(Post);
        }

        public void Update(Post Post)
        {
            context.Entry(Post).State = EntityState.Modified;
        }

        public void Delete(Post Post)
        {
            context.Posts.Remove(Post);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}