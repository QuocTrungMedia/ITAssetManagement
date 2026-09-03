using ITAssetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = default!;

        public DbSet<Computer> Computers { get; set; } = default!;
    }
}