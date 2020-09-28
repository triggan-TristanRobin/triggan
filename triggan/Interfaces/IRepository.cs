using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Model;

namespace triggan.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
	{
		IEnumerable<TEntity> Get();
		TEntity Get(int id);
		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
		void Insert(TEntity tentity);
		void Update(TEntity tentity);
		void Delete(TEntity tentity);
		void Save();
	}
}
