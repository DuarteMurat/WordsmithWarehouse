using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IForumRepository : IGenericRepository<Forum>
    {

        Task<Forum> GetForumById(int id);
    }
}
