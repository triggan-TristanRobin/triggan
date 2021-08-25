using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace triggan.BlogManager.Interfaces
{
    public interface IRepository
    {

    }

    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity Get(string slug);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, int count = 0, string includeProperties = "");

        void Add(TEntity tentity);
        void Update(string slug, TEntity tentity);

        void Delete(TEntity tentity);

        void Save();
    }
}