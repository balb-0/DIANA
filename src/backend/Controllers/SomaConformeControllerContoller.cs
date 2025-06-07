using Microsoft.AspNetCore.Mvc;
using SomaConformeApi.Models;
using SomaConformeApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaConformeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SomaConformeController : ControllerBase
    {
        private readonly ISomaConformeService _service;

        public SomaConformeController(ISomaConformeService service)
        {
            _service = service;
        }

        [HttpGet("por-categoria")]
        public async Task<ActionResult<List<SomaConformeCategoriaDto>>> GetSomaConformePorCategoria()
        {
            var result = await _service.SomaConformePorCategoriaAsync();
            return Ok(result);
        }
    }
}
