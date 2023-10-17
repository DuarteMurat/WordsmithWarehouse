using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class LibrariesController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IConverterHelper _converterHelper;

        public LibrariesController(ILibraryRepository libraryRepository, IConverterHelper converterHelper)
        {
            _libraryRepository = libraryRepository;
            _converterHelper = converterHelper;
        }

        // GET: Libraries
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Index()
        {
            return View(_libraryRepository.GetAll().OrderBy(b => b.Name));
        }

        // GET: Libraries/Details/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _libraryRepository.GetByIdAsync(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ConvertToLibraryViewModel(library);

            return View(model);
        }

        // GET: Libraries/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var library = _converterHelper.ConvertToLibrary(model, true);

                await _libraryRepository.CreateAsync(library);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Libraries/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _libraryRepository.GetByIdAsync(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ConvertToLibraryViewModel(library);

            return View(model);
        }

        // POST: Libraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibraryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var library = await _libraryRepository.GetByIdAsync(model.Id);
                    library = _converterHelper.ConvertToLibrary(model, false);
                    await _libraryRepository.UpdateAsync(library);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _libraryRepository.ExistAsync(model.Id))
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

        // GET: Libraries/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _libraryRepository.GetByIdAsync(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var library = await _libraryRepository.GetByIdAsync(id);
            await _libraryRepository.DeleteAsync(library);
            return RedirectToAction(nameof(Index));
        }
    }
}
