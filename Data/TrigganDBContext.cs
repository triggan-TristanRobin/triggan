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
			//modelBuilder.Entity<Post>().HasData(new Post() { Id = 1, Title = "First post", Content = "Test 1" });
			var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
			modelBuilder.Entity<Post>().Property(nameof(Post.Tags)).HasConversion(splitStringConverter);
			//modelBuilder.HasSequence<int>("PostId", schema: "dbo")
			//	.StartsAt(0)
			//	.IncrementsBy(1);

			//modelBuilder.Entity<Post>()
			//	.Property(p => p.Id)
			//	.HasDefaultValueSql("NEXT VALUE FOR dbo.PostId");
			base.OnModelCreating(modelBuilder);
		}
	}
}
