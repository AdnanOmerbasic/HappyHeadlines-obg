using CommentService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Application.Db
{
    public class CommentDbContext: DbContext
    {
        public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

                entity.Property(e => e.Content).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2(3)").HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2(3)").IsRequired(false);
                entity.Property(e => e.Continent).HasConversion<string>().HasColumnType("nvarchar(5)").IsRequired();
                entity.Property(e => e.ArticleId).HasColumnType("int").IsRequired();
            });
        }
        public DbSet<Comment> Comments => Set<Comment>();

    }
}
