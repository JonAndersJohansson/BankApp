using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.DispositionRepositories
{
    public class DispositionRepository : IDispositionRepository
    {
        private readonly BankAppDataContext _context;

        public DispositionRepository(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<List<Disposition>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddAsync(Disposition disposition)
        {
            await _context.Dispositions.AddAsync(disposition);
        }

        public void Delete(Disposition disposition)
        {
            _context.Dispositions.Remove(disposition);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
