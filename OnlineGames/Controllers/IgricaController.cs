using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineGames.Models;

namespace OnlineGames.Controllers
{
    public class IgricaController : Controller
    {
        private readonly Context _context;
        public UserManager<IdentityUser> _userManager;

        public IgricaController(Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Igrica
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Index(string searchFilter)
        {
            var context = _context.Igrica.Include(k => k.SlikaIgrice).AsQueryable();
            
            ViewData["Pretraga"] = searchFilter;
            
            if (!String.IsNullOrEmpty(searchFilter))
            {
                context = context.Where(a => a.Naziv.Contains(searchFilter));
            }

            return View(await context.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Prodavnica(string sortOrder, string categoryFilter, string searchFilter)
        {
            ViewData["SortCijena"] = sortOrder == "Cijena_asc" ? "Cijena_desc" : "Cijena_asc";
            ViewData["SortDatum"] = sortOrder == "Godina_asc" ? "Godina_desc" : "Godina_asc";
            ViewData["SortNaziv"] = sortOrder == "Naziv_asc" ? "Naziv_desc" : "Naziv_asc";
            ViewData["Pretraga"] = searchFilter;

            var igrice = from b in _context.KategorijaIgrica.Include(k => k.Igrica.SlikaIgrice)
                         .Include(k => k.Igrica).AsQueryable() select b;
            var kategorija = _context.Kategorija.ToList();
            var igricee = _context.Igrica.ToList();

            ViewData["Kategorije"] = kategorija;

            var kategorija1 = _context.Kategorija.AsQueryable();

            if (!String.IsNullOrEmpty(searchFilter))
            {
                igrice = igrice.Where(a => a.Igrica.Naziv.Contains(searchFilter));
            }

            if (!String.IsNullOrEmpty(categoryFilter))
            {
                kategorija1 = kategorija1.Where(k => k.Naziv == categoryFilter);
                int id = kategorija1.Single().KategorijaId;
                igrice = igrice.Where(a => a.KategorijaId == id);
            }

            switch (sortOrder)
            {
                case "Cijena_asc":
                    igrice = igrice.OrderBy(b => b.Igrica.Cijena);
                    igricee = igricee.OrderBy(b => b.Cijena).ToList();
                    break;
                case "Cijena_desc":
                    igrice = igrice.OrderByDescending(b => b.Igrica.Cijena);
                    igricee = igricee.OrderByDescending(b => b.Cijena).ToList();
                    break;
                case "Godina_asc":
                    igrice = igrice.OrderBy(b => b.Igrica.DatumIzlaska);
                    igricee = igricee.OrderBy(b => b.DatumIzlaska).ToList();
                    break;
                case "Godina_desc":
                    igrice = igrice.OrderByDescending(b => b.Igrica.DatumIzlaska);
                    igricee = igricee.OrderByDescending(b => b.DatumIzlaska).ToList();
                    break;
                case "Naziv_asc":
                    igrice = igrice.OrderBy(b => b.Igrica.Naziv);
                    igricee = igricee.OrderBy(b => b.Naziv).ToList();
                    break;
                case "Naziv_desc":
                    igrice = igrice.OrderByDescending(b => b.Igrica.Naziv);
                    igricee = igricee.OrderByDescending(b => b.Naziv).ToList();
                    break;
                default:
                    break;
            }
            ViewData["Igrice"] = igricee;
            return View(await igrice.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> KupiIgricu(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igrica = await _context.Igrica.Include(k => k.SlikaIgrice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igrica == null)
            {
                return NotFound();
            }
  
            return View(igrica);
        }

        public async Task<IActionResult> PotvrdiKupovinu(int? id)
        {
            string korisnickiID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            
            if(korisnickiID == null)
            {
                return NotFound();
            }
            int ID = (int)id;
            var nalogIgrice = new NalogIgrice { IgricaId = ID, NalogId = korisnickiID };
            _context.Add(nalogIgrice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MojeIgrice));
        }

        public async Task<IActionResult> OIgrici(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igrica = _context.KategorijaIgrica.Include(k => k.Igrica.SlikaIgrice).Include(k => k.Kategorija).AsQueryable()
                .Where(m => m.Igrica.Id == id);

            if (igrica == null)
            {
                return NotFound();
            }

            return View(await igrica.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> MojeIgrice(string searchFilter)
        {
            ViewData["Pretraga"] = searchFilter;

            var igrice = from b in _context.NalogIgrice.Include(k => k.Igrica.SlikaIgrice)
                         .Include(k => k.Igrica).AsQueryable()
                         select b;

            string korisnickiID = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            igrice = igrice.Where(b => b.NalogId == korisnickiID);

            if (!String.IsNullOrEmpty(searchFilter))
            {
                igrice = igrice.Where(a => a.Igrica.Naziv.Contains(searchFilter));
            }

            return View(await igrice.AsNoTracking().ToListAsync());
        }

        // GET: Igrica/Details/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igrica = await _context.Igrica.Include(k => k.SlikaIgrice)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (igrica == null)
            {
                return NotFound();
            }

            return View(igrica);
        }

        // GET: Igrica/Create
        [Authorize(Roles = "Glavni,Admin")]
        public IActionResult Create()
        {
            ViewData["SlikaIgriceId"] = new SelectList(_context.SlikaIgrice, "SlikaIgriceId", "ImageTitle");
            return View();
        }

        // POST: Igrica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,DatumIzlaska,Izdavac,Specifikacije,Cijena,SlikaIgriceId")] Igrica igrica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(igrica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SlikaIgriceId"] = new SelectList(_context.SlikaIgrice, "SlikaIgriceId", "SlikaIgiriceId", igrica.SlikaIgriceId);
            return View(igrica);
        }

        // GET: Igrica/Edit/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igrica = await _context.Igrica.FindAsync(id);
            if (igrica == null)
            {
                return NotFound();
            }
            ViewData["SlikaIgriceId"] = new SelectList(_context.SlikaIgrice, "SlikaIgriceId", "ImageTitle", igrica.SlikaIgriceId);
            return View(igrica);
        }

        // POST: Igrica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,DatumIzlaska,Izdavac,Specifikacije,Cijena,SlikaIgriceId")] Igrica igrica)
        {
            if (id != igrica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(igrica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IgricaExists(igrica.Id))
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
            ViewData["SlikaIgriceId"] = new SelectList(_context.SlikaIgrice, "SlikaIgriceId", "SlikaIgiriceId", igrica.SlikaIgriceId);
            return View(igrica);
        }

        // GET: Igrica/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igrica = await _context.Igrica.Include(k => k.SlikaIgrice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igrica == null)
            {
                return NotFound();
            }

            return View(igrica);
        }

        // POST: Igrica/Delete/5
        [Authorize(Roles = "Glavni,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igrica = await _context.Igrica.FindAsync(id);
            _context.Igrica.Remove(igrica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Glavni,Admin")]
        private bool IgricaExists(int id)
        {
            return _context.Igrica.Any(e => e.Id == id);
        }
    }
}
