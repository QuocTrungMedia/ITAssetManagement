namespace ITAssetManagement.Models
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }

        public int TotalComputers { get; set; }

        public int AssignedComputers { get; set; }

        public int AvailableComputers { get; set; }

        public int EmployeesWithoutComputer { get; set; }

        public List<Employee> RecentEmployees { get; set; }
            = new List<Employee>();

        public List<Computer> RecentComputers { get; set; }
            = new List<Computer>();
    }
}