using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEL.Application.DTOs
{
    public class TotalSalaryResponseDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public decimal TotalSalary { get; set; }
    }
}
