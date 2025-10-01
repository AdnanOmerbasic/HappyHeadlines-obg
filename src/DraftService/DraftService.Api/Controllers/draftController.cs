using DraftService.Application.Interfaces;
using DraftService.Contracts.Requests;
using DraftService.Contracts.Responses;
using DraftService.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DraftService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class draftController : ControllerBase
    {
        private readonly IDraftRepository _repo;

        public draftController(IDraftRepository repo)
        {
            _repo = repo;
        }

        // GET: api/<draftController>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var drafts = await _repo.GetAllDraftAsync(ct);
            var response = new DraftsResponse
            {
                Drafts = drafts.Select(d => new DraftResponse
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    Continent = d.Continent,
                    AuthorName = d.AuthorName,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
            };
            
            return Ok(response);
        }

        // GET api/<draftController>/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
        {
            var draft = await _repo.GetDraftByIdAsync(id, ct);  
            if (draft == null)
            {
                return NotFound();
            }
            var response = new DraftResponse
            {
                Id = draft.Id,
                Title = draft.Title,
                Content = draft.Content,
                Continent = draft.Continent,
                AuthorName = draft.AuthorName,
                CreatedAt = draft.CreatedAt,
                UpdatedAt = draft.UpdatedAt
            };

            return Ok(response);
        }

        // POST api/<draftController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDraftRequest req, CancellationToken ct)
        {
            var draft = new Draft
            {
                Title = req.Title,
                Content = req.Content,
                AuthorName = req.AuthorName,
                Continent = req.Continent,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdDraft = await _repo.CreateDraftAsync(draft, ct);
            return CreatedAtAction(nameof(Get), new { id = createdDraft.Id }, createdDraft);
        }

        // PUT api/<draftController>/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDraftRequest req, CancellationToken ct)
        {
            var draft = new Draft
            {
                Id = id,
                Title = req.Title,
                Content = req.Content,
                Continent = req.Continent,
                AuthorName = req.AuthorName,
                UpdatedAt = DateTime.UtcNow
            };
            var updatedDraft = await _repo.UpdateDraftAsync(draft, ct);

            if (updatedDraft == null)
            {
                return NotFound();
            }

            var response = new DraftResponse
            {
                Id = updatedDraft.Id,
                Title = updatedDraft.Title,
                Content = updatedDraft.Content,
                AuthorName = updatedDraft.AuthorName,
                CreatedAt = updatedDraft.CreatedAt,
                UpdatedAt = updatedDraft.UpdatedAt
            };

            return Ok(response);
        }

        // DELETE api/<draftController>/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var deletedDraft = await _repo.DeleteDraftAsync(id, ct);
            if (deletedDraft == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
