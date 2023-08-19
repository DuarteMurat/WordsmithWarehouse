using ClassLibrary.Entities;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers.Classes
{
    public class ConverterHelper : IConverterHelper
    {
        public Book ConvertToBook(BookViewModel model, string path, bool isNew)
        {
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                ImageURL = path,
                IsAvailableOnline = model.IsAvailableOnline,
                IsAvailablePhysical = model.IsAvailablePhysical,
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                Subtitle = model.Subtitle,
                User = model.User,
            };
        }

        public BookViewModel ConvertToBookViewModel(Book book)
        {
            return new BookViewModel
            {
                Id = book.Id,
                ImageURL = book.ImageURL,
                IsAvailableOnline = book.IsAvailableOnline,
                IsAvailablePhysical = book.IsAvailablePhysical,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Subtitle = book.Subtitle,
                User = book.User,
            };
        }
    }
}
