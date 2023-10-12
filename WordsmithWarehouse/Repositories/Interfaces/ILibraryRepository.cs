using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ILibraryRepository : IGenericRepository<Library>
    {
        IEnumerable<SelectListItem> GetComboLibraries();
    }
}
