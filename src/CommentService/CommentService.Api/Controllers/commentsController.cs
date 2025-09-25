using CommentService.Application.Interface;
using CommentService.Contracts.Requests;
using CommentService.Contracts.Responses;
using CommentService.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class commentsController : ControllerBase
    {
        // GET: api/<commentsController>
        private readonly ICommentRepository _repo;
        private readonly IHttpClientFactory _httpClientFactory; 

        public commentsController(ICommentRepository repo, IHttpClientFactory httpClientFactory)
        {
            _repo = repo;
            _httpClientFactory = httpClientFactory;
        }

        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy =
            Policy<HttpResponseMessage>.Handle<HttpRequestException>()
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        [HttpGet]
        [Route("{continent}/{articleId}")]
        public async Task<IActionResult> GetAll([FromRoute] Continent continent, [FromRoute] int articleId, CancellationToken ct)
        {
            var comments = await _repo.GetAllAsync(continent, articleId, ct);
            var response = new CommentsResponse
            {
                Comments = comments.Select(a => new CommentResponse
                {
                    Id = a.Id,
                    Content = a.Content,
                    Continent = a.Continent,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    ArticleId = a.ArticleId,

                })
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{continent}/{articleId}/{id}")]
        public async Task<IActionResult> Get([FromRoute] Continent continent, [FromRoute] int articleId, [FromRoute] int id, CancellationToken ct)
        {
            var comment = await _repo.GetCommentById(id, ct);
            if (comment == null)
            {
                return NotFound();
            }
            var response = new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                Continent = comment.Continent,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                ArticleId = comment.ArticleId,
            };
            return Ok(response);
        }

        // POST api/<commentsController>
        [HttpPost]
        [Route("{continent}/{articleId}")]
        public async Task<IActionResult> Create([FromRoute] Continent continent, [FromRoute] int articleId, [FromBody] CreateCommentRequest req, CancellationToken ct)
        {
            var httpClient = _httpClientFactory.CreateClient("ProfanityCheck");
            var text = req.Content;
            bool isContentClean = false;
            try
            {
                var response = await _circuitBreakerPolicy.ExecuteAsync(async (ct) => await httpClient.PostAsJsonAsync("api/profanity", new { text = req.Content }, ct), ct);
                var res = await response.Content.ReadFromJsonAsync<ProfanityResponse>(ct);
                if (res != null)
                {
                    text = res.FilteredText;
                    isContentClean = !res.IsContentClean;
                }
                var filteredComment = new Comment
                {
                    Content = text,
                    Continent = continent,
                    ArticleId = articleId,
                };

                var createdFilteredComment = await _repo.CreateCommentAsync(filteredComment, ct);

                return CreatedAtAction(nameof(Get), new { id = filteredComment.Id, continent = filteredComment.Continent, articleId = filteredComment.ArticleId }, createdFilteredComment);
            }
            catch
            {

            }

            var comment = new Comment
            {
                Content = req.Content,
                Continent = continent,
                ArticleId = articleId,
            };
            var createdComment = await _repo.CreateCommentAsync(comment, ct);

            return CreatedAtAction(nameof(Get), new { id = comment.Id, continent = comment.Continent, articleId = comment.ArticleId }, createdComment);
        }

        // PUT api/<commentsController>/5
        [HttpPut]
        [Route("{continent}/{articleId}/{id}")]
        public async Task<IActionResult> Update([FromRoute] Continent continent, [FromRoute] int articleId, [FromRoute] int id, [FromBody] UpdateCommentRequest req, CancellationToken ct)
        {
            var updatedComment = new Comment
            {
                Id = id,
                Continent = continent,
                Content = req.Content,
                ArticleId = articleId,
            };

            var updated = await _repo.UpdateCommentAsync(updatedComment, ct);

            if (updated == null)
            {
                return NotFound();
            }

            var response = new CommentResponse
            {
                Id = updated.Id,
                Content = updated.Content,
                Continent = updated.Continent,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt,
                ArticleId = updated.ArticleId,
            };

            return Ok(response);
        }

        // DELETE api/<commentsController>/5
        [HttpDelete("{continent}/{articleId}/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Continent continent, [FromRoute] int articleId, [FromRoute] int id, CancellationToken ct)
        {
            var comment = await _repo.DeleteCommentAsync(continent, articleId, id, ct);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }
}
