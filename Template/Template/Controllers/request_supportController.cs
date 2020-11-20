using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Template.Context;
using Template.Models;

namespace Template.Controllers
{
    public class request_supportController : Controller
    {
        private readonly CustomDbContext _context;

        public request_supportController(CustomDbContext context)
        {
            _context = context;
        }

        // GET: request_support
        public async Task<IActionResult> Index()
        {
            return View(await _context.request_Supports.ToListAsync());
        }

        //GET: ThankYou
        public ActionResult Thankyou()
        {
            return View();
        }

        // GET: request_support/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request_support = await _context.request_Supports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request_support == null)
            {
                return NotFound();
            }

            return View(request_support);
        }

        // GET: request_support/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: request_support/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Details,supportType")] request_support request_support)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request_support);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Thankyou));
            }
            return View(request_support);
        }

        // GET: request_support/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request_support = await _context.request_Supports.FindAsync(id);
            if (request_support == null)
            {
                return NotFound();
            }
            return View(request_support);
        }

        // POST: request_support/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Details,supportType")] request_support request_support)
        {
            if (id != request_support.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request_support);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!request_supportExists(request_support.Id))
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
            return View(request_support);
        }

        // GET: request_support/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request_support = await _context.request_Supports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request_support == null)
            {
                return NotFound();
            }

            return View(request_support);
        }

        // POST: request_support/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request_support = await _context.request_Supports.FindAsync(id);
            _context.request_Supports.Remove(request_support);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool request_supportExists(int id)
        {
            return _context.request_Supports.Any(e => e.Id == id);
        }
    }
}
