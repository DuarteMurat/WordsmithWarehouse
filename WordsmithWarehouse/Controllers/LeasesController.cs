﻿using ClassLibrary.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Migrations;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

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
        private readonly IMailHelper _mailHelper;
        private readonly IBookQuantityRepository _bookQuantityRepository;
        private readonly IBookReservationRepository _bookReservationRepository;

        public LeasesController(DataContext context,
            IUserHelper userHelper,
            ILeaseRepository leaseRepository,
            IBookRepository bookRepository,
            ILibraryRepository libraryRepository,
            IAuthorRepository authorRepository,
            IConverterHelper converterHelper,
            IMailHelper mailHelper,
            IBookQuantityRepository bookQuantityRepository,
            IBookReservationRepository bookReservationRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _leaseRepository = leaseRepository;
            _authorRepository = authorRepository;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
            _bookQuantityRepository = bookQuantityRepository;
            _bookReservationRepository = bookReservationRepository;
        }

        // GET: Leases
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            var list = await _leaseRepository.GetAll().OrderBy(l => l.IsCompleted).ThenBy(l => !l.OnGoing).ToListAsync();

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

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = _leaseRepository.GetByIdAsync(id.Value);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Leases/Create

        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> Create(DetailsBookViewModel dets)
        {
            var model = new LeaseViewModel();
            model.Book = await _bookRepository.GetByIdAsync(dets.Id);
            model.User = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            model.LibraryList = await _libraryRepository.GetComboLibraries();
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
            model.Book = await _bookRepository.GetByIdAsync(model.Book.Id);
            model.Library = await _libraryRepository.GetByIdAsync(model.LibraryId);

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                model.UserId = user.Id;

                var quant = await _bookQuantityRepository.GetQuantityByIdsAsync(model.Book.Id, model.LibraryId);

                if (quant.StockAvailable > 0)
                {
                    var lease = _converterHelper.ConvertToLease(model, true);

                    await _leaseRepository.CreateAsync(lease);

                    model.Book.Author = await _authorRepository.GetAuthorById(model.Book.AuthorId);
                    quant.stockOnHold++;
                    quant.StockAvailable--;
                    await _bookQuantityRepository.UpdateAsync(quant);

                    await _mailHelper.SendEmail(user.Email, "Lease",
                            $"Dear {user.UserName}, <br/>" +
                            $"We are delighted to confirm your recent book lease from {model.Library.Name}! Thank you for choosing us to fulfill your reading needs. Here are the details of your book lease:<br/>" +
                            $"Book Title: {model.Book.Title}, <br/>" +
                            $"Author: {model.Book.Author.Name}, <br/>" +
                            $"We hope you enjoy reading this book and find it both informative and entertaining. Our library is dedicated to providing a wide range of books to our members, and we trust this selection meets your expectations.<br/>" +
                            $"If you did not initiate this lease request or suspect any unauthorized access to your account, please contact our support team immediately at <a>wordsmithwarehouse@outlook.pt</a>.<br/>" +
                            $"Thank you for choosing <b>WordsmithWarehouse</b>. We appreciate your trust in us.<br/>" +
                            $"Best regards,<br/><br/>" +
                            $"WordsmithWarehouse.");
                }
                else
                {
                    model.LibraryList = await _libraryRepository.GetComboLibraries();
                    model.Libraries = await _libraryRepository.GetAll().ToListAsync();
                    model.Book.Author = await _authorRepository.GetAuthorById(model.Book.AuthorId);
                    model.ErrorMessage = $"Unfortunately, the book {model.Book.Title} is currently out of stock in this Library ({model.Library.Name})! Would you like to place a reservation?";
                    model.LibraryId = model.Library.Id;
                    model.ViewReturn = true;

                    return View(model);
                }

                return RedirectToAction(nameof(UserLeases));
            }
            return View(model);
        }

        public async Task<IActionResult> CreateReservation(LeaseViewModel model)
        {

            model.Book = await _bookRepository.GetByIdAsync(model.Book.Id);
            model.Library = await _libraryRepository.GetByIdAsync(model.LibraryId);

            var reservationModel = new BookReservationViewModel
            {
                BookId = model.Book.Id,
                LibraryId = model.Library.Id,
            };

            return View(reservationModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateReservation(BookReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                model.UserId = user.Id;

                int QueueNumber = await _bookReservationRepository.GetHighestQueue(model.BookId, model.LibraryId);

                BookReservation br = new BookReservation
                {
                    BookId = model.BookId,
                    LibraryId = model.LibraryId,
                    UserId = user.Id,
                    QueueNumber = QueueNumber + 1,
                };

                await _bookReservationRepository.CreateAsync(br);

                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }

        // GET: Leases/Edit/5

        [Authorize(Roles = "Admin, Employee")]
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
        public async Task<IActionResult> Edit(LeaseViewModel model)
        {
            model.Library = await _libraryRepository.GetByIdAsync(model.LibraryId);

            if (ModelState.IsValid)
            {
                try
                {
                    TimeSpan timeSpan = TimeSpan.FromDays(14);

                    var lease = await _leaseRepository.GetByIdAsync(model.Id);
                    var user = await _userHelper.GetUserByIdAsync(model.UserId);

                    Lease leaseEmail = _converterHelper.ConvertToLease(model, false);

                    if (leaseEmail != lease)
                    {
                        if (leaseEmail.OnGoing && !leaseEmail.IsCompleted)
                        {
                            leaseEmail.ReturnDate = leaseEmail.PickUpDate + timeSpan;

                            await _mailHelper.SendEmail(user.Email, "Your lease has been updated",
                       $"Dear {user.UserName}, <br/>" +
                       $"We hope this email finds you well. We want to inform you of recent updates to your library lease.<br/>" +
                       $"At <b>WordsmithWarehouse</b>, we are committed to provide you with the best resources and services to enhance your reading and learning experience.<br/>" +
                       $"Here are the key details of the lease update:<br/><br/>" +
                       $"Lease ID: {model.Id},<br/>" +
                       $"Book: {model.Book.Title},<br/>" +
                       $"Pickup Date:{model.PickUpDate}<br/>" +
                       $"Return Date: {leaseEmail.ReturnDate} <br/>" +
                       $"Thank you for choosing <b>WordsmithWarehouse</b>. We appreciate your trust in us.<br/>" +
                       $"Best regards,<br/><br/>" +
                       $"WordsmithWarehouse.");

                            var quant = await _bookQuantityRepository.GetQuantityByIdsAsync(lease.BookId, lease.LibraryId);
                            quant.stockOnHold++;
                            quant.StockAvailable--;
                            await _bookQuantityRepository.UpdateAsync(quant);
                        }

                        if (leaseEmail.IsCompleted && !leaseEmail.OnGoing)
                        {
                            await _mailHelper.SendEmail(user.Email, "Your lease has been updated",
                       $"Dear {user.UserName}, <br/>" +
                       $"We hope this email finds you well. We want to inform you of recent updates to your library lease.<br/>" +
                       $"At <b>WordsmithWarehouse</b>, we are committed to provide you with the best resources and services to enhance your reading and learning experience.<br/>" +
                       $"Here are the key details of the lease update:<br/><br/>" +
                       $"Lease ID: {model.Id},<br/>" +
                       $"Book: {model.Book.Title},<br/>" +
                       $"Lease Status: Complete.<br/>" +
                       $"Thank you for choosing <b>WordsmithWarehouse</b>. We appreciate your trust in us.<br/>" +
                       $"Best regards,<br/><br/>" +
                       $"WordsmithWarehouse.");

                            var quant = await _bookQuantityRepository.GetQuantityByIdsAsync(lease.BookId, lease.LibraryId);
                            quant.StockBeingUsed--;
                            quant.StockAvailable++;
                            await _bookQuantityRepository.UpdateAsync(quant);
                        }

                    }

                    lease = _converterHelper.ConvertToLease(model, false);
                    lease.ReturnDate = leaseEmail.ReturnDate;


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
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var lease = _leaseRepository.GetByIdAsync(id.Value);

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
            var lease = await _leaseRepository.GetByIdAsync(id);
            await _leaseRepository.DeleteAsync(lease);
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

        public async Task<IActionResult> GetFines()
        {
            List<Fine> fines = await _leaseRepository.GetFinesAsync();

            return View(fines);
        }

        public async Task<IActionResult> GetLeaseAmount()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            var leases = await _leaseRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync();

            string LeasesAmount = leases.Count().ToString();
            return Content(LeasesAmount);
        }

        public async Task<IActionResult> GetUserFines()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);

            var fines = await _leaseRepository.GetFinesAsync();

            var userFines = fines.Where(f => f.UserId == user.Id);

            return View(userFines);
        }
    }
}
