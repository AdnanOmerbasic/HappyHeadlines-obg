using DraftService.Application.Events;
using DraftService.Application.Interfaces;
using DraftService.Contracts.Requests;
using DraftService.Contracts.Responses;
using DraftService.Domain.Entity;
using Monitoring.Shared;
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
            Serilog.Log.Information("{Event} Started retrieving all drafts", LogEvents.RetrivedAllDrafts);
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

            Serilog.Log.Information("{Event} Successfully retrieved all drafts", LogEvents.RetrivedAllDrafts);
            return Ok(response);
        }

        // GET api/<draftController>/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
        {
            Serilog.Log.Information("{Event} Started retrieving draft by id", LogEvents.RetrivedDraftById);
            var draft = await _repo.GetDraftByIdAsync(id, ct);  
            if (draft == null)
            {
                Serilog.Log.Warning("{Event} Couldnt retrieve any draft with id: {DraftId}", LogEvents.FailedToRetriveDraftById, id);
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

            Serilog.Log.Information("{Event} Retrived draft by id: {DraftId}", LogEvents.RetrivedDraftById, id);
            return Ok(response);
        }

        // POST api/<draftController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDraftRequest req, CancellationToken ct)
        {
            Serilog.Log.Information("{Event} Started to create draft", LogEvents.CreatedDraft);
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
            Serilog.Log.Information("{Event} Successfully created draft", LogEvents.CreatedDraft);
            return CreatedAtAction(nameof(Get), new { id = createdDraft.Id }, createdDraft);
        }

        // PUT api/<draftController>/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDraftRequest req, CancellationToken ct)
        {
            Serilog.Log.Information("{Event} Started to update draft", LogEvents.UpdatedDraft);
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
                Serilog.Log.Warning("{Event} Couldnt update any draft with id: {DraftId}", LogEvents.FailedToUpdateDraft, id);
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

            Serilog.Log.Information("{Event} Successfully updated draft with id: {DraftId}", LogEvents.UpdatedDraft, id);
            return Ok(response);
        }

        // DELETE api/<draftController>/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            Serilog.Log.Information("{Event} Started to delete draft", LogEvents.DeletedDraft);
            var deletedDraft = await _repo.DeleteDraftAsync(id, ct);
            if (deletedDraft == null)
            {
                Serilog.Log.Warning("{Event} Couldnt delete any draft with id: {DraftId}", LogEvents.FailedToDeleteDraft, id);
                return NotFound();
            }

            Serilog.Log.Information("{Event} Successfully deleted draft with id: {DraftId}", LogEvents.DeletedDraft, id);
            return NoContent();
        }
    }
}
