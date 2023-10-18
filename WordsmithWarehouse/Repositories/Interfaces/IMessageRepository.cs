using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {

        Task<List<Message>> GetMessagesByForumId(int forumId);
    }
}
