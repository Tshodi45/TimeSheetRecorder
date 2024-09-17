using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeSheetRecorder.Data;

namespace TimeSheetRecorder.Controllers
{
    public class TimeSheetHeadersController : Controller
    {
        private readonly timeSheetRecorderContext _context;

        public TimeSheetHeadersController(timeSheetRecorderContext context)
        {
            _context = context;
        }

        // GET: TimeSheetHeaders
        public async Task<IActionResult> Index()
        {
              return _context.TimeSheetHeader != null ? 
                          View(await _context.TimeSheetHeader.ToListAsync()) :
                          Problem("Entity set 'timeSheetRecorderContext.TimeSheetHeader'  is null.");
        }

        // GET: TimeSheetHeaders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TimeSheetHeader == null)
            {
                return NotFound();
            }

            var timeSheetHeader = await _context.TimeSheetHeader
                .FirstOrDefaultAsync(m => m.TimeSheetHeaderId == id);
            if (timeSheetHeader == null)
            {
                return NotFound();
            }

            return View(timeSheetHeader);
        }

        // GET: TimeSheetHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeSheetHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeSheetHeaderId,Week,UserId,CaptureDate")] TimeSheetHeader timeSheetHeader)
        {
            if (ModelState.IsValid)
            {
                timeSheetHeader.TimeSheetHeaderId = Guid.NewGuid();
                _context.Add(timeSheetHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeSheetHeader);
        }

        // GET: TimeSheetHeaders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TimeSheetHeader == null)
            {
                return NotFound();
            }

            var timeSheetHeader = await _context.TimeSheetHeader.FindAsync(id);
            if (timeSheetHeader == null)
            {
                return NotFound();
            }
            return View(timeSheetHeader);
        }

        // POST: TimeSheetHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TimeSheetHeaderId,Week,UserId,CaptureDate")] TimeSheetHeader timeSheetHeader)
        {
            if (id != timeSheetHeader.TimeSheetHeaderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSheetHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSheetHeaderExists(timeSheetHeader.TimeSheetHeaderId))
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
            return View(timeSheetHeader);
        }

        // GET: TimeSheetHeaders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TimeSheetHeader == null)
            {
                return NotFound();
            }

            var timeSheetHeader = await _context.TimeSheetHeader
                .FirstOrDefaultAsync(m => m.TimeSheetHeaderId == id);
            if (timeSheetHeader == null)
            {
                return NotFound();
            }

            return View(timeSheetHeader);
        }

        // POST: TimeSheetHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TimeSheetHeader == null)
            {
                return Problem("Entity set 'timeSheetRecorderContext.TimeSheetHeader'  is null.");
            }
            var timeSheetHeader = await _context.TimeSheetHeader.FindAsync(id);
            if (timeSheetHeader != null)
            {
                _context.TimeSheetHeader.Remove(timeSheetHeader);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeSheetHeaderExists(Guid id)
        {
          return (_context.TimeSheetHeader?.Any(e => e.TimeSheetHeaderId == id)).GetValueOrDefault();
        }
    }
}
