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
    public class SuggestionsController : Controller
    {
        private readonly CustomDbContext _context;

        public SuggestionsController(CustomDbContext context)
        {
            _context = context;
        }

        // GET: Suggestions
        public async Task<IActionResult> Index(Suggestion model)
        {
            var customDbContext = model.DepartmentId > 0?_context.Suggestions.Where(s => s.DepartmentId == model.DepartmentId).Select(k => new Suggestion {Details= k.Details, DepartmentId = k.DepartmentId, Title = k.Title, Id = k.Id, Department = k.Department }): _context.Suggestions.Include(s => s.Department); 
            
            ViewData["DepartmentId"] = _context.Departments.Select(c => new SelectListItem
            {
                Text = c.NameEN,
                Value = Convert.ToString(c.Id)
            });
            return View(await customDbContext.ToListAsync());
        }

        // GET: Suggestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return View(suggestion);
        }

        // GET: Suggestions/Create
        public IActionResult Create()
        {
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["DepartmentId"] = _context.Departments.Select(c => new SelectListItem
            {
                Text = c.NameEN,
                Value = Convert.ToString(c.Id)
            });
            return View();
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Details,DepartmentId")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suggestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThankYou));
            }
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", suggestion.DepartmentId);
            ViewData["DepartmentId"] = _context.Departments.Select(c => new SelectListItem
            {
                Text = c.NameEN,
                Value = Convert.ToString(c.Id)
            });
            return View(suggestion);
        }

        //GET: ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }

        // GET: Suggestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", suggestion.DepartmentId);
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Details,DepartmentId")] Suggestion suggestion)
        {
            if (id != suggestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suggestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuggestionExists(suggestion.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", suggestion.DepartmentId);
            return View(suggestion);
        }

        // GET: Suggestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return View(suggestion);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);
            _context.Suggestions.Remove(suggestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
