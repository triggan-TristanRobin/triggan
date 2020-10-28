using Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

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
			modelBuilder.Entity<Entity>().HasKey(e => e.Id);
			modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Entity>().Property(e => e.Created).ValueGeneratedOnAdd();
			modelBuilder.Entity<Entity>().HasIndex(e => e.Slug).IsUnique();

			var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
			modelBuilder.Entity<Post>().Property(p => p.Tags).HasConversion(splitStringConverter);

			modelBuilder.Entity<Message>().HasKey(e => e.Id);
			modelBuilder.Entity<Message>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Message>().Property(e => e.Created).ValueGeneratedOnAdd();

			base.OnModelCreating(modelBuilder);
		}
	}
}
