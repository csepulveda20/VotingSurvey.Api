using Microsoft.AspNetCore.Mvc;
using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.UseCases.Vote.Commands;
using VotingSurvey.Application.UseCases.Vote.Queries;

namespace VotingSurvey.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ApiBaseController
    {
        [HttpPost("{votingId:guid}")]
        public async Task<IActionResult> Select([FromQuery] Guid votingId, [FromBody] SelectVote request, [FromHeader(Name = "UserId")] Guid userId)
        {
            request = request with { VotingId = votingId, UserId = userId };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpPut("{votingId:guid}/confirm")]
        public async Task<IActionResult> Confirm([FromQuery] Guid votingId, [FromBody] ConfirmVote request, [FromHeader(Name = "UserId")] Guid userId)
        {
            request = request with { VotingId = votingId, UserId = userId };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("{votingId:guid}/status")]
        public async Task<IActionResult> Status([FromQuery] Guid votingId, [FromHeader(Name = "UserId")] Guid userId)
        {
            var query = new GetUserVoteStatus { VotingId = votingId, UserId = userId };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }
    }
}
