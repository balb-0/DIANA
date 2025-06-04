using backend.Models;
using bakend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
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
        /// Retorna todas as carteiras de um tipo específico de risco
        /// </summary>
        /// <param name="tipoRisco">Tipo de risco (Conservador, Moderado, Sofisticado)</param>
        /// <returns>Lista de carteiras filtradas por tipo de risco</returns>
        [HttpGet("tipo/{tipoRisco}")]
        [ProducesResponseType(typeof(ApiResponse<List<CarteiraDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<List<CarteiraDto>>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCarteirasByTipo([FromRoute] string tipoRisco)
        {
            var response = await _carteiraService.GetCarteirasByRiscoAsync(tipoRisco);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
