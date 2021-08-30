using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using triggan.BlogManager.Helpers;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class TrigganContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DBProvider DbType { get; set; }

        public TrigganContext(DbContextOptions<TrigganContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (DbType)
            {
                case DBProvider.MSSQL:
                    OnMSModelCreating(modelBuilder);
                    break;

                case DBProvider.Cosmos:
                    OnCosmosModelCreating(modelBuilder);
                    break;
            }
        }

        protected void OnMSModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().HasIndex(e => e.Slug).IsUnique();

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            modelBuilder.Entity<Post>().Property(p => p.Tags).HasConversion(splitStringConverter);
            modelBuilder.Entity<Project>().OwnsMany(p => p.Updates);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<User>().Ignore(e => e.Token);

            base.OnModelCreating(modelBuilder);
        }

        protected void OnCosmosModelCreating(ModelBuilder modelBuilder)
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
                if (name != "Id")
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

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is Entity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (Entity)entityEntry.Entity;
                entity.Updated = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.Created = entity.Created == DateTime.MinValue ? DateTime.Now : entity.Created;
                }
            }

            return base.SaveChanges();
        }
    }
}