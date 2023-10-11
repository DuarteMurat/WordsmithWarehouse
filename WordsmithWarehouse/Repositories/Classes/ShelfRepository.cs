using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class ShelfRepository :GenericRepository<Shelf>, IShelfRepository
    {
        private readonly DataContext _context;

        public ShelfRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Shelf>> GetShelvesByUserAsync(string source)
        {
            List<Shelf> shelves = new List<Shelf>();
            if (string.IsNullOrEmpty(source))
                return shelves;

            string[] ids = source.Split(',');

            if (ids.Length <= 0) return shelves;

            foreach (string id in ids)
            {
                shelves.Add(await GetByIdAsync(int.Parse(id)));
            }

            return shelves;
        }
    }
}
