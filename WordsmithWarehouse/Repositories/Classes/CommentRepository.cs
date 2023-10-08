using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {

        private readonly DataContext _context;

        public CommentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBookId(int bookId)
        {
            return await _context.Comments.Where(c => c.BookId == bookId).ToListAsync();
        } 
    }
}
