using Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace Data
{
    public class TrigganDBContext : DbContext
	{
		public DbSet<Post> Posts { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Message> Messages { get; set; }

		static TrigganDBContext()
		{
			
		}

		public TrigganDBContext(DbContextOptions<TrigganDBContext> options)
			: base(options)
		{ }

		public TrigganDBContext()
        {
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultContainer("entities");
			modelBuilder.Entity<Post>().ToContainer("posts");
			modelBuilder.Entity<Project>().ToContainer("entities");
			modelBuilder.Entity<Post>().HasPartitionKey(e => e.Id);
			modelBuilder.Entity<Post>().HasPartitionKey(e => e.Id);
			modelBuilder.Entity<Post>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Post>().Property(e => e.Created).ValueGeneratedOnAdd();
			modelBuilder.Entity<Post>().Property(e => e.Updated).ValueGeneratedOnAddOrUpdate();
			modelBuilder.Entity<Post>().HasIndex(e => e.Slug).IsUnique();
			modelBuilder.Entity<Project>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Project>().Property(e => e.Created).ValueGeneratedOnAdd();
			modelBuilder.Entity<Project>().Property(e => e.Updated).ValueGeneratedOnAddOrUpdate();
			modelBuilder.Entity<Project>().HasIndex(e => e.Slug).IsUnique();
			modelBuilder.Entity<Project>().OwnsMany(p => p.Updates);

			foreach (var property in typeof(Entity).GetProperties())
			{
				var name = property.Name;
				if(name != "Id")
                {
                    modelBuilder.Entity<Entity>().Property(name).ToJsonProperty($"{name.First().ToString().ToLowerInvariant()}{name.Substring(1)}");
                }
            }
			var prop = typeof(Post).GetProperties();
			foreach (var property in typeof(Post).GetProperties())
			{
				var name = property.Name;
				if (name != "Id")
				{
					modelBuilder.Entity<Post>().Property(name).ToJsonProperty($"{name.First().ToString().ToLowerInvariant()}{name.Substring(1)}");
				}
			}
			foreach (var property in typeof(Project).GetProperties())
			{
				var name = property.Name;
				if (name != "Id" && name != "Updates")
				{
					modelBuilder.Entity<Project>().Property(name).ToJsonProperty($"{name.First().ToString().ToLowerInvariant()}{name.Substring(1)}");
				}
			}

			//var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
			//modelBuilder.Entity<Post>().Property(p => p.Tags).HasConversion(splitStringConverter);

			modelBuilder.Entity<Post>().Ignore(p => p.Tags);

			modelBuilder.Entity<Message>().ToContainer("messages");
			modelBuilder.Entity<Message>().HasPartitionKey(e => e.Id);
			modelBuilder.Entity<Message>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Message>().Property(e => e.Created).ValueGeneratedOnAdd();
			modelBuilder.Entity<Message>().Property(e => e.Updated).ValueGeneratedOnAddOrUpdate();

			foreach (var property in typeof(Message).GetProperties())
			{
				var name = property.Name;
				if (name != "Id")
				{
					modelBuilder.Entity<Message>().Property(name).ToJsonProperty($"{name.First().ToString().ToLowerInvariant()}{name.Substring(1)}");
				}
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
