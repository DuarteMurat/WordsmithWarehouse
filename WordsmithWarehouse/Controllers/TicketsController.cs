using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class TicketsController : Controller
    {
        private readonly DataContext _context;
        private readonly ITicketRepository _ticketRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public TicketsController(DataContext context,
            ITicketRepository ticketRepository,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        // GET: Tickets
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketRepository.GetAll().OrderBy(b => b.Title).ToListAsync();

            return View(tickets);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketRepository.GetByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create

        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ticket = _converterHelper.ConvertToTicket(model, true);
                if (this.User.Identity.IsAuthenticated)
                {
                    ticket.Username = this.User.Identity.Name;
                    var user = await _userHelper.GetUserByUsernameAsync(ticket.Username);
                    ticket.UserEmail = user.Email;
                }
                await _ticketRepository.CreateAsync(ticket);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Tickets/Edit/5

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BookNotFound");

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TicketDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ticket = await _ticketRepository.GetByIdAsync(model.Id);
                    ticket = _converterHelper.ConvertToTicket(model, false);

                    await _ticketRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ticketRepository.ExistAsync(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
