using AEL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEL.Application.DTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Employee Employees { get; set; }
    }
}
