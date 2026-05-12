using FinanceService.Application.Commands;
using FinanceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("finance")]
    public sealed class FinanceController : BaseController
    {
        public FinanceController(IMediator mediator) : base(mediator) { }

        [HttpGet("rates")]
        public async Task<IActionResult> GetRates(CancellationToken ct)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return Unauthorized();

            var rates = await _mediator.Send(new GetRatesQuery(userId), ct);
            return Ok(rates);
        }

        [HttpPost("favorites/{currencyId:guid}")]
        public async Task<IActionResult> AddFavorite(Guid currencyId, CancellationToken ct)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return Unauthorized();

            await _mediator.Send(new AddFavoriteCommand(userId, currencyId), ct);
            return NoContent();
        }

        [HttpDelete("favorites/{currencyId:guid}")]
        public async Task<IActionResult> RemoveFavorite(Guid currencyId, CancellationToken ct)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return Unauthorized();

            await _mediator.Send(new RemoveFavoriteCommand(userId, currencyId), ct);
            return NoContent();
        }
    }
}
