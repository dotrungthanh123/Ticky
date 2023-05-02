using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ticky.Data;
using Ticky.Models;
using Ticky.Server.Controllers;

namespace Ticky.Controllers
{
    [Authorize(Policy = "IsCustomer")]
    public class OrderController : Controller
    {
        TickyContext _context;

        public OrderController(TickyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int EventId)
        {
            var od = await _context.OrderDetails.Include(o => o.Event).FirstOrDefaultAsync(o => o.OrderId == getCurrentCart().OrderId && o.EventId == EventId);
            return View(od);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int EventId, int quantity)
        {
            var od = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderId == getCurrentCart().OrderId && o.EventId == EventId);
            if (od != null)
            {
                od.Quantity = quantity;
                _context.Update(od);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Order");
        }

        private TicketOrder getCurrentCart()
        {
            int accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int customerId = _context.Customers.SingleOrDefault(c => c.AccountId == accountId).CustomerId;
            var current_cart = _context.TicketOrders.OrderByDescending(o => o.OrderId).FirstOrDefault(o => o.CustomerId == customerId);
            if (current_cart == null || current_cart.BuyDate != null)
            {
                current_cart = new TicketOrder()
                {
                    CustomerId = customerId,
                };
                _context.Add(current_cart);
                _context.SaveChanges();
            }
            return current_cart;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int quantity, int EventId, string ReturnUrl = "/")
        {
            var current_cart = getCurrentCart();
            if (_context.OrderDetails.FirstOrDefault(o => o.OrderId == current_cart.OrderId && o.EventId == EventId) == null)
            {
                var orderDetail = new OrderDetail()
                {
                    Quantity = quantity,
                    EventId = EventId,
                    OrderId = current_cart.OrderId
                };
                _context.Add(orderDetail);
            }
            else
            {
                _context.OrderDetails.FirstOrDefault(o => o.OrderId == current_cart.OrderId && o.EventId == EventId).Quantity += quantity;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Checkout(int OrderId)
        {
            var order = getCurrentCart();
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                order.BuyDate = DateTime.Today;
                _context.Update(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var orderDetails = await _context.OrderDetails.Include(od => od.TicketOrder).Include(od => od.Event)
                .Where(a => a.OrderId == getCurrentCart().OrderId).ToListAsync();
            return View(orderDetails);
        }

        //public async Task<IActionResult> Edit(int? EventId)
        //{
        //    if (EventId == null || _context.OrderDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    var orderdetail = await _context.OrderDetails.Include(od => od.Event).FirstOrDefaultAsync(od => od.EventId == EventId && od.OrderId == getCurrentCart().OrderId);
        //    if (orderdetail == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(orderdetail);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int EventId, int OrderId, [Bind("EventId, Quantity, OrderId")] OrderDetail orderDetail)
        //{
        //    if (EventId != orderDetail.EventId || OrderId != orderDetail.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(orderDetail);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(orderDetail);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int EventId)
        {
            var orderdetail = await _context.OrderDetails.FirstAsync(od => od.OrderId == getCurrentCart().OrderId && od.EventId == EventId);
            _context.OrderDetails.Remove(orderdetail);
            await _context.SaveChangesAsync(true);
            return RedirectToAction("Index", "Order");
        }
    }
}
