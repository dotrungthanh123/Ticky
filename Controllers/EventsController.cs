using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticky.Data;
using Ticky.Models;

namespace Ticky.Controllers
{
    public class EventsController : Controller
    {
        private readonly TickyContext _context;

        public EventsController(TickyContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var TickyContext = _context.Events.Include(a => a.Category).Include(a => a.Retailer);
            ViewBag.Categories = _context.Categories.ToList();
            return View(await TickyContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string SearchString)
        {
            //return _context.Events != null ? 
            //            View(await _context.Events.ToListAsync()) :
            //            Problem("Entity set 'WebApplication5Context.Events'  is null.");
            ViewData["CurrentFilter"] = SearchString;
            var events = from p in _context.Events.Include(a => a.Category).Include(a => a.Retailer)
                         select p;
            ViewBag.Categories = _context.Categories.ToList();
            if (!String.IsNullOrEmpty(SearchString))
            {
                events = events.Where(p => p.Name.Contains(SearchString));
                return View(events);
            }
            else
            {
                var webApplication5Context = _context.Events.Include(a => a.Category).Include(a => a.Retailer);
                return View(await webApplication5Context.ToListAsync());
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var aevent = await _context.Events
                .Include(a => a.Category)
                .Include(a => a.Retailer)
                .SingleAsync(m => m.EventId == id);
            if (aevent == null)
            {
                return NotFound();
            }

            return View(aevent);
        }

        // GET: Events/Create
        [Authorize(Policy = "isRetailer")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            
            return View();
        }
        [Authorize(Policy = "IsRetailer")]
        public IActionResult Manage()
        {
            int accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			int retailerId = _context.Retailers.First(r => r.AccountId == accountId).RetailerId;
            var events = _context.Events
                .Include(e => e.Category)
                .Where(e => e.RetailerId == retailerId);
            return View(events);
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Seat,Price,StartDate,Address, Url")] Event aevent)
        {
            int accountid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            aevent.RetailerId = _context.Retailers.Single(r => r.AccountId == accountid).RetailerId;
            if (ModelState.IsValid)
            {
                _context.Add(aevent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", aevent.CategoryId);
            return View(aevent);
        }

        // GET: Events/Edit/5
        [Authorize(Policy = "isRetailer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var aevent = await _context.Events.FindAsync(id);
            if (aevent == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", aevent.CategoryId);
            return View(aevent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId, RetailerId, CategoryId,Name,Seat,Price,StartDate,Address, Url")] Event aevent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(aevent.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", aevent.CategoryId);
            return View(aevent);
        }

        // GET: Events/Delete/5
        [Authorize(Policy = "isRetailer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var aevent = await _context.Events.Include(e => e.Category).Include(e => e.Retailer)
                .SingleAsync(m => m.EventId == id);
            if (aevent == null)
            {
                return NotFound();
            }

            return View(aevent);
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'TickyContext.Events'  is null.");
            }
            var aevent = await _context.Events.FindAsync(id);
            if (aevent != null)
            {
                _context.Events.Remove(aevent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
