using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ILeaseRepository : IGenericRepository<Lease>
    {
        Task<List<Fine>> GetFinesAsync();
    }
}
