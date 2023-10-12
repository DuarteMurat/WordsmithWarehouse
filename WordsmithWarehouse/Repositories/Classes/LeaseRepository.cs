using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class LeaseRepository : GenericRepository<Lease>, ILeaseRepository
    {
        private readonly DataContext _context;

        public LeaseRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
