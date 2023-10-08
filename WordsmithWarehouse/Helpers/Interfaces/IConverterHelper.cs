using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Book ConvertToBook(BookViewModel model, string path, bool isNew);

        Book ConvertToBook(DetailsBookViewModel model, string path, bool isNew);

        BookViewModel ConvertToBookViewModel(Book book);

        DetailsBookViewModel ConvertToDetailsBookViewModel(Book book);

        Author ConvertToAuthor(AuthorViewModel model, string path, bool isNew);

        AuthorViewModel ConvertToAuthorViewModel(Author author);

        SearchBookViewModel ConvertToSearchBookViewModel(Book book);

        Library ConvertToLibrary(LibraryViewModel model, bool isNew);

        LibraryViewModel ConvertToLibraryViewModel(Library library);

        BookReservation ConvertToBookReservation(BookReservationViewModel model, bool Isnew);

        BookReservationViewModel ConvertToBookReservationViewModel(BookReservation bookReservation);

        User ConvertToUser (RegisterNewUserViewModel model, string path, bool isNew);

        RegisterNewUserViewModel ConvertToRegisterNewUserViewModel(User user);

        ManageUserViewModel ConvertToManageUserViewModel(User user);

        Task<IEnumerable<ManageUserViewModel>> BulkConvertToManageUserViewModel(IEnumerable<User> users);

        
    }
}
