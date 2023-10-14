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
using Microsoft.AspNetCore.Authorization;
using System.Data;

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
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            var list = await _leaseRepository.GetAll().OrderBy(l => l.Id).ToListAsync();

            List<LeaseViewModel> leases = new List<LeaseViewModel>();
            foreach (var item in list)
            {
                var itemToAdd = _converterHelper.ConvertToLeaseViewModel(item);
                itemToAdd.Book = await _bookRepository.GetByIdAsync(item.BookId);
                var user = await _userHelper.GetUserByIdAsync(item.UserId);
                itemToAdd.User = user;

                leases.Add(itemToAdd);
            }

            return View(leases);
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
            model.Libraries = await _libraryRepository.GetAll().ToListAsync();
            model.PickUpDate = null;
            model.ReturnDate = null;
            model.LeaseTime = null;


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
                model.Library = await _libraryRepository.GetByIdAsync(model.LibraryId);
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                model.UserId = user.Id;
                var lease = _converterHelper.ConvertToLease(model, true);

                await _leaseRepository.CreateAsync(lease);

                return RedirectToAction(nameof(UserLeases));
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

            var lease = await _leaseRepository.GetByIdAsync(id.Value);
            if (lease == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ConvertToLeaseViewModel(lease);
            model.User = await _userHelper.GetUserByIdAsync(model.UserId);
            model.Book = await _bookRepository.GetByIdAsync(lease.BookId);
            model.Library = await _libraryRepository.GetByIdAsync(lease.LibraryId);


            return View(model);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(LeaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lease = await _leaseRepository.GetByIdAsync(model.Id);
                    lease = _converterHelper.ConvertToLease(model, false);

                    await _leaseRepository.UpdateAsync(lease);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _leaseRepository.ExistAsync(model.Id))
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

        public async Task<IActionResult> UserLeases()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            var model = new UserLeasesViewModel
            {
                User = user,
                Leases = await _leaseRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync(),
                ExampleLease = new Lease(),
                Books = await _bookRepository.GetAll().ToListAsync(),
                Libraries = await _libraryRepository.GetAll().ToListAsync(),
            };

            return View(model);

        }
        
        public async Task<IActionResult> GetLeaseAmount()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            var leases = await _leaseRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync();

            string LeasesAmount = leases.Count().ToString();
            return Content(LeasesAmount);
        }
    }
}
