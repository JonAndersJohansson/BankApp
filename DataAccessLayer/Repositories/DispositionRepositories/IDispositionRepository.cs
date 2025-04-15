using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.DispositionRepositories
{
    public interface IDispositionRepository
    {
        Task<List<Disposition>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(Disposition disposition);
        void Delete(Disposition disposition);
        Task SaveAsync();
    }
}
