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
using System.Numerics;
using DemandaDiaria.Models;

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
            return View(await _context.Plazas
                .Include(p => p.Sucursales)
                .ToListAsync());                     
        }

        // GET: Plazas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plaza plaza = await _context.Plazas
                .Include(p => p.Sucursales)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plaza == null)
            {
                return NotFound();
            }

            return View(plaza);
        }

        public async Task<IActionResult> DetailsSucursal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sucursal sucursal = await _context.Sucursales
                .Include(s => s.Plaza)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // GET: Plazas/Create
        public IActionResult Create()
        {
            Plaza plaza = new() { Sucursales = new List<Sucursal>() };
            return View(plaza);
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

        public async Task<IActionResult> AddSucursal(int? id)
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

            SucursalViewModel model = new()
            {
                PlazaId = plaza.Id,
            };

            return View(model);
        }

        // POST: Plazas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSucursal(SucursalViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Sucursal sucursal = new()
                    {
                        Plaza = await _context.Plazas.FindAsync(model.PlazaId),
                        Name = model.Name,
                    };
                    _context.Add(sucursal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.PlazaId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Sucursal con el mismo nombre.");
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
            return View(model);

        }

        // GET: Plazas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plaza plaza = await _context.Plazas
                .Include(p => p.Sucursales)
                .FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<IActionResult> EditSucursal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sucursal sucursal = await _context.Sucursales
                .Include(s => s.Plaza)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            SucursalViewModel model = new()
            {
                PlazaId = sucursal.Plaza.Id,
                Id = sucursal.Id,
                Name= sucursal.Name,
            
            };
            return View(model);
        }

        // POST: Plazas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSucursal(int id, SucursalViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Sucursal sucursal = new()
                    {
                        Id = model.Id,
                        Name= model.Name,
                    };
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new {Id = model.PlazaId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una sucursal con el mismo nombre.");
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
            return View(model);
        }

        // GET: Plazas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plaza = await _context.Plazas
                .Include(p => p.SucursalesNumber)
                .FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<IActionResult> DeleteSucursal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sucursal sucursal = await _context.Sucursales
                .Include(s => s.Plaza)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Plazas/Delete/5
        [HttpPost, ActionName("DeleteSucursal")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSucursalConfirmed(int id)
        {

            Sucursal sucursal = await _context.Sucursales
                .Include(s => s.Plaza)
                .FirstOrDefaultAsync(s => s.Id == id);
            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {Id = sucursal.Plaza.Id});
        }


    }
}
