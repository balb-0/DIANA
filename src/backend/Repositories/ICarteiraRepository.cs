using bakend.Models.DTOs;

namespace backend.Repositories
{
    public interface ICarteiraRepository
    {
        Task<List<CarteiraDto>> GetCarteirasByRiscoAsync(string tipoRisco);
    }
}