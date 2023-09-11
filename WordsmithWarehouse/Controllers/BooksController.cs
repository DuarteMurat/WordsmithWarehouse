﻿using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
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

        public BooksController(IBookRepository bookRepository,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            ITagRepository tagRepository,
            IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _tagRepository = tagRepository;
            _authorRepository = authorRepository;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll().OrderBy(b => b.Title));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BookNotFound");

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
                return new NotFoundViewResult("BookNotFound");

            var model = _converterHelper.ConvertToBookViewModel(book);

            model.Author = _authorRepository.GetAuthorById(model.AuthorId);

            return View(model);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
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
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                }

                var book = _converterHelper.ConvertToBook(model, path, true);


                await _bookRepository.CreateAsync(book);

                await _tagRepository.CreateBookTags(book, model.Tags);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books/Edit/5 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            
            var model = _converterHelper.ConvertToBookViewModel(book);
            model.ModelAuthor = _authorRepository.GetAuthorById(book.AuthorId);

            model.Authors = _authorRepository.GetComboAuthors();
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

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");

                    var book = _converterHelper.ConvertToBook(model, path, false);


                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

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
    }
}
