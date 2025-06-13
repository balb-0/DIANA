using backend.Models;

namespace backend.Services
{
    public interface IInvestmentService
    {
        Task<IEnumerable<Investment>> GetInvestmentsByClientIdAsync(string clientId);
    }
} 