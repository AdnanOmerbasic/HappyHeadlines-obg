using Microsoft.AspNetCore.Mvc;
using ProfanityService.Application.Db;
using ProfanityService.Application.Interface;
using ProfanityService.Contracts.Requests;
using ProfanityService.Contracts.Responses;
using ProfanityService.Domain.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProfanityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class profanityController : ControllerBase
    {
        private readonly IProfanityRepository _repo;
        public profanityController(IProfanityRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profanities = await _repo.GetAllAsync();
            return Ok(profanities);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProfanities([FromBody] CheckProfanityRequest req, CancellationToken ct)
        {
            var profanity = new Profanity
            {
                Word = req.Text,
            };
            var checkProfanity = await _repo.CheckForProfanities(profanity, ct);

            var response = new ProfanityResponse { FilteredText = checkProfanity.Item1, IsContentClean = !checkProfanity.Item2 };
            return Ok(response);
        }

    }
}
