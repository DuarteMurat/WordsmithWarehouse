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

        public Library ConvertToLibrary(LibraryViewModel model, bool isNew)
        {
            return new Library
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                City = model.City,
                Country = model.Country,
                PostalCode = model.PostalCode,
                OpeningHour = model.OpeningHour,
                ClosingHour = model.ClosingHour,
                PhoneNumber = model.PhoneNumber,
                IsOpened = model.IsOpened,
                Region = model.Region,
                Adress = model.Adress,
            };
        }

        public LibraryViewModel ConvertToLibraryViewModel(Library library)
        {
            return new LibraryViewModel
            {
                Id = library.Id,
                Name = library.Name,
                City = library.City,
                Country = library.Country,
                PostalCode = library.PostalCode,
                OpeningHour = library.OpeningHour,
                ClosingHour = library.ClosingHour,
                PhoneNumber = library.PhoneNumber,
                IsOpened = library.IsOpened,
                Region = library.Region,
                Adress = library.Adress,
            };
        }

        public BookReservation ConvertToBookReservation(BookReservationViewModel model, bool isNew)
        {
            return new BookReservation
            {
                Id = model.Id,
                Book = model.Book,
                User = model.User,
                Library = model.Library,
                ReservationDate = model.ReservationDate,
                PickupDate = model.PickupDate,
                ReturnDate = model.ReturnDate,
                IsCancelled = model.IsCancelled,
                IsCompleted = model.IsCompleted,
            };
        }

        public BookReservationViewModel ConvertToBookReservationViewModel (BookReservation bookReservation)
        {
            return new BookReservationViewModel
            {
                Id = bookReservation.Id,
                Book = bookReservation.Book,
                User = bookReservation.User,
                Library = bookReservation.Library,
                ReservationDate = bookReservation.ReservationDate,
                PickupDate = bookReservation.PickupDate,
                ReturnDate = bookReservation.ReturnDate,
                IsCancelled = bookReservation.IsCancelled,
                IsCompleted = bookReservation.IsCompleted,
            };
        }
    }
}
