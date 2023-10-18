using ClassLibrary.Entities;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserHelper _userHelper;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly ILibraryRepository _libraryRepository;

        public HomeController(ILogger<HomeController> logger,
            ITagRepository tagRepository,
            IBookRepository bookRepository,
            IUserHelper userHelper,
            IAuthorRepository authorRepository,
            ILeaseRepository leaseRepository,
            ILibraryRepository libraryRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _bookRepository = bookRepository;
            _userHelper = userHelper;
            _authorRepository = authorRepository;
            _leaseRepository = leaseRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = new FrontPageViewModel
            {
                Books = _bookRepository.GetBooksList(),
            };

            model.BestSellerBooks = await _tagRepository.GetBooksWithTags(model.Books, "Best Seller");

            model.BookOfTheMonth = await _tagRepository.GetBooksWithTags(model.Books, "BookOfTheMonth");

            model.Classics = await _tagRepository.GetBooksWithTags(model.Books, "Classics");

            model.NewReleases = await _tagRepository.GetBooksWithTags(model.Books, "New Release");

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> GetWebsiteAnalytics()
        {
            var books = await _bookRepository.GetAll().ToListAsync();
            var users = await _userHelper.GetAllAsync();
            var authors = await _authorRepository.GetAll().ToListAsync();
            var leases = await _leaseRepository.GetAll().ToListAsync();
            var tags = await _tagRepository.GetAll().ToListAsync();
            var libraries = await _libraryRepository.GetAll().ToListAsync();

            XLWorkbook workbook = new();

            var usersData = workbook.AddWorksheet("Users");
            var booksData = workbook.AddWorksheet("Books");
            var authorsData = workbook.AddWorksheet("Authors");
            var tagsData = workbook.AddWorksheet("Tags");
            var leasesData = workbook.AddWorksheet("Leases");
            var librariesData = workbook.AddWorksheet("Libraries");

            usersData.Cell(1, 1).Value = "Id";
            usersData.Column(1).Width = 37;
            usersData.Cell(1, 2).Value = "Full Name";
            usersData.Column(2).Width = 20;
            usersData.Cell(1, 3).Value = "Username";
            usersData.Column(3).Width = 17;
            usersData.Cell(1, 4).Value = "Email";
            usersData.Column(4).Width = 29;
            usersData.Cell(1, 5).Value = "Address";
            usersData.Column(5).Width = 40;

            booksData.Cell(1, 1).Value = "Id";
            booksData.Column(1).Width = 10;
            booksData.Cell(1, 2).Value = "Title";
            booksData.Column(2).Width = 30;
            booksData.Cell(1, 3).Value = "ISBN";
            booksData.Column(3).Width = 15;
            booksData.Cell(1, 4).Value = "Author";
            booksData.Column(4).Width = 27;
            booksData.Cell(1, 5).Value = "TotalStock";
            booksData.Column(5).Width = 11;
            booksData.Cell(1, 6).Value = "Tags";
            booksData.Column(6).Width = 13;
            booksData.Cell(1, 7).Value = "Cover Type";
            booksData.Column(7).Width = 15;
            booksData.Cell(1, 8).Value = "Pages";
            booksData.Column(8).Width = 11;
            booksData.Cell(1, 9).Value = "Publisher";
            booksData.Column(9).Width = 20;
            booksData.Cell(1, 10).Value = "Release Year";
            booksData.Column(10).Width = 12;

            authorsData.Cell(1, 1).Value = "Id";
            authorsData.Column(1).Width = 10;
            authorsData.Cell(1, 2).Value = "Name";
            authorsData.Column(2).Width = 25;

            tagsData.Cell(1, 1).Value = "Id";
            tagsData.Column(1).Width = 10;
            tagsData.Cell(1, 2).Value = "Name";
            tagsData.Column(3).Width = 15;
            tagsData.Cell(1, 3).Value = "Administrative";
            tagsData.Column(3).Width = 10;

            leasesData.Cell(1, 1).Value = "Id";
            leasesData.Column(1).Width = 10;
            leasesData.Cell(1, 2).Value = "BookId";
            leasesData.Column(2).Width = 10;
            leasesData.Cell(1, 3).Value = "LibraryId";
            leasesData.Column(3).Width = 10;
            leasesData.Cell(1, 4).Value = "UserId";
            leasesData.Column(4).Width = 37;
            leasesData.Cell(1, 5).Value = "PickUp Date";
            leasesData.Column(5).Width = 26;
            leasesData.Cell(1, 6).Value = "Return Date";
            leasesData.Column(6).Width = 26;
            leasesData.Cell(1, 7).Value = "On Going";
            leasesData.Column(7).Width = 10;
            leasesData.Cell(1, 8).Value = "Completed";
            leasesData.Column(8).Width = 10;

            librariesData.Cell(1, 1).Value = "Id";
            librariesData.Column(1).Width = 10;
            librariesData.Cell(1, 2).Value = "Name";
            librariesData.Column(2).Width = 15;
            librariesData.Cell(1, 3).Value = "Country";
            librariesData.Column(3).Width = 15;
            librariesData.Cell(1, 4).Value = "City";
            librariesData.Column(4).Width = 20;
            librariesData.Cell(1, 5).Value = "Region";
            librariesData.Column(5).Width = 20;
            librariesData.Cell(1, 6).Value = "Address";
            librariesData.Column(6).Width = 35;
            librariesData.Cell(1, 7).Value = "Postal Code";
            librariesData.Column(7).Width = 14;
            librariesData.Cell(1, 8).Value = "Phone Number";
            librariesData.Column(8).Width = 14;
            librariesData.Cell(1, 9).Value = "Opening Hour";
            librariesData.Column(9).Width = 14;
            librariesData.Cell(1, 10).Value = "Closing Hour";
            librariesData.Column(10).Width = 14;
            librariesData.Cell(1, 11).Value = "Opening Duration";
            librariesData.Column(11).Width = 14;
            librariesData.Cell(1, 12).Value = "Is Opened";
            librariesData.Column(12).Width = 14;

            for (int i = 0; i < users.Count; i++)
            {
                usersData.Cell(i + 2, 1).Value = users[i].Id;
                usersData.Cell(i + 2, 2).Value = users[i].FullName;
                usersData.Cell(i + 2, 3).Value = users[i].UserName;
                usersData.Cell(i + 2, 4).Value = users[i].Email;
                usersData.Cell(i + 2, 5).Value = users[i].Address;

            }

            for (int i = 0; i < books.Count; i++)
            {
                booksData.Cell(i + 2, 1).Value = books[i].Id;
                booksData.Cell(i + 2, 2).Value = books[i].Title;
                booksData.Cell(i + 2, 3).Value = books[i].ISBN;
                booksData.Cell(i + 2, 4).Value = books[i].AuthorId;
                booksData.Cell(i + 2, 5).Value = books[i].TotalStock;
                booksData.Cell(i + 2, 8).Value = books[i].tagIds;
                booksData.Cell(i + 2, 6).Value = books[i].CoverType;
                booksData.Cell(i + 2, 7).Value = books[i].Pages;
                booksData.Cell(i + 2, 9).Value = books[i].Publisher;
                booksData.Cell(i + 2, 9).Value = books[i].ReleaseYear;
            }
            for (int i = 0; i < authors.Count; i++)
            {
                authorsData.Cell(i + 2, 1).Value = authors[i].Id;
                authorsData.Cell(i + 2, 2).Value = authors[i].Name;
            }
            for (int i = 0; i < tags.Count; i++)
            {
                tagsData.Cell(i + 2, 1).Value = tags[i].Id;
                tagsData.Cell(i + 2, 2).Value = tags[i].Name;
                tagsData.Cell(i + 2, 3).Value = tags[i].isAdmin.ToString();
            }
            for (int i = 0; i < leases.Count; i++)
            {
                leasesData.Cell(i + 2, 1).Value = leases[i].Id;
                leasesData.Cell(i + 2, 2).Value = books[leases[i].BookId].Title;
                leasesData.Cell(i + 2, 3).Value = libraries[leases[i].LibraryId].Name;
                leasesData.Cell(i + 2, 4).Value = leases[i].UserId;
                leasesData.Cell(i + 2, 5).Value = leases[i].PickUpDate;
                leasesData.Cell(i + 2, 6).Value = leases[i].ReturnDate;
                leasesData.Cell(i + 2, 7).Value = leases[i].OnGoing.ToString();
                leasesData.Cell(i + 2, 8).Value = leases[i].IsCompleted.ToString();
            }
            for (int i = 0; i < libraries.Count; i++)
            {
                librariesData.Cell(i + 2, 1).Value = libraries[i].Id;
                librariesData.Cell(i + 2, 2).Value = libraries[i].Name;
                librariesData.Cell(i + 2, 3).Value = libraries[i].Country;
                librariesData.Cell(i + 2, 4).Value = libraries[i].City;
                librariesData.Cell(i + 2, 5).Value = libraries[i].Region;
                librariesData.Cell(i + 2, 6).Value = libraries[i].Adress;
                librariesData.Cell(i + 2, 7).Value = libraries[i].PostalCode;
                librariesData.Cell(i + 2, 8).Value = libraries[i].PhoneNumber;
                librariesData.Cell(i + 2, 9).Value = libraries[i].OpeningHour.ToString();
                librariesData.Cell(i + 2, 10).Value = libraries[i].ClosingHour.ToString();
                librariesData.Cell(i + 2, 11).Value = libraries[i].OpenDuration.ToString();
                librariesData.Cell(i + 2, 12).Value = libraries[i].IsOpened.ToString();
            }
            MemoryStream xlsxStream = new();

            workbook.SaveAs(xlsxStream);

            return File(xlsxStream.ToArray(), "application/xlsx", "analytics.xlsx");
        }
    }
}
