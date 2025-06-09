using Microsoft.AspNetCore.Mvc;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}/perfil")]
        public IActionResult GetPerfilDeRisco(string id)
        {
            var perfil = _clientService.GetPerfilDeRisco(id);

            if (perfil == null)
                return NotFound("Cliente não encontrado.");

            return Ok(new { id = id, perfil_de_risco = perfil });
        }
    }
}
