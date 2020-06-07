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
    public class KategorijaIgricasController : Controller
    {
        private readonly Context _context;

        public KategorijaIgricasController(Context context)
        {
            _context = context;
        }

        // GET: KategorijaIgricas
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Index(string searchFilter)
        {
            var context = _context.KategorijaIgrica.Include(k => k.Igrica).Include(k => k.Kategorija).AsQueryable();
            
            ViewData["Pretraga"] = searchFilter;

            if (!String.IsNullOrEmpty(searchFilter))
            {
                context = context.Where(a => a.Kategorija.Naziv.Contains(searchFilter)
                                    ||  a.Igrica.Naziv.Contains(searchFilter));
            }

            return View(await context.AsNoTracking().ToListAsync());
        }

        // GET: KategorijaIgricas/Details/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorijaIgrica = await _context.KategorijaIgrica
                .Include(k => k.Igrica)
                .Include(k => k.Kategorija)
                .FirstOrDefaultAsync(m => m.KategorijaIgricaId == id);
            if (kategorijaIgrica == null)
            {
                return NotFound();
            }

            return View(kategorijaIgrica);
        }

        // GET: KategorijaIgricas/Create
        [Authorize(Roles = "Glavni,Admin")]
        public IActionResult Create()
        {
            ViewData["IgricaId"] = new SelectList(_context.Igrica, "Id", "Naziv");
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "KategorijaId", "Naziv");
            return View();
        }

        // POST: KategorijaIgricas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategorijaIgricaId,IgricaId,KategorijaId")] KategorijaIgrica kategorijaIgrica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategorijaIgrica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IgricaId"] = new SelectList(_context.Igrica, "Id", "Id", kategorijaIgrica.IgricaId);
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "KategorijaId", "KategorijaId", kategorijaIgrica.KategorijaId);
            return View(kategorijaIgrica);
        }

        // GET: KategorijaIgricas/Edit/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorijaIgrica = await _context.KategorijaIgrica.FindAsync(id);
            if (kategorijaIgrica == null)
            {
                return NotFound();
            }
            ViewData["IgricaId"] = new SelectList(_context.Igrica, "Id", "Naziv", kategorijaIgrica.IgricaId);
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "KategorijaId", "Naziv", kategorijaIgrica.KategorijaId);
            return View(kategorijaIgrica);
        }

        // POST: KategorijaIgricas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategorijaIgricaId,IgricaId,KategorijaId")] KategorijaIgrica kategorijaIgrica)
        {
            if (id != kategorijaIgrica.KategorijaIgricaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategorijaIgrica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorijaIgricaExists(kategorijaIgrica.KategorijaIgricaId))
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
            ViewData["IgricaId"] = new SelectList(_context.Igrica, "Id", "Id", kategorijaIgrica.IgricaId);
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "KategorijaId", "KategorijaId", kategorijaIgrica.KategorijaId);
            return View(kategorijaIgrica);
        }

        // GET: KategorijaIgricas/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorijaIgrica = await _context.KategorijaIgrica
                .Include(k => k.Igrica)
                .Include(k => k.Kategorija)
                .FirstOrDefaultAsync(m => m.KategorijaIgricaId == id);
            if (kategorijaIgrica == null)
            {
                return NotFound();
            }

            return View(kategorijaIgrica);
        }

        // POST: KategorijaIgricas/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorijaIgrica = await _context.KategorijaIgrica.FindAsync(id);
            _context.KategorijaIgrica.Remove(kategorijaIgrica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Glavni,Admin")]
        private bool KategorijaIgricaExists(int id)
        {
            return _context.KategorijaIgrica.Any(e => e.KategorijaIgricaId == id);
        }
    }
}
