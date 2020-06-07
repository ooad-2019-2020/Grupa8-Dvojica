using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineGames.Models;

namespace OnlineGames.Controllers
{
    public class SlikaIgriceController : Controller
    {
        private readonly Context _context;

        public SlikaIgriceController(Context context)
        {
            _context = context;
        }

        // GET: SlikaIgrice
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Index(string searchFilter)
        {
            var context = _context.SlikaIgrice.AsQueryable();

            ViewData["Pretraga"] = searchFilter;

            if (!String.IsNullOrEmpty(searchFilter))
            {
                context = context.Where(a => a.ImageTitle.Contains(searchFilter));
            }

            return View(await context.AsNoTracking().ToListAsync());
        }

        // GET: SlikaIgrice/Details/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slikaIgrice = await _context.SlikaIgrice
                .FirstOrDefaultAsync(m => m.SlikaIgriceId == id);
            if (slikaIgrice == null)
            {
                return NotFound();
            }

            return View(slikaIgrice);
        }

        // GET: SlikaIgrice/Create
        [Authorize(Roles = "Glavni,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SlikaIgrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlikaIgriceId,ImageTitle,ImageData")] SlikaIgrice slikaIgrice)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in Request.Form.Files)
                {
                    SlikaIgrice img = new SlikaIgrice();
                    img.ImageTitle = slikaIgrice.ImageTitle;

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.ImageData = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    _context.Add(img);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slikaIgrice);
        }

        // GET: SlikaIgrice/Edit/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slikaIgrice = await _context.SlikaIgrice.FindAsync(id);
            if (slikaIgrice == null)
            {
                return NotFound();
            }
            return View(slikaIgrice);
        }

        // POST: SlikaIgrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlikaIgriceId,ImageTitle,ImageData")] SlikaIgrice slikaIgrice)
        {
            if (id != slikaIgrice.SlikaIgriceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in Request.Form.Files)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        
                        slikaIgrice.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();

                        _context.Update(slikaIgrice);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlikaIgriceExists(slikaIgrice.SlikaIgriceId))
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
            return View(slikaIgrice);
        }

        // GET: SlikaIgrice/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slikaIgrice = await _context.SlikaIgrice
                .FirstOrDefaultAsync(m => m.SlikaIgriceId == id);
            if (slikaIgrice == null)
            {
                return NotFound();
            }

            return View(slikaIgrice);
        }

        // POST: SlikaIgrice/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slikaIgrice = await _context.SlikaIgrice.FindAsync(id);
            _context.SlikaIgrice.Remove(slikaIgrice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Glavni,Admin")]
        private bool SlikaIgriceExists(int id)
        {
            return _context.SlikaIgrice.Any(e => e.SlikaIgriceId == id);
        }
    }
}
