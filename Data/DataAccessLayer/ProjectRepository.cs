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
    public class ProjectRepository : Repository<Project>
    {
        private ISlugRepository<Post> postRepository;

        public ProjectRepository(ISlugRepository<Post> postRepo, TrigganContext context)
            : base(context)
        {
            postRepository = postRepo;
        }

        public override Project Get(string slug)
        {
            var project = dbSet.FirstOrDefault(e => e.Slug == slug);
            return project;
        }

        public override IEnumerable<Project> Get(Expression<Func<Project, bool>> filter = null, Func<IQueryable<Project>, IOrderedQueryable<Project>> orderBy = null, int count = 0, string includeProperties = "")
        {
            IQueryable<Project> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = orderBy != null ? orderBy(query) : query;
            query = count == 0 ? query : query.Take(count);
            
            var projects = query.ToList();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                }
            }

            return projects;
        }
    }
}
