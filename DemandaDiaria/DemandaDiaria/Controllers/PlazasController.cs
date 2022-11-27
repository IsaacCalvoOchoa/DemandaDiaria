using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemandaDiaria.Data;
using DemandaDiaria.Data.Entities;
using System.Diagnostics.Metrics;

namespace DemandaDiaria.Controllers
{
    public class PlazasController : Controller
    {
        private readonly DataContext _context;

        public PlazasController(DataContext context)
        {
            _context = context;
        }

        // GET: Plazas
        public async Task<IActionResult> Index()
        {
              return _context.Plazas != null ? 
                          View(await _context.Plazas.ToListAsync()) :
                          Problem("Entity set 'DataContext.Plazas'  is null.");
        }

        // GET: Plazas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Plazas == null)
            {
                return NotFound();
            }

            Plaza plaza = await _context.Plazas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plaza == null)
            {
                return NotFound();
            }

            return View(plaza);
        }

        // GET: Plazas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plazas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(plaza);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un plaza con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(plaza);

        }

        // GET: Plazas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plaza plaza = await _context.Plazas.FindAsync(id);
            if (plaza == null)
            {
                return NotFound();
            }
            return View(plaza);
        }

        // POST: Plazas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plaza plaza)
        {
            if (id != plaza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plaza);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un plaza con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(plaza);
        }

        // GET: Plazas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plaza = await _context.Plazas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plaza == null)
            {
                return NotFound();
            }

            return View(plaza);
        }

        // POST: Plazas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Plazas == null)
            {
                return Problem("Entity set 'DataContext.Plazas'  is null.");
            }
            Plaza plaza = await _context.Plazas.FindAsync(id);
            if (plaza != null)
            {
                _context.Plazas.Remove(plaza);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
