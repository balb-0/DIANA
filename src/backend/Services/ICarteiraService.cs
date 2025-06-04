using backend.Models;
using backend.Models.DTOs;

namespace backend.Services
{
    public interface ICarteiraService
    {
        Task<ApiResponse<List<CarteiraDto>>> GetCarteirasByRiscoAsync(string tipoRisco);
    }
}