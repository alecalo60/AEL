using AEL.Domain.Entities;
using AEL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEL.Application.DTOs
{
    public class CreateEmployeeDTO
    {
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
