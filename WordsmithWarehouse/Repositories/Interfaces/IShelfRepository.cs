using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IShelfRepository : IGenericRepository<Shelf>
    {
        Task<List<Shelf>> GetShelvesByUserAsync(string shelves);
    }
}
