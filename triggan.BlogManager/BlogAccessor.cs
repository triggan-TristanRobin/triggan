using System.Collections.Generic;
using System.Linq;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class BlogAccessor
    {
        private List<IRepository> repositories;
        public ProjectRepository ProjectRepository { get; set; }
        public PostRepository PostRepository { get; set; }

        public BlogAccessor(TrigganContext context)
        {
            repositories = new List<IRepository>
            {
                new PostRepository(context),
                new ProjectRepository(context)
            };
        }

        private IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            return repositories.Single(rep => rep is IRepository<TEntity>) as IRepository<TEntity>;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(int count) where TEntity : Entity
        {
            return GetRepository<TEntity>().Get(count: count);
        }

        public Entity Get(string slug)
        {
            foreach(dynamic repo in repositories)
            {
                var entity = repo.Get(slug);
                if (entity != null)
                {
                    return entity;
                }
            }
            return null;
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            var repo = GetRepository<TEntity>();
            repo.Add(entity);
            repo.Save();
            return repo.Get(entity.Slug);
        }

        public TEntity Update<TEntity>(string slug, TEntity entity) where TEntity : Entity
        {
            var repo = GetRepository<TEntity>();
            repo.Update(slug, entity);
            repo.Save();
            return repo.Get(entity.Slug);
        }

        public Project AddUpdate(string slug, Update update)
        {
            var repo = GetRepository<Project>();
            var savedProject = repo.Get(slug);
            savedProject.SetUpdate(update);
            return Update(slug, savedProject);
        }

        public Project SetUpdates(string slug, List<Update> updates)
        {
            var repo = GetRepository<Project>();
            var savedProject = repo.Get(slug);
            savedProject.Updates = updates;
            return Update(slug, savedProject);
        }

        public List<Update> GetProjectUpdates(string slug)
        {
            var repo = GetRepository<Project>() as ProjectRepository;
            return repo.GetUpdates(slug);
        }

        public Entity Delete(string slug)
        {
            foreach (dynamic repo in repositories)
            {
                var entity = repo.Get(slug);
                if (entity != null)
                {
                    repo.Delete(entity);
                    repo.Save();
                }
            }
            throw new KeyNotFoundException($"Could not delete entity {slug} as it wasn't found.");
        }

        public int Star(string slug)
        {
            foreach (dynamic repo in repositories)
            {
                var entity = repo.Get(slug);
                if (entity != null)
                {
                    entity.Stars++;
                    repo.Update(slug, entity);
                    repo.Save();
                    return entity.Stars;
                }
            }
            throw new KeyNotFoundException($"Could not star entity {slug} as it wasn't found.");
        }
    }
}
