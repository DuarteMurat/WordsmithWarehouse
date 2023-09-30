using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        IEnumerable<SelectListItem> GetComboAuthors();

        Task<Author> GetAuthorById(int id);
    }
}
