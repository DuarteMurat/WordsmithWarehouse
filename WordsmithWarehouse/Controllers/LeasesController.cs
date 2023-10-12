using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Repositories.Classes;
using WordsmithWarehouse.Repositories.Interfaces;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Controllers
{
    public class LeasesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IConverterHelper _converterHelper;

        public LeasesController(DataContext context, 
            IUserHelper userHelper,
            ILeaseRepository leaseRepository,
            IBookRepository bookRepository,
            ILibraryRepository libraryRepository,
            IAuthorRepository authorRepository,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _leaseRepository = leaseRepository;
            _authorRepository = authorRepository;
            _converterHelper = converterHelper;
        }

        // GET: Leases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lease.ToListAsync());
        }

        // GET: Leases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Lease
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Leases/Create
        public async Task<IActionResult> Create(DetailsBookViewModel dets)
        {
            var model = new LeaseViewModel();
            model.Book = await _bookRepository.GetByIdAsync(dets.Id);
            model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            model.LibraryList = _libraryRepository.GetComboLibraries();
            model.Book.Author = await _authorRepository.GetByIdAsync(model.Book.AuthorId);
            //model.Libraries =  await _libraryRepository.GetByIdAsync(model);

            return View(model);
        }

        // POST: Leases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Book = await _bookRepository.GetByIdAsync(model.Book.Id);
                model.Library = await _libraryRepository.GetByIdAsync(model.Library.Id);
                model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                var lease = _converterHelper.ConvertToLease(model, true);
                
                await _leaseRepository.CreateAsync(lease);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Leases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Lease.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }
            return View(lease);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,LibraryId,UserId,PickUpDate,ReturnDate,LeaseTime,OnGoing,IsCompleted")] Lease lease)
        {
            if (id != lease.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseExists(lease.Id))
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
            return View(lease);
        }

        // GET: Leases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Lease
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lease = await _context.Lease.FindAsync(id);
            _context.Lease.Remove(lease);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseExists(int id)
        {
            return _context.Lease.Any(e => e.Id == id);
        }
    }
}
