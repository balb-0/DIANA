using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceService _complianceService;
        private readonly ILogger<ComplianceController> _logger;

        public ComplianceController(IComplianceService complianceService, ILogger<ComplianceController> logger)
        {
            _complianceService = complianceService;
            _logger = logger;
        }

        [HttpGet("clients-count")]
        public async Task<ActionResult<int>> GetClientsCount([FromQuery] bool nonconforming = false)
        {
            try
            {
                var count = await _complianceService.GetClientsCountAsync(nonconforming);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients count");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}