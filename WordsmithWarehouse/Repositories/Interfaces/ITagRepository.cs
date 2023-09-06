using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
       List<Tag> GetTagsList();
    }
}