using Microsoft.EntityFrameworkCore;
using SubscriberService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Application.Db
{
    public class SubscriberDbContext: DbContext
    {
        public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

                entity.Property(e => e.Email).HasColumnType("nvarchar(255)").IsRequired();
                entity.Property(e => e.IsSubscribed).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2(3)").HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2(3)").IsRequired(false);
            });
        }

        public DbSet<Subscription> Subscriptions => Set<Subscription>();
    }
}
