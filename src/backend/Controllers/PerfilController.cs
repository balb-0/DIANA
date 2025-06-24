using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        [HttpGet("{clienteId}")]
        public IActionResult GetPerfilDistribuicao(string clienteId)
        {
            var perfil = _perfilService.ObterPerfilDistribuicao(clienteId);

            if (perfil == null)
            {
                return NotFound(new { message = "Cliente não encontrado ou sem perfil de risco calculado" });
            }

            return Ok(perfil);
        }
    }
}
