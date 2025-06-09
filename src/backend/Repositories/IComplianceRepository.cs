namespace backend.Repositories
{
    public interface IComplianceRepository
    {
        Task<int> GetConformingClientsCountAsync();
        Task<int> GetNonConformingClientsCountAsync();
    }
}
