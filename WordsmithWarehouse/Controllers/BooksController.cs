﻿using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Migrations;
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
        private readonly IShelfRepository _shelfRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IBookQuantityRepository _bookQuantityRepository;
        Random r;

        public BooksController(IBookRepository bookRepository,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            ITagRepository tagRepository,
            IAuthorRepository authorRepository,
            IUserHelper userHelper,
            ICommentRepository commentRepository,
            IShelfRepository shelfRepository,
            ILibraryRepository libraryRepository,
            IBookQuantityRepository bookQuantityRepository)
        {
            _bookRepository = bookRepository;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _tagRepository = tagRepository;
            _authorRepository = authorRepository;
            _userHelper = userHelper;
            _commentRepository = commentRepository;
            _shelfRepository = shelfRepository;
            _libraryRepository = libraryRepository;
            _bookQuantityRepository = bookQuantityRepository;
        }

        // GET: Books
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
            List<Book> books = await _bookRepository.GetAll().ToListAsync();
            Book book;

            if (id == null)
            {
                r = new Random();
                id = r.Next(0, books.Count());
                book = books[id.Value];
            }
            else
                book = await _bookRepository.GetByIdAsync(id.Value);

            if (book == null)
                return new NotFoundViewResult("BookNotFound");

            var model = await CreateDetailsModel(book);


            return View(model);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create()
        {
            var model = new BookViewModel
            {
                Authors = _authorRepository.GetComboAuthors(),
                Tags = await _tagRepository.GetTagsList(),
            };

            return View(model);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {

            if (ModelState.IsValid)
            {
                var path = string.Empty;
                var bookFile = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                }
                else
                {
                    path = "/images/Books/notfound.png";
                };

                if (model.BookFile != null && model.BookFile.Length > 0)
                {
                    bookFile = await _bookRepository.UploadBookFileAsync(model.BookFile, "Books");
                }

                model.tagIds = _tagRepository.GetTagIds(model.Tags);

                var book = _converterHelper.ConvertToBook(model, path, true, bookFile);
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
            model.Tags = await _tagRepository.MatchTagList(book.tagIds);

            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageURL;
                    var bookFile = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                    }

                    if (model.BookFile != null && model.BookFile.Length > 0)
                    {
                        bookFile = await _bookRepository.UploadBookFileAsync(model.BookFile, "Books");
                    }

                    var book = await _bookRepository.GetByIdAsync(model.Id);
                    book = _converterHelper.ConvertToBook(model, path, false, bookFile);

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
                itemToAdd.Tags = await _tagRepository.MatchTagList(itemToAdd.tagIds);
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


        public async Task<IActionResult> BookStock(int? id)
        {
            if (id == null)
                return BookNotFound();

            var model = new BookStockViewModel
            {
                Id = id.Value,
                Book = await _bookRepository.GetByIdAsync(id.Value),
                Libraries = await _libraryRepository.GetAll().ToListAsync(),
                quantities = await _bookQuantityRepository.GetAll().Where(q => q.BookId == id).ToListAsync(),
            };

            return View(model);
        }

        public async Task<object> GetBookStock(string[] values, int bookId)
        {
            List<Library> libraries = await _libraryRepository.GetAll().ToListAsync();
            var book = await _bookRepository.GetByIdAsync(bookId);
            List<BookQuantity> bookQuantity = await _bookQuantityRepository.GetAll().ToListAsync();
            for (int i = 0; i < values.Length; i++)
            {
                var query = bookQuantity.Where(x => x.BookId == bookId && x.LibraryId == libraries[i].Id).FirstOrDefault();
                var quant = new BookQuantity
                {
                    BookId = bookId,
                    LibraryId = libraries[i].Id,
                    TotalStock = int.Parse(values[i]),
                    StockAvailable = int.Parse(values[i]),
                };
                if (query == null)
                    await _bookQuantityRepository.CreateAsync(quant);
                else
                {
                    query.TotalStock = int.Parse(values[i]);
                    query.StockAvailable = query.TotalStock - query.StockBeingUsed;
                    await _bookQuantityRepository.UpdateAsync(query);
                }

            }
            int sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += int.Parse(values[i]);
            }
            book.TotalStock = sum;
            await _bookRepository.UpdateAsync(book);

            return Json("success");
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            await _commentRepository.DeleteAsync(comment);

            return RedirectToAction("Details");

        }

        public async Task<object> UpdateComment(string commentId, string userId, string commentText, string commentRating, string bookId)
        {
            var book = await _bookRepository.GetByIdAsync(int.Parse(bookId));

            var comment = await _commentRepository.GetAll().Where(c => c.BookId == int.Parse(bookId) && c.Id == int.Parse(commentId)).FirstOrDefaultAsync();

            var user = _userHelper.GetUserByIdAsync(userId);

            comment.Text = commentText;
            comment.Rating = float.Parse(commentRating);

            await _commentRepository.UpdateAsync(comment);


            return Json("success");
        }

        private async Task<DetailsBookViewModel> CreateDetailsModel(Book book)
        {
            var model = _converterHelper.ConvertToDetailsBookViewModel(book);
            model.Tags = await _tagRepository.GetTagsFromString(book.tagIds);
            model.Author = await _authorRepository.GetAuthorById(model.AuthorId);
            model.Comments = new List<Comment>(await _commentRepository.GetCommentsByBookId(book.Id));
            model.AverageRatings = _commentRepository.GetAverageRatings(model.Comments);
            model.TotalReviews = model.Comments.Count().ToString();
            model.BookFileUrl = book.BookFileURL;
            if (this.User.Identity.IsAuthenticated)
            {
                model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                model.hasComment = _commentRepository.CheckForComment(model.Comments, model.User.Id);
                model.Shelves = await _shelfRepository.GetShelvesByUserAsync(model.User.ShelfIds);
            }

            return model;
        }
    }
}