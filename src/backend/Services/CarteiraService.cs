using backend.Models;
using bakend.Models.DTOs;
using backend.Repositories;
 

namespace backend.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly ILogger<CarteiraService> _logger;

        public CarteiraService(ICarteiraRepository carteiraRepository, ILogger<CarteiraService> logger)
        {
            _carteiraRepository = carteiraRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<List<CarteiraDto>>> GetCarteirasByRiscoAsync(string tipoRisco)
        {
            try
            {
                // Validar tipo de risco
                var tiposValidos = new[] { "geral", "conservador", "moderado", "sofisticado" };
                if (!tiposValidos.Contains(tipoRisco.ToLower()))
                {
                    return ApiResponse<List<CarteiraDto>>.CreateError("Tipo de risco inválido. Valores aceitos: Conservador, Moderado, Sofisticado");
                }

                var carteiras = await _carteiraRepository.GetCarteirasByRiscoAsync(tipoRisco);

                _logger.LogInformation($"Encontradas {carteiras.Count} carteiras do tipo {tipoRisco}");

                return ApiResponse<List<CarteiraDto>>.CreateSuccess(carteiras, $"Carteiras do tipo {tipoRisco} encontradas com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar carteiras do tipo {tipoRisco}");
                return ApiResponse<List<CarteiraDto>>.CreateError("Erro interno do servidor");
            }
        }
    }
}