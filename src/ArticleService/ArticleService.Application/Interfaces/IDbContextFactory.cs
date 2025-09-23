using ArticleService.Application.Db;
using ArticleService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Interfaces
{
    public interface IDbContextFactory
    {
        ArticleDbContext CreateDbContext(Continent continent);
    }
}
