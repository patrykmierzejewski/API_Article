using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Entities
{
    public class ArticleContext : DbContext
    {
        private string _connectionString = "Server=localhost\\SQLEXPRESS;Database=ArticleDb;Trusted_Connection=True";
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Information> Informations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role);

            modelBuilder.Entity<Article>()
                .HasOne(x => x.Source)
                .WithOne(y => y.Article)
                .HasForeignKey<Source>(i => i.ArticleId);

            modelBuilder.Entity<Article>()
                .HasMany(x => x.Informations)
                .WithOne(y => y.Article)
                .HasForeignKey(x => x.ArticleId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
