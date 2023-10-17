using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class TagsController : Controller
    {
        private readonly DataContext _context;
        private readonly ITagRepository _tagRepository;

        public TagsController(DataContext context,
            ITagRepository tagRepository)
        {
            _context = context;
            _tagRepository = tagRepository;
        }

        // GET: Tags
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Index()
        {
            return View(_tagRepository.GetAll().OrderBy(t => t.Name));
        }

        // GET: Tags/Details/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var tag = await _tagRepository.GetByIdAsync(id.Value);

            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // GET: Tags/Create

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagRepository.CreateAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var tag = await _tagRepository.GetByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _tagRepository.UpdateAsync(tag);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tagRepository.ExistAsync(tag.Id))
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
            return View(tag);
        }

        // GET: Tags/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var tag = await _tagRepository.GetByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            await _tagRepository.DeleteAsync(tag);
            return RedirectToAction(nameof(Index));
        }
    }
}
