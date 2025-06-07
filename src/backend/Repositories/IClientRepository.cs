namespace backend.Repositories
{
    public interface IComplianceRepository
    {
        // Defina os métodos necessários, por exemplo:
        Task<int> GetConformingClientsCountAsync();
        Task<int> GetNonConformingClientsCountAsync();
    }
}