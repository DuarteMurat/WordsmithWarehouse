using ClassLibrary.Entities;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Book ConvertToBook(BookViewModel model, string path, bool isNew);

        BookViewModel ConvertToBookViewModel(Book book);

        Author ConvertToAuthor(AuthorViewModel model, string path, bool isNew);

        AuthorViewModel ConvertToAuthorViewModel(Author author);

        SearchBookViewModel ConvertToSearchBookViewModel(Book book);

        Library ConvertToLibrary(LibraryViewModel model, bool isNew);

        LibraryViewModel ConvertToLibraryViewModel(Library library);
    }
}
