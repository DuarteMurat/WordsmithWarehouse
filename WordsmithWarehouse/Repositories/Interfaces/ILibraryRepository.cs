using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ILibraryRepository : IGenericRepository<Library>
    {
        Task<IEnumerable<SelectListItem>> GetComboLibraries();
    }
}
