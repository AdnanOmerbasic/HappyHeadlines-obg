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
            modelBuilder.Entity<Article>().Property(Article => Article.Continent).HasConversion<string>();

        }

        public DbSet<Article> Articles => Set<Article>();
    }
}
