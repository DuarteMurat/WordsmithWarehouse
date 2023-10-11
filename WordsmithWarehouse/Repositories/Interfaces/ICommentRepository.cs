using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {

        Task<IEnumerable<Comment>> GetCommentsByBookId(int bookId);

        bool CheckForComment(List<Comment> comments, string userId);

        float GetAverageRatings(List<Comment> comments);

    }
}
