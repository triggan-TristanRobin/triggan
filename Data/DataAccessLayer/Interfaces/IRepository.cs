using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Model;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
	{
		IEnumerable<TEntity> GetAll();
		TEntity Get(int id);
		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, int count = 0, string includeProperties = "");
		void Insert(TEntity tentity);
		void Update(TEntity tentity);
		void Delete(TEntity tentity);
		void Save();
	}
}
