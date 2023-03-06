using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemandaDiaria.Data;
using DemandaDiaria.Data.Entities;
using DemandaDiaria.Helpers;
using DemandaDiaria.Enums;

namespace DemandaDiaria.Controllers
{
    public class DemandasController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelpers _userHelpers;

        public DemandasController(DataContext context, IUserHelpers userHelpers)
        {
            _context = context;
            _userHelpers = userHelpers;
        }

        // GET: Demandas
        public async Task<IActionResult> Index()
        {
            return _context.Demandas != null ?
                      View(await _context.Demandas
                      .Include(s => s.Products)
                      .ToListAsync()) :
                      Problem("Entity set 'DataContext.Demandas'  is null.");
        }

        // GET: Demandas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Demandas == null)
            {
                return NotFound();
            }

            var demanda = await _context.Demandas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demanda == null)
            {
                return NotFound();
            }

            return View(demanda);
        }

        // GET: Demandas/Create
        public async Task<IActionResult> Create()
        {
            User user = await _userHelpers.GetUserAsync(User.Identity.Name);
            Demanda demanda = new()
            {
                Date = DateTime.Now,
                Products = new List<Product>(),
                User = user,
                UniUser = user.Uni,
                DemandaStatus = DemandaStatus.Abierto
            };
            if (ModelState.IsValid)
            {
                _context.Add(demanda);
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index));
        }

        // POST: Demandas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Demanda demanda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demanda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(demanda);
        }

        // GET: Demandas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Demandas == null)
            {
                return NotFound();
            }

            var demanda = await _context.Demandas.FindAsync(id);
            if (demanda == null)
            {
                return NotFound();
            }
            return View(demanda);
        }

        // POST: Demandas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Demanda demanda)
        {
            if (id != demanda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demanda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandaExists(demanda.Id))
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
            return View(demanda);
        }

        // GET: Demandas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Demandas == null)
            {
                return NotFound();
            }

            var demanda = await _context.Demandas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demanda == null)
            {
                return NotFound();
            }

            return View(demanda);
        }

        // POST: Demandas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Demandas == null)
            {
                return Problem("Entity set 'DataContext.Demandas'  is null.");
            }
            var demanda = await _context.Demandas.FindAsync(id);
            if (demanda != null)
            {
                _context.Demandas.Remove(demanda);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandaExists(int id)
        {
          return _context.Demandas.Any(e => e.Id == id);
        }
    }
}
