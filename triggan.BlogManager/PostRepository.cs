using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class PostRepository : Repository<Post>
    {

        public PostRepository(TrigganContext context) : base(context) { }

        public override IEnumerable<Post> Get(Expression<Func<Post, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IEnumerable<Post> posts = base.Get(filter, 0, includeProperties);
            posts = posts.Where(p => p.Public && p.PublicationDate >= DateTime.Now).OrderByDescending(p => p.PublicationDate);
            posts = count == 0 ? posts : posts.Take(count);
            return posts;
        }
    }
}