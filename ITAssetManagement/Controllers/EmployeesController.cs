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
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(
        string? search,
        string? department)
        {
            var query = _context.Employees
                .Include(employee => employee.Computers)
                .AsQueryable();

            // Tìm theo tên, email hoặc số điện thoại
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(employee =>
                    employee.FullName.Contains(search) ||
                    (employee.Email != null &&
                     employee.Email.Contains(search)) ||
                    (employee.Phone != null &&
                     employee.Phone.Contains(search)));
            }

            // Lọc theo phòng ban
            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(employee =>
                    employee.Department == department);
            }

            // Danh sách phòng ban cho ô chọn
            ViewBag.Departments = await _context.Employees
                .Where(employee => employee.Department != null &&
                                   employee.Department != "")
                .Select(employee => employee.Department)
                .Distinct()
                .OrderBy(departmentName => departmentName)
                .ToListAsync();

            // Giữ lại nội dung người dùng đã tìm
            ViewBag.Search = search;
            ViewBag.SelectedDepartment = department;

            var employees = await query
                .OrderBy(employee => employee.EmployeeID)
                .ToListAsync();

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,FullName,Department,Email,Phone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã thêm nhân viên {employee.FullName} thành công.";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,FullName,Department,Email,Phone")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Đã cập nhật nhân viên {employee.FullName} thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(employee => employee.Computers)
                .FirstOrDefaultAsync(employee =>
                    employee.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees
                .Include(employee => employee.Computers)
                .FirstOrDefaultAsync(employee =>
                    employee.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Không cho xóa nếu nhân viên đang được cấp máy
            if (employee.Computers.Any())
            {
                TempData["ErrorMessage"] =
                    $"Không thể xóa {employee.FullName} vì nhân viên đang được cấp máy. Vui lòng thu hồi máy trước.";

                return RedirectToAction(nameof(Index));
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Đã xóa nhân viên {employee.FullName}.";

            return RedirectToAction(nameof(Index));

        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(employee =>
                employee.EmployeeID == id);
        }
    }
}
