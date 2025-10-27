using ArticleService.Application.Interfaces;
using ArticleService.Contracts.Requests;
using ArticleService.Contracts.Responses;
using ArticleService.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Redis.Shared.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArticleService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IRedisCache _cache;
        private readonly IArticleRepository _repo;
        
        private static readonly Counter ArticleCacheHits = Metrics.CreateCounter("article_cache_hits_total", "Total number of article cache hits");
        private static readonly Counter ArticleCacheMisses = Metrics.CreateCounter("article_cache_misses_total", "Total number of article cache misses");

        public ArticlesController(IArticleRepository repo, IRedisCache cache)
        {
            _cache = cache;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleRequest req, CancellationToken ct)
        {
            var article = new Article
            {
                Title = req.Title,
                AuthorName = req.AuthorName,
                Content = req.Content,
                Continent = req.Continent
            };
            var createdArticle = await _repo.CreateArticleAsync(article, ct);

            await _cache.RemoveAsync($"article:{createdArticle.Continent}:{createdArticle.Id}");

            return CreatedAtAction(nameof(Get), new { id = createdArticle.Id, continent = createdArticle.Continent }, createdArticle);
        }

        [HttpGet]
        [Route("{continent}")]
        public async Task<IActionResult> GetAll([FromRoute] Continent continent, CancellationToken ct)
        {
            var articles = await _repo.GetAllAsync(continent, ct);
            var response = new ArticlesResponse
            {
                Articles = articles.Select(a => new ArticleResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    AuthorName = a.AuthorName,
                    Content = a.Content,
                    Continent = a.Continent,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a?.UpdatedAt,

                })
            };
            return Ok(response);

        }

        [HttpGet]
        [Route("{continent}/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromRoute] Continent continent, CancellationToken ct)
        {
            var cached = await _cache.GetDataAsync<Article>($"article:{continent}:{id}");

            if (cached != null)
            {
                ArticleCacheHits.Inc();
                return Ok(new ArticleResponse
                {
                    Id = cached.Id,
                    Title = cached.Title,
                    AuthorName = cached.AuthorName,
                    Content = cached.Content,
                    Continent = cached.Continent,
                    CreatedAt = cached.CreatedAt,
                    UpdatedAt = cached?.UpdatedAt,
                });
            }

            var article = await _repo.GetArticleByIdAsync(id, continent, ct);
            if (article == null)
            {
                return NotFound();
            }

            var response = new ArticleResponse
            {
                Id = article.Id,
                Title = article.Title,
                AuthorName = article.AuthorName,
                Content = article.Content,
                Continent = article.Continent,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article?.UpdatedAt,
            };
            ArticleCacheMisses.Inc();
            return Ok(response);
        }


        [HttpPut]
        [Route("{continent}/{id}")]
        public async Task<IActionResult> Update([FromRoute] Continent continent, [FromRoute] int id, [FromBody] UpdateArticleRequest req, CancellationToken ct)
        {
            var updatedArticle = new Article
            {
                Id = id,
                Title = req.Title,
                AuthorName = req.AuthorName,
                Content = req.Content,
                Continent = continent,              
            };

            var updated = await _repo.UpdateArticleAsync(updatedArticle, ct);

            if(updated == null)
            {
                return NotFound();
            }

            await _cache.RemoveAsync($"article:{updated.Continent}:{updated.Id}");

            var response = new ArticleResponse
            {
                Id = updated.Id,
                Title = updated.Title,
                AuthorName = updated.AuthorName,
                Content = updated.Content,
                Continent = updated.Continent,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(response);    
        }

        [HttpDelete]
        [Route("{continent}/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromRoute] Continent continent, CancellationToken ct)
        {
            var article = await _repo.DeleteArticleAsync(id, continent, ct);
            if (article == null)
            {
                return NotFound();
            }

            await _cache.RemoveAsync($"article:{continent}:{id}");

            return Ok(article);
        }
    }
}
