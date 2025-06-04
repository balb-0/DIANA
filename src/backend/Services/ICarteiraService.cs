using backend.Models;
using bakend.Models.DTOs;

namespace backend.Services
{
    public interface ICarteiraService
    {
        Task<ApiResponse<List<CarteiraDto>>> GetCarteirasByRiscoAsync(string tipoRisco);
    }
}