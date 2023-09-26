using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class BookReservationsController : Controller
    {
        private readonly IBookReservationRepository _bookReservationRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBookRepository _bookRepository;

        public BookReservationsController(IBookReservationRepository bookReservationRepository,
            IConverterHelper converterHelper,
            IBookRepository bookRepository)
        {
            _bookReservationRepository = bookReservationRepository;
            _converterHelper = converterHelper;
            _bookRepository = bookRepository;
        }

        // GET: BookReservations
        public IActionResult Index()
        {
            var list = _bookReservationRepository.GetAll().OrderBy(br => br.ReturnDate);
            return View(list);
        }

        // GET: BookReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReservation = await _bookReservationRepository.GetByIdAsync(id.Value);
            if (bookReservation == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ConvertToBookReservationViewModel(bookReservation);
            
            model.Books = await _bookRepository.GetBooksFromString(bookReservation.BookIds);


            return View(model);
        }

        // GET: BookReservations/Create
        public IActionResult Create()
        {
            var model = new BookReservationViewModel
            {
                Books = _bookRepository.GetBooksList(),
            };
            return View(model);
        }

        // POST: BookReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.BookIds = _bookRepository.GetBookIds(model.Books);

                var bookReservation = _converterHelper.ConvertToBookReservation(model, true);

                await _bookReservationRepository.CreateAsync(bookReservation);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BookReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReservation = await _bookReservationRepository.GetByIdAsync(id.Value);
            if (bookReservation == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ConvertToBookReservationViewModel(bookReservation);

            return View(model);
        }

        // POST: BookReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bookReservation = await _bookReservationRepository.GetByIdAsync(model.Id);
                    bookReservation = _converterHelper.ConvertToBookReservation(model, false);
                    await _bookReservationRepository.UpdateAsync(bookReservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookReservationRepository.ExistAsync(model.Id))
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

        // GET: BookReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReservation = await _bookReservationRepository.GetByIdAsync(id.Value);
            if (bookReservation == null)
            {
                return NotFound();
            }

            return View(bookReservation);
        }

        // POST: BookReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookReservation = await _bookReservationRepository.GetByIdAsync(id);
            await _bookReservationRepository.DeleteAsync(bookReservation);
            return RedirectToAction(nameof(Index));
        }
    }
}
