using Microsoft.EntityFrameworkCore;
using ProfanityService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityService.Application.Db
{
    public class ProfanityDbContext: DbContext
    {
        public ProfanityDbContext(DbContextOptions<ProfanityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profanity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

                entity.Property(e => e.Word).HasColumnType("nvarchar(max)").IsRequired();

                entity.HasData(
                    new Profanity { Id = 1, Word = "fuck" },
                    new Profanity { Id = 2, Word = "retard" },
                    new Profanity { Id = 3, Word = "docker" },
                    new Profanity { Id = 4, Word = "monolith" }
                );
            });
        }


        public DbSet<Profanity> Profanities => Set<Profanity>();

    }
}
