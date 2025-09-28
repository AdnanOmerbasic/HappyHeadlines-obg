using ArticleService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Db
{
    public class ArticleDbContext: DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

                entity.Property(e => e.Title).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.Content).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.AuthorName).HasColumnType("nvarchar(200)").IsRequired();
                entity.Property(e => e.Continent).HasConversion<string>().HasColumnType("nvarchar(5)").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2(3)").HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2(3)").IsRequired(false);
            });
        }

        public DbSet<Article> Articles => Set<Article>();
    }
}
