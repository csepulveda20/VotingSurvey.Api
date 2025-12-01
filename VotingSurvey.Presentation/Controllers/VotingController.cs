using Microsoft.AspNetCore.Mvc;
using MediatR;
using VotingSurvey.Application.Models;
using VotingSurvey.Application.UseCases.Voting.Commands;
using VotingSurvey.Application.UseCases.Voting.Queries;
using VotingSurvey.Application.UseCases.Voting.Dtos;

namespace VotingSurvey.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotingController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoting request, [FromHeader(Name = "UserId")] Guid userId)
        {
            request = request with { CreatedById = userId };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpPost("{id:guid}/recipients")]
        public async Task<IActionResult> AddRecipients([FromQuery] Guid id, [FromBody] AddRecipients request)
        {
            request = request with { VotingId = id };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpPut("{id:guid}/edit")]
        public async Task<IActionResult> Edit([FromQuery] Guid id, [FromBody] EditVotingBeforeStart request)
        {
            request = request with { VotingId = id };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpPut("{id:guid}/extend-end")]
        public async Task<IActionResult> ExtendEnd([FromQuery] Guid id, [FromBody] ExtendVotingEnd request)
        {
            request = request with { VotingId = id };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpPut("{id:guid}/close-early")]
        public async Task<IActionResult> CloseEarly([FromQuery] Guid id, [FromBody] CloseVotingEarly request, [FromHeader(Name = "UserId")] Guid adminId)
        {
            request = request with { VotingId = id, AdminId = adminId };
            var result = await Sender.Send(request);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail([FromQuery] Guid id, [FromHeader(Name = "UserId")] Guid? userId)
        {
            var query = new GetVotingDetail { VotingId = id, UserId = userId };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("creator")]
        public async Task<IActionResult> ListByCreator([FromHeader(Name = "UserId")] Guid userId, [FromQuery] QueryParam qp)
        {
            var query = new ListByCreator { CreatorId = userId, Query = qp };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("recipient")]
        public async Task<IActionResult> ListForRecipient([FromHeader(Name = "UserId")] Guid userId, [FromQuery] QueryParam qp)
        {
            var query = new ListForRecipient { UserId = userId, Query = qp };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("{id:guid}/participants")]
        public async Task<IActionResult> Participants([FromQuery] Guid id)
        {
            var query = new ListVotingParticipants { VotingId = id };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("{id:guid}/participants-separated")]
        public async Task<IActionResult> ParticipantsSeparated([FromQuery] Guid id)
        {
            var query = new GetVotingParticipantsSeparated { VotingId = id };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("{id:guid}/recipients")]
        public async Task<IActionResult> Recipients([FromQuery] Guid id, [FromHeader(Name = "UserId")] Guid adminId)
        {
            var query = new AdminGetVotingRecipients { VotingId = id, AdminId = adminId };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }

        [HttpGet("dashboard/summary")]
        public async Task<IActionResult> DashboardSummary([FromHeader(Name = "UserId")] Guid adminId)
        {
            var query = new AdminDashboardSummary { AdminId = adminId };
            var result = await Sender.Send(query);
            return result.Match(success => Ok(result), error => Problem([.. error!]));
        }
    }
}
