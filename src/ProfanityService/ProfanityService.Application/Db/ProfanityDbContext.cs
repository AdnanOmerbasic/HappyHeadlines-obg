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

        public DbSet<Profanity> Profanities => Set<Profanity>();

    }
}
