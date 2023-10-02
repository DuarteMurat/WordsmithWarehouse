using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
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
                CoverType = model.CoverType,
                Pages = model.Pages,
                Publisher = model.Publisher,
                Synopsis = model.Synopsis,
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
                CoverType= book.CoverType,
                Pages = book.Pages,
                Publisher = book.Publisher,
                Synopsis= book.Synopsis,
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
                User = model.User,
                Library = model.Library,
                ReservationDate = model.ReservationDate,
                PickupDate = model.PickupDate,
                ReturnDate = model.ReturnDate,
                IsCancelled = model.IsCancelled,
                IsCompleted = model.IsCompleted,
                BookIds = model.BookIds,
            };
        }

        public BookReservationViewModel ConvertToBookReservationViewModel (BookReservation bookReservation)
        {
            return new BookReservationViewModel
            {
                Id = bookReservation.Id,
                User = bookReservation.User,
                Library = bookReservation.Library,
                ReservationDate = bookReservation.ReservationDate,
                PickupDate = bookReservation.PickupDate,
                ReturnDate = bookReservation.ReturnDate,
                IsCancelled = bookReservation.IsCancelled,
                IsCompleted = bookReservation.IsCompleted,
            };
        }

        public User ConvertToUser(RegisterNewUserViewModel model, string path, bool isNew)
        {
            return new User 
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageURL = path,
                Email = model.Email,
                UserName = model.Username,
            };

        }

        public RegisterNewUserViewModel ConvertToRegisterNewUserViewModel(User user)
        {
            return new RegisterNewUserViewModel 
            { 
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 ImageURL = user.ImageURL,
            };

        }

        public ManageUserViewModel ConvertToManageUserViewModel(User user)
        {
            return new ManageUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageURL = user.ImageURL,
                Email = user.Email,
                UserName= user.UserName,
            };
        }

        public IEnumerable<ManageUserViewModel> BulkConvertToManageUserViewModel(IEnumerable<User> users)
        {
            List<ManageUserViewModel> convertedUsers = new List<ManageUserViewModel>();
            foreach(var user in users)
            {
                convertedUsers.Add(new ManageUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageURL = user.ImageURL,
                    Email = user.Email,
                    UserName = user.UserName,
                });

            }
            
            return convertedUsers;
        }
    }
}
