using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : Entity
    {
        internal TrigganContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(TrigganContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity Get(string slug)
        {
            return dbSet.SingleOrDefault(e => e.Slug == slug);
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

            return query.AsEnumerable();
        }

        public virtual void Add(TEntity updatedEntity)
        {
            var entity = dbSet.SingleOrDefault(e => e.Slug == updatedEntity.Slug);
            if (entity != null)
            {
                throw new DbUpdateException($"Cannot add entity with existing slug {updatedEntity.Slug}");
            }
            else
            {
                dbSet.Add(updatedEntity);
            }
        }

        public virtual void Update(string slug, TEntity updatedEntity)
        {
            var entity = dbSet.SingleOrDefault(e => e.Slug == slug);
            if (entity != null)
            {
                entity.Update(updatedEntity);
                dbSet.Update(entity);
            }
            else
            {
                dbSet.Add(updatedEntity);
            }
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

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Save();
        }
    }
}