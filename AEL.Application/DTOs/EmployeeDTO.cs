using AEL.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace AEL.Domain.Entities
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public JobPosition Position { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }

}
