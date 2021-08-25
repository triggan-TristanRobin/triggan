using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class ProjectRepository : Repository<Project>
    {
        public ProjectRepository(TrigganContext context) : base(context) { }

        public override IEnumerable<Project> Get(Expression<Func<Project, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IEnumerable<Project> projects = base.Get(filter, 0, includeProperties);
            projects = projects.OrderByDescending(p => p.LastUpdate);
            projects = count == 0 ? projects : projects.Take(count);
            return projects;
        }

        public List<Update> GetUpdates(string slug)
        {
            var project = dbSet.Include(e => e.Updates).Single(p => p.Slug == slug);
            return project.Updates;
        }
    }
}