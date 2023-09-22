using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;
using System.Runtime.CompilerServices;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Controllers
{
    public class BookReservationsController : Controller
    {
        private readonly IBookReservationRepository _bookReservationRepository;
        private readonly IConverterHelper _converterHelper;

        public BookReservationsController(IBookReservationRepository bookReservationRepository, IConverterHelper converterHelper)
        {
            _bookReservationRepository = bookReservationRepository;
            _converterHelper = converterHelper;
        }

        // GET: BookReservations
        public IActionResult Index()
        {
            return View(_bookReservationRepository.GetAll().OrderBy(br => br.ReturnDate));
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

            return View(model);
        }

        // GET: BookReservations/Create
        public IActionResult Create()
        {
            return View();
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
