using backend.Models;
using backend.Models.DTOs;
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

        public async Task<ApiResponse<List<ClienteSimplificadoDto>>> GetClientesByRiscoDeclaradoAsync(string tipoRisco, string assessorId)
        {
            try
            {
                _logger.LogInformation($"Iniciando busca por clientes com risco declarado: {tipoRisco} para o assessor: {assessorId}");

                // Validar tipo de risco - incluindo as variações de case
                var tiposValidos = new[] { "geral", "conservador", "moderado", "sofisticado" };
                if (!tiposValidos.Contains(tipoRisco.ToLower()))
                {
                    _logger.LogWarning($"Tipo de risco inválido recebido: {tipoRisco}");
                    return ApiResponse<List<ClienteSimplificadoDto>>.CreateError("Tipo de risco inválido. Valores aceitos: Conservador, Moderado, Sofisticado, Geral");
                }

                var clientes = await _carteiraRepository.GetClientesByRiscoDeclaradoAsync(tipoRisco, assessorId);

                _logger.LogInformation($"Encontrados {clientes.Count} clientes com risco declarado {tipoRisco} para o assessor {assessorId}");

                return ApiResponse<List<ClienteSimplificadoDto>>.CreateSuccess(clientes,
                    $"Clientes com risco declarado {tipoRisco} encontrados com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar clientes com risco declarado {tipoRisco} para o assessor {assessorId}. Detalhes: {ex.Message}");
                return ApiResponse<List<ClienteSimplificadoDto>>.CreateError("Erro interno do servidor");
            }
        }
    }
}