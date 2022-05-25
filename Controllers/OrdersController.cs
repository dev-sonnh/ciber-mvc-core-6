using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciber.Data;
using Ciber.Models;
using X.PagedList;

namespace Ciber.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ProdSortParm = String.IsNullOrEmpty(sortOrder) ? "prod_desc" : ""; ;
                ViewBag.CustSortParm = sortOrder == "cust" ? "cust_desc" : "cust";
                ViewBag.CateSortParm = sortOrder == "cate" ? "cate_desc" : "cate";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var orders = _context.Order
                    .Include(o => o.Customer)
                    .Include(o => o.Product)
                        .ThenInclude(p => p.Category)
                    .AsNoTracking();

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o => o.Product.Category.Name.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "cust":
                        orders = orders.OrderBy(s => s.Customer.Name);
                        break;
                    case "cust_desc":
                        orders = orders.OrderByDescending(s => s.Customer.Name);
                        break;
                    case "cate":
                        orders = orders.OrderBy(s => s.Product.Category.Name);
                        break;
                    case "cate_desc":
                        orders = orders.OrderByDescending(s => s.Product.Category.Name);
                        break;
                    case "prod_desc":
                        orders = orders.OrderByDescending(s => s.Product.Name);
                        break;
                    default:
                        orders = orders.OrderBy(s => s.Product.Name);
                        break;
                }
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(orders.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, $"An error has been orcurred 〣( ºΔº )〣");
                return View();
            }



        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(o => o.Customer).Include(o => o.Product).AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            PopulateCustomersDropDownList();
            PopulateProductsDropDownList();
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CustomerId,ProductId,Id,Amount,OrderDate")] Order order)
        {
            try
            {
                var existingProd = await _context.Product.Where(p => p.Id == order.ProductId).FirstOrDefaultAsync();

                if ((existingProd?.Quantity ?? 0) > order.Amount)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(order);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                }
                else
                {
                    ModelState.AddModelError(String.Empty, $"Quantity of {existingProd.Name} is not enough to create this order!" +
                        $" Please check amount of your order again ¯\\_(ツ)_/¯");
                }
                PopulateCustomersDropDownList(order.CustomerId);
                PopulateProductsDropDownList(order.ProductId);
                return View(order);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, $"An error has been orcurred 〣( ºΔº )〣");
                return NotFound();
            }
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            PopulateCustomersDropDownList(order.CustomerId);
            PopulateProductsDropDownList(order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,CustomerId,ProductId,Id,Amount,OrderDate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            PopulateCustomersDropDownList(order.CustomerId);
            PopulateProductsDropDownList(order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'CiberContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private async void PopulateProductsDropDownList(object selectedProduct = null)
        {
            var productsQuery = from d in _context.Product
                                orderby d.Name
                                select d;
            ViewBag.ProductId = new SelectList(productsQuery.AsNoTracking(), "Id", "Name", selectedProduct);
        }

        private async void PopulateCustomersDropDownList(object selectedCustomer = null)
        {
            var customersQuery = from c in _context.Customer
                                 orderby c.Name
                                 select c;
            ViewBag.CustomerId = new SelectList(customersQuery.AsNoTracking(), "Id", "Name", selectedCustomer);
        }
    }
}
