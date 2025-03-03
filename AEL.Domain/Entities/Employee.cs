using AEL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEL.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; private set; }
        public JobPosition Position { get; set; }
        public int DepartmentId { get; set; }
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
