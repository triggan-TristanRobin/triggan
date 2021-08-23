using System;
using System.Collections.Generic;
using Model;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer
{
    public class Repository<TEntity> : ISlugRepository<TEntity> where TEntity : Entity
    {
        internal TrigganContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(TrigganContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity Get(string slug)
        {
            return dbSet.FirstOrDefault(e => e.Slug == slug);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
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

            query = count == 0 ? query : query.Take(count);

            return query.ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }


        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public static class RepositoryExtensions
    {
        public static Entity Get(this Repository<Entity> repository, string slug)
        {
            return repository.dbSet.FirstOrDefault(e => e.Slug == slug);
        }
    }
}
