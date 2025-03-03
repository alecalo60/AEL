using AEL.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace AEL.Application.DTOs
{
    public class UpdateEmployeeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }
        [Required]
        [EnumDataType(typeof(JobPosition), ErrorMessage = "Invalid job position.")]
        public JobPosition Position { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}
