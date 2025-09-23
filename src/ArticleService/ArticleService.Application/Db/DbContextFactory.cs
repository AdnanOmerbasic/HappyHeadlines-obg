using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Db
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ArticleDbContext CreateDbContext(Continent continent)
        {
            var dbKey = continent.ToString();
            var connectionString = _configuration.GetConnectionString(dbKey) ?? throw new ArgumentException($"Connection string for continent '{dbKey}' not found.");
            var builder = new DbContextOptionsBuilder<ArticleDbContext>();

            builder.UseSqlServer(connectionString);

            return new ArticleDbContext(builder.Options);
        }
    }
}
