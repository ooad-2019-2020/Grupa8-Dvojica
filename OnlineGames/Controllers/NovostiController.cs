using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineGames.Models;

namespace OnlineGames.Controllers
{
    public class NovostiController : Controller
    {
        private readonly Context _context;

        public NovostiController(Context context)
        {
            _context = context;
        }

        // GET: Novosti
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Index(string searchFilter)
        {
            var context = _context.Novosti.AsQueryable();

            ViewData["Pretraga"] = searchFilter;

            if (!String.IsNullOrEmpty(searchFilter))
            {
                context = context.Where(a => a.Naslov.Contains(searchFilter)
                                    || a.Tekst.Contains(searchFilter));
            }

            return View(await context.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Novosti(string searchFilter)
        {
            var context = _context.Novosti.AsQueryable();

            ViewData["Pretraga"] = searchFilter;

            if (!String.IsNullOrEmpty(searchFilter))
            {
                context = context.Where(a => a.Naslov.Contains(searchFilter)
                                    || a.Tekst.Contains(searchFilter));
            }

            return View(await context.AsNoTracking().ToListAsync());
        }

        // GET: Novosti/Details/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novosti = await _context.Novosti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (novosti == null)
            {
                return NotFound();
            }

            return View(novosti);
        }

        // GET: Novosti/Create
        [Authorize(Roles = "Glavni,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Novosti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naslov,Tekst,Link")] Novosti novosti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(novosti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(novosti);
        }

        // GET: Novosti/Edit/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novosti = await _context.Novosti.FindAsync(id);
            if (novosti == null)
            {
                return NotFound();
            }
            return View(novosti);
        }

        // POST: Novosti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naslov,Tekst,Link")] Novosti novosti)
        {
            if (id != novosti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(novosti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovostiExists(novosti.Id))
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
            return View(novosti);
        }

        // GET: Novosti/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novosti = await _context.Novosti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (novosti == null)
            {
                return NotFound();
            }

            return View(novosti);
        }

        // POST: Novosti/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novosti = await _context.Novosti.FindAsync(id);
            _context.Novosti.Remove(novosti);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Glavni,Admin")]
        private bool NovostiExists(int id)
        {
            return _context.Novosti.Any(e => e.Id == id);
        }
    }
}
