using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticky.Data;
using Ticky.Models;

namespace Ticky.Controllers
{
    public class RetailersController : Controller
    {
        private readonly TickyContext _context;
        private IAccountRepository _accountRepository;

        public RetailersController(TickyContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        // GET: Retailers
        public async Task<IActionResult> Index()
        {
            var TickyContext = _context.Retailers
                .Include(r => r.Account)
                .Where(r => r.Account.Role == 3);
            return View(await TickyContext.ToListAsync());
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name, Phone, Email")] Retailer retailer, string username, string password)
        {
            Account? account = null;
            if (_context.Accounts.FirstOrDefault(a => a.Username == username) != null)
            {
                string message = "Existed username";
            }
            else if (ModelState.IsValid)
            {
                _context.Add(new Account()
                {
                    Username = username,
                    Password = password,
                    Role = 3,
                });
                await _context.SaveChangesAsync();
                account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
                retailer.AccountId = account.AccountId;
                  _context.Add(retailer);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _context.Accounts.Remove(account);
                    await _context.SaveChangesAsync();
                    ViewBag.message = "Failed creating account. Try again later";
                    ViewBag.retailer = retailer;
                    ViewBag.account = new Account()
                    {
                        Username = username,
                        Password = password,
                    };
                    return View();
                }
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.message = "Existed Username";
            ViewBag.retailer = retailer;
            ViewBag.account = new Account()
            {
                Username = username,
                Password = password,
            };
            return View(retailer);
        }

        // GET: Retailers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Retailers == null)
            {
                return NotFound();
            }

            var retailer = await _context.Retailers
                .Include(r => r.Account)
                .FirstOrDefaultAsync(m => m.RetailerId == id);
            if (retailer == null)
            {
                return NotFound();
            }

            return View(retailer);
        }

        // GET: Retailers/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["AdminId"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            return View();
        }

        // POST: Retailers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RetailerId,AccountId,AdminId,Phone,Email,ApproveDate")] Retailer retailer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retailer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", retailer.AccountId);
            return View(retailer);
        }

        // GET: Retailers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Retailers == null)
            {
                return NotFound();
            }

            var retailer = await _context.Retailers.FindAsync(id);
            if (retailer == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", retailer.AccountId);
            return View(retailer);
        }

        // POST: Retailers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RetailerId,AccountId,AdminId,Phone,Email,ApproveDate")] Retailer retailer)
        {
            if (id != retailer.RetailerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retailer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetailerExists(retailer.RetailerId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", retailer.AccountId);
            return View(retailer);
        }

        // GET: Retailers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Retailers == null)
            {
                return NotFound();
            }

            var retailer = await _context.Retailers
                .Include(r => r.Account)
                .FirstOrDefaultAsync(m => m.RetailerId == id);
            if (retailer == null)
            {
                return NotFound();
            }

            return View(retailer);
        }

        // POST: Retailers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Retailers == null)
            {
                return Problem("Entity set 'TickyContext.Retailers'  is null.");
            }
            var retailer = await _context.Retailers.FindAsync(id);
            if (retailer != null)
            {
                _context.Retailers.Remove(retailer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetailerExists(int id)
        {
          return (_context.Retailers?.Any(e => e.RetailerId == id)).GetValueOrDefault();
        }
    }
}
