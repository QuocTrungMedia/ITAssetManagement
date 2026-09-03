using System.Diagnostics;
using ITAssetManagement.Data;
using ITAssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalEmployees =
                    await _context.Employees.CountAsync(),

                TotalComputers =
                    await _context.Computers.CountAsync(),

                AssignedComputers =
                    await _context.Computers
                        .CountAsync(x => x.EmployeeID != null),

                AvailableComputers =
                    await _context.Computers
                        .CountAsync(x => x.EmployeeID == null),

                EmployeesWithoutComputer = await _context.Employees
                    .CountAsync(employee => !employee.Computers.Any()),

                RecentEmployees =
                    await _context.Employees
                        .OrderByDescending(x => x.EmployeeID)
                        .Take(5)
                        .ToListAsync(),

                RecentComputers =
                    await _context.Computers
                        .Include(x => x.Employee)
                        .OrderByDescending(x => x.ComputerID)
                        .Take(5)
                        .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}