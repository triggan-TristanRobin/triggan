using System;
using System.Collections.Generic;
using Model;
using DataAccessLayer.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ProjectRepository : Repository<Project>
    {

        public ProjectRepository(TrigganContext context)
            : base(context)
        {
        }

        public override Project Get(string slug)
        {
            var project = dbSet.Include(proj => proj.Updates).FirstOrDefault(e => e.Slug == slug);
            return project;
        }

        public override IEnumerable<Project> Get(Expression<Func<Project, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IQueryable<Project> query = dbSet;
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

            query = query.OrderByDescending(p => p.LastUpdate);
            query = count == 0 ? query : query.Take(count);

            var projects =  query.ToList();
            return projects;
        }
    }
}
