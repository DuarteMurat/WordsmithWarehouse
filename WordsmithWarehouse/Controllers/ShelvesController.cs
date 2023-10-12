using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        IConverterHelper _converterHelper;

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

        // GET: Shelves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelves
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }

            return View(shelf);
        }

        // GET: Shelves/Create
        public IActionResult Create()
        {
            var model = new ShelfViewModel();

            return View(model);
        }

        // POST: Shelves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShelfViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shelf = _converterHelper.ConvertToShelf(model, true);
                await _shelfRepository.CreateAsync(shelf);

                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);

                user.ShelfIds = updateShelfIds(user.ShelfIds, shelf.Id);

                await _userHelper.UpdateUserAsync(user);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Shelves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelves.FindAsync(id);
            if (shelf == null)
            {
                return NotFound();
            }
            return View(shelf);
        }

        // POST: Shelves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BookIds,Description")] Shelf shelf)
        {
            if (id != shelf.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelfExists(shelf.Id))
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
            return View(shelf);
        }

        // GET: Shelves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelves
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }

            return View(shelf);
        }

        // POST: Shelves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelf = await _context.Shelves.FindAsync(id);
            _context.Shelves.Remove(shelf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelfExists(int id)
        {
            return _context.Shelves.Any(e => e.Id == id);
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
                    source = source.Substring(source.Length - 1);
                }
            }

            return source;
        }

        [HttpPost]
        public object AddToShelf()
        {
            object obj = null;
            try
            {
                
            }
            catch (System.Exception)
            {

                throw;
            }

            return obj;
        }
    }
}
