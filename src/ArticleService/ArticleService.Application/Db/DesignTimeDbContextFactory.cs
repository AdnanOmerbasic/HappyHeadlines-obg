using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Db
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ArticleDbContext>
    {
        public ArticleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ArticleDbContext>().
                UseSqlServer("Server=localhost,1433;Database=ArticleDb;User Id=sa;Passw0rd!;TrustServerCertificate=True");

            return new ArticleDbContext(builder.Options);
        }
    }
}
