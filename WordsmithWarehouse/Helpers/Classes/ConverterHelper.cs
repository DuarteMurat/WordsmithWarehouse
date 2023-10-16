using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Helpers.Classes
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly IBookRepository _bookRepository;
        public ConverterHelper(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }
        public Book ConvertToBook(BookViewModel model, string path, bool isNew, string bookpath)
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
                ReleaseYear = model.ReleaseYear,
                BookFileURL = bookpath,
            };
        }

        public Book ConvertToBook(DetailsBookViewModel model, string path, bool isNew, string bookpath)
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
                ReleaseYear = model.ReleaseYear,
                BookFileURL = bookpath,
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
                tagIds = book.tagIds,
                CoverType = book.CoverType,
                Pages = book.Pages,
                Publisher = book.Publisher,
                Synopsis = book.Synopsis,
                ReleaseYear = book.ReleaseYear,
            };
        }

        public DetailsBookViewModel ConvertToDetailsBookViewModel(Book book)
        {
            return new DetailsBookViewModel
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
                tagIds = book.tagIds,
                CoverType = book.CoverType,
                Pages = book.Pages,
                Publisher = book.Publisher,
                Synopsis = book.Synopsis,
                ReleaseYear = book.ReleaseYear,
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
                UserId = model.Id,
                BookId = model.Id,
                LibraryId = model.Id,
                ReservationDate = model.ReservationDate,
            };
        }

        public BookReservationViewModel ConvertToBookReservationViewModel(BookReservation bookReservation)
        {
            return new BookReservationViewModel
            {
                Id = bookReservation.Id,
                UserId= bookReservation.UserId,
                LibraryId= bookReservation.LibraryId,
                ReservationDate = bookReservation.ReservationDate,
                BoookId = bookReservation.BookId,
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageURL = user.ImageURL,
                Email = user.Email,
                UserName = user.UserName,
            };
        }

        public async Task<IEnumerable<ManageUserViewModel>> BulkConvertToManageUserViewModel(IEnumerable<User> users)
        {
            string customer = "Customer";
            string employee = "Employee";
            string admin = "Admin";
            string deactivated = "Deactivated";
            string role = string.Empty;

            List<ManageUserViewModel> convertedUsers = new List<ManageUserViewModel>();
            foreach (var user in users)
            {
                if (await _userHelper.IsUserInRoleAsync(user, customer))
                    role = customer;
                if (await _userHelper.IsUserInRoleAsync(user, employee))
                    role = employee;
                if (await _userHelper.IsUserInRoleAsync(user, admin))
                    role = admin;
                if (await _userHelper.IsUserInRoleAsync(user, deactivated))
                    role = deactivated;


                convertedUsers.Add(new ManageUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageURL = user.ImageURL,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = role,
                });

            }

            return convertedUsers;
        }


        public Shelf ConvertToShelf(ShelfViewModel model, bool isNew)
        {
            return new Shelf
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Description = model.Description,
            };
        }

        public ShelfViewModel ConvertToShelfViewModel(Shelf shelf)
        {
            return new ShelfViewModel
            {
                Id = shelf.Id,
                Name = shelf.Name,
                Description = shelf.Description,
                BookIds = shelf.BookIds,
            };
        }

        public Lease ConvertToLease(LeaseViewModel model, bool isNew)
        {
            return new Lease 
            {
                Id = isNew ? 0 : model.Id,
                BookId = model.Book.Id,
                UserId = model.UserId,
                LibraryId = model.LibraryId,
                ReturnDate = model.ReturnDate,
                IsCompleted = model.IsCompleted,
                OnGoing = model.OnGoing,
                LeaseTime = model.LeaseTime,
                PickUpDate = model.PickUpDate,
            };

        }

        public LeaseViewModel ConvertToLeaseViewModel(Lease lease)
        {
            return new LeaseViewModel
            {
                Id = lease.Id,
                IsCompleted = lease.IsCompleted,
                OnGoing = lease.OnGoing,
                LeaseTime = lease.LeaseTime,
                PickUpDate = lease.PickUpDate,
                ReturnDate = lease.ReturnDate,
                UserId = lease.UserId,
                LibraryId = lease.LibraryId,
                
            };

        }

    }
}
