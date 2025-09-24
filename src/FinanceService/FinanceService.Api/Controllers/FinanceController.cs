using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Api.Controllers
{
    [ApiController]
    [Route("finance/api/[controller]")]
    [Authorize]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpPost("{userId}/favorites/{currencyId}")]
        public async Task<IActionResult> AddFavorite(Guid userId, int currencyId)
        {
            await _financeService.AddFavoriteAsync(userId, currencyId);
            return Ok();
        }

        [HttpDelete("{userId}/favorites/{currencyId}")]
        public async Task<IActionResult> RemoveFavorite(Guid userId, int currencyId)
        {
            await _financeService.RemoveFavoriteAsync(userId, currencyId);
            return Ok();
        }

        [HttpGet("{userId}/favorites")]
        public async Task<ActionResult<List<Currency>>> GetFavorites(Guid userId)
        {
            var favorites = await _financeService.GetUserFavoriteCurrenciesAsync(userId);
            return Ok(favorites);
        }
    }
}
