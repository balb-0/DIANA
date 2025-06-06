using backend.Models;
using backend.Models.DTOs;

namespace backend.Services
{
    public interface ICarteiraService
    {
        Task<ApiResponse<List<ClienteSimplificadoDto>>> GetClientesByRiscoDeclaradoAsync(string tipoRisco, string assessorId);
    }
}