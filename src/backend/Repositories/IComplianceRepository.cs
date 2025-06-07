using backend.Models;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<IEnumerable<Client>> GetBySegmentAsync(string segment);
    Task<IEnumerable<Client>> GetByRiskProfileAsync(string risk);
}