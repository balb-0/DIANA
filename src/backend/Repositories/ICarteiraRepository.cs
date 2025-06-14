using backend.Models.DTOs;

namespace backend.Repositories
{
    public interface ICarteiraRepository
    {
        Task<List<ClienteSimplificadoDto>> GetClientesByRiscoDeclaradoAsync(string tipoRisco, string assessorId);
    }
}