using backend.Repositories;

namespace backend.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly IComplianceRepository _complianceRepository;
        private readonly ILogger<ComplianceService> _logger;

        public ComplianceService(IComplianceRepository complianceRepository, ILogger<ComplianceService> logger)
        {
            _complianceRepository = complianceRepository;
            _logger = logger;
        }

        public async Task<int> GetClientsCountAsync(bool nonconforming = false)
        {
            try
            {
                var count = nonconforming 
                    ? await _complianceRepository.GetNonConformingClientsCountAsync()
                    : await _complianceRepository.GetConformingClientsCountAsync();

                _logger.LogInformation($"Retrieved {count} {(nonconforming ? "non-conforming" : "conforming")} clients");

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in compliance service while retrieving clients count");
                throw;
            }
        }
    }
}