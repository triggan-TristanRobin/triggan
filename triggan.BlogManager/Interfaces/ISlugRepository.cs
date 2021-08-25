using triggan.BlogModel;

namespace triggan.BlogManager.Interfaces
{
    public interface ISlugRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        TEntity Get(string slug);
    }
}