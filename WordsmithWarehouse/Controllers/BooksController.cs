using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ITagRepository _tagRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUserHelper _userHelper;
        private readonly ICommentRepository _commentRepository;
        Random r;

        public BooksController(IBookRepository bookRepository,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            ITagRepository tagRepository,
            IAuthorRepository authorRepository,
            IUserHelper userHelper,
            ICommentRepository commentRepository)
        {
            _bookRepository = bookRepository;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _tagRepository = tagRepository;
            _authorRepository = authorRepository;
            _userHelper = userHelper;
            _commentRepository = commentRepository;
        }

        // GET: Books
        [Authorize(Roles ="Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            var list = await _bookRepository.GetAll().OrderBy(b => b.Title).ToListAsync();

            List<BookViewModel> books = new List<BookViewModel>();
            foreach (var item in list)
            {
                var itemToAdd = _converterHelper.ConvertToBookViewModel(item);
                itemToAdd.Author = await _authorRepository.GetAuthorById(itemToAdd.AuthorId);

                books.Add(itemToAdd);
            }
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BookNotFound");

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
                return new NotFoundViewResult("BookNotFound");

            var model = _converterHelper.ConvertToDetailsBookViewModel(book);

            model.Tags = await _tagRepository.GetTagsFromString(book.tagIds);
            model.Author = await _authorRepository.GetAuthorById(model.AuthorId);
            model.Comments = new List<Comment>(await _commentRepository.GetCommentsByBookId(book.Id));
            model.AverageRatings = _commentRepository.GetAverageRatings(model.Comments);
            model.TotalReviews = model.Comments.Count().ToString();
            if (this.User.Identity.IsAuthenticated)
            {
                model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                model.hasComment = _commentRepository.CheckForComment(model.Comments, model.User.Id);
            }

            
            return View(model);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create()
        {
            var model = new BookViewModel
            {
                Authors = _authorRepository.GetComboAuthors(),
                Tags = _tagRepository.GetTagsList(),
            };

            return View(model);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create(BookViewModel model)
        {

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                }

                else
                {
                    path = "/images/Books/notfound.png";
                };

                model.tagIds = _tagRepository.GetTagIds(model.Tags);

                var book = _converterHelper.ConvertToBook(model, path, true);
                await _bookRepository.CreateAsync(book);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books/Edit/5 
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BookNotFound");

            var book = await _bookRepository.GetByIdAsync(id.Value);

            if (book == null)
                return new NotFoundViewResult("BookNotFound");


            var model = _converterHelper.ConvertToBookViewModel(book);
            model.ModelAuthor = await _authorRepository.GetAuthorById(book.AuthorId);

            model.Authors = _authorRepository.GetComboAuthors();
            model.Tags = _tagRepository.MatchTagList(book.tagIds);

            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageURL;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                    }

                    var book = await _bookRepository.GetByIdAsync(model.Id);
                    book = _converterHelper.ConvertToBook(model, path, false);

                    book.tagIds = _tagRepository.GetTagIds(model.Tags);

                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BookNotFound");

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
                return new NotFoundViewResult("BookNotFound");

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            await _bookRepository.DeleteAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BookNotFound()
        {
            return View();
        }

        public async Task<IActionResult> SearchBooks()
        {
            var list = await _bookRepository.GetAll().OrderBy(b => b.Title).ToListAsync();

            List<BookViewModel> books = new List<BookViewModel>();
            foreach (var item in list)
            {
                var itemToAdd = _converterHelper.ConvertToBookViewModel(item);
                itemToAdd.Tags = _tagRepository.MatchTagList(itemToAdd.tagIds);
                books.Add(itemToAdd);
            }

            return View(books);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(DetailsBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = await _bookRepository.GetByIdAsync(model.Id);
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);

                var comment = new Comment
                {
                    BookId = book.Id,
                    DateCreated = DateTime.Now,
                    Name = user.UserName,
                    Text = model.CurrentUserComment,
                    Rating = model.CurrentUserCommentRating,
                    UserImage = user.ImageURL,
                    UserIdString = user.Id,
                };

                await _commentRepository.CreateAsync(comment);

                return RedirectToAction(nameof(Details));
            }

            return View();

        }

        [HttpGet, ActionName("RandomDetails")]
        public async Task<IActionResult> RandomBook()
        {
            List<Book> books = await _bookRepository.GetAll().ToListAsync();

            r = new Random();
            
            int id = r.Next(0, books.Count());

            var model = _converterHelper.ConvertToDetailsBookViewModel(books[id]);

            model.Tags = await _tagRepository.GetTagsFromString(books[id].tagIds);
            model.Author = await _authorRepository.GetAuthorById(model.AuthorId);
            model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            model.Comments = new List<Comment>(await _commentRepository.GetCommentsByBookId(books[id].Id));

            return View("Details", model);
        }
    }
}