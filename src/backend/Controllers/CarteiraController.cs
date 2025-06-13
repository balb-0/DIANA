using backend.Models;
using backend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarteirasController : ControllerBase
    {
        private readonly ICarteiraService _carteiraService;

        public CarteirasController(ICarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        /// <summary>
        /// Retorna todos os clientes (id e nome) filtrados por tipo de risco declarado
        /// </summary>
        /// <param name="tipoRisco">Tipo de risco declarado (Conservador, Moderado, Sofisticado, Geral)</param>
        /// <returns>Lista de clientes filtrados por tipo de risco declarado</returns>
        [HttpGet("clientes/risco-declarado/{tipoRisco}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<ClienteSimplificadoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<List<ClienteSimplificadoDto>>), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetClientesByRiscoDeclarado([FromRoute] string tipoRisco)
        {
            // Obter o ID do usuário (assessor) do contexto
            var assessorId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(assessorId))
            {
                return Unauthorized(new { message = "ID do assessor não encontrado" });
            }

            var response = await _carteiraService.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}