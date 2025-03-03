using AEL.Domain.Enums;
using AEL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AEL.Domain.Entities
{
    public class Employee : BaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public JobPosition Position { get; set; }
        public int DepartmentId { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }

        public void CalculateSalary(decimal baseSalary)
        {
            decimal bonusPercentage = Position switch
            {
                JobPosition.Developer => 0.10m,
                JobPosition.Manager => 0.20m,
                _ => 0.00m 
            };

            Salary = baseSalary + (baseSalary * bonusPercentage);
        }
    }

}
