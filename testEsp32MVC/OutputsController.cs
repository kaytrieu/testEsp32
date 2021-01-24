using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testEsp32MVC.Data;
using testEsp32MVC.Models;

namespace testEsp32MVC
{
    public class OutputsController : Controller
    {
        private readonly EspconnecttestContext _context;

        public OutputsController(EspconnecttestContext context)
        {
            _context = context;
        }

        // GET: Outputs
        public async Task<IActionResult> Index()
        {
            var espconnecttestContext = _context.Outputs.Include(o => o.BoardNavigation);
            return View(await espconnecttestContext.ToListAsync());
        }

        // GET: Outputs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outputs = await _context.Outputs
                .Include(o => o.BoardNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outputs == null)
            {
                return NotFound();
            }

            return View(outputs);
        }

        // GET: Outputs/Create
        public IActionResult Create()
        {
            ViewData["Board"] = new SelectList(_context.Boards, "Id", "Id");
            return View();
        }

        // POST: Outputs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Board,Gpio,State")] Outputs outputs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outputs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Board"] = new SelectList(_context.Boards, "Id", "Id", outputs.Board);
            return View(outputs);
        }

        // GET: Outputs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outputs = await _context.Outputs.FindAsync(id);
            if (outputs == null)
            {
                return NotFound();
            }
            ViewData["Board"] = new SelectList(_context.Boards, "Id", "Id", outputs.Board);
            return View(outputs);
        }

        // POST: Outputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Board,Gpio,State")] Outputs outputs)
        {
            if (id != outputs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outputs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutputsExists(outputs.Id))
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
            ViewData["Board"] = new SelectList(_context.Boards, "Id", "Id", outputs.Board);
            return View(outputs);
        }

        // GET: Outputs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outputs = await _context.Outputs
                .Include(o => o.BoardNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outputs == null)
            {
                return NotFound();
            }

            return View(outputs);
        }

        // POST: Outputs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outputs = await _context.Outputs.FindAsync(id);
            _context.Outputs.Remove(outputs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutputsExists(int id)
        {
            return _context.Outputs.Any(e => e.Id == id);
        }
    }
}
