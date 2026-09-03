using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITAssetManagement.Data;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Computers
        public async Task<IActionResult> Index(
     string? search,
     string? status,
     string? assignment)
        {
            var query = _context.Computers
                .Include(computer => computer.Employee)
                .AsQueryable();

            // Tìm theo tên máy, hệ điều hành hoặc tên nhân viên
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(computer =>
                    computer.ComputerName.Contains(search) ||
                    (computer.OperatingSystem != null &&
                     computer.OperatingSystem.Contains(search)) ||
                    (computer.Employee != null &&
                     computer.Employee.FullName.Contains(search)));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(computer =>
                    computer.Status == status);
            }

            // Lọc theo tình trạng cấp phát
            if (assignment == "assigned")
            {
                query = query.Where(computer =>
                    computer.EmployeeID != null);
            }
            else if (assignment == "unassigned")
            {
                query = query.Where(computer =>
                    computer.EmployeeID == null);
            }

            // Giữ lại các giá trị đang tìm kiếm
            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedAssignment = assignment;

            var computers = await query
                .OrderBy(computer => computer.ComputerID)
                .ToListAsync();

            return View(computers);
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.ComputerID == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName");
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComputerID,ComputerName,EmployeeID,OperatingSystem,Status")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] =  $"Đã thêm máy tính {computer.ComputerName} thành công.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", computer.EmployeeID);
            return View(computer);
        }

        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", computer.EmployeeID);
            return View(computer);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComputerID,ComputerName,EmployeeID,OperatingSystem,Status")] Computer computer)
        {
            if (id != computer.ComputerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Đã cập nhật máy tính {computer.ComputerName} thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerExists(computer.ComputerID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", computer.EmployeeID);
            return View(computer);
        }

        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.ComputerID == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if (computer == null)
            {
                return NotFound();
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Đã xóa máy tính {computer.ComputerName} thành công.";

            return RedirectToAction(nameof(Index));
        }

        private bool ComputerExists(int id)
        {
            return _context.Computers.Any(e => e.ComputerID == id);
        }
    }
}
