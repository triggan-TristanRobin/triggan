﻿using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Data;
using triggan.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace triggan.DataAccessLayer
{
    public class PostRepository : IRepository<Post>
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
            throw new NotImplementedException();
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
