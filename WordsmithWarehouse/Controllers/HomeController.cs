using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public HomeController(ILogger<HomeController> logger,
            ITagRepository tagRepository, 
            IBookRepository bookRepository,
            IUserHelper userHelper)
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _bookRepository = bookRepository;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            // get all books
            var model = new FrontPageViewModel
            {
                Books = _bookRepository.GetBooksList(),
            };

            // separate best sellers
            model.BestSellerBooks = await _tagRepository.GetBooksWithTags(model.Books, "Best Seller");

            model.Books = await _bookRepository.GetBooksFromString(model.BestSellerBooks);
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
    }
}
