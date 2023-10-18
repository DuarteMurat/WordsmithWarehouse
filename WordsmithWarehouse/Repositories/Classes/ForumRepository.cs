using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class ForumRepository : GenericRepository<Forum>, IForumRepository
    {
        private readonly DataContext _context;

        public ForumRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Forum> GetForumById(int id)
        {
            var forum = await _context.Forums.FirstOrDefaultAsync(a => a.Id == id);

            return forum;
        }
    }
}
