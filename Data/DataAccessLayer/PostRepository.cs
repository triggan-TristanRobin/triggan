using System;
using System.Collections.Generic;
using Model;
using Data;
using triggan.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace DataAccessLayer
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

        public IEnumerable<Post> Get(Expression<Func<Post, bool>> filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, int count = 0, string includeProperties = "")
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

            query = orderBy != null ? orderBy(query) : query;
            query = count == 0 ? query : query.Take(count);

            var posts = query.ToList();

            posts.ForEach(p => p.Tags = p.Tags ?? new List<string>());
            return posts;
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
