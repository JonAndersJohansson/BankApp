using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
