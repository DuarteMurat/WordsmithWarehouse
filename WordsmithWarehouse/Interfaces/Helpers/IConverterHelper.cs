using ClassLibrary.Entities;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Interfaces.Helpers
{
    public interface IConverterHelper
    {
        Book ConvertToBook(BookViewModel model, string path, bool isNew);

        BookViewModel ConvertToBookViewModel(Book book);
    }
}
