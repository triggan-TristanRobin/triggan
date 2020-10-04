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
            return context.Posts.ToList();
        }

        public Post Get(int PostId)
        {
            return context.Posts.Find(PostId);
        }

        public Post Get(string slug)
        {
            return context.Posts.FirstOrDefault(e => e.Slug == slug);
        }

        public IEnumerable<Post> Get(Expression<Func<Post, bool>> filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, string includeProperties = "")
        {
            return context.Posts.Where(filter).ToList();
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
