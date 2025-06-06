using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Npgsql;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComplianceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ComplianceController> _logger;

        public ComplianceController(IConfiguration configuration, ILogger<ComplianceController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet("clients-count")]
        public async Task<ActionResult<int>> GetClientsCount([FromQuery] bool nonconforming = false)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                var functionName = nonconforming 
                    ? "obter_quantidade_clientes_inconformes" 
                    : "obter_quantidade_clientes_conformes";

                using var command = new NpgsqlCommand($"SELECT {functionName}()", connection);
                var result = await command.ExecuteScalarAsync();

                var count = Convert.ToInt32(result);
                
                _logger.LogInformation($"Retrieved {count} {(nonconforming ? "non-conforming" : "conforming")} clients");
                
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