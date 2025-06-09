namespace backend.Services
{
    public interface IComplianceService
    {
        Task<int> GetClientsCountAsync(bool nonconforming = false);
    }
}