using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Models
{
    public class Computer
    {
        [Key]
        public int ComputerID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên máy tính")]
        [StringLength(100)]
        [Display(Name = "Tên máy tính")]
        public string ComputerName { get; set; } = string.Empty;

        [Display(Name = "Nhân viên sử dụng")]
        public int? EmployeeID { get; set; }

        [StringLength(50)]
        [Display(Name = "Hệ điều hành")]
        public string? OperatingSystem { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Active";

        [ForeignKey(nameof(EmployeeID))]
        [Display(Name = "Nhân viên sử dụng")]
        public Employee? Employee { get; set; }
    }
}