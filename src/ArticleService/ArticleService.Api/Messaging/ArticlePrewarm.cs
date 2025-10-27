using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using Redis.Shared.Interfaces;

namespace ArticleService.Api.Messaging
{
    public class ArticlePrewarm:BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
       public ArticlePrewarm(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IArticleRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<IRedisCache>();

                var recentArticles = await repo.GetRecentArticles(14, Continent.NA, stoppingToken);

                foreach (var article in recentArticles)
                {
                    await cache.SetDataAsync($"article:{article.Continent}:{article.Id}", article);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
