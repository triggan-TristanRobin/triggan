using System;
using System.Collections.Generic;
using Model;
using Data;
using triggan.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class PostRepository : ISlugRepository<Post>
    {
        private readonly TrigganDBContext context;

        public PostRepository()
        {
            context = new TrigganDBContext();
        }

        public PostRepository(TrigganDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Post> Get()
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

        public IEnumerable<Post> Get(Expression<Func<Post, bool>> filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, string includeProperties = "")
        {
            var posts = context.Posts.Where(filter).ToList();
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
