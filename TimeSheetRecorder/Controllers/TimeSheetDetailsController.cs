using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeSheetRecorder.Data;
using TimeSheetRecorder.Models;
using TimeSheetRecorder.Utilities;

namespace TimeSheetRecorder.Controllers
{
    public class TimeSheetDetailsController : Controller
    {
        private readonly timeSheetRecorderContext _context;

        public TimeSheetDetailsController(timeSheetRecorderContext context)
        {
            _context = context;
        }

        // GET: TimeSheetDetails
        public async Task<IActionResult> Index()
        {
            //List<TimeSheetDetails> tsd = new List<TimeSheetDetails>();

            var timeSheetRecorderContext = _context.TimeSheetDetails.Include(t => t.Project);
            return View(await timeSheetRecorderContext.ToListAsync());
        }

        [Authorize]
        public IActionResult CaptureTimeSheet(bool? isNew, Guid? headerId, string? month)
        {

            bool newTimeSheet = isNew == null ? true : false;
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Title");
            var numberOfWeeks = Enumerable.Range(1, 52).ToList();
            ViewData["NumberOfWeeks"] = new SelectList(numberOfWeeks);

            var currentWeek = DateUtilities.GetIso8601WeekOfYear(DateTime.Now);
 
            if(!String.IsNullOrEmpty(month))
                currentWeek = Convert.ToInt32(month);

            var timeSheetHeader = _context.TimeSheetHeader.FirstOrDefault(t => t.UserId == User.Identity.Name && t.Week == currentWeek);
            if (timeSheetHeader == null)
            {
                timeSheetHeader = new TimeSheetHeader
                {
                    CaptureDate = DateTime.Now,
                    Week = currentWeek
                };
            }

            List<TimeSheetDetails> timeSheetDetails = _context.TimeSheetDetails.Where(x => x.TimeSheetHeaderId == timeSheetHeader.TimeSheetHeaderId).ToList();
            TimeSheetCaptureViewModel timeSheetCaptureViewModel = new TimeSheetCaptureViewModel()
            {
                IsNew = newTimeSheet,
                TimeSheetHeader = timeSheetHeader,
                TimeSheetDetails = timeSheetDetails,
                TimeSheetDetail = new TimeSheetDetails
                {
                    ActivityHours = 8,
                    ActivityDate = DateTime.Now,
                }
            };
            return View(timeSheetCaptureViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CaptureTimeSheet(TimeSheetCaptureViewModel timeSheet)
        {
            var currentUserId = User.Identity.Name.ToString();
            Guid tsHeaderId = Guid.NewGuid();
            var timeSheetHeader = _context.TimeSheetHeader.FirstOrDefault(t => t.UserId == currentUserId && t.Week == timeSheet.TimeSheetHeader.Week);

            if(timeSheetHeader == null)
            {
                timeSheetHeader = new TimeSheetHeader
                {
                    TimeSheetHeaderId = Guid.NewGuid(),
                    Week = timeSheet.TimeSheetHeader.Week,
                    UserId = currentUserId,
                    CaptureDate = timeSheet.TimeSheetHeader.CaptureDate
                };
                _context.Add(timeSheetHeader);
            }
          
            timeSheet.IsNew = false;

            timeSheet.TimeSheetDetail.TimeSheetDetailsId = Guid.NewGuid();
            timeSheet.TimeSheetDetail.TimeSheetHeaderId = timeSheetHeader.TimeSheetHeaderId;
            _context.Add(timeSheet.TimeSheetDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CaptureTimeSheet), new { isNew = timeSheet.IsNew, headerId = timeSheet.TimeSheetHeader.TimeSheetHeaderId });
        }

        // GET: TimeSheetDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TimeSheetDetails == null)
            {
                return NotFound();
            }

            var timeSheetDetails = await _context.TimeSheetDetails
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TimeSheetDetailsId == id);
            if (timeSheetDetails == null)
            {
                return NotFound();
            }

            return View(timeSheetDetails);
        }

        // GET: TimeSheetDetails/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
            return View();
        }

        // POST: TimeSheetDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeSheetDetailsId,TimeSheetId,ActivityDate,Title,Details,ActivityHours,ProjectId")] TimeSheetDetails timeSheetDetails)
        {
            if (ModelState.IsValid)
            {
                timeSheetDetails.TimeSheetDetailsId = Guid.NewGuid();
                _context.Add(timeSheetDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", timeSheetDetails.ProjectId);
            return View(timeSheetDetails);
        }

        // GET: TimeSheetDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TimeSheetDetails == null)
            {
                return NotFound();
            }

            var timeSheetDetails = await _context.TimeSheetDetails.FindAsync(id);
            if (timeSheetDetails == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", timeSheetDetails.ProjectId);
            return View(timeSheetDetails);
        }

        // POST: TimeSheetDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TimeSheetDetailsId,TimeSheetId,ActivityDate,Title,Details,ActivityHours,ProjectId")] TimeSheetDetails timeSheetDetails)
        {
            if (id != timeSheetDetails.TimeSheetDetailsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSheetDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSheetDetailsExists(timeSheetDetails.TimeSheetDetailsId))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId", timeSheetDetails.ProjectId);
            return View(timeSheetDetails);
        }

        // GET: TimeSheetDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TimeSheetDetails == null)
            {
                return NotFound();
            }

            var timeSheetDetails = await _context.TimeSheetDetails
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TimeSheetDetailsId == id);
            if (timeSheetDetails == null)
            {
                return NotFound();
            }

            return View(timeSheetDetails);
        }

        // POST: TimeSheetDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TimeSheetDetails == null)
            {
                return Problem("Entity set 'timeSheetRecorderContext.TimeSheetDetails'  is null.");
            }
            var timeSheetDetails = await _context.TimeSheetDetails.FindAsync(id);
            if (timeSheetDetails != null)
            {
                _context.TimeSheetDetails.Remove(timeSheetDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeSheetDetailsExists(Guid id)
        {
          return (_context.TimeSheetDetails?.Any(e => e.TimeSheetDetailsId == id)).GetValueOrDefault();
        }
    }
}
