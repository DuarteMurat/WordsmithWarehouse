using ClassLibrary.Data;
using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class ShelvesController : Controller
    {
        private readonly DataContext _context;
        private readonly IShelfRepository _shelfRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public ShelvesController(DataContext context,
            IUserHelper userHelper, IShelfRepository shelfRepository,
            IBookRepository bookRepository,
            IConverterHelper converterHelper)
        {
            _context = context;
            _shelfRepository = shelfRepository;
            _bookRepository = bookRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Shelves
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);

            var model = new MainShelfViewModel
            {
                shelves = await _shelfRepository.GetShelvesByUserAsync(user.ShelfIds),
            };

            foreach (var shelf in model.shelves)
            {
                if (shelf.BookIds != null)
                {
                    shelf.Books = await _bookRepository.GetBooksFromString(shelf.BookIds);
                }
            }
            return View(model);

        }

        // POST: Shelves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<object> Create(string name, string description)
        {
            var model = new ShelfViewModel
            {
                Name = name,
                Description = description,
            };

            if (ModelState.IsValid)
            {
                var shelf = _converterHelper.ConvertToShelf(model, true);
                await _shelfRepository.CreateAsync(shelf);

                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);

                user.ShelfIds = updateShelfIds(user.ShelfIds, shelf.Id);

                await _userHelper.UpdateUserAsync(user);

                return Json("success");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Shelves/Delete/5
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();



            var shelf = await _shelfRepository.GetByIdAsync(id.Value);
            if (shelf == null)
                return new NotFoundViewResult("BookNotFound");

            return View(shelf);
        }

        // POST: Shelves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelf = await _shelfRepository.GetByIdAsync(id);
            await _shelfRepository.DeleteAsync(shelf);

            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            List<string> ids = user.ShelfIds.Split(',').ToList();
            if (ids.Contains(id.ToString()))
                ids.Remove(id.ToString());

            user.ShelfIds = string.Empty;

            if (ids.Count > 0)
            {
                foreach (var val in ids)
                {
                    user.ShelfIds += val + ',';
                }
            }

            if (user.ShelfIds.Length > 0)
            {
                user.ShelfIds = user.ShelfIds.Substring(0, user.ShelfIds.Length - 1);
            }
            await _userHelper.UpdateUserAsync(user);

            return RedirectToAction(nameof(Index));
        }

        private string updateShelfIds(string source, int idToAdd)
        {
            if (string.IsNullOrEmpty(source))
            {
                source += idToAdd.ToString();
            }
            else
            {
                if (!source.EndsWith(','))
                {
                    source += ',' + idToAdd.ToString();
                }
                else
                {
                    source = source.Substring(0, source.Length - 1);
                }
            }

            return source;
        }

        [HttpPost]
        public async Task<object> AddToShelf(AddBookShelf info)
        {
            var shelf = await _shelfRepository.GetByIdAsync(info.shelfId);
            if (string.IsNullOrEmpty(shelf.BookIds))
            {
                shelf.BookIds = "";
            }

            if (shelf.BookIds.Length >= 1)
            {
                shelf.BookIds += ',' + info.bookId.ToString();
            }
            else
            {
                shelf.BookIds += info.bookId.ToString();
            }

            await _shelfRepository.UpdateAsync(shelf);

            return Json("success");
        }

        [HttpPost]
        public async Task<object> RemoveFromShelf(AddBookShelf info)
        {
            var shelf = await _shelfRepository.GetByIdAsync(info.shelfId);
            List<string> stringSplit = shelf.BookIds.Split(',').ToList();

            if (stringSplit.Contains(info.bookId.ToString()))
            {
                stringSplit.Remove(info.bookId.ToString());
            }

            shelf.BookIds = string.Empty;

            foreach (var item in stringSplit)
            {
                shelf.BookIds += item + ',';
            }

            shelf.BookIds = shelf.BookIds.Substring(0, shelf.BookIds.Length - 1);

            await _shelfRepository.UpdateAsync(shelf);

            return Json("success");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShelf(ShelfDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var shelf = await _shelfRepository.GetByIdAsync(model.Shelf.Id);
                    if (shelf.Name != model.Shelf.Name)
                        shelf.Name = model.Shelf.Name;

                    if (shelf.Description != model.Shelf.Description)
                        shelf.Description = model.Shelf.Description;

                    await _shelfRepository.UpdateAsync(shelf);
                }
                catch (DbUpdateConcurrencyException)
                {
                    await _shelfRepository.ExistAsync(model.Shelf.Id);
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Shelf(int id)
        {
            var shelf = await _shelfRepository.GetByIdAsync(id);

            var model = new ShelfDetailsViewModel
            {
                Shelf = shelf,
            };

            model.Shelf.Books = await _bookRepository.GetBooksFromString(shelf.BookIds);
            model.Shelf.Books.OrderBy(b => b.Title);

            return View(model);
        }
    }
}
