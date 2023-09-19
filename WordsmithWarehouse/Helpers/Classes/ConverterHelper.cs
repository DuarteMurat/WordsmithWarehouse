using ClassLibrary.Entities;
using System.Collections;
using System.Collections.Generic;
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
                ISBN = model.ISBN,
                Subtitle = model.Subtitle,
                User = model.User,
                AuthorId = model.AuthorId,
                tagIds = model.tagIds,
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
                ISBN = book.ISBN,
                Subtitle = book.Subtitle,
                User = book.User,
                AuthorId = book.AuthorId,
                tagIds= book.tagIds,
            };
        }

        public Author ConvertToAuthor(AuthorViewModel model, string path, bool isNew)
        {
            return new Author
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Description = model.Description,
                ImageURL = path,
            };
        }

        public AuthorViewModel ConvertToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                Name = author.Name,
                ImageURL = author.ImageURL,
                Description = author.Description,
            };
        }

        public SearchBookViewModel ConvertToSearchBookViewModel(Book book)
        {
            return new SearchBookViewModel
            {
                BookId = book.Id,
                ImageURL = book.ImageURL,
                IsAvailableOnline = book.IsAvailableOnline,
                IsAvailablePhysical = book.IsAvailablePhysical,
                Title = book.Title,
                AuthorId = book.AuthorId,
            };
        }
    }
}
