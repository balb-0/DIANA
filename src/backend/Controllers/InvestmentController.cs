using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        private readonly ILogger<InvestmentController> _logger;

        public InvestmentController(IInvestmentService investmentService, ILogger<InvestmentController> logger)
        {
            _investmentService = investmentService;
            _logger = logger;
        }

        [HttpGet("{clientId}")]
        public async Task<ActionResult<IEnumerable<Investment>>> GetInvestments(string clientId)
        {
            try
            {
                var investments = await _investmentService.GetInvestmentsByClientIdAsync(clientId);
                return Ok(investments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching investments for client {ClientId}", clientId);
                return StatusCode(500, "An error occurred while fetching investments");
            }
        }
    }
} 